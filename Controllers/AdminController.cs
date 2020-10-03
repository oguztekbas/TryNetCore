using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using EntityLayer.ORM.Entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using TryNetCore.Models;
using TryNetCore.Utils;

namespace TryNetCore.Controllers
{
    public class AdminController : Controller
    {

        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment host;

        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment _host)
        {
            _logger = logger;
            host = _host;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBlog()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddBlog(AddBlog blog)
        {
            var result = new SuccessResult();

            try
            {
                using (var db = new TryNetCoreContext())
                {
                    var newblog = new Blog();

                    newblog.BlogName = blog.blogname;
                    newblog.BlogAuthor = blog.blogauthorname;
                    newblog.BlogCategoryName = blog.blogcategoryname;
                    newblog.BlogPostDate = DateTime.Now;
                    

                    var blogcount = db.Blog.Count();
                    newblog.BlogRouteUrl = FriendlyUrl.FriendlyUrlMethod(blog.blogname + "-" + blogcount);

                    newblog.BlogImagePath = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, blog.blogimage, null);

                    var blogtags = blog.blogtags.Split(',');
                    for (int i = 0; i < blogtags.Length; i++)
                    {
                        newblog.BlogTags.Add(new BlogTags() { BlogTagName = blogtags[i] });
                    }

                    // ------------------------- //
                    // BLOG CONTENT AGİLİTY PACK //


                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(blog.blogcontent);
                    

                    var htmlimages = doc.DocumentNode.SelectNodes("//img");
                    
                    if(htmlimages != null)
                    {
                        foreach (var htmlimage in htmlimages)
                        {

                           var srcattribute = htmlimage.GetAttributeValue("src", "defaultvalue");
                           var parseattributearray = srcattribute.Split(',');
                           var parsebase64 = parseattributearray[1];

                           byte[] imageBytes = Convert.FromBase64String(parsebase64);
                            
                           using(var ms = new MemoryStream()) 
                            {
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                IFormFile file = new FormFile(ms, 0, imageBytes.Length, blog.blogimage.Name, blog.blogimage.FileName);
                                
                                var blogimagepath = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, file, null);
                                newblog.BlogImages.Add(new BlogImages() { ImagePath = blogimagepath });

                                var updatesrcattribute = blogimagepath;
                                htmlimage.SetAttributeValue("src", updatesrcattribute);
                               

                            }
                        }
                    }


                    newblog.BlogContent = doc.DocumentNode.InnerHtml;
                    await db.Blog.AddAsync(newblog);
                    await db.SaveChangesAsync();
                  
                    


                    
                }

                result.isSuccess = true;
                result.Message = "Başarılı";
                return Json(result);

            }
            catch (Exception e)
            {


                result.isSuccess = false;
                result.Message = "Hata";
                return Json(result);
            }

            
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