using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Customers;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Customers
{
    public interface ICustomerService : IStoreBaseService<Customer>
    {
        Task<Customer> GetByUser(string userId);
    }

    public class CustomerService : StoreBaseService<Customer>, ICustomerService
    {
        public CustomerService(
            StoreDbContext context, 
            IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public async Task<Customer> GetByUser(string userId)
        {
            var query = PrepareQuery();
            query = query.Include(x => x.Addresses.Where(address => !address.Deleted));
            query = query.Where(p => p.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }
    }
}