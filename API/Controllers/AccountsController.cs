using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository _repository;

        private IConfiguration _configuration;
        public AccountsController(AccountRepository _repository, IConfiguration _configuration) : base(_repository)
        {
            this._repository = _repository;
            this._configuration = _configuration;
        }

        [AllowAnonymous]
        [HttpPut("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM change)
        {
            try
            {
                var put = _repository.ChangePassord(change);
                return put switch
                {
                    0 => NotFound(new { message = "Account not found!" }),
                    1 => NotFound(new { message = "OTP incorrect!" }),
                    2 => NotFound(new { message = "OTP already used!" }),
                    3 => NotFound(new { message = "Password does not match" }),
                    _ => Ok(new { message = "Password changed successfully" })
                };
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [AllowAnonymous]
        [HttpPut("ForgotPassword")]
        [Route("")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var post = _repository.ForgotPassword(email);
                return post == 0
                    ? NotFound(new { message = "Account Not Found!" })
                    : (ActionResult)Ok(new { message = "A secret code is sent to your email, Please check your email!" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult Login(LoginVM login)
        {
            try
            {
                var get = _repository.Login(login);
                switch (get)
                {
                    case 0:
                        return NotFound(new { message = "Account Not Found!" });
                    case 1:
                        return NotFound(new { message = "Password Incorrect" });
                    default:
                        var role = _repository.UserRole(login.Email);

                        var claims = new List<Claim>()
                        {
                            new Claim("email", login.Email),
                            new Claim("nik", _repository.userNIK(login.Email))
                        };

                        foreach (var i in role)
                        {
                            claims.Add(new Claim("role", i));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn
                            );

                        var idToken = new JwtSecurityTokenHandler().WriteToken(token);
                        claims.Add(new Claim("Token Security", idToken.ToString()));

                        return Ok(new {idToken, message = "Login Berhasil" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        //[Authorize(Roles = "Finance")]
        [AllowAnonymous]
        [HttpGet("Master")]
        public ActionResult GetAllMaster()
        {
            try
            {
                var get = _repository.MasterEmployeeData();
                return get == null
                    ? NotFound(new { message = "Data Empty" })
                    : (ActionResult)Ok(_repository.MasterEmployeeData());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult Register(RegisterVM register)
        {
            try
            {
                var post = _repository.Register(register);
                return post == 0
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(new { message = "Data Saved Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
