using System.Web;
using System.Web.Http;
using Autofac;
using Data;

namespace WebApi
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = Bootstrapper.Initialize();
            GlobalConfiguration.Configure(config => WebApiConfig.Register(config, container));

            var databaseInitializer = container.Resolve<DatabaseInitializer>();
            databaseInitializer.GenerateMasterData();
        }
    }
}
