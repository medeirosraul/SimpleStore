using SimpleStore.Core.Entities;

namespace SimpleStore.Core.Services
{
    public interface IStoreBaseService<TStoreEntity>: IBaseService<TStoreEntity> 
        where TStoreEntity : StoreEntity
    {

    }
}
