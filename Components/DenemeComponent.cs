using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TryNetCore.Components
{
    public class DenemeComponent : ViewComponent
    {


        public string Invoke() 
        {
            return "DenemeComponent";
        }

    }
}
