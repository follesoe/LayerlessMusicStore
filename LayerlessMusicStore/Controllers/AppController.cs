using System.Web.Mvc;

namespace LayerlessMusicStore.Controllers
{
    public class AppController : LayerlessController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}