using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(IWalkerRepository walkerRepository, IWalkRepository walkRepo, INeighborhoodRepository neighborhoodRepo, IOwnerRepository ownerRepo)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepo;
            _neighborhoodRepo = neighborhoodRepo;
            _ownerRepo = ownerRepo;
        }

        // GET: WalkersController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();
            Owner owner = _ownerRepo.GetOwnerById(ownerId);
            List<Walker> walkers = new List<Walker>();

            if (owner != null)
            {
                //only show walkers in user's neighborhood
                walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);
            }
            else
            {
                walkers = _walkerRepo.GetAllWalkers();
            }
            
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
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(id != null)
            {
                return int.Parse(id);
            }
            else
            {
                //0 will result in a null when null checked
                return 0;
            }
        }

        //public ActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<ActionResult> Login(LoginViewModel viewModel)
        //{
        //    Walker walker = _walkerRepo.GetWalkerByEmail(viewModel.Email);
        //    Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

        //    if (walker != null)
        //    {
        //        List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, walker.Id.ToString()),
        //        new Claim(ClaimTypes.Email, walker.Email),
        //        new Claim(ClaimTypes.Role, "DogWalker"),
        //    };

        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(
        //            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity));

        //        return RedirectToAction("Index", "Walkers");
        //    }
        //    else if(owner != null)
        //    {
        //        List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        //        new Claim(ClaimTypes.Email, owner.Email),
        //        new Claim(ClaimTypes.Role, "DogOwner"),
        //    };

        //        ClaimsIdentity claimsIdentity = new ClaimsIdentity(
        //            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity));

        //        return RedirectToAction("Index", "Dog");
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}
    }
}
