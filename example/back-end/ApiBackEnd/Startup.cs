using System.Web.Http;
using ApiBackEnd;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace ApiBackEnd
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure Web API for self-host. 
            var httpConfiguration = new HttpConfiguration();

            // Register web api configuration.
            WebApiConfig.Register(httpConfiguration);

            // Register web API module.
            app.UseWebApi(httpConfiguration);
        }
    }
}