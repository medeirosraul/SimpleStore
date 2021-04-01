using SimpleStore.Core.Entities;
using SimpleStore.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services
{
    public interface IStoreBaseService<TStoreEntity>: IBaseService<TStoreEntity> 
        where TStoreEntity : StoreEntity
    {

    }
}
