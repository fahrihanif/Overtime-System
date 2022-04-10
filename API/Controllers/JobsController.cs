using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    //[Authorize(Roles = "Finance")]
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : BaseController<Job, JobRepository, int>
    {
        public JobsController(JobRepository repository) : base(repository)
        {
        }
    }
}
