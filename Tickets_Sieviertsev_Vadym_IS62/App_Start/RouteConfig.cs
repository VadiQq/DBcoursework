
using System.Web.Mvc;
using System.Web.Routing;

namespace Tickets_Sieviertsev_Vadym_IS62
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Cashier", action = "RoutesList", id = UrlParameter.Optional }
            );
        }
    }
}
