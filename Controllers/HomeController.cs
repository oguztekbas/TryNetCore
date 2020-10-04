using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using EntityLayer.ORM.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                using(var db = new TryNetCoreContext())
                {
                    var blogs = await db.Blog
                        .Include(i => i.BlogImages)
                        .Include(i => i.BlogTags)
                        .ToListAsync();

                    return View(blogs);
                }    
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        
    
        [Route("bloglar/{blogroute}")]
        
        public IActionResult BlogSingle(string blogroute)
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
            
        public async Task<JsonResult> ContactForm(ContactForm form)
        {

            var result = new SuccessResult();
            try
            {
                var body = new StringBuilder();
                body.AppendLine("Ad Soyad: " + form.name + "<br />");
                body.AppendLine("Email: " + form.email + "<br /> <br />");
               
                body.AppendLine("Mesaj: " + form.message + "<br />");

                await TryNetCore.Utils.SendMail.emailSend("no-reply@oguztekbas.xyz", "info@oguztekbas.xyz", form.name, form.subject, body.ToString(), "Tenekeci55..55*");
                result.isSuccess = true;
                result.Message = "Başarılı";
                return Json(result);
            }
            catch(Exception e)
            {

                result.isSuccess = false;
                result.Message = e.Message;
                return Json(result);
               
            }

            
        
        }

        [HttpPost]
        public async Task<IActionResult> TryFileUpload(IFormFile file)
        {

           string isSuccess = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, file, "12c73c34-5f66-451d-9eed-399ec3b28860.jpg");
           return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Privacy()
        {
            try
            {
                await TryNetCore.Utils.SendMail.emailSend("no-reply@oguztekbas.xyz", "info@oguztekbas.xyz", "Oğuz", "Oğuz İletişim Mesajı", "İletişim Mesaj İçeriği", "Tenekeci55..55*");
            }
          
            catch(Exception e)
            {

                var a = e.Message;
                return RedirectToAction("Index", "Home");
            }
            using (var db = new TryNetCoreContext())
            {
                


                var model = await db.Blog.FirstOrDefaultAsync();
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


