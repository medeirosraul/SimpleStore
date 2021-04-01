using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Storages;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Pictures
{
    public interface IStorageObjectService: IStoreBaseService<StorageObject>
    {

    }

    public class StorageObjectService : StoreBaseService<StorageObject>, IStorageObjectService
    {
        public StorageObjectService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        { 
            
        }
    }
}