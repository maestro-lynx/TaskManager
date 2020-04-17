using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TaskManager.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TaskManager.WEB.Controllers" }
            );

            routes.MapRoute(
                name: "Search", 
                url:"Search/{query}/{startIndex}",
                defaults: new
                        {
                            controller = "Account",
                            action = "Search",
                            query = "",
                            startIndex = 0,
                            pageSize = 9
                        });;

            routes.MapRoute(
                name: "Admin",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "TaskManager.WEB.Areas.Admin.Controllers" }
            );

        }
    }
}
