using BusinessObjects.Models;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
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
                    ViewBag.UserRecord = sessionData;
                }
                else
                {
                    context.Result = new RedirectResult(Url.Action("Index", "Account"));
                }
                base.OnActionExecuting(context);
            }
        }
    }
}
