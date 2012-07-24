using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Controllers;
using FirePuckStore.Models;
using FirePuckStore.Tests.Services;
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
            var expectedCart = TestHelper.Get2RandomOrders();

            var serviceMock = new Mock<ICartService>();
            serviceMock.Setup(x => x.GetCart()).Returns(expectedCart);

            var controller = new CartController(serviceMock.Object);
            var result = controller.Details() as PartialViewResult;

            Assert.NotNull(result);
            serviceMock.Verify(x => x.GetCart(), Times.Once());
            result.WithNameHasModelType<IList<Order>>("_Cart");
            Assert.Equal(2, expectedCart.Count);
        }

        [Fact]
        public void TestAddAction()
        {
            var expectedCart = TestHelper.Get2RandomOrders();
            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var expectedOrder = TestHelper.CreateRandomOrderForCardId(cardId);

            var serviceMock = new Mock<ICartService>();
            serviceMock.Setup(x => x.Place1QuantityOrderAndReturnSummaryOrderForCard(cardId)).Returns(expectedOrder);
            serviceMock.Setup(x => x.GetCart()).Returns(expectedCart);

            var controller = new CartController(serviceMock.Object);
            var actualResult = controller.Add(new CartInputModel{CardId = cardId}) as PartialViewResult;

            serviceMock.Verify(x => x.Place1QuantityOrderAndReturnSummaryOrderForCard(cardId), Times.Once());
            serviceMock.Verify(x => x.GetCart(), Times.Once());

            actualResult.WithNameHasModel<IList<Order>>("_Cart", expectedCart);

            AssertCartContain(expectedCart, actualResult);
        }

        [Fact]
        public void TestDeletection()
        {
            var expectedCart = TestHelper.Get2RandomOrders();
            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var expectedOrder = TestHelper.CreateRandomOrderForCardId(cardId);

            var serviceMock = new Mock<ICartService>();
            serviceMock.Setup(x => x.UnPlace1QuantityOrderAndReturnSummaryOrderForCard(cardId)).Returns(expectedOrder);
            serviceMock.Setup(x => x.GetCart()).Returns(expectedCart);

            var controller = new CartController(serviceMock.Object);
            var actualResult = controller.Delete(new CartInputModel { CardId = cardId }) as PartialViewResult;

            serviceMock.Verify(x => x.UnPlace1QuantityOrderAndReturnSummaryOrderForCard(cardId), Times.Once());
            serviceMock.Verify(x => x.GetCart(), Times.Once());

            actualResult.WithNameHasModel<IList<Order>>("_Cart", expectedCart);

            AssertCartContain(expectedCart, actualResult);
        }
        private static void AssertCartContain(List<Order> expectedCart, PartialViewResult actualResult)
        {
            var actualCart = (List<Order>) actualResult.Model;
            Assert.Equal(expectedCart.Count, actualCart.Count);
            Assert.Equal(expectedCart[0], actualCart[0], new OrderComparer());
            Assert.Equal(expectedCart[1], actualCart[1], new OrderComparer());
        }
    }
}
