﻿namespace Zavand.Web.Mvc.Manana.Framework
{
    public interface IPageableOptional
    {
        int? Page { get; set; }
        int? PageSize { get; set; }
    }
}