using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalkRepository walkRepo, INeighborhoodRepository neighborhoodRepo)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepo;
            _neighborhoodRepo = neighborhoodRepo;
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            return View(walkers);
        }

        // GET: WalkersController/Details/5
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            List<Walk> walks = _walkRepo.GetWalksByWalkerId(walker.Id);

            WalkerProfileViewModel vm = new WalkerProfileViewModel
            {
                Walks = walks,
                Walker = walker,
            };
            vm.TotalWalkTime();

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            WalkerFormViewModel vm = new WalkerFormViewModel()
            {
                Walker = new Walker(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: WalkersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WalkerFormViewModel vm)
        {
            try
            {

                _walkerRepo.AddWalker(vm.Walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAll();

            WalkerFormViewModel vm = new WalkerFormViewModel()
            {
                Walker = walker,
                Neighborhoods = neighborhoods
            };

            if(walker == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: WalkersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, WalkerFormViewModel vm)
        {
            try
            {
                _walkerRepo.UpdateWalker(vm.Walker);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);

            if(walker == null)
            {
                return NotFound();
            }

            return View(walker);
        }

        // POST: WalkersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walker walker)
        {
            try
            {
                _walkerRepo.DeleteWalker(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(walker);
            }
        }
    }
}
