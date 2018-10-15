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
    public class LimitedAccessRepository : ILimitedAccessRepository
    {
        string connectionString = null;
        public LimitedAccessRepository(string conn)
        {
            connectionString = conn;
        }

        public List<LimitedAccess> GetFileAccess(int file_id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<LimitedAccess>("SELECT * FROM LimitedAccess WHERE File_id=@file_id", new { file_id }).ToList();
            }
        }
        public List<LimitedAccess> GetUserAccess(int user_id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<LimitedAccess>("SELECT * FROM LimitedAccess WHERE User_id=@user_id", new { user_id }).ToList();
            }
        }

        public void Create(LimitedAccess limitedAccess)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "INSERT INTO LimitedAccess (User_id, File_id, Write, Link) VALUES(@User_id, @File_id, @Write, @Link)";
                db.Execute(sqlQuery, limitedAccess);
            }
        }

        public void Update(LimitedAccess limitedAccess)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "UPDATE LimitedAccess SET Write = @Write, Link = @Link  WHERE User_id = @User_id AND File_id = @File_id";
                db.Execute(sqlQuery, limitedAccess);
            }
        }

        public void Delete(int file_id, int user_id)
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                var sqlQuery = "DELETE FROM LimitedAccess WHERE User_id = @User_id AND File_id = @File_id";
                db.Execute(sqlQuery, new { file_id, user_id });
            }
        }
    }
}
