using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.Controllers;
using FirePuckStore.Models;
using FirePuckStore.Tests.Helpers;
using Moq;
using Xunit;

namespace FirePuckStore.Tests.Controllers
{
    public class CardControllerUnitTest
    {
        [Fact]
        public void TestIndexActionReturnsAllCards()
        {
            var expected = TestHelper.Create2RandomCardsWithDifferentCategories();

            var cardServiceMock = new Mock<ICardService>();
            cardServiceMock.Setup(x => x.GetAllCards()).Returns(expected);

            var controller = new CardController(cardServiceMock.Object);
            var actual = controller.Index() as ViewResult;

            cardServiceMock.Verify(x => x.GetAllCards(), Times.Once());

            Assert.NotNull(actual);
            actual.WithNameHasModelType<IList<Card>>(string.Empty);

            var actualModel = (IList<Card>) actual.Model;
            Assert.Equal(2, actualModel.Count);

            actualModel.ContainsCard(expected[0]);
            actualModel.ContainsCard(expected[1]);
        }

        [Fact]
        public void TestDeleteActionAndRedirectToIndexAction()
        {
            var cardServiceMock = new Mock<ICardService>();
            var cardId = TestHelper.CreateRandomId();

            var controller = new CardController(cardServiceMock.Object);

            var actual = controller.DeleteConfirmed(cardId);

            cardServiceMock.Verify(service => service.DeleteCard(cardId), Times.Once());

            actual.ShouldBeRedirectionTo(new { action = "Index" });
        }

        [Fact]
        public void TestCreateGetActionAndRedirectToCreateView()
        {
            var cardServiceMock = new Mock<ICardService>();           
            var controller = new CardController(cardServiceMock.Object);
            var actual = controller.Create() as ViewResult;

            Assert.NotNull(actual);
        }        

        [Fact]
        public void TestCreatePostActionAndRedirectToIndexView()
        {
            var cardServiceMock = new Mock<ICardService>();
            var card = TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomId());

            var controller = new CardController(cardServiceMock.Object);
            var actual = controller.Create(card);

            cardServiceMock.Verify(service => service.Add(card), Times.Once());

            actual.ShouldBeRedirectionTo(new { action = "Index" });
        }        

        [Fact]
        public void TestCreateActionRedirectToCreateViewAgainIfModelStateIsInvalid()
        {
            var cardServiceMock = new Mock<ICardService>();
            var cardId = TestHelper.CreateRandomId();
            var card = TestHelper.CreateRandomCardWithId(cardId);

            var controller = new CardController(cardServiceMock.Object);
            controller.ModelState.AddModelError("Some Error", new Exception());
            var actual = controller.Create(card) as ViewResult;

            cardServiceMock.Verify(service => service.Add(card), Times.Never());

            Assert.NotNull(actual);
            Assert.False(controller.ModelState.IsReadOnly);
            actual.WithNameHasModelType<Card>(string.Empty);
        }
    }
}
