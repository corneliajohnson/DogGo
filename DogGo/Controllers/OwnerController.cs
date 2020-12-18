using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using DogGo.Repositories.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IDogRepository _dogRepository;
        private IWalkerRepository _walkerRepository;
        private INeighborhoodRepository _neighborhoodRepository;
        private IWalkRepository _walkRepository;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerController(IConfiguration config)
        {
            _ownerRepository = new OwnerRepository(config);
            _dogRepository = new DogRepository(config);
            _walkerRepository = new WalkerRepository(config);
            _neighborhoodRepository = new NeighborhoodRepository(config);
            _walkRepository = new WalkRepository(config);

        }

        // GET: OwnerController
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            return RedirectToAction(nameof(Details), new { id = ownerId });
        }

        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            int currentUser = GetCurrentUserId();
            if (id != currentUser)
            {
                return NotFound();
            }

            Owner owner = _ownerRepository.GetOwnerById(id);
            List<Dog> dogs = _dogRepository.GetDogByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepository.GetWalkersInNeighborhood(owner.NeighborhoodId);
            List<Walk> walks = _walkRepository.GetWalksByOwnerId(currentUser);

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers,
                Walks = walks
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
            int currentUser = GetCurrentUserId();
            if (id != currentUser)
            {
                return NotFound();
            }

            List<Neighborhood> neighborhoods = _neighborhoodRepository.GetAll();
            Owner owner = _ownerRepository.GetOwnerById(id);

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Neighborhoods = neighborhoods,
                Owner = owner
            };

            if (owner == null)
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
            int currentUser = GetCurrentUserId();
            if (id != currentUser)
            {
                return NotFound();
            }

            Owner owner = _ownerRepository.GetOwnerById(id);

            if (owner == null)
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

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepository.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        new Claim(ClaimTypes.Email, owner.Email),
        new Claim(ClaimTypes.Role, "DogOwner"),
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dog");
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
