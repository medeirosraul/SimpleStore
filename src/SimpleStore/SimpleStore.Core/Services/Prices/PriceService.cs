using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Prices;
using SimpleStore.Framework.Contexts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface IPriceService: IStoreBaseService<Price>
    {
        Task<ICollection<Price>> GetProductPrices(string productId);
    }

    public class PriceService : StoreBaseService<Price>, IPriceService
    {
        public PriceService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public async Task<ICollection<Price>> GetProductPrices(string productId)
        {
            var query = PrepareQuery();
            return await query.Where(p => p.CatalogItemId == productId).ToListAsync();
        }
    }
}