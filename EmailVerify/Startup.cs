using System.Collections.Generic;
using System.Web.Http;
using System.Net;
using Owin;

namespace EmailVerify
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Ignore certificate errors
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            var httpConfig = new HttpConfiguration();

            // Configure Web API Routes:
            // - Enable Attribute Mapping
            // - Enable Default routes at /api.
            httpConfig.MapHttpAttributeRoutes();

            app.UseWebApi(httpConfig);
        }
    }
}
