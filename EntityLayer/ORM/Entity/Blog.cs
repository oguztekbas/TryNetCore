using System;
using System.Collections.Generic;

namespace EntityLayer.ORM.Entity
{
    public partial class Blog
    {
        public Blog()
        {
            BlogImages = new HashSet<BlogImages>();
            BlogTags = new HashSet<BlogTags>();
        }

        public int Id { get; set; }
        public string BlogName { get; set; }
        public string BlogRouteUrl { get; set; }
        public string BlogImagePath { get; set; }
        public string BlogAuthor { get; set; }
        public DateTime BlogPostDate { get; set; }
        public string BlogContent { get; set; }
        public string BlogCategoryName { get; set; }

        public virtual ICollection<BlogImages> BlogImages { get; set; }
        public virtual ICollection<BlogTags> BlogTags { get; set; }
    }
}
