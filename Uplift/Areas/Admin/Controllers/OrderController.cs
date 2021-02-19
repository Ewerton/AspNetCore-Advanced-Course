using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Uplift.DataAccess.Data.Repository.IRepository;
using Uplift.Models.ViewModels;
using Uplift.Utility;

namespace Uplift.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderViewModel orderVM = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepository.Get(id),
                OrderDetails = _unitOfWork.OrderDetailsRepository.GetBy(filter: o => o.OrderHeaderId == id)
            };

            return View(orderVM);
        }

        public IActionResult Approve(int id)
        {
            var orderFromDB = _unitOfWork.OrderHeaderRepository.Get(id);
            if (orderFromDB == null)
                return NotFound();

            _unitOfWork.OrderHeaderRepository.ChangeOrderStatus(id, Constants.StatusApproved);
            
            return View(nameof(Index));
        }

        public IActionResult Reject(int id)
        {
            var orderFromDB = _unitOfWork.OrderHeaderRepository.Get(id);
            if (orderFromDB == null)
                return NotFound();

            _unitOfWork.OrderHeaderRepository.ChangeOrderStatus(id, Constants.StatusRejected);

            return View(nameof(Index));
        }

        #region "API Calls"

        public IActionResult GetAllOrders()
        {
            return Json(new 
            { 
                data = _unitOfWork.OrderHeaderRepository.GetAll() 
            });
        }

        public IActionResult GetAllPendingOrders()
        {
            return Json(new 
            { 
                data = _unitOfWork.OrderHeaderRepository.GetBy(filter:o => o.Status == Constants.StatusSubmitted) 
            });
        }

        public IActionResult GetAllApprovedOrders()
        {
            return Json(new 
            {
                data = _unitOfWork.OrderHeaderRepository.GetBy(filter:o => o.Status == Constants.StatusApproved)
            });
        }


        #endregion
    }
}
