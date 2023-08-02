using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BRB.Attributes
{
    public class GlobalFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ReturnType = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType.FullName;
            if (!ReturnType.Contains("JsonResult"))
            {
                context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.HttpContext.Response.Headers["Expires"] = "-1";
                context.HttpContext.Response.Headers["Pragma"] = "no-cache";
                //base.OnActionExecuting(context);
            }
        }

    }
}
