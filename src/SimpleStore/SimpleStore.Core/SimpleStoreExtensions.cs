using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SimpleStore.Core.Contexts;
using SimpleStore.Core.Entities.Identity;
using SimpleStore.Core.Repositories;
using SimpleStore.Core.Services;
using SimpleStore.Core.Services.Carts;
using SimpleStore.Core.Services.Catalog;
using SimpleStore.Core.Services.Customers;
using SimpleStore.Core.Services.MelhorEnvio;
using SimpleStore.Core.Services.Monetaries;
using SimpleStore.Core.Services.Orders;
using SimpleStore.Core.Services.Payments;
using SimpleStore.Core.Services.Payments.BankTransfer;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Core.Services.Products;
using SimpleStore.Core.Services.Schedules;
using SimpleStore.Core.Services.Shipping;
using SimpleStore.Core.Services.Stores;
using SimpleStore.Core.Services.Subscriptions;
using SimpleStore.Framework.Contexts;
using SimpleStore.Framework.Helpers;
using SimpleStore.Framework.Identity;
using SimpleStore.Framework.Repositories;

namespace SimpleStore.Core
{
    public static class SimpleStoreExtensions
    {
        public static IServiceCollection AddSimpleStore(this IServiceCollection services, string connectionString, Action<SimpleStoreOptions> options = null)
        {
            // Config
            //services.Configure(options);

            // Generics
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IStoreBaseService<>), typeof(StoreBaseService<>));

            // Framework
            services.AddScoped<IWebHelper, WebHelper>();

            // Contexts
            services.AddScoped<IStoreContext, StoreContext>();
            services.AddScoped<ICustomerContext, CustomerContext>();

            // Services
            services.AddScoped<SubscriptionService>();
            services.AddScoped<StoreService>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerAddressService, CustomerAddressService>();

            services.AddScoped<ICatalogProductService, CatalogProductService>();
            services.AddScoped<ICatalogItemProvider, CatalogItemProvider>();

            services.AddScoped<ICartService, CartService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderProcessingService, OrderProcessingService>();
            services.AddScoped<IOrderCalculationService, OrderCalculationService>();

            services.AddScoped<IMonetaryService, MonetaryService>();

            // Shipping
            services.AddScoped<IShippingService, ShippingService>();
            services.AddScoped<IShippingMethodServiceResolver, ShippingMethodServiceResolver>();
            services.AddScoped<IShippingMethodService, MelhorEnvioService>();
            services.AddScoped<IMelhorEnvioSettingsService, MelhorEnvioSettingsService>();

            // Payment
            services.AddScoped<IPaymentServiceResolver, PaymentServiceResolver>();
            services.AddScoped<IPaymentMethod, BankTransferPaymentMethod>();
            // services.AddScoped<IPaymentService, MercadoPagoPaymentService>();

            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IPictureProvider, PictureProvider>();
            services.AddScoped<IStorageObjectService, StorageObjectService>();

            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<ISchedulePeriodService, SchedulePeriodService>();
            services.AddScoped<IScheduleDateService, ScheduleDateService>();
            services.AddScoped<IScheduleActivityService, ScheduleActivityService>();
            services.AddScoped<IPeriodProvider, PeriodProvider>();

            // Authorization
            services.AddScoped<IUserClaimsPrincipalFactory<StoreIdentity>, StoreIdentityClaimsPrincipalFactory>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("store-owner", policy =>
                {
                    policy.RequireClaim("StoreOwner");
                });
            });

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
