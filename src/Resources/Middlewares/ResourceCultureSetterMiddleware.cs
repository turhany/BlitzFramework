using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlitzFramework.Resources.Middlewares
{
    public class ResourceCultureSetterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ResourceMiddlewareOptions _resourceMiddlewareOptions;

        public ResourceCultureSetterMiddleware(RequestDelegate next, ResourceMiddlewareOptions resourceMiddlewareOptions)
        {
            _next = next;
            _resourceMiddlewareOptions = resourceMiddlewareOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            CultureInfo resourceCulture = _resourceMiddlewareOptions.DefaultLanguage;

            foreach (var language in context.Request.GetTypedHeaders().AcceptLanguage)
            {
                var selectedCulture = _resourceMiddlewareOptions.SupportedLanguages.First(p => string.Equals(p.Name, language.Value.Value, StringComparison.CurrentCultureIgnoreCase));
                if (selectedCulture != null)
                {
                    resourceCulture = selectedCulture;
                    break;
                }
            }

            Thread.CurrentThread.CurrentUICulture = resourceCulture;
            await _next(context);
        }
    }
}
