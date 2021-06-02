using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.MelhorEnvio;
using SimpleStore.Framework.Contexts;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.MelhorEnvio
{
    public interface IMelhorEnvioSettingsService : IStoreBaseService<MelhorEnvioSettings> 
    {
        Task<MelhorEnvioSettings> GetByCurrentStore();
    }


    public class MelhorEnvioSettingsService : StoreBaseService<MelhorEnvioSettings>, IMelhorEnvioSettingsService
    {
        public MelhorEnvioSettingsService(StoreDbContext context, IStoreContext storeContext) : base(context, storeContext)
        {

        }

        public async Task<MelhorEnvioSettings> GetByCurrentStore()
        {
            var query = PrepareQuery();
            var settings = await query.FirstOrDefaultAsync();

            if (settings == null)
            {
                settings = new MelhorEnvioSettings();
                await Insert(settings);
            }

            return settings;
        }
    }
}
