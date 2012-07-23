using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public ActionResult Index()
        {
            return new EmptyResult();
        }
        //
        // GET: /Cart/
        [ChildActionOnly]
        public ActionResult Details()
        {
            return PartialView("_Cart",_cartService.GetCart());
        }

        [HttpPost]
        public JsonResult Add(CartInputModel cartInputModel)
        {
            return Json(new {Quantity = 5, Price = 25, Rest = 1});
        }
    }
}
