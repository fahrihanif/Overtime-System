using API.Models;
using API.ViewModels;
using Client.Base;
using Client.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [AllowAnonymous]
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
            var claim = ExtractClaims(token);
            var role = claim.Where(claim => claim.Type == "role").Select(s => s.Value).ToList();
            var nik = claim.Where(claim => claim.Type == "nik").Select(s => s.Value).Single();

            if (token == null)
            {
                return RedirectToAction("index");
            }

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("nik", nik);

            

            if (role.Contains("Finance"))
            {
                return RedirectToAction("index", "finance");
            } 
            else if (role.Contains("Manager"))
            {
                return RedirectToAction("index", "manager");
            }
            else
            {
                return RedirectToAction("index", "employee");
            }
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }
    }
}
