using BRB.Attributes;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            ajaxResponse.Message = "Unable to create voucher";
            var sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
            voucher.CreatedBy = sessionData.UserName;
            voucher.IsActive = true;
            voucher.GeneratedDate = DateTime.Today;
            _dbContext.Vouchers.Add(voucher);
          var IsAdded =  _dbContext.SaveChanges();
            if (IsAdded > 0)
            {
                ajaxResponse.Success = true;
                ajaxResponse.Message = "Voucher successfully Created";
            }
           
            return Json(ajaxResponse);
        }
    }
}
