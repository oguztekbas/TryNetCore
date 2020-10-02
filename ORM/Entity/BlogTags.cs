using System;
using System.Collections.Generic;

namespace TryNetCore.ORM.Entity
{
    public partial class BlogTags
    {
        public int Id { get; set; }
        public string BlogTagName { get; set; }
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
