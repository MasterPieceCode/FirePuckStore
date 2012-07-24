using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;
using FirePuckStore.Tests.Helpers;
using Moq;
using Xunit;

namespace FirePuckStore.Tests.Services
{
    public class CardServiceUnitTests : GenericServiceUnitTests<ICardRepository, Card>
    {
        [Fact]
        public void TestGetAllCards()
        {
            var expectedCards = CreateExpectedEntites();

            RepositoryMock.Setup(repository => repository.GetAllCardsWithPlayerInfo()).Returns(expectedCards);

            var actualCards = Service.GetAll();

            RepositoryMock.Verify(cardRepository => cardRepository.GetAllCardsWithPlayerInfo(), Times.Once());

            Assert.Equal(expectedCards.Count, actualCards.Count);
            Assert.Equal(expectedCards[0], actualCards[0], GetEntityComparer());
            Assert.Equal(expectedCards[1], actualCards[1], GetEntityComparer());
        }
        
        protected override IGenericService<Card> CreateService()
        {
            return new CardService(RepositoryMock.Object, FileServiceMock.Object);
        }

        protected override IEqualityComparer<Card> GetEntityComparer()
        {
            return new CardComparer();
        }

        protected override List<Card> CreateExpectedEntites()
        {
                return new List<Card>()
                       {
                           TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomNumber(1, 5)),
                           TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomNumber(6, 10)),
                       };
        }

        protected override Card CreateRandomEntity(int entityId)
        {
            return TestHelper.CreateRandomCardWithId(entityId);
        }
    }
}
