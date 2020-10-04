using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Web;

namespace TryNetCore.Filter
{
    public class AdminLoginFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
           var session = context.HttpContext.Session.Get("Admin");
           //var b =  context.HttpContext.Session.GetString("B");
          
            if (session == null)
            {
                context.Result = new RedirectResult("~/Admin/Login");
                return;
            }
            

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }

}
