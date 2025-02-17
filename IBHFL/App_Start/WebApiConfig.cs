using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;

namespace IBHFL
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //////// Enable CORS for all origins, headers, and methods.
            //////var cors = new EnableCorsAttribute("*", "*", "*");
            //////config.EnableCors(cors);
            config.MapHttpAttributeRoutes();

            ////config.Routes.MapHttpRoute(
            ////    name: "DefaultApi",
            ////    routeTemplate: "api/{controller}/{id}",
            ////    defaults: new { id = RouteParameter.Optional }
            ////);

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               ////////routeTemplate: "api/{controller}/{id}",
               routeTemplate: "api/{controller}/{actions}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
