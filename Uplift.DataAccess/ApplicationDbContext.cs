using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uplift.Models;

namespace Uplift.DataAccess
{

    // ApplicationDbContext herda de IdentityDbContext porque irei utilizar o Identity
    // de qualquer forma IdentityDbContext acaba herdando de DbContext, então, 
    // ApplicationDbContext é o meu DbContext do EF
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        /*
          1 - Abre o Nuget package Manager
          2 - Em Default Project, muda para o Uplift.DataAccess (isso acontece porque o DataAccess está em um projeto separado e não no projeto MVC)
          3 - add-migration <UmNomeQualquer> 
          4 - update-database // Para executar os versionamentos
        */

        public DbSet<Category> Categories { get; set; }


        public DbSet<Frequency> Frequencies { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<WebImage> WebImages { get; set; }
    }
}
