﻿using System;
using System.Web.Mvc;
using LayerlessMusicStore.Models;

namespace LayerlessMusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        public const string CartSessionKey = "CartId";
       
        [HttpGet]
        public JsonResult ViewCart()
        {
            return new JsonResult { Data = GetShoppingCart(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            var cart = GetShoppingCart();            
            cart.AddToChart(LayerlessApp.CurrentSession.Load<Album>(id));
            LayerlessApp.CurrentSession.Store(cart);
            LayerlessApp.CurrentSession.SaveChanges();
            return new JsonResult { Data = cart };
        }

        [HttpPost]
        public JsonResult RemoveFromCart(string id)
        {
            var cart = GetShoppingCart();
            cart.RemoveFromChart(id);
            LayerlessApp.CurrentSession.Store(cart);
            LayerlessApp.CurrentSession.SaveChanges();
            return new JsonResult { Data = cart };
        }

        private ShoppingCart GetShoppingCart()
        {
            var cartId = GetCartId();
            var cart = LayerlessApp.CurrentSession.Load<ShoppingCart>(cartId);
            if (cart == null)
            {
                cart = new ShoppingCart {Id = cartId};
            }
            return cart;
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