using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Prices;
using SimpleStore.Core.Services.Products;
using SimpleStore.Framework.Contexts;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.ViewComponents
{
    public class CatalogNewItemsViewComponent:ViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly ICatalogItemProvider _productProvider;
        private readonly IPriceProvider _priceProvider;
        private readonly IPictureProvider _pictureProvider;

        public CatalogNewItemsViewComponent(IStoreContext storeContext, ICatalogItemProvider productProvider, IPriceProvider priceProvider, IPictureProvider pictureProvider)
        {
            _storeContext = storeContext;
            _productProvider = productProvider;
            _priceProvider = priceProvider;
            _pictureProvider = pictureProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productProvider.GetNewCatalogItems(8);

            if (products == null || products.Count() == 0) return View(null);

            var result = new List<CatalogItemViewModel>();
            foreach (var p in products)
            {
                var price = await _priceProvider.GetCatalogItemPrice(p);

                var product = new CatalogItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    PriceValueString = _priceProvider.GetPriceValueString(price),
                    PriceOldValueString = _priceProvider.GetPriceOldValueString(price)
                };

                // Product Pictures
                if (p.Pictures != null && p.Pictures.Count > 0)
                {
                    product.Picture = _pictureProvider.GetCatalogItemPictureUrl(p.Pictures.FirstOrDefault()?.Picture, 400);
                }

                result.Add(product);
            }

            return View(result);
        }
    }
}