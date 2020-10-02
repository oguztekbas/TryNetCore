using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TryNetCore.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBlog()
        {

            return View();
        }

        public IActionResult UpdateBlog()
        {

            return View();
        }

        [HttpPost]
        public IActionResult UpdateBlog(int id)
        {

            return RedirectToAction("BlogIndex", "Admin");
        }

        public IActionResult BlogIndex()
        {

            return View();
        }

        public IActionResult Login()
        {


            return View();

        }

        public IActionResult Logout()
        {


            return RedirectToAction("Index", "Home");

        }
    }
}