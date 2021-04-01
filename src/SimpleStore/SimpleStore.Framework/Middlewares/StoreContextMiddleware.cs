using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using SimpleStore.Framework.Contexts;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Middlewares
{
    public class StoreContextMiddleware
    {
        private readonly RequestDelegate _next;

        public StoreContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IStoreContext storeContext, IConfiguration configuration)
        {
            if (httpContext == null || httpContext.Request == null) return;

            // Get current host.
            var host = httpContext.Request?.Headers[HeaderNames.Host];

            // If current host is not main system host, load current store.
            if (host.Value != configuration["Host"] && host.Value != $"www.{configuration["Host"]}")
            {
                await storeContext.SetCurrentStore();

                // If store not found, redirect to error page.
                if (storeContext.CurrentStore == null)
                    httpContext.Response.Redirect($"https://{configuration["Host"]}/Error/StoreNotFound");
            }

            await _next(httpContext);
        }
    }
}