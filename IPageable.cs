namespace Zavand.Web.Mvc.Manana.Framework
{
    public interface IPageable
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}