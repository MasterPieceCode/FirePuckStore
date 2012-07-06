using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using FirePuckStore.Controllers;
using Moq;
using Xunit;
using MvcContrib.TestHelper;

namespace FirePuckStore.Tests
{
    public class TestRoutes
    {
        [Fact]
        public void TestDefaultRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            "~/".ShouldMapTo<HomeController>(x => x.Index());
        }
    }
}
