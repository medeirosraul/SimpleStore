using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Framework.Helpers
{
    public interface IWebHelper
    {
        string GetHost();
    }

    public class WebHelper : IWebHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WebHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetHost()
        {
            return _httpContextAccessor.HttpContext.Request.Host.Host;
        }
    }
}