using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        // Update não foi implementado pois o UnityOfWork será o responsável
        // por salvar as entidades

        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null, // Permite filtrar
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // Permite ordenar
            params Expression<Func<T, object>>[] includedProperties
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null, // Permite filtrar
             params Expression<Func<T, object>>[] includedProperties
            );

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);
    }
}
