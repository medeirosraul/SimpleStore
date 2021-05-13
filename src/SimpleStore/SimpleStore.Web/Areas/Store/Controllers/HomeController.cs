using Microsoft.AspNetCore.Mvc;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class HomeController : Controller
    {
        private readonly ICustomerContext _customerContext;

        public HomeController(ICustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}