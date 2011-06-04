using System;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace LayerlessMusicStore.Controllers
{
    public class AppController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Delete(string id)
        {
            var item = MvcApplication.CurrentSession.Load<JObject>(id);
            MvcApplication.CurrentSession.Delete(item);
            MvcApplication.CurrentSession.SaveChanges();
        }

        [HttpPost]
        public ActionResult Save(string item, [DynamicJson] JObject model)
        {
            MvcApplication.CurrentSession.Advanced.OnEntityConverted += (e, d, m) => {
                m["Raven-Entity-Name"] = item;
                m.Remove("Raven-Clr-Type");                
            };

            if (model["Id"] == null) model["Id"] = Guid.NewGuid().ToString();
            MvcApplication.CurrentSession.Store(model);
            MvcApplication.CurrentSession.SaveChanges();
            return new JsonResult();
        }
    }
}