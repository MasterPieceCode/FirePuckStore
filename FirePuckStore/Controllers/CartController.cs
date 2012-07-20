using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;

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

    }
}
