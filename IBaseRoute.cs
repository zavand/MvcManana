using System;

namespace Zavand.Web.Mvc.Manana.Framework
{
    public interface IBaseRoute
    {
        string Locale { get; set; }
        string Controller { get; set; }
        string Action { get; set; }
        string Area { get; set; }
        string GetName();
        string GetNameLocalized();
        string GetUrl();
        string GetUrlLocalized();
        object GetDefaults();
        object GetConstraints();
        object GetConstraintsLocalized();
        string[] GetNamespaces();
        void FollowContext(IBaseRoute r);
        void MakeTheSameAs(IBaseRoute r);
        string GetDomain();
        void SetDomain(string domain);
        BaseRoute.UrlProtocol GetProtocol();
        void SetProtocol(BaseRoute.UrlProtocol protocol);
        int GetPort();
        void SetPort(int port);
        T Clone<T>(Action<T> action) where T : IBaseRoute;
        IBaseRoute Clone();
        IBaseRoute GetParentRoute();
        void SetParentRoute(IBaseRoute parentRoute);
    }
}