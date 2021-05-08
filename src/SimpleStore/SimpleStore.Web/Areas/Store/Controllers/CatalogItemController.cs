using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Prices;
using SimpleStore.Core.Services.Products;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class CatalogItemController : Controller
    {
        private readonly ICatalogItemProvider _catalogItemProvider;
        private readonly IPriceProvider _priceProvider;
        private readonly IPictureProvider _pictureProvider;

        public CatalogItemController(ICatalogItemProvider catalogItemProvider, IPriceProvider priceProvider, IPictureProvider pictureProvider)
        {
            _catalogItemProvider = catalogItemProvider;
            _priceProvider = priceProvider;
            _pictureProvider = pictureProvider;
        }

        public async Task<IActionResult> Details(string id)
        {
            var item = await _catalogItemProvider.GetCatalogItemDetails(id);

            if (item == null)
                return NotFound();

            var itemViewModel = new CatalogItemDetailsViewModel
            {
                Id = item.Id,
                Name = item.Name,
                ShortDescription = item.ShortDescription,
                FullDescription = item.FullDescription
            };

            // Price
            var price = await _priceProvider.GetProductPrice(item);

            if (price != null)
            {
                itemViewModel.Price = new CatalogItemPriceViewModel
                {
                    Value = price.Value,
                    OldValue = price.OldValue,
                    ValueString = _priceProvider.GetPriceValueString(price),
                    OldValueString = _priceProvider.GetPriceOldValueString(price)
                };
            }

            // Pictures
            foreach (var picture in item.Pictures)
            {
                var pictureViewModel = new CatalogItemPictureViewModel
                {
                    Title = picture.Picture.Title,
                    Url = _pictureProvider.GetProductPictureUrl(picture.Picture, 1000)
                };

                itemViewModel.Pictures.Add(pictureViewModel);
            }

            return View(itemViewModel);
        }
    }
}