using BRB.Attributes;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc.Ajax;

namespace BRB.Controllers
{
    [AuthFilter("Admin")]
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductReport()
        {
            return View("ProductReport");
        }

        public IActionResult Voucher()
        {
            return View("Voucher");
        }

        [HttpPost]

        public IActionResult CreateVoucher(Voucher voucher)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            ajaxResponse.Success = false;
            ajaxResponse.Data = null;
            ajaxResponse.Message = "Voucher successfully Created";
            
            return Json("Voucher");
        }
    }
}
