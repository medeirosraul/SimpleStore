using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Entities.Catalog;
using SimpleStore.Core.Services.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface ICatalogItemProvider
    {
        Task<IEnumerable<CatalogProduct>> GetNewProducts(int quantity);
        Task<CatalogProduct> GetProductDetails(string id);
    }


    public class CatalogItemProvider: ICatalogItemProvider
    {
        private readonly ICatalogProductService _catalogProductService;

        public CatalogItemProvider(ICatalogProductService productService)
        {
            _catalogProductService = productService;
        }

        public async Task<CatalogProduct> GetProductDetails(string id)
        {
            var query = _catalogProductService.PrepareQuery();
            query = query.Include(x => x.Pictures.Where(y => !y.Deleted))
                .ThenInclude(x => x.Picture);

            query = query.Where(x => x.Id == id);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogProduct>> GetNewProducts(int quantity)
        {
            var query = _catalogProductService.PrepareQuery();
            query = query.Where(p => p.Published);
            query = query.Include(p => p.Pictures.Where(y => !y.Deleted))
                .ThenInclude(p => p.Picture);
            query = query.OrderByDescending(p => p.CreatedAt);

            return await query.Take(quantity).ToListAsync();
        }
    }
}
