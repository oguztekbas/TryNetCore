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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TryNetCore.Filter;
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

        [ServiceFilter(typeof(AdminLoginFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [ServiceFilter(typeof(AdminLoginFilter))]
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

        [ServiceFilter(typeof(AdminLoginFilter))]
        public IActionResult UpdateBlog(int blogid)
        {
            try
            {
                using(var db = new TryNetCoreContext() )
                {

                    var blog = db.Blog.Where(i => i.Id == blogid)
                        .Include(i => i.BlogTags)                       
                        .FirstOrDefault();

                    return View(blog);
                }

                
            }
            catch (Exception e)
            {

                return RedirectToAction("BlogIndex", "Admin");
            }

            
        }

        [ServiceFilter(typeof(AdminLoginFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBlog(AddBlog blog ,int blogid)
        {

            var result = new SuccessResult();

            try
            {
                using (var db = new TryNetCoreContext())
                {
                    var updateblog = db.Blog.Where(i => i.Id == blogid)
                        .Include(i => i.BlogTags)
                        .Include(i => i.BlogImages)
                        .FirstOrDefault();

                    updateblog.BlogName = blog.blogname;
                    updateblog.BlogAuthor = blog.blogauthorname;
                    updateblog.BlogCategoryName = blog.blogcategoryname;
                  
                    var blogcount = db.Blog.Count();
                    updateblog.BlogRouteUrl = FriendlyUrl.FriendlyUrlMethod(blog.blogname + "-" + blogcount);

                    if(blog.blogimage != null && blog.blogimage.Length > 0)
                    {
                        updateblog.BlogImagePath = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, blog.blogimage, updateblog.BlogImagePath);
                    }

                    db.BlogTags.RemoveRange(updateblog.BlogTags);

                    var blogtags = blog.blogtags.Split(',');
                    for (int i = 0; i < blogtags.Length; i++)
                    {
                        updateblog.BlogTags.Add(new BlogTags() { BlogTagName = blogtags[i] });
                    }

                    // ------------------------- //
                    // BLOG CONTENT AGİLİTY PACK //

                    HtmlDocument deletedoc = new HtmlDocument();
                    deletedoc.LoadHtml(updateblog.BlogContent);

                    var deletehtmlimages = deletedoc.DocumentNode.SelectNodes("//img");
                    if(deletehtmlimages != null)
                    {
                        foreach (var updateblogimage in updateblog.BlogImages)
                        {
                            System.IO.File.Delete(Path.Combine(host.ContentRootPath, "wwwroot", updateblogimage.ImagePath.Remove(0, 1)));
                        }

                        
                    }

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(blog.blogcontent);


                    var htmlimages = doc.DocumentNode.SelectNodes("//img");

                    if (htmlimages != null)
                    {
                        foreach (var htmlimage in htmlimages)
                        {

                            var srcattribute = htmlimage.GetAttributeValue("src", "defaultvalue");
                            var parseattributearray = srcattribute.Split(',');
                            var parsebase64 = parseattributearray[1];

                            byte[] imageBytes = Convert.FromBase64String(parsebase64);

                            using (var ms = new MemoryStream())
                            {
                                ms.Write(imageBytes, 0, imageBytes.Length);
                                IFormFile file = new FormFile(ms, 0, imageBytes.Length, blog.blogimage.Name, blog.blogimage.FileName);

                                var blogimagepath = await TryNetCore.Utils.FileUpload.ImageUpload(host.ContentRootPath, file, null);
                                updateblog.BlogImages.Add(new BlogImages() { ImagePath = blogimagepath });

                                var updatesrcattribute = blogimagepath;
                                htmlimage.SetAttributeValue("src", updatesrcattribute);


                            }
                        }
                    }


                    updateblog.BlogContent = doc.DocumentNode.InnerHtml;
                    
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

        [ServiceFilter(typeof(AdminLoginFilter))]
        public IActionResult BlogIndex()
        {
            try
            {
                using (var db = new TryNetCoreContext())
                {

                    var blogs = db.Blog.ToList();
                    return View(blogs);
                }

                
            }
            catch(Exception e)
            {
                return View("Error");
            }
         
        }

        [ServiceFilter(typeof(AdminLoginFilter))]
        public async Task<IActionResult> RemoveBlog(int blogid) 
        {
            
            

            try
            {
                using(var db = new TryNetCoreContext()) 
                {
                   
                    var blog = db.Blog.Where(i => i.Id == blogid).Include(i => i.BlogImages).FirstOrDefault();
               
                    System.IO.File.Delete(Path.Combine(host.ContentRootPath, "wwwroot", blog.BlogImagePath.Remove(0, 1)));

                    foreach (var blogimage in blog.BlogImages)
                    {
                        System.IO.File.Delete(Path.Combine(host.ContentRootPath, "wwwroot", blogimage.ImagePath.Remove(0,1)));
                    }

                    db.Blog.Remove(blog);
                    await db.SaveChangesAsync();
                    return RedirectToAction("BlogIndex", "Admin");
                }
                
            }
            catch(Exception e)
            {
                var a = e.Message;
                return RedirectToAction("BlogIndex", "Admin");
            }

            
        }

        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(string username , string password)
        {
            if (HttpContext.Session.Get("Admin") != null)
            { return RedirectToAction("Index", "Admin"); }
            else
            {
                using (var db = new TryNetCoreContext())
                {
                    User user = await db.User.Where(i => i.Username == username && i.Password == password).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        HttpContext.Session.SetString("Admin", user.Username);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        TempData["Hata"] = "Kullanıcı Adı veya Parola Yanlış";
                        return View();
                    }
                }
            }

        }

        public IActionResult Logout()
        {

            HttpContext.Session.Remove("Admin");

            return RedirectToAction("Index", "Home");

        }
    }
}