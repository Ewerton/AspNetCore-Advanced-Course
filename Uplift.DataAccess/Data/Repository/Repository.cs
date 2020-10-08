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
                return query.ToList();
        }

        // É possível usar uma Expression para definir os Includes e desta forma não 
        // confiar em Strings
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


        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
                                   params Expression<Func<T, object>>[] includedProperties)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            query = AddIncludes(query, includedProperties);

            return query.FirstOrDefault();
        }

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
            foreach (var includedProp in includedProperties)
            {
                var memberExpression = includedProp.Body as MemberExpression;

                if (memberExpression != null)
                    query = query.Include(memberExpression.Member.Name);
            }

            return query;
        }
    }
}
