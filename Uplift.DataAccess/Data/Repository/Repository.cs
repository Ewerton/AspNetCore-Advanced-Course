using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;

            // Seta o T no contexto, assim, sempre que alterarmos o contexto 
            // estaremos alterando o T
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            // O Salvamento é feito pelo UnitOfWOrk
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        #region "GetAll() overloads"

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.AsEnumerable();
        }

        //public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includedProperties)
        //{
        //    IQueryable<T> query = dbSet;

        //    query = AddIncludes(query, includedProperties);

        //    return query.ToList();
        //}

        //public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter)
        //{
        //    IQueryable<T> query = dbSet;

        //    if (filter != null)
        //        query = query.Where(filter);

        //    return query.ToList();
        //}

        //public IEnumerable<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        //{
        //    IQueryable<T> query = dbSet;

        //    if (orderBy != null)
        //        return orderBy(query).ToList();
        //    else
        //        return query.ToList();
        //}

        public IEnumerable<T> GetAll(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           params Expression<Func<T, object>>[] includedProperties)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includedProperties);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.AsEnumerable();
        }

        #endregion

        // É possível usar uma Expression para definir os Includes e desta forma não confiar em Strings
        //public IEnumerable<T> GetAll(
        //    Expression<Func<T, bool>> filter = null,
        //    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        //    string includedProperties = null)
        //{
        //    IQueryable<T> query = dbSet;

        //    if (filter != null)
        //        query = query.Where(filter);

        //    if (includedProperties != null)
        //    {
        //        foreach (var includedProp in includedProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includedProp);
        //        }
        //    }

        //    if (orderBy != null)
        //        return orderBy(query).ToList();
        //    else
        //        return query.ToList();
        //}


        #region "FirstOrDefalut overloads"
        public T GetFirstOrDefault()
        {
            IQueryable<T> query = dbSet;
            return query.FirstOrDefault();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
                                   params Expression<Func<T, object>>[] includedProperties)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includedProperties);

            return query.FirstOrDefault();
        }

        #endregion

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
            Remove(entityToRemove);
        }

        public void Remove(T entity)
        {
            // O Salvamento é feito pelo UnitOfWOrk
            dbSet.Remove(entity);
        }

        /// <summary>
        /// Adiciona os Includes em uma query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="includedProperties"></param>
        /// <returns></returns>
        private IQueryable<T> AddIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includedProperties)
        {
            if (includedProperties != null)
            {
                foreach (var includedProp in includedProperties)
                {
                    var memberExpression = includedProp.Body as MemberExpression;

                    if (memberExpression != null)
                        query = query.Include(memberExpression.Member.Name);
                }
            }

            return query;
        }
    }

    public static class RepoExtensions
    {
        // Este método de extensão pode ser chamado para adicionar propriedades de navegação nos resultados
        // Tome cuidado para não criar multiplas consultas no banco, exemplo.
        // Vc escreve uma query, faz ToList() que executa a query no banco, depois, chama Include() que vai forçar ir no banco novamente.
        public static IEnumerable<T> Include<T>(this IEnumerable<T> query, params Expression<Func<T, object>>[] includedProperties) where T : class
        {
            if (includedProperties != null)
            {
                foreach (var includedProp in includedProperties)
                {
                    var memberExpression = includedProp.Body as MemberExpression;

                    if (memberExpression != null)
                        query = query.AsQueryable().Include(memberExpression.Member.Name);
                }
            }

            return query;
        }

        //public static IEnumerable<T> Where<T>(this IEnumerable<T> query, Expression<Func<T, bool>> filter) where T : class
        //{
        //    if (filter != null)
        //        query = query.AsQueryable().Where(filter);

        //    return query;
        //}
    }
}
