using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void TestIndexActionReturnsCards()
        {
            var expected = TestHelper.Create2RandomCardsWithDifferentCategories();

            var serviceMock = new Mock<ICardService>();
            serviceMock.Setup(x => x.GetAllCards()).Returns(expected);

            var controller = new CardController(serviceMock.Object);
            var actual = controller.Index() as ViewResult;

            serviceMock.Verify(x => x.GetAllCards(), Times.Once());

            Assert.NotNull(actual);
            actual.WithNameHasModelType<IList<Card>>(string.Empty);

            var actualModel = (IList<Card>) actual.Model;
            Assert.Equal(2, actualModel.Count);

            actualModel.ContainsCard(expected[0]);
            actualModel.ContainsCard(expected[1]);
        }

        [Fact]
        public void TestDeleteActionDeleteCardAndRedirectToIndexAction()
        {
            var serviceMock = new Mock<ICardService>();
            var cardId = TestHelper.CreateRandomId();

            serviceMock.Setup(service => service.DeleteCard(cardId));

            var controller = new CardController(serviceMock.Object);
            var actual = controller.DeleteConfirmed(cardId);

            serviceMock.Verify(service => service.DeleteCard(cardId), Times.Once());

            actual.ShouldBeRedirectionTo(new {action = "Index"});
        }
    }
}
