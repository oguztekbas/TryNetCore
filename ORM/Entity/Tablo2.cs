using System;
using System.Collections.Generic;

namespace TryNetCore.ORM.Entity
{
    public partial class Tablo2
    {
        public int Id { get; set; }
        public string Tablo2Name { get; set; }
        public int Tablo1Id { get; set; }

        public virtual Tablo1 Tablo1 { get; set; }
    }
}
