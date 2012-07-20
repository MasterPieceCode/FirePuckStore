using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Controllers;
using FirePuckStore.Models;
using Moq;
using Xunit;
using FirePuckStore.Tests.Helpers;

namespace FirePuckStore.Tests.Controllers
{
    public class CartControllerUnitTest
    {
        [Fact]
        public void TestDetailsAction()
        {
            var orderedCards = TestHelper.GetRandomOrders();

            var serviceMock = new Mock<ICartService>();
            serviceMock.Setup(x => x.GetCart()).Returns(orderedCards);

            var controller = new CartController(serviceMock.Object);
            var result = controller.Details() as PartialViewResult;

            serviceMock.Verify(x => x.GetCart(), Times.Once());
            result.WithNameHasModelType<IList<Order>>("_Cart");
            Assert.Equal(2, orderedCards.Count);
        }
    }
}
