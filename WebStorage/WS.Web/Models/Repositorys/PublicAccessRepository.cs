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
    public class PublicAccessRepository : IPublicAccessRepository
    {
        string connectionString = null;
        public PublicAccessRepository(string conn)
        {
            connectionString = conn;
        }

        public List<PublicAccess> GetFileAccess(int file_id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<PublicAccess>("SELECT * FROM PublicAccess WHERE File_id = @file_id", new { file_id}).ToList();
            }
        }
        public PublicAccess Get(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<PublicAccess>("SELECT * FROM PublicAccess WHERE Id = @id", new { id }).FirstOrDefault();
            }
        }

        public void Create(PublicAccess publicAccess)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO PublicAccess (Link, File_id, Write) VALUES(@Link, @File_id, @Write)";
                db.Execute(sqlQuery, publicAccess);
            }
        }

        public void Update(PublicAccess publicAccess)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE PublicAccess SET Link = @Link, File_id = @File_id, Write = @Write WHERE Id = @Id";
                db.Execute(sqlQuery, publicAccess);
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE PublicAccess Users WHERE Id = @id";
                db.Execute(sqlQuery, new { id });
            }
        }
    }
}
