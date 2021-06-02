using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Serilog;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Framework.Contexts;
using System;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    [AllowAnonymous]
    public class PictureController : Controller
    {
        private IStoreContext _storeContext;
        private readonly IPictureProvider _pictureProvider;

        public PictureController(IStoreContext storeContext, IPictureProvider pictureProvider)
        {
            _storeContext = storeContext;
            _pictureProvider = pictureProvider;
        }

        [HttpGet("Picture/Product/{size}/{name}")]
        public async Task<IActionResult> Product(int size, string name)
        {
            try
            {
                var path = await _pictureProvider.GetPicturePath("Products", name, size);
                return PhysicalFile(path, "image/jpeg");
            }
            catch (NullReferenceException e)
            {
                Log.Logger.Error(e.Message + " {name}", name);
                return NotFound();
            }
        }

        [HttpGet("Picture/Site/{size}/{name}")]
        public async Task<IActionResult> Site(int size, string name)
        {
            try
            {
                var path = await _pictureProvider.GetPicturePath("Site", name, size);
                return PhysicalFile(path, "image/jpeg");
            }
            catch (NullReferenceException e)
            {
                Log.Logger.Error(e.Message + " {name}", name);
                return NotFound();
            }
        }
    }
}