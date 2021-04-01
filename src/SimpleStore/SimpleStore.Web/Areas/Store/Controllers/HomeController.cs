using Microsoft.AspNetCore.Mvc;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}