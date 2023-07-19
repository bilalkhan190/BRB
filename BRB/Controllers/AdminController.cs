using BRB.Attributes;
using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
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


        public JsonResult SearchProductReport(DateTime? fromDate, DateTime? toDate, string? voucher, string? email)
        {
            AjaxResponse ajaxResponse = new AjaxResponse();
            try
            {

                SqlParameter[] sqlParameters = new SqlParameter[]
                {
                new SqlParameter("@FromDt",fromDate?? (object)DBNull.Value),
                new SqlParameter("@ToDt", toDate?? (object)DBNull.Value),
                new SqlParameter("@VoucherCode", voucher?? (object)DBNull.Value),
                new SqlParameter("@Domain", email?? (object)DBNull.Value)
                };

                var dt = SqlHelper.GetDataTable("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "sp_AdminGetProductReport_New", CommandType.StoredProcedure, sqlParameters);

                ajaxResponse.Success = true;
                ajaxResponse.Data = dt.SerializeObjectJson_();
            }
            catch (Exception ex)
            {
                ajaxResponse.Success = false;
                ajaxResponse.Message = ex.Message;
            }


            return Json(ajaxResponse);
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
