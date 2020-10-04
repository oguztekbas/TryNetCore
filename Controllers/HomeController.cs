using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EntityLayer.ORM.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TryNetCore.Models;


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

           

            return View();


        }

        public IActionResult BlogSingle()
        {
            

            return View();

        }


        


        //[HttpPost]
        //public IActionResult Post()
        //{
        //    var response = Request["g-recaptcha-response"];
        //    const string secret = "6LeKKSMUAAAAAC4s-mflMky8XggtaatxKcx-cQ1y";
        //    //Kendi Secret keyinizle değiştirin.

        //    var client = new WebClient();
        //    var reply =
        //        client.DownloadString(
        //            string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

        //    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

        //    if (!captchaResponse.Success)
        //        TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
        //    else
        //        TempData["Message"] = "Güvenlik başarıyla doğrulanmıştır.";
        //    return RedirectToAction("Index");
        //}

            [HttpPost]
            
        public async Task<IActionResult> ContactForm(ContactForm form)
        {

            await TryNetCore.Utils.SendMail.emailSend("no-reply@oguztekbas.site", "info@oguztekbas.site", "Oğuz", "Oğuz İletişim Mesajı", "İletişim Mesaj İçeriği", "oguz*1234");
            return RedirectToAction("Index","Home");
        
        }

        [HttpPost]
        public async Task<IActionResult> TryFileUpload(IFormFile file)
        {

           string isSuccess = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, file, "12c73c34-5f66-451d-9eed-399ec3b28860.jpg");
           return RedirectToAction("Index", "Home");

        }

        public IActionResult Privacy()
        {

            using (var db = new TryNetCoreContext())
            {

                var model = db.Blog.FirstOrDefault();
                return View(model);
            }
            
        }

        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


