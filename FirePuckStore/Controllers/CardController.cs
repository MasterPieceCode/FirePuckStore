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
            return View(_cardService.GetAllCards());
        }

        // GET: /Card/Create

        public ActionResult Create()
        {
            ViewBag.PlayerId = new SelectList(db.Players, "PlayerId", "FullName");
            return View();
        }

        //
        // POST: /Card/Create

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Card card)
        {
            if (ModelState.IsValid)
            {
                if (Request.Files.Count > 0)
                {
                    var logoFile = Request.Files[0];
                    if (logoFile != null)
                    {
                        var physicalPath = HttpContext.Server.MapPath("/Content");
                        var player = db.Players.Single(p => p.PlayerId == card.PlayerId);
                        var imageFileName = string.Format("{0}_{1}{2}", player.FullName, db.Cards.Count() + 1, Path.GetExtension(logoFile.FileName));
                        var imageUrl = Path.Combine(physicalPath, "images", imageFileName);
                        logoFile.SaveAs(imageUrl);
                        card.ImageUrl = string.Format("/{0}/{1}", "Content/Images", imageFileName);
                    }
                }

                db.Cards.Add(card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PlayerId = new SelectList(db.Players, "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // GET: /Card/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            ViewBag.PlayerId = new SelectList(db.Players, "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // POST: /Card/Edit/5

        [HttpPost]
        public ActionResult Edit(Card card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PlayerId = new SelectList(db.Players, "PlayerId", "FullName", card.PlayerId);
            return View(card);
        }

        //
        // POST: /Card/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _cardService.DeleteCard(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}