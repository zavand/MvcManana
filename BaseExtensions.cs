using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Zavand.Web.Mvc.Manana.Framework
{
    public static class BaseExtensions
    {
        public static MvcHtmlString ActionLink<T>(this HtmlHelper helper, string linkText, IBaseRoute currentRoute, T r, object htmlAttributes = null, object extraParams = null) where T : IBaseRoute
        {
            var uh = new UrlHelper(new RequestContext(helper.ViewContext.RequestContext.HttpContext, new RouteData()));
            var url = uh.RouteUrl(currentRoute, r, extraParams);

            return MvcHtmlString.Create(String.Format("<a href=\"{0}\"{2}>{1}</a>", url, linkText, GetAttributesString(htmlAttributes)));
        }

        public static MvcHtmlString ActionLinkCloneGeneral(this HtmlHelper helper, string linkText, IBaseRoute currentRoute, Action<IBaseRoute> action, object htmlAttributes = null)
        {
            var r = currentRoute.Clone(action);
            return ActionLink(
                helper,
                linkText,
                null, // No need to follow context as exact clone was created
                r,    // We created a copy of existing route. No need to follow context.
                htmlAttributes
                );
        }

        public static MvcHtmlString ActionLinkClone<T>(this HtmlHelper helper, string linkText, T currentRoute, Action<T> action, object htmlAttributes = null) where T : IBaseRoute
        {
            var r = currentRoute.Clone(action);
            return ActionLink(
                helper,
                linkText,
                null, // No need to follow context as exact clone was created
                r,    // We created a copy of existing route. No need to follow context.
                htmlAttributes
                );
        }
        public static void MapRoute<T>(this RouteCollection routes, bool isLocalizationSupported) where T : IBaseRoute, new()
        {
            var r = new T();
            if (isLocalizationSupported)
            {
                routes.MapRoute(r.GetNameLocalized(), r.GetUrlLocalized(), r.GetDefaults(), r.GetConstraintsLocalized(), r.GetNamespaces());
            }
            routes.MapRoute(r.GetName(), r.GetUrl(), r.GetDefaults(), r.GetConstraints(), r.GetNamespaces());
        }
        public static void MapRoute<T>(this AreaRegistrationContext context, bool isLocalizationSupported) where T : IBaseRoute, new()
        {
            var r = new T();
            if (isLocalizationSupported)
            {
                context.MapRoute(r.GetNameLocalized(), r.GetUrlLocalized(), r.GetDefaults(), r.GetConstraintsLocalized(), r.GetNamespaces());
            }
            context.MapRoute(r.GetName(), r.GetUrl(), r.GetDefaults(), r.GetConstraints(), r.GetNamespaces());
        }

        public static String RouteUrl<T>(this UrlHelper u, IBaseRoute currentRoute, T r, object extraParams = null) where T : IBaseRoute
        {
            if (currentRoute != null)
                r.FollowContext(currentRoute);
            var domain = r.GetDomain();

            // ------------------------
            // Route local url
            // ------------------------
            if (String.IsNullOrEmpty(domain))
            {
                var rd = new RouteValueDictionary(r);
                if (String.IsNullOrEmpty(r.Area))
                {
                    rd.Remove("Area");
                }
                if (String.IsNullOrEmpty(r.Locale))
                {
                    rd.Remove("Locale");
                }

                if (extraParams != null)
                {
                    var extraParamsDictionary = new RouteValueDictionary(extraParams);
                    foreach (var k in extraParamsDictionary.Keys)
                    {
                        if (rd.ContainsKey(k))
                            rd[k] = extraParamsDictionary[k];
                        else
                            rd.Add(k, extraParamsDictionary[k]);
                    }
                }

                var routeName = String.IsNullOrEmpty(r.Locale) ? r.GetName() : r.GetNameLocalized();
                return u.RouteUrl(
                    routeName,
                    rd
                    );
            }

            // ------------------------
            // Route external domain
            // ------------------------
            var protocol = r.GetProtocol();
            var port = r.GetPort();

            // Determine required protocol
            if (protocol == BaseRoute.UrlProtocol.Inherited)
            {
                // Try to get protocol from current context
                try
                {
                    if (u.RequestContext.HttpContext.Request.Url != null &&
                        !Enum.TryParse(u.RequestContext.HttpContext.Request.Url.Scheme, true, out protocol))
                    {
                        throw new Exception(); // Protocol can't be parsed. Internal exception.
                    }
                }
                catch // In case if anything of following is null: u.RequestContext.HttpContext.Request.Url
                {
                    protocol = BaseRoute.UrlProtocol.Http;
                }
            }
            var protocolUrl = protocol.ToString().ToLower();

            // Determine required port
            var porturl = "";
            if (protocol == BaseRoute.UrlProtocol.Http && port == BaseRoute.DefaultHttpPort)
                port = 0;
            if (protocol == BaseRoute.UrlProtocol.Https && port == BaseRoute.DefaultHttpsPort)
                port = 0;

            if (port != 0)
                porturl = String.Format(":{0}", port);

            var path = r.GetUrl();
            if (!String.IsNullOrEmpty(path))
                path = "/" + path;

            return String.Format("{0}://{1}{2}{3}", protocolUrl, domain, porturl, path);
        }

        public static String RouteUrlCloneGeneral(this UrlHelper u, IBaseRoute currentRoute, Action<IBaseRoute> action)
        {
            var r = currentRoute.Clone(action);
            return RouteUrl(u, null, r); // We created a copy of existing route. No need to follow context.
        }

        public static String RouteUrlClone<T>(this UrlHelper u, T currentRoute, Action<T> action) where T : BaseRoute, new()
        {
            var r = currentRoute.Clone(action);
            return RouteUrl(u, null, r); // We created a copy of existing route. No need to follow context.
        }

        public static MvcHtmlString SubmitButton(this HtmlHelper u, string text, object htmlAttributes = null)
        {
            var s = MvcHtmlString.Create(String.Format("<input name=\"{0}\" type=\"submit\" value=\"{1}\"{2} />",
                                                       BaseController.SubmitButtonName,
                                                       text,
                                                       GetAttributesString(htmlAttributes)));
            return s;
        }

        public static MvcHtmlString SubmitButton(this HtmlHelper u, string name, string text, object htmlAttributes = null)
        {
            var s = MvcHtmlString.Create(String.Format("<input name=\"{0}\" type=\"submit\" value=\"{1}\"{2} />",
                                                       name,
                                                       text,
                                                       GetAttributesString(htmlAttributes)));
            return s;
        }
        public static MvcHtmlString SubmitButtonImage(this HtmlHelper u, string src, object htmlAttributes = null)
        {
            var s = MvcHtmlString.Create(String.Format("<input name=\"{0}\" type=\"image\" src=\"{1}\"{2} />",
                                                       BaseController.SubmitButtonName,
                                                       src,
                                                       GetAttributesString(htmlAttributes)));
            return s;
        }
        public static MvcHtmlString SubmitButtonImage(this HtmlHelper u, string name, string src, object htmlAttributes = null)
        {
            var s = MvcHtmlString.Create(String.Format("<input name=\"{0}\" type=\"image\" src=\"{1}\"{2} />",
                                                       name,
                                                       src,
                                                       GetAttributesString(htmlAttributes)));
            return s;
        }
        static string GetAttributesString(object htmlAttributes)
        {
            var attributes = "";
            if (htmlAttributes != null)
            {
                RouteValueDictionary d;
                if (htmlAttributes is IDictionary<string, object>)
                    d = new RouteValueDictionary((IDictionary<string, object>) htmlAttributes);
                else
                    d = new RouteValueDictionary(htmlAttributes);
                foreach (var k in d)
                {
                    attributes += String.Format(" {0}=\"{1}\"", k.Key, k.Value);
                }
            }
            return attributes;
        }

        public static FormWriter BeginForm(this HtmlHelper h, IBaseRoute currentRoute, IBaseRoute newRoute = null, FormMethod formMethod = FormMethod.Post, object htmlAttributes=null)
        {
            var uh = new UrlHelper(new RequestContext(h.ViewContext.RequestContext.HttpContext, new RouteData()));
            if (newRoute == null)
                newRoute = currentRoute.Clone();
            var url = uh.RouteUrl(currentRoute, newRoute);
            var start = String.Format("<form action=\"{0}\" method=\"{1}\" enctype=\"multipart/form-data\"{2}>", url, formMethod.ToString().ToUpper(), GetAttributesString(htmlAttributes));
            var end = "</form>";
            return new FormWriter(h, start, end);
        }

        public static void RenderAction<T>(this HtmlHelper h, T r) where T : IBaseRoute
        {
            h.RenderAction(r.Action, r);
        }

        public static void RenderAction<T>(this HtmlHelper h, IBaseRoute currectRoute, T r) where T : IBaseRoute
        {
            h.RenderAction(r.Action, r);
        }

        public static void RenderPartial<TPartialViewModel>(this HtmlHelper h, TPartialViewModel m) where TPartialViewModel : BasePartialViewModel
        {
            h.RenderPartial(m.ViewName, m);
        }

        public static MvcForm BeginFormFileUpload(this HtmlHelper h, IBaseRoute currectRoute)
        {
            return h.BeginForm(currectRoute.Action, currectRoute.Controller, currectRoute, FormMethod.Post, new { enctype = "multipart/form-data" });
        }

        public static MvcHtmlString FileUploadFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel,TProperty>> expression, object htmlAttributes=null)
        {
            return htmlHelper.TextBoxFor(expression, new RouteValueDictionary(htmlAttributes) {{"type", "file"}});
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper currentHtmlHelper, TModel model, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression, object htmlAttributes=null) where TModel : new()
        {
            var name = String.Empty;
            object value=null;

            var member = expression.Body as MemberExpression;
            var propInfo = member?.Member as PropertyInfo;
            if (propInfo != null)
            {
                value = propInfo.GetValue(model);
                name = propInfo.Name;
            }

            return currentHtmlHelper.TextBox(name, value, new RouteValueDictionary(htmlAttributes));
        }
    }
}
