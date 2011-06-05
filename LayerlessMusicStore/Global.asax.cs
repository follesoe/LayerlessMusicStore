using System.Web.Mvc;
using System.Web.Routing;

namespace LayerlessMusicStore
{
    public class MvcApplication : LayerlessApp
    {
        protected override void Application_Start()
        {
            base.Application_Start();

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Save",
                "{controller}/Save/{item}",
                new { controller = "App", action = "Save" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}