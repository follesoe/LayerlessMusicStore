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
        public ActionResult Save(string item, [DynamicJson] JObject model)
        {
            MvcApplication.CurrentSession.Advanced.OnEntityConverted += (e, d, m) => {
                m["Raven-Entity-Name"] = item;
                m.Remove("Raven-Clr-Type");
            };
            MvcApplication.CurrentSession.Store(model);
            MvcApplication.CurrentSession.SaveChanges();
            return new JsonResult();
        }
    }
}