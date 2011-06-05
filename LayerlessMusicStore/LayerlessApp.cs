using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace LayerlessMusicStore
{
    public abstract class LayerlessApp : HttpApplication
    {
        private const string RavenSessionKey = "Raven.Session";
        private static DocumentStore _documentStore;

        public static IDocumentSession CurrentSession
        {
            get { return (IDocumentSession)HttpContext.Current.Items[RavenSessionKey]; }
        }

        protected LayerlessApp()
        {
            BeginRequest += Application_BeginRequest;
            EndRequest += Application_EndRequest;
        }

        protected virtual void Application_Start()
        {
            _documentStore = new DocumentStore { Url = "http://localhost:8081" };
            _documentStore.Initialize();
            _documentStore.DatabaseCommands.EnsureDatabaseExists("MusicStore");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items[RavenSessionKey] = _documentStore.OpenSession("MusicStore");
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var disposable = HttpContext.Current.Items[RavenSessionKey] as IDisposable;
            if (disposable != null) disposable.Dispose();    
        }
    }

    public abstract class LayerlessController : Controller
    {
        [HttpPost]
        public void Delete(string id)
        {
            var item = LayerlessApp.CurrentSession.Load<JObject>(id);
            LayerlessApp.CurrentSession.Delete(item);
            LayerlessApp.CurrentSession.SaveChanges();
        }

        [HttpPost]
        public ActionResult Save(string item, [DynamicJson] JObject model)
        {
            LayerlessApp.CurrentSession.Advanced.OnEntityConverted += (e, d, m) =>
            {
                m["Raven-Entity-Name"] = item;
                m.Remove("Raven-Clr-Type");
            };

            if (model["Id"] == null) model["Id"] = Guid.NewGuid().ToString();
            LayerlessApp.CurrentSession.Store(model);
            LayerlessApp.CurrentSession.SaveChanges();
            return new JsonResult();
        }
    }

    public class DynamicJsonBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var inputStream = controllerContext.HttpContext.Request.InputStream;
            inputStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            string bodyText = reader.ReadToEnd();
            reader.Close();

            return String.IsNullOrEmpty(bodyText) ? null : JsonConvert.DeserializeObject(bodyText);
        }
    }

    public class DynamicJsonAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new DynamicJsonBinder();
        }
    }
}