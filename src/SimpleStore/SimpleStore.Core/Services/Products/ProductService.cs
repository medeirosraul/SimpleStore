using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface IProductService: IStoreBaseService<CatalogItem>
    {

    }

    public class ProductService : StoreBaseService<CatalogItem>, IProductService
    {
        private readonly IPriceService _priceService;

        public ProductService(
            StoreDbContext context, 
            IStoreContext storeContext, 
            IPriceService priceService) : base(context, storeContext)
        {
            _priceService = priceService;
        }

        public override async Task<CatalogItem> GetById(string id)
        {
            var query = PrepareQuery()
                .Where(p => p.Id == id)
                .Include(p => p.Prices)
                .Include(p => p.Pictures)
                .ThenInclude(p => p.Picture);


            var result = await Get(query);
            return result.FirstOrDefault();
        }

        public override async Task<int> InsertOrUpdate(CatalogItem product)
        {
            TrackEntity(product);

            if (product.Prices != null && product.Prices.Count > 0)
                _priceService.TrackEntity(product.Prices);

            return await SaveChanges();
        }
    }
}