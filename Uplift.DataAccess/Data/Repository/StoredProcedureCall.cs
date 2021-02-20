using Dapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class StoredProcedureCall : IStoredProcedureCall
    {
        private readonly ApplicationDbContext _db;
        private readonly string ConnectionString = "";

        public StoredProcedureCall(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = _db.Database.GetDbConnection().ConnectionString; // obtem a connection string a partir do DB Context
        }

        public IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                return sqlConn.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                sqlConn.Execute(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                sqlConn.Open();
                var returnedValue = sqlConn.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
                var convertedObject = Convert.ChangeType(returnedValue, typeof(T));
                return (T)convertedObject;
            }
        }

      

      

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
