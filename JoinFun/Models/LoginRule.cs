using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JoinFun
{
    public class LoginRule : ActionFilterAttribute
    {
        public bool Front { get; set; }
        public bool isVisiter { get; set; }
        public bool hasEmptyStr { get; set; }
        void AdmLoginCheck(HttpContext context)
        {
            if (context.Session["admid"] == null)
                context.Response.Redirect("/Adm/Login");
        }
        void UserLoginCheck(HttpContext context)
        {
            if (context.Session["memid"] == null)
                context.Response.Redirect("/Login/Login?c=1");
        }
        void UserHasEmptyStrLoginCheck(HttpContext context)
        {
            if (context.Session["memid"] != null)
                if(context.Session["memid"].ToString()=="")
                context.Response.Redirect("/Login/Login?c=1");
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Front)
            {
                if (!isVisiter) {
                    if (filterContext.RouteData.GetRequiredString("controller").Equals("Login", StringComparison.CurrentCultureIgnoreCase)
                     && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return;
                    }
                    else if (hasEmptyStr) {
                        UserHasEmptyStrLoginCheck(HttpContext.Current);
                    }
                    else {
                        UserLoginCheck(HttpContext.Current);
                    }

                    
                }
            }
            else {
                if (filterContext.RouteData.GetRequiredString("controller").Equals("Adm", StringComparison.CurrentCultureIgnoreCase)
                     && filterContext.RouteData.GetRequiredString("action").Equals("Login", StringComparison.CurrentCultureIgnoreCase))
                {
                    return;
                }

                AdmLoginCheck(HttpContext.Current);
            }

        }

    }
}