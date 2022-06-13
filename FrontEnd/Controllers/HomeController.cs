using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontEnd.Models;

namespace FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Posts(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Albums(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Transactions()
        {
            return View();
        }
    }
}
