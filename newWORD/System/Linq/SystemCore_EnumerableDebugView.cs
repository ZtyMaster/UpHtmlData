using System.Collections.Generic;
using Ivony.Html;

namespace System.Linq
{
    internal class SystemCore_EnumerableDebugView<T>
    {
        private IEnumerable<IHtmlElement> span;

        public SystemCore_EnumerableDebugView(IEnumerable<IHtmlElement> span)
        {
            this.span = span;
        }
    }
}