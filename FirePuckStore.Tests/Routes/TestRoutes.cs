using System.Web.Routing;
using FirePuckStore.Controllers;
using Xunit;
using MvcContrib.TestHelper;

namespace FirePuckStore.Tests.Routes
{
    public class TestRoutes
    {
        /*[Fact]*/
        public void TestDefaultRoute()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            "~/".ShouldMapTo<HomeController>(x => x.Index());
        }
    }
}
