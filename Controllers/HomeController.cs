using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TryNetCore.Models;
using TryNetCore.ORM.Entity;

namespace TryNetCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment host;
        
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment _host)
        {
            _logger = logger;
             host = _host;
        }

        public IActionResult Index()
        {
            using (var db = new TryNetCoreContext())
            {
                //db.Tablo1.Add(new Tablo1() { Tablo1Name = "Ahmet" });
                //db.SaveChanges();
                ViewBag.Deneme = db.Tablo1.FirstOrDefault();
            }

            return View();

        }


        [HttpPost]
        public async Task<IActionResult> TryFileUpload(IFormFile file)
        {

           bool isSuccess = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, file, "12c73c34-5f66-451d-9eed-399ec3b28860.jpg");
           return RedirectToAction("Index", "Home");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
