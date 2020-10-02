using System;
using System.Collections.Generic;

namespace TryNetCore.ORM.Entity
{
    public partial class BlogImages
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
