using BusinessObjects.Helper;
using BusinessObjects.Models.DTOs;
using BusinessObjects.Models.MetaData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace BRB.Attributes
{
    public class AuthFilter : Attribute, IActionFilter
    {

        private readonly string Role;
        public AuthFilter(string Role)
        {
            this.Role = Role;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var ReturnType = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReturnType.FullName;
            if (!ReturnType.Contains("JsonResult"))
            {
                context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.HttpContext.Response.Headers["Expires"] = "-1";
                context.HttpContext.Response.Headers["Pragma"] = "no-cache";

                if(context.HttpContext.Session.GetString("_userData") != null)
                {
                    var userRecord = JsonConvert.DeserializeObject<UserSessionData>(context.HttpContext.Session.GetString("_userData"));
                    if (userRecord.UserType != Role)
                    {
                        context.Result = new RedirectResult("/account/index");
                    }

                    var dtIsOptOut = SqlHelper.GetDataTable("Data Source=A2NWPLSK14SQL-v02.shr.prod.iad2.secureserver.net;Initial Catalog=WH4LProd;User Id=brbdbuser; Password=brb!!!***;;Encrypt=False;TrustServerCertificate=True", "SP_GetIsOptOut", CommandType.StoredProcedure, new SqlParameter("ResumeId", userRecord.ResumeId));
                    if (dtIsOptOut.Rows.Count > 0)
                    {
                        string IsOptOutJson = JsonConvert.SerializeObject(dtIsOptOut);                        
                    }
                }
               
            }


        }
    }
}
