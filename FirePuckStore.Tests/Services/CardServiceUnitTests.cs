using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;
using FirePuckStore.Tests.Helpers;
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

            var randomCards = TestHelper.Create2RandomCardsWithDifferentCategories();

            mockRepository.Setup(cardRepository => cardRepository.GetCards()).Returns(randomCards);

            var service = new CardService(mockRepository.Object);

            var actual = service.GetOrderedByLeagueWithMixedCards();

            mockRepository.Verify(cardRepository => cardRepository.GetCards(), Times.Once());

            var categoriesCount = randomCards.Count + 1; // cards categories in the list plus one mixed category
            Assert.Equal(actual.Count, categoriesCount);

            const string mixedCategoryName = "Mixed";
            Assert.True(actual.ContainsKey(mixedCategoryName));

            var cardCountInMixedCategory = actual.Where(cardCategory => !cardCategory.Key.Equals(mixedCategoryName, StringComparison.OrdinalIgnoreCase))
                                                   .Sum(cardCategory => cardCategory.Value.Count);
            Assert.Equal(actual[mixedCategoryName].Count, cardCountInMixedCategory);

            AssertDictionaryContainsCategoryWithOneCardWithin(actual, randomCards[0].Category, randomCards[0]);
            AssertDictionaryContainsCategoryWithOneCardWithin(actual, randomCards[1].Category, randomCards[1]);
        }

        private static void AssertDictionaryContainsCategoryWithOneCardWithin(IDictionary<string, List<Card>> actual, string category, Card card)
        {
            Assert.True(actual.ContainsKey(category));
            Assert.Equal(actual[card.Category].Count, 1);
            actual[card.Category].ContainsCard(card);
        }

        [Fact]
        public void TestGetAllCards()
        {
            var mockRepository = new Mock<ICardRepository>();

            var expected = TestHelper.Create2RandomCardsWithDifferentCategories();

            mockRepository.Setup(cardRepository => cardRepository.GetCardsWithPlayerInfo()).Returns(expected);

            var service = new CardService(mockRepository.Object);
            var actual = service.GetAllCards();

            mockRepository.Verify(cardRepository => cardRepository.GetCardsWithPlayerInfo(), Times.Once());

            Assert.Equal(actual.Count, 2);

            actual.ContainsCard(expected[0]);
            actual.ContainsCard(expected[1]);
        }

        [Fact]
        public void TestDeleteCardById()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var card = TestHelper.CreateRandomCardWithId(cardId);

            mockRepository.Setup(cardRepository => cardRepository.FindCardById(cardId)).Returns(card);

            var service = new CardService(mockRepository.Object);
            service.DeleteCard(cardId);

            mockRepository.Verify(cardRepository => cardRepository.FindCardById(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.DeleteCard(card), Times.Once());
        }

        [Fact]
        public void TestDeleteCardThrowExceptionIfCardNotFound()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);

            mockRepository.Setup(cardRepository => cardRepository.FindCardById(cardId)).Returns((Card)null);

            var service = new CardService(mockRepository.Object);

            Assert.Throws<KeyNotFoundException>(() => service.DeleteCard(cardId));

            mockRepository.Verify(cardRepository => cardRepository.FindCardById(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.DeleteCard(null), Times.Never());
        }
    }
}
