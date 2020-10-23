using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnityOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository CategoryRepository { get; private set; }
        public IFrequencyRepository FrequencyRepository { get; private set; }

        public UnityOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            FrequencyRepository = new FrequencyRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
