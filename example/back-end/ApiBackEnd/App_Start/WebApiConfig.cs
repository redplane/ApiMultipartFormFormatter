using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using ApiBackEnd.Services;
using ApiMultiPartFormData;
using ApiMultiPartFormData.Services.Interfaces;
using Autofac;
using Autofac.Integration.WebApi;
using Newtonsoft.Json.Serialization;

namespace ApiBackEnd
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            var containerBuilder = new ContainerBuilder();

            // Once a listener has been fully constructed and is
            // ready to be used, automatically start listening.
            //containerBuilder.RegisterType<NotImplementedMultipartFormDataModelBinderService>()
            //    .As<IMultiPartFormDataModelBinderService>();

            var container = containerBuilder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);


            // Using camel-cased naming convention.
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Register multipart/form-data formatter.
            config.Formatters.Add(new MultipartFormDataFormatter());
        }
    }
}