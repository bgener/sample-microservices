using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Data;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IContainer container)
        {
            // Web API configuration and services
            var resolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = resolver;


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name : "DefaultApi",
                routeTemplate : "api/{controller}/{id}",
                defaults : new
                {
                    id = RouteParameter.Optional
                }
            );
        }
    }
}
