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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
    }
}
