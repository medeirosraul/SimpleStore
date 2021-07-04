using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Products;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class CatalogController : Controller
    {
        private readonly ICatalogItemProvider _catalogItemProvider;
        private readonly IPictureProvider _pictureProvider;

        public CatalogController(ICatalogItemProvider catalogItemProvider, IPictureProvider pictureProvider)
        {
            _catalogItemProvider = catalogItemProvider;
            _pictureProvider = pictureProvider;
        }

        public async Task<IActionResult> ProductDetails(string id)
        {
            var item = await _catalogItemProvider.GetProductDetails(id);

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
            //if (price != null)
            //{
            //    itemViewModel.Price = new CatalogItemPriceViewModel
            //    {
            //        Value = price.Value,
            //        OldValue = price.OldValue,
            //        ValueString = _priceProvider.GetPriceValueString(price),
            //        OldValueString = _priceProvider.GetPriceOldValueString(price)
            //    };
            //}

            // Pictures
            foreach (var picture in item.Pictures)
            {
                var pictureViewModel = new CatalogItemPictureViewModel
                {
                    Title = picture.Picture.Title,
                    Url = _pictureProvider.GetCatalogProductPictureUrl(picture.Picture, 1000),
                    ThumbUrl = _pictureProvider.GetCatalogProductPictureUrl(picture.Picture, 100)
                };

                itemViewModel.Pictures.Add(pictureViewModel);
            }

            itemViewModel.MainPicture = itemViewModel.Pictures.FirstOrDefault();

            return View(itemViewModel);
        }
    }
}