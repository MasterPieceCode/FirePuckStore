using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.DAL.Repositories.Interfaces;
using Xunit;
using Moq;

namespace FirePuckStore.Tests.Services
{
    public class CardServiceUnitTests
    {
        [Fact]
        public void TestGetOrderedByLeagueWithMixedCards()
        {
            var mockRepository = new Mock<ICardRepository>();
            var randomCards = TestHelper.CreateRandomCards();

            mockRepository.Setup(cardRepository => cardRepository.GetCards()).Returns(randomCards);

            var service = new CardService(mockRepository.Object);

            var actual = service.GetOrderedByLeagueWithMixedCards();

            mockRepository.Verify(cardRepository => cardRepository.GetCards(), Times.Once());
            

//            var orderedCards = cards.GroupBy(c => c.Category);
/*
            foreach (var card in orderedCards)
            {
                result[card.Key] = card.ToList();
                // get 5 cards from every category to mixed category
                result["Mixed"].AddRange(card.Take(MixedCardsCount).Select(ConverToMixedLeagueCard));
            }
            return result;
*/
        }
    }
}
