using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using ApiMultiPartFormData;
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

            // Using camel-cased naming convention.
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Register multipart/form-data formatter.
            config.Formatters.Add(new MultipartFormDataFormatter());
        }
    }
}