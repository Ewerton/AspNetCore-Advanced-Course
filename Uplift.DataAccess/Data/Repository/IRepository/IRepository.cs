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

        IEnumerable<T> GetAll(); // Se precisar filtrar ou ordenar use Where() e Order(). Nunca retorna List() na classe concreta, sempre IEnumerable

        IEnumerable<T> GetBy( Expression<Func<T, bool>> filter = null);

        IEnumerable<T> GetBy( Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        IEnumerable<T> GetBy(params Expression<Func<T, object>>[] includedProperties);

        /// <summary>
        /// Permite obter todos os objetos e, opcionalmente, filtrar, ordenar, e incluir propriedades de navegação
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <param name="orderBy">Uma expressão para ordenar os resultados</param>
        /// <param name="includedProperties">Uma ou várias expressões para incluir propriedades de navegação.</param>
        /// <returns></returns>
        IEnumerable<T> GetBy(
            Expression<Func<T, bool>> filter = null, // Permite filtrar
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // Permite ordenar
            params Expression<Func<T, object>>[] includedProperties
            );


        T GetFirstOrDefault(); // Se precisar filtrar use Where(). Nunca retorna List() na classe concreta, sempre IEnumerable

        T GetFirstOrDefault(Expression<Func<T, bool>> filter = null);

        T GetFirstOrDefault(params Expression<Func<T, object>>[] includedProperties);

        /// <summary>
        /// Permite obter um objeto e, opcionalmente, filtrar ou incluir propriedades de navegação
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <param name="includedProperties">Uma ou várias expressões para incluir propriedades de navegação.</param>
        /// <returns></returns>
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null, // Permite filtrar
             params Expression<Func<T, object>>[] includedProperties
            );

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);
    }
}
