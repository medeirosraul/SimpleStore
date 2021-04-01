using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SimpleStore.Core.Contexts;
using SimpleStore.Core.Repositories;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Prices;
using SimpleStore.Core.Services.Products;
using SimpleStore.Core.Services.Schedules;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Core.Services.Subscriptions;
using SimpleStore.Framework.Contexts;
using SimpleStore.Framework.Helpers;
using SimpleStore.Framework.Repositories;
using System;

namespace SimpleStore.Core
{
    public static class SimpleStoreExtensions
    {
        public static IServiceCollection AddSimpleStore(this IServiceCollection services, string connectionString, Action<SimpleStoreOptions> options = null)
        {
            // Config
            //services.Configure(options);
            // Repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Framework
            services.AddScoped<IWebHelper, WebHelper>();

            // Contexts
            services.AddScoped<IStoreContext, StoreContext>();

            // Services
            services.AddScoped<SubscriptionService>();
            services.AddScoped<StoreService>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductProvider, ProductProvider>();
            services.AddScoped<IProductPictureService, ProductPictureService>();

            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IPriceProvider, PriceProvider>();

            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPictureProvider, PictureProvider>();
            services.AddScoped<IStorageObjectService, StorageObjectService>();

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IPeriodProvider, PeriodProvider>();

            // Return
            return services;
        }

        public static IApplicationBuilder UseSimpleStore(this IApplicationBuilder builder)
        {
            // builder.UseMiddleware<StoreContextMiddleware>();
            return builder;
        }
    }
}
