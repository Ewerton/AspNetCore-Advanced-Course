using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            // Carrega todos os usuários do banco exceto o usuário que está logado porque não quero correr o risco de permitir que o usuário se bloqueie
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // retorna todos os usuários exceto o atual
            var dadosRetornar = _unitOfWork.UsersRepository.GetBy(u => u.Id != claims.Value);
            return View(dadosRetornar);
        }

        public IActionResult Lock(string Id)
        {
            if (String.IsNullOrWhiteSpace(Id))
                return NotFound();

            _unitOfWork.UsersRepository.LockUser(Id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Unlock(string Id)
        {
            if (String.IsNullOrWhiteSpace(Id))
                return NotFound();

            _unitOfWork.UsersRepository.UnlockUser(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
