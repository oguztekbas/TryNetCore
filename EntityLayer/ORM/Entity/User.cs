﻿using System;
using System.Collections.Generic;

namespace EntityLayer.ORM.Entity
{
    public partial class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
