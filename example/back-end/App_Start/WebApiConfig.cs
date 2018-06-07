using System.Web.Http;
using ApiMultiPartFormData;

namespace HttpFormatter.Test
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

            config.Formatters.Add(new MultipartFormDataFormatter());
        }
    }
}