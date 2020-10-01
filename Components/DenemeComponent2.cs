using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace TryNetCore.Components
{
    public class DenemeComponent2 : ViewComponent
    {

        public IViewComponentResult Invoke()
        {

            return View("DenemeComponent2View");

        }

    }
}
