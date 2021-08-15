using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Web.Areas.Store.ViewModels.Cart;
using SimpleStore.Web.Areas.Store.ViewModels.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Store.ViewModelProviders
{
    public interface ICartViewModelProvider
    {
        Task<CartViewModel> CreateCartViewModel(Cart cart);
    }
    public class CartViewModelProvider : ICartViewModelProvider
    {
        private readonly ICatalogProductService _catalogProductService;
        private readonly IMonetaryService _monetaryService;
        private readonly IOrderCalculationService _orderCalculationService;
        private readonly IPictureProvider _pictureProvider;

        public CartViewModelProvider(ICatalogProductService catalogProductService, IMonetaryService monetaryService, IOrderCalculationService orderCalculationService, IPictureProvider pictureProvider)
        {
            _catalogProductService = catalogProductService;
            _monetaryService = monetaryService;
            _orderCalculationService = orderCalculationService;
            _pictureProvider = pictureProvider;
        }

        public async Task<CartViewModel> CreateCartViewModel(Cart cart)
        {
            // Cart
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                ShippingZipCode = cart.ShippingZipCode,
                SelectedAddress = cart.SelectedAddress,
                Items = new List<CartItemViewModel>(),
                ShippingOptions = new List<CartShippingOptionViewModel>()
            };

            // Items
            foreach (var item in cart.Items)
            {
                var catalogItem = await _catalogProductService.GetById(item.CatalogItemId);
                var catalogItemViewModel = new CatalogProductViewModel
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Picture = _pictureProvider.GetCatalogProductPictureUrl(catalogItem, 200),
                    Price = catalogItem.OldPrice,
                    OldPrice = catalogItem.Price,
                    OldPriceString = _monetaryService.GetValueString(catalogItem.OldPrice),
                    PriceString = _monetaryService.GetValueString(catalogItem.Price)
                };

                var cartItemViewModel = new CartItemViewModel
                {
                    Id = item.Id,
                    Quantity = item.Quantity,
                    CatalogItem = catalogItemViewModel
                };

                cartViewModel.Items.Add(cartItemViewModel);
            }

            // Shipping Options
            if (cart.ShippingOptions != null)
            {
                foreach (var option in cart.ShippingOptions)
                {
                    var optionViewModel = new CartShippingOptionViewModel
                    {
                        Id = option.Id,
                        Method = option.Method,
                        Name = option.Name,
                        Description = option.Description,
                        Value = option.Value,
                        ValueString = _monetaryService.GetValueString(option.Value),
                        Selected = option.Selected
                    };

                    cartViewModel.ShippingOptions.Add(optionViewModel);
                }

                cartViewModel.ShippingOptions = cartViewModel.ShippingOptions.OrderBy(x => x.Value).ToList();
            }

            // Values
            cartViewModel.Subtotal = await _orderCalculationService.GetSubtotal(cart);
            cartViewModel.ShippingValue = await _orderCalculationService.GetShippingValue(cart);
            cartViewModel.Total = await _orderCalculationService.GetTotal(cart);

            cartViewModel.SubtotalString = _monetaryService.GetValueString(cartViewModel.Subtotal);
            cartViewModel.ShippingValueString = _monetaryService.GetValueString(cartViewModel.ShippingValue);
            cartViewModel.TotalString = _monetaryService.GetValueString(cartViewModel.Total);

            return cartViewModel;
        }
    }
}
