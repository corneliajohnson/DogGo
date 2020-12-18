using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly IDogRepository _dogRepo;

        public WalksController(IWalkRepository walksRepo, IWalkerRepository walkerRepo, IDogRepository dogRepo)
        {
            _walkRepo = walksRepo;
            _walkerRepo = walkerRepo;
            _dogRepo = dogRepo;
        }

        // GET: WalksController
        public ActionResult Index()
        {
            return View();
        }

        // GET: WalksController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalksController/Create
        public ActionResult Create(int walkerId)
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepo.GetDogByOwnerId(ownerId);
            Walk walk = new Walk()
            {
                //add walker from anynomous object passed in Owner/Detail
                WalkerId = walkerId,
                Date = DateTime.Today
            };

            Walker walker = _walkerRepo.GetWalkerById(walkerId);
            WalksFormViewModel vm = new WalksFormViewModel()
            {
                Walker = walker,
                Walk = walk,
                Dogs = dogs
            };
            return View(vm);
        }

        // POST: WalksController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalksFormViewModel vm)
        {
            try
            {
                Walker walker = _walkerRepo.GetWalkerById(vm.Walk.WalkerId);
                vm.Walk.Walker = walker;
                vm.Walk.Duration = vm.Walk.Duration * 60;
                _walkRepo.AddWalk(vm.Walk);
                return RedirectToAction("Index", "Owner");

            }
            catch
            {
                return View(vm);
            }
        }

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
