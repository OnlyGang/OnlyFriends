using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlyFriends
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Aceasta ruta este pentru GroupRequests ca sa poata lua GroupId ca al treilea argument
            routes.MapRoute(
                 name: "GroupRequest",
                 url: "GroupRequests/{action}/{GroupId}",
                 defaults: new { controller = "GroupRequests", action = "Index", GroupId = UrlParameter.Optional }
             );

            // Aceasta ruta este pentru GroupMembers ca sa poata lua GroupId ca al treilea argument
            routes.MapRoute(
                 name: "GroupMembers",
                 url: "GroupMembers/{action}/{GroupId}",
                 defaults: new { controller = "GroupMembers", action = "Index", GroupId = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
