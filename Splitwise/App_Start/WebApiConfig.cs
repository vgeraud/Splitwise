using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Splitwise
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            config.DependencyResolver = new UnityResolver(UnityConfig.Container);

            // Web API configuration and services
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("text/html"));

        }
    }
}
