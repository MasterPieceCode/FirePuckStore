﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.Models;
using FirePuckStore.DAL;

namespace FirePuckStore.Controllers
{
    public class CardController : Controller
    {
        private PuckStoreDbContext db = new PuckStoreDbContext();

        //
        // GET: /Card/

        public ActionResult Index()
        {
            var cards = db.Cards.Include(c => c.Player);
            return View(cards.ToList());
        }

        //
        // GET: /Card/Details/5

        public ActionResult Details(int id = 0)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // GET: /Card/Create

        public ActionResult Create()
        {
            ViewBag.PlayerId = new SelectList(db.Players, "PlayerId", "FullName");
            return View();
        }

        //
        // POST: /Card/Create

        [HttpPost]
        public ActionResult Create(Card card)
        {
            if (ModelState.IsValid)
            {
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
        // GET: /Card/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        //
        // POST: /Card/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Card card = db.Cards.Find(id);
            db.Cards.Remove(card);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}