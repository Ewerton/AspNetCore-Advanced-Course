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
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _db.Frequencies.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            var objFromDB = _db.Frequencies.FirstOrDefault(s => s.Id == frequency.Id);

            ///TODO: Tem como automatizar? Automapper?
            objFromDB.Name = frequency.Name;
            objFromDB.FrequencyCount = frequency.FrequencyCount;

            _db.SaveChanges();
        }
    }
}
