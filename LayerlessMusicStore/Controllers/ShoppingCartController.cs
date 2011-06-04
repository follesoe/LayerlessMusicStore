using System;
using System.Web.Mvc;
using LayerlessMusicStore.Models;

namespace LayerlessMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        public const string CartSessionKey = "CartId";

        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            var chart = GetShoppingChart();            
            chart.AddToChart(MvcApplication.CurrentSession.Load<Album>(id));
            return new JsonResult { Data = chart };
        }

        [HttpPost]
        public JsonResult RemoveFromCart(string id)
        {
            var chart = GetShoppingChart();
            chart.RemoveFromChart(id);
            return new JsonResult { Data = chart };
        }

        private ShoppingCart GetShoppingChart()
        {
            var chartId = GetCartId();
            var chart = MvcApplication.CurrentSession.Load<ShoppingCart>(chartId);
            if (chart == null)
            {
                chart = new ShoppingCart {Id = chartId};
                MvcApplication.CurrentSession.Store(chart);
            }
            return chart;
        }

        private string GetCartId()
        {
            if (HttpContext.Session[CartSessionKey] == null)
            {
                Guid tempCartId = Guid.NewGuid();
                HttpContext.Session[CartSessionKey] = tempCartId.ToString();
            }
            return HttpContext.Session[CartSessionKey].ToString();
        }
    }
}