using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        //
        // GET: /Player/

        public ActionResult Index()
        {
            return View(_playerService.GetAll());
        }


        public ActionResult Create()
        {
            ViewBag.LeagueIdList = new SelectList(_playerService.GetLeagues(), "LeagueId", "Name");
            return View(new Player());
        }

        //
        // POST: /Player/Create

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Player player)
        {
            if (ModelState.IsValid)
            {
                _playerService.Add(player);

                return RedirectToAction("Index");
            }

            ViewBag.LeagueIdList = new SelectList(_playerService.GetLeagues(), "LeagueId", "Name");
            return View(player);
        }

        public ActionResult Edit(int id = 0)
        {
            var player = _playerService.GetById(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeagueIdList = new SelectList(_playerService.GetLeagues(), "LeagueId", "Name", player.LeagueId);
            return View(player);
        }

        //
        // POST: /Player/Edit/5

        [HttpPost]
        public ActionResult Edit(Player player)
        {
            if (ModelState.IsValid)
            {
                _playerService.Update(player);

                return RedirectToAction("Index");
            }
            ViewBag.LeagueIdList = new SelectList(_playerService.GetLeagues(), "LeagueId", "Name", player.LeagueId);
            return View(player);
        }


        // POST: /Player/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _playerService.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _playerService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
