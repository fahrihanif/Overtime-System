﻿using API.Base;
using API.Models;
using API.Repository.Data;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class OvertimesController : BaseController<Overtime, OvertimeRepository, int>
    {
        private readonly OvertimeRepository _repository;
        public OvertimesController(OvertimeRepository repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpGet("Detail/{id}/{date}")]
        public ActionResult GetById(string id, DateTime date)
        {
            try
            {
                var get = _repository.DetailOvertime(id, date);
                return get == null
                    ? NotFound(new { message = "Data Not Found" })
                    : (ActionResult)Ok(get);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpGet("ListFinance")]
        public ActionResult ListFinanceOvertime()
        {
            try
            {
                var post = _repository.ListFinanceOvertime();
                return post == null
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(_repository.ListFinanceOvertime());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("List")]
        public ActionResult ListOvertime()
        {
            try
            {
                var post = _repository.ListOvertime();
                return post == null
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(_repository.ListOvertime());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        [HttpGet("List/{id}")]
        public ActionResult ListOvertimeById(string id)
        {
            try
            {
                var post = _repository.ListOvertimeById(id);
                return post == null
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(_repository.ListOvertimeById(id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("Remaining/{id}")]
        public ActionResult RemainingOvertime(string id)
        {
            try
            {
                var post = _repository.RemainingOvertime(id);
                return post == null
                    ? NotFound(new { message = "Data Failed to Change Please Check Again" })
                    : (ActionResult)Ok(_repository.RemainingOvertime(id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("Request")]
        public ActionResult Request(AddOvertimeVM overtime)
        {
            try
            {
                var post = _repository.Request(overtime);
                return post switch
                {
                    0 => NotFound(new { message = "Request Failed Please Check Again" }),
                    1 => NotFound(new { message = "You Cannot Apply Overtime On The Same Date" }),
                    _ => (ActionResult)Ok(new { message = "Overtime Requested" })
                };
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
