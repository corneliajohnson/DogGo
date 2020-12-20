using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DogGo.Controllers
{
    public class AuthController : Controller
    {
        private readonly IOwnerRepository _ownerRepository;
        private IWalkerRepository _walkerRepository;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public AuthController(IConfiguration config)
        {
            _ownerRepository = new OwnerRepository(config);
            _walkerRepository = new WalkerRepository(config);;

        }

        public ActionResult Login()
        {
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepository.GetOwnerByEmail(viewModel.Email);
            Walker walker = _walkerRepository.GetWalkerByEmail(viewModel.Email);

            if (owner == null && walker == null)
            {
                return Unauthorized();
            }

            List<Claim> claims = new List<Claim>();
            if (owner != null)
            {
                claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
                new Claim(ClaimTypes.Email, owner.Email),
                new Claim(ClaimTypes.Role, "DogOwner"),
            };
            }

            if (walker != null)
            {
                claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, walker.Id.ToString()),
                new Claim(ClaimTypes.Email, walker.Email),
                new Claim(ClaimTypes.Role, "DogWalker"),
            };
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
    }
}
