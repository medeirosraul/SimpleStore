using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult StoreNotFound()
        {
            return View();
        }
    }
}
