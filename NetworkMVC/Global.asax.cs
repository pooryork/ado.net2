using NetworkDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace NetworkMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoggerUtil.InitLogger($"Logs\\LogsAppStart_{DateTime.Now.ToString("dd/MM/yyyy_hh_mm_tt")}\\Info.log", "%d  %-5p %m%n", "Logger");
        }
    }
}
