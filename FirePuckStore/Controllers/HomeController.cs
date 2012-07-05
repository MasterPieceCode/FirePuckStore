﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.Controllers.Services.Implementation;
using FirePuckStore.Controllers.Services.Interfaces;

namespace FirePuckStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICardService _cardService;

        public HomeController() : this(new CardService())
        {
            
        }

        public HomeController(ICardService cardService)
        {
            _cardService = cardService;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View(_cardService.GetOrderedByLeagueWithMixedCards());
        }

    }
}
