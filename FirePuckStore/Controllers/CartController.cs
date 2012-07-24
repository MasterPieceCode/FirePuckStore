using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Models;
using System.Web.Mvc.Html
;


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
        public ActionResult Add(CartInputModel cartInputModel)
        {
            try
            {
                _cartService.Place1QuantityOrderAndReturnSummaryOrderForCard(cartInputModel.CardId);                
            }
            catch (InvalidOperationException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, e.Message);
            }
            
            return PartialView("_Cart", _cartService.GetCart());
        }

        [HttpPost]
        public ActionResult Delete(CartInputModel cartInputModel)
        {
            try
            {
                _cartService.UnPlace1QuantityOrderAndReturnSummaryOrderForCard(cartInputModel.CardId);
            }
            catch(InvalidOperationException e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, e.Message);
            }

            return PartialView("_Cart", _cartService.GetCart());
        }

        protected override void Dispose(bool disposing)
        {
            _cartService.Dispose();
            base.Dispose(disposing);
        }
    }
}
