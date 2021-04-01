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
    public class CatalogNewProductsViewComponent:ViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IProductProvider _productProvider;
        private readonly IPriceProvider _priceProvider;
        private readonly IPictureProvider _pictureProvider;

        public CatalogNewProductsViewComponent(IStoreContext storeContext, IProductProvider productProvider, IPriceProvider priceProvider, IPictureProvider pictureProvider)
        {
            _storeContext = storeContext;
            _productProvider = productProvider;
            _priceProvider = priceProvider;
            _pictureProvider = pictureProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var products = await _productProvider.GetNewProducts(8);

            if (products == null || products.Count() == 0) return View(null);

            var result = new List<CatalogProductViewModel>();
            foreach (var p in products)
            {
                var product = new CatalogProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                };

                // Product Price
                var price = await _priceProvider.GetProductPrice(p);
                if(price != null)
                {
                    product.Price = new CatalogProductPriceViewModel
                    {
                        ValueString = _priceProvider.GetPriceValueString(price),
                        OldValueString = _priceProvider.GetPriceOldValueString(price)
                    };
                }

                // Product Pictures
                if (p.Pictures != null && p.Pictures.Count > 0)
                {
                    product.Picture = _pictureProvider.GetProductPictureUrl(p.Pictures.FirstOrDefault()?.Picture, 200);
                }

                result.Add(product);
            }

            return View(result);
        }
    }
}