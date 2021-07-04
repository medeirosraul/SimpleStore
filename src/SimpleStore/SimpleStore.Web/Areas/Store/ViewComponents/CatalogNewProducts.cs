using Microsoft.AspNetCore.Mvc;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Pictures;
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
        private readonly IMonetaryService _monetaryService;
        private readonly ICatalogItemProvider _productProvider;
        private readonly IPictureProvider _pictureProvider;

        public CatalogNewProductsViewComponent(IStoreContext storeContext, ICatalogItemProvider productProvider, IPictureProvider pictureProvider, IMonetaryService monetaryService)
        {
            _storeContext = storeContext;
            _productProvider = productProvider;
            _pictureProvider = pictureProvider;
            _monetaryService = monetaryService;
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
                    Name = p.Name,
                    PriceString = _monetaryService.GetValueString(p.Price),
                    OldPriceString = _monetaryService.GetValueString(p.OldPrice)
                };

                // Product Pictures
                if (p.Pictures != null && p.Pictures.Count > 0)
                {
                    product.Picture = _pictureProvider.GetCatalogProductPictureUrl(p.Pictures.FirstOrDefault()?.Picture, 400);
                }

                result.Add(product);
            }

            return View(result);
        }
    }
}