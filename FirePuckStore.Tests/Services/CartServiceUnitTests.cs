using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;
using Moq;
using Xunit;

namespace FirePuckStore.Tests.Services
{
    public class CartServiceUnitTests
    {
        [Fact]
        public void TestGetCart()
        {
            var mockRepositroy = new Mock<ICartRepository>();

            var orderForCard1 = TestHelper.CreateRandomOrderForCardId(TestHelper.CreateRandomNumber(1, 5));

            var orderForCard2 = TestHelper.CreateRandomOrderForCardId(TestHelper.CreateRandomNumber(6, 10));

            var expectedCart = new List<Order>
                                   {
                                       orderForCard1,
                                       orderForCard2
                                   };


           mockRepositroy.Setup(repository => repository.GetCartOrder()).Returns(expectedCart);

           var mockCardService = new Mock<ICardService>();

            var cartService = new CartService(mockRepositroy.Object, mockCardService.Object);
            var actualCart = cartService.GetCart();

            mockRepositroy.Verify(repository => repository.GetCartOrder(), Times.Once());

            Assert.Equal(expectedCart.Count, actualCart.Count);
            Assert.Equal(expectedCart[0], actualCart[0], new OrderComparer());
            Assert.Equal(expectedCart[1], actualCart[1], new OrderComparer());

            AsserPriceForOrderInCart(orderForCard1, actualCart[0]);
            AsserPriceForOrderInCart(orderForCard2, actualCart[1]);
        }

        private static void AsserPriceForOrderInCart(Order expectedOrder, Order actualOrder)
        {
            Assert.Equal( CalculatePriceForOrder(expectedOrder), actualOrder.Price);
        }

        private static decimal CalculatePriceForOrder(Order expectedOrder)
        {
            return expectedOrder.Quantity * expectedOrder.Card.Price;
        }

        [Fact]
        public void TestPlaceOrderForCardIdIfNoExists()
        {
            var mockRepositroy = new Mock<ICartRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var dbCard = TestHelper.CreateRandomCardWithId(cardId);
            var initailQuantity = dbCard.Quantity;

            var mockCardService = new Mock<ICardService>();

            SetupCheckingOfExistanceOrderForSpecifiedCard(mockRepositroy, null, mockCardService, dbCard, cardId);

            var cartService = new CartService(mockRepositroy.Object, mockCardService.Object);

            var actualOrder = cartService.Place1QuantityOrderAndReturnSummaryOrderForCard(cardId);

            VerifyCheckingOfExistanceOrderForSpecifiedCardInvokations(mockRepositroy, mockCardService, cardId);

            Assert.Equal(1, actualOrder.Quantity);
            Assert.Equal(CalculatePriceForOrder(actualOrder), actualOrder.Price);
            Assert.Equal(--initailQuantity, dbCard.Quantity);
        }


        private static void SetupCheckingOfExistanceOrderForSpecifiedCard(Mock<ICartRepository> mockRepositroy, Order order, Mock<ICardService> mockCardService, Card dbCard, int cardId)
        {
            mockCardService.Setup(cardService => cardService.GetById(cardId)).Returns(dbCard);
            mockRepositroy.Setup(repository => repository.GetOrderForCardId(cardId)).Returns(order);
        }

        private static void VerifyCheckingOfExistanceOrderForSpecifiedCardInvokations(Mock<ICartRepository> mockRepositroy, Mock<ICardService> mockCardService, int cardId)
        {
            mockCardService.Verify(cardService => cardService.GetById(cardId));
            mockRepositroy.Verify(repository => repository.GetOrderForCardId(cardId), Times.Once());
        }

        [Fact]
        public void TestPlacingOrderUpdatingForCardIdIfExists()
        {
            var mockRepositroy = new Mock<ICartRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var expectedOrder = TestHelper.CreateRandomOrderForCardId(cardId);
            var dbCard = TestHelper.CreateRandomCardWithId(cardId);
            var initailQuantityInStore = dbCard.Quantity;
            var initailQuantityInOrder = expectedOrder.Quantity;
            var mockCardService = new Mock<ICardService>();

            SetupCheckingOfExistanceOrderForSpecifiedCard(mockRepositroy, expectedOrder, mockCardService, dbCard, cardId);

            var cartService = new CartService(mockRepositroy.Object, mockCardService.Object);

            var actualOrder = cartService.Place1QuantityOrderAndReturnSummaryOrderForCard(cardId);

            VerifyCheckingOfExistanceOrderForSpecifiedCardInvokations(mockRepositroy, mockCardService, cardId);
            mockRepositroy.Verify(repository => repository.UpdateOrder(expectedOrder), Times.Once());

            Assert.Equal(expectedOrder, actualOrder, new OrderComparer());
            Assert.Equal(CalculatePriceForOrder(expectedOrder), actualOrder.Price);
            Assert.Equal(++initailQuantityInOrder, expectedOrder.Quantity);
            Assert.Equal(--initailQuantityInStore, dbCard.Quantity);
        }

        [Fact]
        public void TestPlacingOrderThrowExceptionIfNoCardsInStore()
        {
            var mockRepositroy = new Mock<ICartRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var dbCard = TestHelper.CreateRandomCardWithId(cardId);
            dbCard.Quantity = 0;

            var mockCardService = new Mock<ICardService>();

            mockCardService.Setup(cardService => cardService.GetById(cardId)).Returns(dbCard);

            var cartService = new CartService(mockRepositroy.Object, mockCardService.Object);

            Assert.Throws<InvalidOperationException>(() => cartService.Place1QuantityOrderAndReturnSummaryOrderForCard(cardId));

            mockCardService.Verify(cardService => cardService.GetById(cardId));
        }

        class OrderComparer : IEqualityComparer<Order>
        {
            public bool Equals(Order x, Order y)
            {
                return x.OrderId.Equals(y.OrderId);
            }

            public int GetHashCode(Order obj)
            {
                return obj.OrderId.GetHashCode();
            }
        }
    }
}
