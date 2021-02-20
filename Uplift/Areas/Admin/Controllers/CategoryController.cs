using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if (id == null)
            {
                return View(category);
            }

            category = _unitOfWork.CategoryRepository.Get(id.GetValueOrDefault());

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                    _unitOfWork.CategoryRepository.Add(category);
                else
                    _unitOfWork.CategoryRepository.Update(category);

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // Chama usando o EF Core 
            //return Json(new { data = _unitOfWork.CategoryRepository.GetAll() });

            // Chama usando StoredProcedures
            return Json(new { data = _unitOfWork.StoredProcedureCall.ReturnList<Category>(Constants.usp_GetAllCategory)});
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _unitOfWork.CategoryRepository.Get(id);
            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.CategoryRepository.Remove(objFromDB);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful." });
        }

        #endregion
    }
}
