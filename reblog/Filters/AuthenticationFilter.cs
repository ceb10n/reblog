using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace reblog.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AuthenticationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Admin", Action = "Login" }));
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}