using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Uplift.Models
{

    // Herdando de Microsoft.AspNetCore.Identity.IdentityUser permite que estas propriedades sejam adicionadas á tabela dbo.AspNetUsers que é criada pelo mecanismo de Identity do ASP.NET
    // Basta adicionar as props aqui e fazer no Nuget Package Manager:
    // - add-migration AddPropertiesToAspNetUsers
    // - update-database
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
    }
}
