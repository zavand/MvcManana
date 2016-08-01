namespace Zavand.Web.Mvc.Manana.Framework
{
    public interface IPageableModel : IPageable
    {
        ulong Total { get; set; }
    }
}