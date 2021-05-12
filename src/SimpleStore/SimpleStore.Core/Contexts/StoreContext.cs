using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Serilog;
using SimpleStore.Core.Entities.Stores;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Framework.Contexts;
using System;
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
            if (IsSimpleStore())
                return;

            // Get store
            if (IsCustomDomain())
            {
                _currentStore = await _storeService.GetStoreByHost(GetHost());
            }
            else if(IsSubDomain())
            {
                _currentStore = await _storeService.GetStoreBySubdomain(GetSubDomain());
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

        public string GetSubDomain()
        {
            if (!IsSubDomain())
                throw new InvalidOperationException("This host is not a subdomain.");

            return GetHost().Split('.')[0];
        }

        public bool IsSimpleStore()
        {
            return GetHost().ToLower() == _configuration["Host"].ToLower();
        }

        public bool IsSubDomain()
        {
            return !IsSimpleStore() && !IsCustomDomain();
        }

        public bool IsCustomDomain()
        {
            return !GetHost().ToLower().Contains(_configuration["Host"].ToLower());
        }
    }
}