using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FrequencyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public FrequencyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Frequency frequency = new Frequency();
            if (id == null)
            {
                return View(frequency);
            }

            frequency = _unitOfWork.FrequencyRepository.Get(id.GetValueOrDefault());

            if (frequency == null)
            {
                return NotFound();
            }

            return View(frequency);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Frequency frequency)
        {
            if (ModelState.IsValid)
            {
                if (frequency.Id == 0)
                    _unitOfWork.FrequencyRepository.Add(frequency);
                else
                    _unitOfWork.FrequencyRepository.Update(frequency);

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(frequency);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.FrequencyRepository.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _unitOfWork.FrequencyRepository.Get(id);
            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.FrequencyRepository.Remove(objFromDB);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful." });
        }

        #endregion
    }
}
