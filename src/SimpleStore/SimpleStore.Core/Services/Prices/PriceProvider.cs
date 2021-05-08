using SimpleStore.Core.Entities.Prices;
using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Core.Services.Products;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Prices
{
    public interface IPriceProvider
    {
        Task<Price> GetProductPrice(CatalogItem product);
        string GetPriceValueString(Price price);
        string GetPriceOldValueString(Price price);
    }

    public class PriceProvider: IPriceProvider
    {
        private readonly IPriceService _priceService;

        public PriceProvider(IPriceService priceService)
        {
            _priceService = priceService;
        }

        public string GetPriceOldValueString(Price price)
        {
            if (price == null)
                return "";

            return price.OldValue.ToString("F2");
        }

        public string GetPriceValueString(Price price)
        {
            if (price == null)
            {
                return "valor indisponível";
            }

            return $"R${price.Value.ToString("F2")}";
        }

        public async Task<Price> GetProductPrice(CatalogItem product)
        {
            var prices = await _priceService.GetProductPrices(product.Id);
            return prices.OrderByDescending(p => p.Active).FirstOrDefault();
        }
    }
}
