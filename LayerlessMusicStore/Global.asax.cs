using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace LayerlessMusicStore
{
    public class MvcApplication : HttpApplication
    {
        private const string RavenSessionKey = "Raven.Session";
        private static DocumentStore _documentStore;

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey]; }
        }

        public MvcApplication()
        {
            BeginRequest += (s, a) => HttpContext.Current.Items[RavenSessionKey] = _documentStore.OpenSession("MusicStore");
            EndRequest += (s, e) =>
            {
                var disposable = HttpContext.Current.Items[RavenSessionKey] as IDisposable;
                if (disposable != null) disposable.Dispose();
            };
        }

        protected void Application_Start()
        {
            _documentStore = new DocumentStore { Url = "http://localhost:8080" };
            _documentStore.Initialize();
            _documentStore.DatabaseCommands.EnsureDatabaseExists("MusicStore");

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

        protected void Application_BeginRequest()
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }
    }
}