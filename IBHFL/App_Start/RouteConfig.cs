using System.Web.Mvc;
using System.Web.Routing;

namespace IBHFL
{
  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
            routes.MapMvcAttributeRoutes();

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                          name: "newName",
                          url: "{name}",
                          defaults: new { controller = "Account", action = "LoginUrl", id = UrlParameter.Optional }
                        );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
  }
}
