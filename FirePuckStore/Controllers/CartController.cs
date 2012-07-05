using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.Controllers.Services.Implementation;
using FirePuckStore.Controllers.Services.Interfaces;

namespace FirePuckStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController():this(new CartService())
        {            
        }

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        //
        // GET: /Cart/
        [ChildActionOnly]
        public ActionResult Details()
        {
            return PartialView("PartialCart",_cartService.GetCart());
        }

    }
}
