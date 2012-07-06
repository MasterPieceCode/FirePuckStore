using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Xunit;

namespace FirePuckStore.Tests.Helpers
{
    public static class AssertExtensions
    {
        public static void WithNameHasModelType<TModel>(this ViewResultBase viewResult, string viewName)
        {
            Assert.NotNull(viewResult);
            Assert.Equal(viewName, viewResult.ViewName, StringComparer.OrdinalIgnoreCase);
            Assert.IsAssignableFrom<TModel>(viewResult.Model);
        }
    }
}
