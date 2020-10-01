using System;
using System.Collections.Generic;

namespace TryNetCore.ORM.Entity
{
    public partial class Tablo1
    {
        public Tablo1()
        {
            Tablo2 = new HashSet<Tablo2>();
        }

        public int Id { get; set; }
        public string Tablo1Name { get; set; }

        public virtual ICollection<Tablo2> Tablo2 { get; set; }
    }
}
