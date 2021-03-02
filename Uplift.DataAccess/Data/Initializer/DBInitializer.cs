using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Data.Initializer
{
    // Esta classe popula dados no banco de dados logo na primeira execução do sistema, assim, um primeiro usuário administrador e a roles principais já são criadas por padrão.
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            // Se existir a role Admin quer dizer que as roles (e potencialmente os outros dados) já foram incluídos no banco.
            if (_db.Roles.Any(r => r.Name == Constants.Admin)) return;

            // Cria as primeiras roles e "espera" para que não seja assincrono, pois a role precisa ser criada para ir para o "proximo passo"
            _roleManager.CreateAsync(new IdentityRole(Constants.Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Constants.Manager)).GetAwaiter().GetResult();

            // Cria o primeiro usuário administrador e adiciona ele à role de Admin
            _userManager.CreateAsync(new ApplicationUser
            {
                Name = "Admin",
                UserName = "admin@email.com",
                Email = "admin@email.com",
                EmailConfirmed = true
            }, "Admin123!@#").GetAwaiter().GetResult();

            //var user = _db.ApplicationUsers.Where(u => u.Email == "admin@gmail.com").ToList().LastOrDefault();
            //_userManager.AddToRoleAsync(user, Constants.Admin).GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "admin@email.com").FirstOrDefault();
            _userManager.AddToRoleAsync(user, Constants.Admin).GetAwaiter().GetResult();
        }
    }
}
