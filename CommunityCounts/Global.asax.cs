using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CommunityCounts
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes (RouteCollection routes)
        {
            routes.MapRoute(
                name: "C1surveysEnterNumeric",
                url: "C1surveysEnter/Results/{id}/{clientid}",
                defaults: new { controller = "C1surveysEnter", action = "Results", id = @"\d+", clientid = @"\d+" });
            routes.MapRoute(
                name: "C1surveysEnterText",
                url: "C1surveysEnter/Text/{id}/{clientid}",
                defaults: new { controller = "C1surveysEnter", action = "Text", id = @"\d+", clientid = @"\d+" });
            routes.MapRoute(
                name: "ClientNeeds",
                url: "C1clientneedsheader/MarkNeeds/{idclient}/{idclientneeds}",
                defaults: new { controller = "C1clientneedheader", action = "MarkNeeds", idclient = @"\d+", idclientneeds = @"\d+" });

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
