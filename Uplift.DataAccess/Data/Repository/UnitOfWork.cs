using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository CategoryRepository { get; private set; }
        public IFrequencyRepository FrequencyRepository { get; private set; }
        public IServiceRepository ServiceRepository { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepository { get; private set; }

        public IOrderDetailsRepository OrderDetailsRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepository = new CategoryRepository(_db);
            FrequencyRepository = new FrequencyRepository(_db);
            ServiceRepository = new ServiceRepository(_db);
            OrderHeaderRepository = new OrderHeaderRepository(_db);
            OrderDetailsRepository = new OrderDetailsRepository(_db);
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
