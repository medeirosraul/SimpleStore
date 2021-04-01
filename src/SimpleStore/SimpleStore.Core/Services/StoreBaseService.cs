﻿using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Framework.Contexts;
using SimpleStore.Framework.Types;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services
{
    public class StoreBaseService<TStoreEntity> : BaseService<TStoreEntity>, IStoreBaseService<TStoreEntity> where TStoreEntity : StoreEntity
    {
        protected IStoreContext StoreContext;
        protected Store CurrentStore => StoreContext.CurrentStore;

        public StoreBaseService(StoreDbContext context, IStoreContext storeContext): base(context)
        {
            StoreContext = storeContext;
        }

        #region Queries
        public override IQueryable<TStoreEntity> PrepareQuery(bool tracking = false, bool deleted = false)
        {
            var prepared = base.PrepareQuery(tracking, deleted);
            prepared = prepared.Where(p => p.StoreId == CurrentStore.Id);
            return prepared;
        }
        public override async Task<bool> Exists(string id)
        {
            var query = PrepareQuery();
            return await query.AnyAsync(p => p.Id == id);
        }

        public override async Task<TStoreEntity> GetById(string id)
        {
            var query = PrepareQuery();
            return await query.FirstOrDefaultAsync(p => p.Id == id && !p.Deleted);
        }

        public override async Task<PagedList<TStoreEntity>> Get(IQueryable<TStoreEntity> query)
        {
            query ??= PrepareQuery();
            return await Get(1, int.MaxValue, query);
        }

        public override async Task<PagedList<TStoreEntity>> Get(int page, int limit, IQueryable<TStoreEntity> query)
        {
            query ??= PrepareQuery();
            return await base.Get(page, limit, query);
        }
        #endregion

        #region Tracking
        protected override void Stamp(TStoreEntity entity)
        {
            entity.StoreId = CurrentStore.Id;
            base.Stamp(entity);
        }
        #endregion
    }
}
