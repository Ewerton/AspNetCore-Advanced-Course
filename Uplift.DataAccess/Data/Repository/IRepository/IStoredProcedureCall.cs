using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IStoredProcedureCall : IDisposable
    {
        IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null); // DynamicParameters do Dapper para permitir chamar uma StoredProcedure que recebe parametros.

        void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null);

        T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null);

    }
}
