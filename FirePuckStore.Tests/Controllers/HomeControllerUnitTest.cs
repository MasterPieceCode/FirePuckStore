using System;
using System.Collections;
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
    public class HomeControllerUnitTest
    {
        [Fact]
        public void TestIndexActionReturnsCards()
        {
            var orderedCards = TestHelper.GetRandomDictionaryCardData();

            var serviceMock = new Mock<ICardService>();
            serviceMock.Setup(x => x.GetOrderedByLeagueWithMixedCards()).Returns(orderedCards);

            var controller = new HomeController(serviceMock.Object);
            var result = controller.Index() as ViewResult;

            serviceMock.Verify(x => x.GetOrderedByLeagueWithMixedCards(), Times.Once());
            result.WithNameHasModelType<IDictionary<string, List<Card>>>(string.Empty); 
            Assert.Equal(2, orderedCards.Count);
        }
    }
}
