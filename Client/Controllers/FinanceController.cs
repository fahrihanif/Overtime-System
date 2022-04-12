using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OvertimeManagement()
        {
            return View();
        }
        public IActionResult EmployeeManagement()
        {
            return View();
        } 
        public IActionResult ProfileUser()
        {
            return View();
        }
    }
}
