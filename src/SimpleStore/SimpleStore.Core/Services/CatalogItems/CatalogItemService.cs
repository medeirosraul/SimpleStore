using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface ICatalogItemService : IStoreBaseService<CatalogItem>
    {

    }

    public class CatalogItemService : StoreBaseService<CatalogItem>, ICatalogItemService
    {
        public CatalogItemService(
            StoreDbContext context, 
            IStoreContext storeContext) : base(context, storeContext)
        {
        }

        public override async Task<CatalogItem> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery(tracking)
                .Where(p => p.Id == id)
                .Include(p => p.Prices)
                .Include(p => p.Pictures)
                .ThenInclude(p => p.Picture);

            var result = await Get(query);
            return result.FirstOrDefault();
        }
    }
}