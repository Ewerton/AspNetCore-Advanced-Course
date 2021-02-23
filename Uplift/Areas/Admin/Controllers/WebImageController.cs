using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uplift.DataAccess;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class WebImageController : Controller
    {
        // Este Controller acessa o DbCOntext diretamente sem passar pelo UnitOfWork propositalmente, isso é apenas para fins de demonstração
        //private readonly IUnitOfWork _unitOfWork;
        ApplicationDbContext _db;

        public WebImageController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            WebImage imageObj = new WebImage();
            if (id.HasValue)
            {
                imageObj = _db.WebImages.SingleOrDefault(i => i.Id == id);

                if (imageObj == null)
                {
                    return NotFound();
                }
            }

            return View(imageObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, WebImage imageObj)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] imageBytes = null;
                    using (Stream fileStream = files[0].OpenReadStream())
                    {
                        using (MemoryStream memoStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoStream);
                            imageBytes = memoStream.ToArray();
                        }
                    }
                    imageObj.Picture = imageBytes;
                }

                if (imageObj.Id == 0) // Insert
                {
                    _db.WebImages.Add(imageObj);
                }
                else // Update
                {
                    var imageFromDB = _db.WebImages.Where(i => i.Id == id).FirstOrDefault();
                    imageFromDB.Name = imageObj.Name;
                    if (files.Count > 0)
                    {
                        imageFromDB.Picture = imageObj.Picture;
                    }
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(imageObj);
        }


        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // Chama usando o EF Core 
            //return Json(new { data = _unitOfWork.CategoryRepository.GetAll() });

            // Chama usando StoredProcedures
            //return Json(new { data = _unitOfWork.StoredProcedureCall.ReturnList<Category>(Constants.usp_GetAllCategory) });

            return Json(new { data = _db.WebImages.ToList() });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            // var objFromDB = _unitOfWork.CategoryRepository.Get(id);
            var objFromDB = _db.WebImages.Find(id);
            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _db.WebImages.Remove(objFromDB);
            _db.SaveChanges();

            return Json(new { success = true, message = "Delete successful." });
        }

        #endregion
    }
}
