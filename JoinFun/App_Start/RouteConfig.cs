using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JoinFun
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //前台查看公告
            routes.MapRoute(
               name: "MemberPost",
               url: "Post/{PostNo}",
               defaults: new { controller = "Info", action = "Post", PostNo = UrlParameter.Optional }
           );
            //前台會員資訊
            routes.MapRoute(
               name: "MemberInfo",
               url: "Info/{memID}",
               defaults: new { controller = "Member", action = "Info" }
           );
            //前台編輯會員資訊
            routes.MapRoute(
               name: "MemberEdit",
               url: "Edit/{memID}",
               defaults: new { controller = "Member", action = "Edit" }
           );
            routes.MapRoute(
               name: "widget",
               url: "Widget",
               defaults: new { controller = "Info", action = "Widget" }
           );
            //啟動自訂路由
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Activity", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
