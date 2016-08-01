namespace Zavand.Web.Mvc.Manana.Framework
{
    public interface IMasterPageModel<out TRoute>
        where TRoute : BaseRoute
    {
        TRoute GetRoute();
    }
}