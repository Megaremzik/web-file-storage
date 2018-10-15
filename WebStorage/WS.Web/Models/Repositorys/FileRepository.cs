using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WS.Web.Models.IRepository;

namespace WS.Web.Models.Repositorys
{
    public class FileRepository : IFileRepository
    {
        string connectionString = null;
        public FileRepository(string conn)
        {
            connectionString = conn;
        }

        public List<File> GetUserFiles(int owner_id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<File>("SELECT * FROM Files WHERE Owner_id=@owner_id", new { owner_id }).ToList();
            }
        }
        public File Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<File>("SELECT * FROM Files WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Create(File file)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO Files (Owner_id, File_name, Data_change, Size, Path) VALUES(@Owner_id, @File_name, @Data_change, @Size, @Path)";
                db.Execute(sqlQuery, file);
            }
        }

        public void Update(File file)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE Files SET Owner_id = @Owner_id, File_name = @File_name, Data_change = @Data_change, Size = @Size, Path = @Path  WHERE Id = @Id";
                db.Execute(sqlQuery, file);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM Files WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }

}
