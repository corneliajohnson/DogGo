using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IDogRepository _dogRepository;
        private IWalkerRepository _walkerRepository;
        private INeighborhoodRepository _neighborhoodRepository;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerController(IConfiguration config)
        {
            _ownerRepository = new OwnerRepository(config);
            _dogRepository = new DogRepository(config);
            _walkerRepository = new WalkerRepository(config);
            _neighborhoodRepository = new NeighborhoodRepository(config);

        }

        // GET: OwnerController
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepository.GetAllOwners();
            return View(owners);
        }

        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepository.GetOwnerById(id);
            List<Dog> dogs = _dogRepository.GetDogByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepository.GetWalkersInNeighborhood(owner.NeighborhoodId);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }

        // GET: OwnerController/Create
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: OwnerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepository.AddOwner(vm.Owner);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }

        }

        // GET: OwnerController/Edit/5
        public ActionResult Edit(int id)
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();
            Owner owner = _ownerRepository.GetOwnerById(id);

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Neighborhoods = neighborhoods,
                Owner = owner
            };

            if(owner == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        // POST: OwnerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OwnerFormViewModel vm)
        {
            try
            {
                _ownerRepository.UpdateOwner(vm.Owner);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: OwnerController/Delete/5
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepository.GetOwnerById(id);

            if(owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: OwnerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepository.DeleteOwner(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(owner);
            }
        }
    }
}
