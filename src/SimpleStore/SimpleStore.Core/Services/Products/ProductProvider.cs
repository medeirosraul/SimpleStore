using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Entities.CatalogItems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface IProductProvider
    {
        Task<IEnumerable<CatalogItem>> GetNewProducts(int quantity);
    }


    public class ProductProvider: IProductProvider
    {
        private readonly IProductService _productService;

        public ProductProvider(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<CatalogItem>> GetNewProducts(int quantity)
        {
            var query = _productService.PrepareQuery();
            query = query.Where(p => p.Published);
            query = query.Include(p => p.Prices);
            query = query.Include(p => p.Pictures)
                .ThenInclude(p => p.Picture);
            query = query.OrderByDescending(p => p.CreatedAt);

            return await query.Take(quantity).ToListAsync();
        }
    }
}
