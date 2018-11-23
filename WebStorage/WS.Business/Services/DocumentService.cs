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

namespace WS.Business.Services
{
    public class DocumentService
    {
        private DocumentRepository repo;
        private readonly IMapper mapper;
        private PathProvider pathprovider;
        public DocumentService(IMapper map,DocumentRepository r, PathProvider p)
        {
            mapper = map;
            repo = r;
            pathprovider = p;
        }

        public IEnumerable<DocumentView> GetAll(string id)
        {
            IEnumerable<Document> documents = repo.GetAll(id);
            return mapper.Map<IEnumerable<Document>, IEnumerable<DocumentView>>(documents); 
        }

        public DocumentView Get(int? id)
        {
            Document document = repo.Get(id);
            return mapper.Map<Document, DocumentView>(document);
        }
        public void Create(IFormFile file, string userId, int parentId=0)
        {
            Document doc = new Document { IsFile = true, Size = (int)file.Length, Name = file.FileName,Extention=file.ContentType , UserId=userId, ParentId = parentId, Date_change=DateTime.Now };
            repo.Create(doc);
        }
        public void Create(string folder, string userId, int parentId=0)
        {
            Document doc = new Document { IsFile = false, Size = 0, Name = folder, Extention = "Folder", UserId = userId, ParentId=parentId,Date_change=DateTime.Now };
            repo.Create(doc);
        }
        public void Update(DocumentView documentView)
        {
            Document document = mapper.Map<DocumentView, Document>(documentView);
            repo.Update(document);
        }

        public void Delete(int? id)
        {
            var document = Get(id);
            if (document.IsFile == true)
            {
                repo.Delete(id);
            }
            else
            {
                DeleteFolder(id);
            }
        }

        public void DeleteFolder(int? id)
        {
            var documents=repo.GetAllChildren(id);
            foreach(var doc in documents)
            {
                Delete(doc.Id);
            }
            repo.Delete(id);
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
        public int CreateFolders(string folders,string userId, int parentId=0)
        {
            if (folders == null) return 0;
            var folder = folders.Split('/');
            for(int i=0; i < folder.Length - 1; i++)
            {
                Create(folder[i], userId, parentId);
                parentId= repo.GetIdByName(userId,folder[i],parentId);
            }
            return parentId;
        }
        public void UpdateParentId(int id, int parentId, string startPath="")
        {
            var path = GetFilePath(id);
            var doc = repo.Get(id);
            doc.ParentId = parentId;
            repo.Update(doc);
            var finishPath = GetFilePath(id);
            if (doc.IsFile == false)
            {
                if (startPath != "") startPath = Path.Combine(startPath,doc.Name);
                else startPath =path;
                pathprovider.AddFoldersWhenCopy(finishPath, doc.UserId);
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                UpdateFolderParentId(id, parentId,ref startPath);
            }
            else
            {
                finishPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, finishPath);
                startPath = Path.Combine(pathprovider.GetRootPath(), doc.UserId, startPath,doc.Name);
                File.Move(startPath, finishPath);
            }
        }
        public void UpdateFolderParentId(int id, int parentId, ref string startPath)
        {
            foreach(var item in repo.GetAllChildren(id))
            {
                UpdateParentId(item.Id, id,startPath);
            }
            var path = startPath.Split('\\');
            startPath = "";
            //path.ElementAt(path.Length - 1).Take(path.Length - 1);
            for(int i = 0; i < path.Length - 1; i++)
            {
                startPath = Path.Combine(startPath, path[i]);
            }
        }
        public void CreateACopy(int id, int parentId)
        {
            var document = repo.Get(id);
            Document doc= new Document { IsFile = document.IsFile, Size = document.Size, Name = document.Name, Extention = document.Extention, UserId = document.UserId, ParentId = parentId, Date_change = DateTime.Now };
            repo.Create(doc);
            var newId = repo.GetIdByName(doc.UserId, doc.Name, parentId);
            var finishPath = GetFilePath(newId);
            string startPath = "";
            if (doc.IsFile == false)
            {
                pathprovider.AddFoldersWhenCopy(finishPath,doc.UserId);
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
                path = Path.Combine(GetParentFolder(ref parentId) , path);
            } while (parentId != 0);
            return path;
        }
        public string GetParentFolder(ref int id)
        {
            var doc=repo.Get(id);
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
            repo.Update(doc);
            Directory.Move(startpath,finishpath);
        }
        public bool IsDocumentExist(int documentId)
        {
            DocumentView document = Get(documentId);
            return document != null;
        }
    }
}
