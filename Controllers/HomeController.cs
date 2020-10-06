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
        
        public async Task<IActionResult> BlogSingle(string blogroute)
        {
            
            try
            {
                using(var db = new TryNetCoreContext())
                {

                    var blog = await db.Blog
                        .Where(i => i.BlogRouteUrl == blogroute)
                        .Include(i => i.BlogTags)
                        .Include(i => i.BlogImages)
                        .FirstOrDefaultAsync();

                    return View(blog);
                }
                
            }
            catch(Exception e)
            {

            }

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

                await TryNetCore.Utils.SendMail.emailSend("gondericiemail", "aliciemail", form.name, form.subject, body.ToString(), "Şifre");
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

        

        [ResponseCache(Duration = 1, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


