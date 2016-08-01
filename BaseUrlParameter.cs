namespace Zavand.Web.Mvc.Manana.Framework
{
    /// <summary>
    ///     This class is needed to fix routing issue described at the page below:
    ///     http://weblogs.asp.net/imranbaloch/archive/2010/12/26/routing-issue-in-asp-net-mvc-3-rc-2.aspx
    /// </summary>
    public class BaseUrlParameter
    {
        public static readonly BaseUrlParameter Optional = new BaseUrlParameter();
    }
}