using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _db;

        public ServiceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service service)
        {
            var objFromDB = _db.Services.FirstOrDefault(s => s.Id == service.Id);

            ///TODO: Tem como automatizar?
            objFromDB.Name = service.Name;
            objFromDB.LongDesc = service.LongDesc;
            objFromDB.Price = service.Price;
            objFromDB.ImageUrl = service.ImageUrl;
            
            objFromDB.FrequencyId = service.FrequencyId;
            objFromDB.CategoryId = service.CategoryId;

            _db.SaveChanges();
        }
    }
}
