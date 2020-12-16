﻿using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DogGo.Controllers
{
    [Authorize]
    public class DogController : Controller
    {
        private readonly IDogRepository _dogRepository;
        private readonly IOwnerRepository _ownerRepository;

        public DogController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepository = dogRepository;
            _ownerRepository = ownerRepository;
        }

        // GET: DogController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            List<Dog> dogs = _dogRepository.GetDogByOwnerId(ownerId);
            return View(dogs);
        }

        // GET: DogController/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }
        // GET: DogController/Create
        public ActionResult Create()
        {
            Dog dog = new Dog();
            return View(dog);
        }

        // POST: DogController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id 
                dog.OwnerId = GetCurrentUserId();

                _dogRepository.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogController/Edit/5
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            List<Owner> owners = _ownerRepository.GetAllOwners();

            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = dog,
                Owners = owners
            };

            if (dog == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: DogController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, DogFormViewModel vm)
        {
            try
            {
                _dogRepository.UpdateDog(vm.Dog);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(vm);
            }
        }

        // GET: DogController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepository.GetDogById(id);
            if (dog == null)
            {
                return NotFound();
            }
            return View(dog);
        }

        // POST: DogController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepository.DeleteDog(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(dog);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
