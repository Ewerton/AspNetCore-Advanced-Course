using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        /// <summary>
        /// Quando um usuário deixa a empresa o login dele precisa ser bloqueado, como o Id de um usuário no banco é um GUID o parametro userId é do tipo string
        /// </summary>
        /// <param name="userId"></param>
        void LockUser(string userId);

        void UnlockUser(string userId);
    }
}
