using BusinessObjects.Helper;
using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace BRB.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Wh4lprodContext _dbContext;
        public BaseController()
        {
            _dbContext = new Wh4lprodContext();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName == "GetStateList")
            {

                base.OnActionExecuting(context);
            }
            else
            {
                if (context.HttpContext.Request.Headers["x-requested-with"] != "XMLHttpRequest")
                {
                    UserSessionData sessionData = null;
                    if (HttpContext.Session.GetString("_userData") != null)
                    {
                        sessionData = JsonConvert.DeserializeObject<UserSessionData>(HttpContext.Session.GetString("_userData"));
                        if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName != "VoucherVerification" &&
                            ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName != "VerifyVoucher" &&
                            string.IsNullOrEmpty(sessionData.VoucherCode))
                        {
                            context.Result = new RedirectResult(Url.Action("VoucherVerification", "Resume"));
                        }



                        var record = _dbContext.Resumes.FirstOrDefault(x => x.ResumeId == sessionData.ResumeId);
                        if (record != null)
                        {
                            //if (!string.IsNullOrEmpty(record.GeneratedFileName) && sessionData.UserType != "Admin")
                            //{
                            //    if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName != "Home")
                            //    {
                            //        context.Result = new RedirectResult(Url.Action("home", "Resume"));
                            //    }

                            //}

                        }
                        ViewBag.UserRecord = sessionData;
                    }
                    else
                    {
                        context.Result = new RedirectResult(Url.Action("Index", "Account"));
                    }
                }                         
                base.OnActionExecuting(context);
            }



        }

       
    }
}
