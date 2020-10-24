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

        #region "GetAll overloads"
        /// <summary>
        /// Permite obter todos os objetos e, opcionalmente, filtrar, ordenar, e incluir propriedades de navegação
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <param name="orderBy">Uma expressão para ordenar os resultados</param>
        /// <param name="includedProperties">Uma ou várias expressões para incluir propriedades de navegação.</param>
        /// <returns></returns>
        //IEnumerable<T> GetAll(
        //    Expression<Func<T, bool>> filter = null, // Permite filtrar
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // Permite ordenar
        //    params Expression<Func<T, object>>[] includedProperties
        //    );

        IEnumerable<T> GetAll(); // Se precisar filtrar ou ordenar use Where() e Order(). Nunca retorna List() na classe concreta, sempre IEnumerable

        /// <summary>
        /// Permite obter todos os objetos e incluir propriedades de navegação
        /// </summary>
        /// <param name="includedProperties">Uma ou várias expressões para incluir propriedades de navegação.</param>
        /// <returns></returns>
        //IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includedProperties);

        /// <summary>
        /// Permite obter todos os objetos e filtrar os resultados
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <returns></returns>
        //IEnumerable<T> GetAll(Expression<Func<T, bool>> filter); // Permite filtrar

        /// <summary>
        /// Permite obter todos os objetos e ordenar os resultados
        /// </summary>
        /// <param name="orderBy">Uma expressão para ordenar os resultados</param>
        /// <returns></returns>
        //IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);// Permite ordenar

        #endregion


        #region "GetFirstOrDefault overloads"
      

        T GetFirstOrDefault(); // Se precisar filtrar use Where(). Nunca retorna List() na classe concreta, sempre IEnumerable

        /// <summary>
        /// Permite obter um objeto e, opcionalmente, filtrar ou incluir propriedades de navegação
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <param name="includedProperties">Uma ou várias expressões para incluir propriedades de navegação.</param>
        /// <returns></returns>
        //T GetFirstOrDefault(
        //    Expression<Func<T, bool>> filter = null, // Permite filtrar
        //     params Expression<Func<T, object>>[] includedProperties
        //    );

        /// <summary>
        /// Permite obter um objeto a partir de um filtro
        /// </summary>
        /// <param name="filter">Uma expressão para filtrar os resultados</param>
        /// <returns></returns>
        //T GetFirstOrDefault(Expression<Func<T, bool>> filter); // Permite filtrar

        #endregion

        void Add(T entity);

        void Remove(int id);

        void Remove(T entity);
    }
}
