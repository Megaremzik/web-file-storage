using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WS.Data;
using WS.Interfaces;
using AutoMapper;
using WS.Business.ViewModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;

namespace WS.Business.Services
{
    public class DocumentService
    {
        public enum Abbr { b, Kb, Mb, Gb, Tb };
        private DocumentRepository repo;
        private readonly IMapper mapper;
        private PathProvider pathprovider;
        private UserService _userService;
        public DocumentService(IMapper map, DocumentRepository r, PathProvider p, UserService userService)
        {
            _userService = userService;
            mapper = map;
            repo = r;
            pathprovider = p;
        }

        public IEnumerable<DocumentView> GetAll(string id)
        {
            IEnumerable<Document> documents = repo.GetAll(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }

        public IEnumerable<DocumentView> GetAllWithotDeleted(string id)
        {
            IEnumerable<Document> documents = repo.GetAllWithotDeleted(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }

        public DocumentView Get(int? id)
        {
            Document document = repo.Get(id);
            return mapper.Map<Document, DocumentView>(document);
        }
        public ICollection<DocumentView> GetAllChildrensForFolder(int folderId)
        {
            List<DocumentView> filesList = new List<DocumentView>();

            var childs = GetAllChildren(folderId);
            foreach (var p in childs)
            {
                filesList.Add(Get(p.Id));
                if (!p.IsFile)
                {
                    filesList.AddRange(GetAllChildrensForFolder(p.Id));
                }
            }
            return filesList;
        }


        public Document GetExactlyDocument(int? id)
        {
            Document document = repo.Get(id);
            return document;
        }
        public ICollection<DocumentView> GetParentFolders(int id)
        {
            var docs = new List<DocumentView>();
            var doc = Get(id);
            int parentId = doc.ParentId;
            while (parentId != 0)
            {
                doc = Get(parentId);
                docs.Add(doc);
                parentId = doc.ParentId;
            }
            return docs;
        }
        public void Create(IFormFile file, string userId, int parentId = 0)
        {
            Document doc = new Document { IsFile = true, Size = (int)file.Length, Name = file.FileName, Extention = file.ContentType, UserId = userId, ParentId = parentId, Date_change = DateTime.Now };
            repo.Create(doc);
        }
        public void Create(string folder, string userId, int parentId = 0)
        {
            Document doc = new Document { IsFile = false, Size = 0, Name = folder, Extention = "Folder", UserId = userId, ParentId = parentId, Date_change = DateTime.Now };
            repo.Create(doc);
        }
        public void Update(int? id)
        {
            //var document = Get(id);
            //Document document = mapper.Map<DocumentView, Document>(documentView);
            //repo.Update(document);
        }

        public void MoveToTrash(int? id, DateTime moveDate)
        {
            var document = GetExactlyDocument(id);
            document.Date_change = moveDate;
            document.Type_change = "Delete";
            var isFile = document.IsFile;

            if (isFile == true)
            {
                RenameFileInFileSysytem(Convert.ToInt32(id));

                repo.Update(document);
            }
            else
            {
                MoveToTrashFolder(id, moveDate);
            }
        }

        public void MoveToTrashFolder(int? id, DateTime moveDate)
        {
            var documents = repo.GetAllChildrenWithoutDeleted(id);
            foreach (var doc in documents)
            {
                MoveToTrash(doc.Id, moveDate);
            }
            var document = GetExactlyDocument(id);
            document.Date_change = moveDate;
            document.Type_change = "Delete";
            RenameFolderInFileSysytem(Convert.ToInt32(id));
            repo.Update(document);
        }

        public void RenameFileInFileSysytem(int id)
        {
            var doc = repo.Get(id);
            if (!doc.IsFile)
                RenameFolderInFileSysytem(id);
            else
            {
                var startpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));
                var extension = doc.Name.Split('.');
                var time = doc.Date_change.GetHashCode() + "." + extension[extension.Length-1];
                var finishpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, "bin", GetNewFilePath(id, time));

                var splitParh = finishpath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                string createDirectory = "";
                for (int q=0; q < splitParh.Length - 1; q++)
                {
                    createDirectory += splitParh[q] + "\\";
                }
                Directory.CreateDirectory(createDirectory);

                File.Move(startpath, finishpath);
            }

        }
        public void RenameFolderInFileSysytem(int id)
        {
            var doc = repo.Get(id);
            var filetpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));

            if (Directory.Exists(filetpath))
                {
                    try
                    {
                        Directory.Delete(filetpath);
                    }
                    catch (System.IO.IOException e)
                    {
                    }
                }

            var startpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, "bin", GetNewFolderFilePath(id));
            var time = doc.Date_change.GetHashCode().ToString();
            var finishpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, "bin", GetNewFilePath(id, time));
            Directory.Move(startpath, finishpath);
        }

        public string GetNewFolderFilePath(int id)
        {
            string path = "";
            int parentId = id;
            do
            {
                path = Path.Combine(GetParentFolder(ref parentId), path);
            } while (parentId != 0 && GetExactlyDocument(parentId).Type_change == "Delete");
            return path;
        }

        public string GetNewFilePath(int id, string time)
        {
            string path = "";
            int parentId = id;
            path = Path.Combine(Convert.ToString(parentId + " " + time), path);
            GetParentFolder(ref parentId);
            while (parentId != 0 && GetExactlyDocument(parentId).Type_change=="Delete")
            {
                path = Path.Combine(GetParentFolder(ref parentId), path);
            }

            return path;
        }

        public Document FindVirtualParent(int id)
        {
            int parentId = id;
            Document doc;
            do
            {
                doc = repo.Get(parentId);
                if (doc.ParentId == 0 || doc.Date_change != repo.Get(doc.ParentId).Date_change || repo.Get(doc.ParentId).Type_change != "SaveForFile")
                    return doc;
                else
                    parentId = doc.ParentId;

            } while (true);
        }

        public void FirstStepDelete(int? id)
        {
            var document = FindVirtualParent(Convert.ToInt32(id));
            Delete(document.Id);
        }

            public void Delete(int? id)
        {
            Document document = GetExactlyDocument(id);
            if (document.IsFile == true)
            {
                var filetpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, "bin", GetNewFilePath(Convert.ToInt32(id)));
                if (File.Exists(filetpath))
                {
                    try
                    {
                        File.Delete(filetpath);
                    }
                    catch (System.IO.IOException e)
                    {
                    }
                }
                repo.Delete(document.Id);
            }
            else
            {
                DeleteFolder(document.Id);
            }
        }

        public void DeleteFolder(int? id)
        {
            var document = GetExactlyDocument(id);
            IEnumerable<Document> documents = null;
            if (document.Type_change == "Delete")
                documents = repo.GetAllChildrenDeletedWithIt(id);
            else if (document.Type_change == "SaveForFile")
                documents = repo.GetAllWirtualChildrenDeletedWithIt(id);
            foreach (var doc in documents)
            {
                Delete(doc.Id);
            }
            documents = repo.GetAllChildren(id);
            var filetpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, "bin", GetNewFilePath(Convert.ToInt32(id)));
            if (Directory.Exists(filetpath))
            {
                try
                {
                    Directory.Delete(filetpath);
                }
                catch (System.IO.IOException e)
                {
                }
            }
            if (documents.Count() == 0)
            {
                repo.Delete(id);
            }
            else
            {
                //var startpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, GetNewFilePath(Convert.ToInt32( id)));
                document.Date_change = documents.First().Date_change;
                document.Type_change = "SaveForFile";
                //var finishpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, GetNewFilePath(Convert.ToInt32(id)));
                //Directory.Move(startpath, finishpath);
                repo.Update(document);
            }
        }

        public string GetNewParentFolder(ref int id)
        {
            var doc = repo.Get(id);
            id = doc.ParentId;
            if (!doc.IsFile)
            {
                return Convert.ToString(doc.Id + " " + doc.Date_change.GetHashCode().ToString());
            }
            else
            {
                var extension = doc.Name.Split('.');
                return Convert.ToString(doc.Id + " " + doc.Date_change.GetHashCode().ToString() + "." + extension[extension.Length - 1]);
            }
        }

        public string GetNewFilePath(int id)
        {
            string path = "";
            int parentId = id;
            do
            {
                var child = parentId;
                path = Path.Combine(GetNewParentFolder(ref parentId), path);
                if (parentId == 0 || GetExactlyDocument(parentId).Date_change != GetExactlyDocument(child).Date_change)
                    break;
            } while (parentId != 0 && GetExactlyDocument(parentId).Type_change == "Delete");

            return path;
        }

        public IEnumerable<DocumentView> GetAllChildren(int? id)
        {
            var documents = repo.GetAllChildren(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }
        public IEnumerable<DocumentView> GetAllRootElements(string userId)
        {
            var documents = repo.GetAllRootElements(userId);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }

        public IEnumerable<DocumentView> GetAllChildrenWithoutDeleted(int? id)
        {
            var documents = repo.GetAllChildrenWithoutDeleted(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }
        public IEnumerable<DocumentView> GetAllRootElementsWithoutDeleted(string userId)
        {
            var documents = repo.GetAllRootElementsWithoutDeleted(userId);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }

        public int CreateFolders(string folders, string userId, int parentId = 0)
        {
            if (folders == null) return 0;
            var folder = folders.Split('/');
            for (int i = 0; i < folder.Length - 1; i++)
            {
                Create(folder[i], userId, parentId);
                parentId = repo.GetIdByName(userId, folder[i], parentId);
            }
            return parentId;
        }

        public void FirstRestore(int id) {
            var path = GetFilePath(id);
            var document = GetExactlyDocument(id);
            pathprovider.AddFoldersWhenCopyFile(path, document.UserId);
            UpdateVirtualParent(id, DateTime.Now);

            Restore(id);
        }

        public void Restore(int id)
        {
            var path = GetFilePath(id);
            var document = GetExactlyDocument(id);
            var newPath = Path.Combine(pathprovider.GetRootPath(), document.UserId, path);
            var oldpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, "bin", GetNewFilePath(Convert.ToInt32(id)));

            if (document.IsFile)
            {
                File.Move(oldpath, newPath);
                document.Date_change = DateTime.Now;
                document.Type_change = "Restore";
                repo.Update(document);
            }
            else
            {
                RestoreFolder(id);
            }

        }

        public void RestoreFolder(int id)
        {
            var path = GetFilePath(id);
            var document = GetExactlyDocument(id);
            var newPath = Path.Combine(pathprovider.GetRootPath(), document.UserId, path);
            Directory.CreateDirectory(newPath);
            var documents = repo.GetAllChildrenDeletedWithIt(id);
            foreach (var item in documents)
            {
                Restore(item.Id);
            }
            var filetpath = Path.Combine(pathprovider.GetRootPath(), document.UserId, "bin", GetNewFilePath(Convert.ToInt32(id)));
            if (Directory.Exists(filetpath))
            {
                try
                {
                    Directory.Delete(filetpath);
                }
                catch (System.IO.IOException e)
                {
                }
            }
            //impotant
            document.Date_change = DateTime.Now;
            document.Type_change = "Restore";
            repo.Update(document);
        }

        public void UpdateVirtualParent(int id, DateTime date)
        {
            int parentId = id;
            Document doc;
            do
            {
                doc = repo.Get(parentId);
                if (doc.ParentId == 0 )
                    break;
                else if (repo.Get(doc.ParentId).Type_change == "SaveForFile")
                {
                    parentId = doc.ParentId;
                    doc = GetExactlyDocument(parentId);
                    doc.Type_change = "Restore";
                    doc.Date_change = date;
                    repo.Update(doc);
                }
                //else
                //{
                //    parentId = doc.ParentId;
                //    doc = GetExactlyDocument(parentId);
                //    repo.Create(doc);
                //}

            } while (true);
        }

        public void UpdateParentId(int id, int parentId, string startPath = "")
        {
            var path = GetFilePath(id);
            var doc = repo.Get(id);
            doc.ParentId = parentId;
            doc.Date_change = DateTime.Now;
            repo.Update(doc);
            var finishPath = GetFilePath(id);
            if (doc.IsFile == false)
            {
                if (startPath != "") startPath = Path.Combine(startPath, doc.Name);
                else startPath = path;
                pathprovider.AddFoldersWhenCopy(finishPath, doc.UserId);
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                UpdateFolderParentId(id, parentId, ref startPath);
            }
            else
            {
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                if (startPath == "")
                {
                    startPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId,path);
                }
                else
                {
                    startPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, startPath, doc.Name);
                }
                
                File.Move(startPath, finishPath);
            }
        }
        public void UpdateFolderParentId(int id, int parentId, ref string startPath)
        {
            foreach (var item in repo.GetAllChildren(id))
            {
                UpdateParentId(item.Id, id, startPath);
            }
            var path = startPath.Split('\\');
            startPath = "";
            //path.ElementAt(path.Length - 1).Take(path.Length - 1);
            for (int i = 0; i < path.Length - 1; i++)
            {
                startPath = Path.Combine(startPath, path[i]);
            }
        }
        public void CreateACopy(int id, int parentId)
        {
            var document = repo.Get(id);
            Document doc = new Document { IsFile = document.IsFile, Size = document.Size, Name = document.Name, Extention = document.Extention, UserId = document.UserId, ParentId = parentId, Date_change = DateTime.Now };
            repo.Create(doc);
            var newId = repo.GetIdByName(doc.UserId, doc.Name, parentId);
            var finishPath = GetFilePath(newId);
            string startPath = "";
            if (doc.IsFile == false)
            {
                pathprovider.AddFoldersWhenCopy(finishPath, doc.UserId);
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                CreateAFolderCopy(id, newId);
            }
            else
            {
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                startPath = GetFilePath(id);
                startPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, startPath);
                File.Copy(startPath, finishPath);
            }

        }
        public void CreateAFolderCopy(int id, int parentId)
        {
            var documents = repo.GetAllChildren(id);
            foreach (var doc in documents)
            {
                CreateACopy(doc.Id, parentId);
            }

        }
        public string GetFilePath(int id)
        {
            string path = "";
            int parentId = id;
            do
            {
                path = Path.Combine(GetParentFolder(ref parentId), path);
            } while (parentId != 0);
            return path;
        }
        public string GetPathToFile(int id)
        {
            string path = string.Empty;
            var doc = Get(id);
            int parentId = doc.ParentId;
            while (parentId != 0)
            {
                doc = Get(parentId);
                path += "\\" + doc.Name;
                parentId = doc.ParentId;
            }
            return path;
        }
        public string GetParentFolder(ref int id)
        {
            var doc = repo.Get(id);
            id = doc.ParentId;
            return doc.Name;
        }
        public void RenameFile(int id, string name)
        {
            var doc = repo.Get(id);
            if (!doc.IsFile)
                RenameFolder(id, name);
            else
            {
                var startpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));
                doc.Name = name;
                var finishpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));
                doc.Date_change = DateTime.Now;
                repo.Update(doc);
                File.Move(startpath, finishpath);
            }

        }
        public void RenameFolder(int id, string name)
        {
            var doc = repo.Get(id);
            var startpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));
            doc.Name = name;
            var finishpath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, GetFilePath(id));
            doc.Date_change = DateTime.Now;
            repo.Update(doc);
            Directory.Move(startpath, finishpath);
        }
        public bool IsDocumentExist(int documentId)
        {
            DocumentView document = Get(documentId);
            return document != null;
        }
        public string GetFullPath(int documentId)
        {
            string path = string.Empty;
            var document = Get(documentId);
            path = document.Name;
            while (document.ParentId != 0)
            {
                document = Get(document.ParentId);
                path = Path.Combine(document.Name, path);
            }
            return Path.Combine(pathprovider.GetRootPath(), document.UserId, path);
        }
        public bool CanBeViewed(DocumentView doc)
        {
            var extention = doc.Extention.Split('/');
            if (extention[0] == "text" || extention[0] == "audio" || extention[0] == "image" || extention[1] == "pdf")
            {
                return true;
            }
            return false;
        }
        public string MakeSizeView(DocumentView document)
        {
            var size = document.Size;
            Abbr abbr = Abbr.b;
            var newsize = (double)size;
            if (document.IsFile == false) return "-";
            while (true)
            {
                if (Math.Round((double)newsize / 1024, 1) > 0.1)
                {
                    newsize = Math.Round((double)newsize / 1024, 1);
                    abbr++;
                }
                else break;
            }
            return newsize + " " + abbr.ToString();
        }
        public IEnumerable<DocumentViewModel> ConvertToViewModel(IEnumerable<DocumentView> document)
        {
            List<DocumentViewModel> documents = new List<DocumentViewModel>();
            foreach (var doc in document)
            {
                documents.Add(new DocumentViewModel(doc, MakeSizeView(doc),false,IconForFile(doc)));
            }
            return documents;
        }
        public string IconForFile(DocumentView document)
        {
            if(document.IsFile==false) return "fa fa-folder fa-2x";
            var name = document.Name;
            var extention = name.Split('.');
            if (extention[extention.Length-1]=="png"|| extention[extention.Length - 1] == "jpg" || extention[extention.Length - 1] == "jpeg" || extention[extention.Length - 1] == "bmp")
            {
                return "fa fa-file-image fa-2x";
            }
            else if (extention[extention.Length - 1] == "txt" || extention[extention.Length - 1] == "js" || extention[extention.Length - 1] == "html")
            {
                return "fa fa-file fa-2x"; 
            }
            else if (extention[extention.Length - 1] == "doc" || extention[extention.Length - 1] == "docx")
            {
                return "fa fa-file-word fa-2x";
            }
            else if (extention[extention.Length - 1] == "pdf")
            {
                return "fa fa-file-pdf fa-2x";
            }
            else if (extention[extention.Length - 1] == "mp3" || extention[extention.Length - 1] == "mp4")
            {
                return "fa fa-file-audio fa-2x";
            }
            else if (extention[extention.Length - 1] == "zip" || extention[extention.Length - 1] == "rar")
            {
                return "fa fa-file-archive fa-2x";
            }
            else if ( extention[extention.Length - 1] == "js" || extention[extention.Length - 1] == "html"|| extention[extention.Length - 1] == "cs"|| extention[extention.Length - 1] == "py")
            {
                return "fa fa-file-code fa-2x";
            }
            else
            {
                return "fa fa-file fa-2x";
            }
        }
        public IEnumerable<DocumentView> GetAllDeletedFiles()
        {
            var documents = repo.GetAllDeletedFiles();
            List<Document> mainDocuments = new List<Document>();
            Document parent;
            foreach (Document doc in documents)
            {
                parent = FindParent(doc.Id);
                if(!mainDocuments.Contains(parent))
                    mainDocuments.Add(FindParent(doc.Id));
            }
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(mainDocuments.AsEnumerable());
        }
        public Document FindParent(int id)
        {
            int parentId = id;
            Document doc;
            do
            {
                doc = repo.Get(parentId);
                if (doc.ParentId == 0 || doc.Date_change != repo.Get(doc.ParentId).Date_change || repo.Get(doc.ParentId).Type_change!="Delete")
                    return doc;
                else
                    parentId = doc.ParentId;

            } while (true);
        }
        public IEnumerable<DocumentView> GetAllDeletedWithIt(int? id)
        {
            IEnumerable<Document> documents = repo.GetAllDeletedWithIt(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents);
        }
    }
}
