using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models.ViewModels;

namespace Uplift.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostEnvironment; // Usado para descobrir os Paths pois este controler vai fazer upload de imagens

        // Significa que está propriedade será automaticamente "bindada" com este controller
        [BindProperty]
        public ServiceVM ServVM { get; set; }

        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            // Aqui, precisa mostrar um Service que possui dois DropDowns, uma para categoria e outro para frquencia.
            // Neste caso criamos uma ViewModel exclusiva pra isso
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Models.Service service = new Models.Service();

            if (id != null)
                service = _unitOfWork.ServiceRepository.Get(id.GetValueOrDefault());
            else
                service = new Models.Service();


            ServVM = new ServiceVM()
            {
                Service = service,
                CategoryList = _unitOfWork.CategoryRepository.GetCategoryListForDropDown(),
                FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyListForDropDown()
            };

            return View(ServVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // public IActionResult Upsert(ServiceVM servVM)
        public IActionResult Upsert() // Como usei [BindProperty] não preciso definir o ServVM como parametro aqui. Não gostei desta abordagem!
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (ServVM.Service.Id == 0)
                {
                    // new service
                    string fileName = Guid.NewGuid().ToString();
                    var uploadsFolder = Path.Combine(webRootPath, @"images\services");
                    // Aqui não precisa validar se o file[0] existe pois já fiz isso na VIew, porém, seria bom validar.
                    var fileExtension = Path.GetExtension(files[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploadsFolder, fileName + fileExtension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStreams);
                    }

                    // Atualiza a ImageUrl no objeto para salvar essa informação no banco
                    ServVM.Service.ImageUrl = @"\images\services\" + fileName + fileExtension;

                    _unitOfWork.ServiceRepository.Add(ServVM.Service);
                }
                else
                {
                    // Edit Service
                    var serviceFromDB = _unitOfWork.ServiceRepository.Get(ServVM.Service.Id);

                    // Se foi postado um arquivo 
                    if (files.Count > 0)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploadsFolder = Path.Combine(webRootPath, @"images\services");
                        // Aqui não precisa validar se o file[0] existe pois já fiz isso na VIew, porém, seria bom validar.
                        var fileExtension = Path.GetExtension(files[0].FileName);

                        // Se o arquivo existe no servidor, deleto ele porque estou atualizando.
                        var imagePath = Path.Combine(webRootPath, serviceFromDB.ImageUrl.TrimStart('\\')); // remove a primeira barra do path
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        // Upload do arquivo novo
                        using (var fileStreams = new FileStream(Path.Combine(uploadsFolder, fileName + fileExtension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStreams);
                        }

                        // Atualiza a ImageUrl no objeto para salvar essa informação no banco
                        ServVM.Service.ImageUrl = @"\images\services\" + fileName + fileExtension;
                    }
                    else
                    {
                        // Se não foi postado nennhum arquivo e esta editando um service, mantenho o ImageURL já existente
                        ServVM.Service.ImageUrl = serviceFromDB.ImageUrl;
                    }

                    _unitOfWork.ServiceRepository.Update(ServVM.Service);
                }

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ServVM.CategoryList = _unitOfWork.CategoryRepository.GetCategoryListForDropDown();
                ServVM.FrequencyList = _unitOfWork.FrequencyRepository.GetFrequencyListForDropDown();
                return View(ServVM);
            }
        }


        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var res = _unitOfWork.ServiceRepository.GetBy(inc => inc.Category, inc => inc.Frequency);
            return Json(new
            {
                data = res.ToList()
            });
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDB = _unitOfWork.ServiceRepository.Get(id);

            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, objFromDB.ImageUrl.TrimStart('\\')); // remove a primeira barra do path
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (objFromDB == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ServiceRepository.Remove(objFromDB);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete successful." });
        }

        #endregion
    }
}

