using System.Collections.Generic;
using System.Globalization;

namespace BlitzFramework.Resources.Middlewares
{
    public class ResourceMiddlewareOptions
    {
        public CultureInfo DefaultLanguage { get; set; }
        public List<CultureInfo> SupportedLanguages { get; set; }
    }
}
