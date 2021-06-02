using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    public class ShippingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public Task<IActionResult> Calculate(string zipcode)
        {

        }
    }
}
