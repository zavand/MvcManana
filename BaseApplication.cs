using System.Web.Mvc;
using System.Web.Routing;

namespace Zavand.Web.Mvc.Manana.Framework
{
    public class BaseApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Set this property to true to prevent call to AreaRegistration.RegisterAllAreas();
        /// This is usefull for unit tests where area should be registered manually
        /// </summary>
        public bool ManualAreaRegistration { get; set; }
        /// <summary>
        /// true if localization presented in the route
        /// </summary>
        public bool IsLocalizationSupported { get; set; }

        public BaseApplication()
        {
            // Localization is not supported by default
            IsLocalizationSupported = false;
        }

        public virtual void RegisterRoutes(RouteCollection routes)
        {
            // Route:
            // <scheme>://<domain>/<localization>/<route>
            // Where:
            // <scheme>: could be one of the following:
            //              http
            //              https
            //              ftp
            // <domain>: 
            // <localization>: interface localization, e.g. en-US
            // <route>: the website route itself, e.g. area1/index/1/2?a=1&b=2

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

//            routes.MapRoute(
//                "Default", // Route name
//                "{controller}/{action}/{id}", // URL with parameters
//                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
//            );
        }
        public virtual void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public virtual void ApplicationStart()
        {
            RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public virtual void RegisterAllAreas()
        {
            if (!ManualAreaRegistration)
                AreaRegistration.RegisterAllAreas();
        }

        public virtual void RegisterArea(BaseAreaRegistration area, AreaRegistrationContext context)
        {
            
        }
    }
}