using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Models;
using FirePuckStore.DAL;

namespace FirePuckStore.Controllers
{
    public class CardController : Controller
    {
        private readonly ICardService _cardService;    
        private PuckStoreDbContext db = new PuckStoreDbContext();
 
        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        //
        // GET: /Card/

        public ActionResult Index()
        {
            return View(_cardService.GetAll());
        }

        // GET: /Card/Create

        public ActionResult Create()
        {
            ViewBag.PlayerIdList = new SelectList(db.Players, "PlayerId", "FullName");
            return View(new Card());
        }

        //
        // POST: /Card/Create

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Card card)
        {            
            if (ModelState.IsValid)
            {
                _cardService.Add(card);

                return RedirectToAction("Index");
            }

            ViewBag.PlayerIdList = new SelectList(db.Players, "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // GET: /Card/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var card = _cardService.GetById(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlayerIdList = new SelectList(db.Players.AsNoTracking(), "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // POST: /Card/Edit/5

        [HttpPost]
        public ActionResult Edit(Card card)
        {
            if (ModelState.IsValid)
            {
                _cardService.Update(card);

                return RedirectToAction("Index");
            }
            ViewBag.PlayerIdList = new SelectList(db.Players.AsNoTracking(), "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // POST: /Card/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _cardService.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _cardService.Dispose();
            base.Dispose(disposing);
        }
    }
}