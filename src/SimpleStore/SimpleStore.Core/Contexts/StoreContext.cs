using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Serilog;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Framework.Contexts;
using System.Threading.Tasks;

namespace SimpleStore.Core.Contexts
{
    public class StoreContext : IStoreContext
    {
        private Store _currentStore;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly StoreService _storeService;

        public StoreContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, StoreService storeService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _storeService = storeService;

            Log.Information("StoreContext created at {Path} at {Host}.", _httpContextAccessor.HttpContext?.Request?.Path, _httpContextAccessor.HttpContext?.Request?.Host);
            Task.Run(async () => await SetCurrentStore()).Wait();
        }

        public Store CurrentStore => _currentStore;

        public async Task SetCurrentStore()
        {
            // Get request host
            var host = GetHost();

            // Define if is custom host or subdomain
            var isCustomHost = !host.Contains(_configuration["Host"]);

            // Get store
            if (isCustomHost)
            {
                _currentStore = await _storeService.GetStoreByHost(host);
            }
            else
            {
                var subdomain = host.Split('.')[0];
                _currentStore = await _storeService.GetStoreBySubdomain(subdomain);
            }

            // Log
            if (_currentStore != null)
                Log.Information("CurrentStore is {Name}.", _currentStore.Name);
            else
                Log.Error("CurrentStore is not loaded.");
        }

        public string GetHost()
        {
            // Get request host
            return _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];
        }
    }
}