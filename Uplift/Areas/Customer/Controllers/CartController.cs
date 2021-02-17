using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Extensions;
using Uplift.Models;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public CartViewModel CartVM { get; set; }


        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CartVM = new CartViewModel()
            {
                OrderHeader = new Models.OrderHeader(),
                ServiceList = new List<Service>()
            };

        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<int>>(Constants.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(Constants.SessionCart);

                foreach (var serviceID in sessionList)
                {
                    Service serviceToAdd = _unitOfWork.ServiceRepository.GetFirstOrDefault(filter => filter.Id == serviceID,
                                                                                           include => include.Frequency, include => include.Category);

                    CartVM.ServiceList.Add(serviceToAdd);
                }
            }

            return View(CartVM);
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.GetObject<List<int>>(Constants.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(Constants.SessionCart);

                foreach (var serviceID in sessionList)
                {
                    Service serviceToAdd = _unitOfWork.ServiceRepository.GetFirstOrDefault(filter => filter.Id == serviceID,
                                                                                           include => include.Frequency, include => include.Category);

                    CartVM.ServiceList.Add(serviceToAdd);
                }
            }

            return View(CartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            if (HttpContext.Session.GetObject<List<int>>(Constants.SessionCart) != null)
            {
                List<int> sessionList = new List<int>();
                sessionList = HttpContext.Session.GetObject<List<int>>(Constants.SessionCart);
                CartVM.ServiceList = new List<Service>();

                foreach (var serviceID in sessionList)
                {
                    Service serviceToAdd = _unitOfWork.ServiceRepository.Get(serviceID);

                    CartVM.ServiceList.Add(serviceToAdd);
                }
            }

            if (!ModelState.IsValid)
            {
                return View(CartVM);
            }
            else
            {
                CartVM.OrderHeader.OrderDate = DateTime.Now;
                CartVM.OrderHeader.Status = Constants.StatusSubmitted;
                CartVM.OrderHeader.ServiceCount = CartVM.ServiceList.Count;

                _unitOfWork.OrderHeaderRepository.Add(CartVM.OrderHeader);
                _unitOfWork.Save();

                foreach (var item in CartVM.ServiceList)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        ServiceId = item.Id,
                        OrderHeaderId = CartVM.OrderHeader.Id,
                        ServiceName = item.Name,
                        Price = item.Price
                    };
                    _unitOfWork.OrderDetailsRepository.Add(orderDetails);
                }
                _unitOfWork.Save();

                HttpContext.Session.SetObject(Constants.SessionCart, new List<int>());

                return RedirectToAction("OrderConfirmation", "Cart", new { id = CartVM.OrderHeader.Id });
            }
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }

        public IActionResult Remove(int serviceID)
        {
            List<int> sessionList = new List<int>();
            sessionList = HttpContext.Session.GetObject<List<int>>(Constants.SessionCart);

            sessionList.Remove(serviceID);

            HttpContext.Session.SetObject(Constants.SessionCart, sessionList);

            return RedirectToAction(nameof(Index));
        }


    }
}
