using Splitwise.Data;
using System.Web.Http;
using System.Web.Mvc;

namespace Splitwise
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Init database
            System.Data.Entity.Database.SetInitializer(new SplitwiseSeedData());

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
