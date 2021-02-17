using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private HomeViewModel HomeVM;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            HomeVM = new HomeViewModel()
            {
                CategoryList = _unitOfWork.CategoryRepository.GetAll(),
                ServiceList = _unitOfWork.ServiceRepository.GetBy(x => x.Frequency)
            };

            return View(HomeVM);
        }

        public IActionResult Details(int id)
        {
            Service serviceFromDB = _unitOfWork.ServiceRepository.GetFirstOrDefault(filter => filter.Id == id,
                                                                                    include => include.Frequency,
                                                                                    include => include.Category);
            return View(serviceFromDB);
        }

        public IActionResult AddToCart(int serviceID)
        {
            List<int> sessionList = new List<int>();
            if (String.IsNullOrWhiteSpace(HttpContext.Session.GetString(Constants.SessionCart))) 
            {
                sessionList.Add(serviceID);
                HttpContext.Session.SetObject(Constants.SessionCart, sessionList);
            }
            else
            {
                sessionList = HttpContext.Session.GetObject<List<int>>(Constants.SessionCart);
                if (!sessionList.Contains(serviceID))
                {
                    sessionList.Add(serviceID);
                    HttpContext.Session.SetObject(Constants.SessionCart, sessionList);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
