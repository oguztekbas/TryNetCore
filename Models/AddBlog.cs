using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TryNetCore.Models
{
    public class AddBlog
    {

        public string blogname { get; set; }

        public string blogcategoryname { get; set; }

        public string blogauthorname { get; set; }

        public IFormFile blogimage { get; set; }

        [AllowHtml]
        public string blogcontent { get; set; }

        public string blogtags { get; set; }

    }
}
