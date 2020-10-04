using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TryNetCore.Models
{
    public class ContactForm
    {

        public  string name { get; set; }

        public string subject { get; set; }

        public string email { get; set; }
        public string message { get; set; }

    }
}
