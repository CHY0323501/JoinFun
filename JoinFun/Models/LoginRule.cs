using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoinFun
{
    public class LoginRule : ActionFilterAttribute
    {
        void LoginCheck(HttpContext context)
        {
            if (context.Session["admid"] == null)
                context.Response.Redirect("/Adm/Login");
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.GetRequiredString("controller").Equals("Adm", StringComparison.CurrentCultureIgnoreCase)
            && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            LoginCheck(HttpContext.Current);
        }

    }
}