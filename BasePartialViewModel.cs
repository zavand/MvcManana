namespace Zavand.Web.Mvc.Manana.Framework
{
    public class BasePartialViewModel
    {
        public BasePartialViewModel(string viewName, IBaseRoute route)
        {
            ViewName = viewName;
            Route = route;
        }

        public string ViewName { get; private set; }

        public IBaseRoute Route { get; private set; }

        public IBaseRoute GetRoute()
        {
            return Route;
        }
    }
}