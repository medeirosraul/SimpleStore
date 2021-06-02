using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Entities.CatalogItems;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface ICatalogItemProvider
    {
        Task<IEnumerable<CatalogItem>> GetNewCatalogItems(int quantity);
        Task<CatalogItem> GetCatalogItemDetails(string id);
    }


    public class CatalogItemProvider: ICatalogItemProvider
    {
        private readonly ICatalogItemService _catalogItemService;

        public CatalogItemProvider(ICatalogItemService productService)
        {
            _catalogItemService = productService;
        }

        public async Task<CatalogItem> GetCatalogItemDetails(string id)
        {
            var query = _catalogItemService.PrepareQuery();

            query = query.Include(x => x.Prices);
            query = query.Include(x => x.Pictures)
                .ThenInclude(x => x.Picture);

            query = query.Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogItem>> GetNewCatalogItems(int quantity)
        {
            var query = _catalogItemService.PrepareQuery();
            query = query.Where(p => p.Published);
            query = query.Include(p => p.Prices);
            query = query.Include(p => p.Pictures)
                .ThenInclude(p => p.Picture);
            query = query.OrderByDescending(p => p.CreatedAt);

            return await query.Take(quantity).ToListAsync();
        }
    }
}
