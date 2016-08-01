using System;
using System.Web.Mvc;

namespace Zavand.Web.Mvc.Manana.Framework
{
    public class FormWriter : IDisposable
    {
        private readonly HtmlHelper _htmlHelper;
        private readonly string _end;
        private readonly string _start;

        public FormWriter(HtmlHelper htmlHelper, string start, string end)
        {
            _htmlHelper = htmlHelper;
            _start = start;
            _end = end;
            _htmlHelper.ViewContext.Writer.Write(_start);
        }
        ~FormWriter()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool dispose)
        {
            if (dispose)
            {
                _htmlHelper.ViewContext.Writer.Write(_end);
                GC.SuppressFinalize(this);
            }
        }
    }
}