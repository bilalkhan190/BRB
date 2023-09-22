using System.Web.Mvc;

namespace BRB.Attributes
{
    public class RedirectionFilter: ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    if (!IsAuthenticRequest())
        //    {
        //        if (cond1 == true)
        //            filterContext.Result = new RedirectToRouteResult(
        //               new RouteValueDictionary { { "controller", "Error" }, { "action", "404NotFound" } });
               
        //    }
        //    base.OnActionExecuting(filterContext);  
        //}


        //public bool IsAuthenticRequest()
        //{

        //}
    }
}
