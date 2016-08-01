using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace Zavand.Web.Mvc.Manana.Framework
{
    public class BaseController : AsyncController
    {
        public const string SubmitButtonName = "SubmitButton";

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            
            // Remove optional params
            if (requestContext != null)
            {
                var httpContext = requestContext.HttpContext;
                if (httpContext != null && httpContext.CurrentHandler is MvcHandler)
                {
                    var rvd = requestContext.RouteData.Values;
                    var matchingKeys = (from entry in rvd
                                        where entry.Value == BaseUrlParameter.Optional
                                        select entry.Key).ToArray();
                    foreach (var key in matchingKeys)
                    {
                        rvd.Remove(key);
                    }
                }
            }
        }

        public TModel GetModel<TModel, TRoute, TController>(TRoute r, TController c)
            where TModel : BaseModel<TRoute, TController>, new()
            where TRoute : BaseRoute
            where TController : BaseController
        {
            var m = new TModel();
            m.SetupModel(c, r);
            return m;
        }

        public void SetupModel<TModel, TRoute, TController>(TModel m, TRoute r, TController c)
            where TModel : BaseModel<TRoute, TController>, new()
            where TRoute : BaseRoute
            where TController : BaseController
        {
            m.SetupModel(c,r);
        }

//        public ActionResult RedirectToAction<TRoute>()
//            where TRoute : BaseRoute, new()
//        {
//            var r = new TRoute();
//            return RedirectToAction(null, r);
//        }

        public virtual ActionResult RedirectToAction<TRoute>(BaseRoute currentRoute, object extraParams = null)
            where TRoute : BaseRoute, new()
        {
            var r = new TRoute();
            return RedirectToAction(currentRoute, r, extraParams);
        }

        public virtual ActionResult RedirectToAction(BaseRoute currentRoute, IBaseRoute r, object extraParams = null)
        {
            return Redirect(Url.RouteUrl(currentRoute, r, extraParams));
        }

        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            // Works here
            var locale = requestContext.RouteData.Values["locale"] as string;
            if (!String.IsNullOrEmpty(locale))
            {
                try
                {
                    var ci = new CultureInfo(locale);
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = ci;
                }
                catch
                {
                }
            }
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}