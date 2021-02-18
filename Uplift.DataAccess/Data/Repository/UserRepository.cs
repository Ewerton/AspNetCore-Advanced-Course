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
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void LockUser(string userId)
        {
            // Seta a coluna [LockoutEnd] da tabela AspNetUsers para 1000 assim o usuário ficará bloqueado "eternamente"
            var userFromDB = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            userFromDB.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void UnlockUser(string userId)
        {
            // Seta a coluna [LockoutEnd] da tabela AspNetUsers para HOJE assim o usuário não ficará bloqueado pois seu bloqueio acabou hoje
            var userFromDB = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            userFromDB.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }
    }
}
