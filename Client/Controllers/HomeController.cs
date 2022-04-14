using API.Models;
using API.ViewModels;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class HomeController : BaseController<Account, HomeRepository, int>
    {
        private readonly HomeRepository _repository;
        public HomeController(HomeRepository repository) : base(repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpGet("Unauthorized/")]
        public IActionResult Unauthorized()
        {
            return View("401");
        }

        [HttpGet("Forbidden/")]
        public IActionResult Forbidden()
        {
            return View("403");
        }

        [HttpGet("Notfound/")]
        public IActionResult Notfound()
        {
            return View("404");
        }

        [HttpPost]
        public async Task<IActionResult> Auth(LoginVM login)
        {
            var jwtToken = await _repository.Auth(login);
            var token = jwtToken.IdToken;

            if (token == null)
            {
                return RedirectToAction("index");
            }

            HttpContext.Session.SetString("JWToken", token);
            //HttpContext.Session.SetString("Name", jwtHandler.GetName(token));
            //HttpContext.Session.SetString("ProfilePicture", "assets/img/theme/user.png");

            return RedirectToAction("index", "admin");
        }
    }
}
