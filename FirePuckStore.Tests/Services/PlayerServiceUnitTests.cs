using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;
using Moq;
using Xunit;

namespace FirePuckStore.Tests.Services
{
    public class PlayerServiceUnitTests : GenericServiceUnitTests<IPlayerRepository, Player>
    {
        [Fact]
        public void TestGetAll()
        {
            var expectedCards = CreateExpectedEntites();


            RepositoryMock.Setup(repository => repository.GetAll()).Returns(expectedCards);

            var actualCards = Service.GetAll();

            RepositoryMock.Verify(cardRepository => cardRepository.GetAll(), Times.Once());

            Assert.Equal(expectedCards.Count, actualCards.Count);
            Assert.Equal(expectedCards[0], actualCards[0], GetEntityComparer());
            Assert.Equal(expectedCards[1], actualCards[1], GetEntityComparer());
        }

        protected override IGenericService<Player> CreateService()
        {
            return new PlayerService(RepositoryMock.Object, FileServiceMock.Object);
        }

        protected override IEqualityComparer<Player> GetEntityComparer()
        {
            return new PlayerComparer();
        }

        protected override List<Player>CreateExpectedEntites()
        {
            return new List<Player>
                       {
                           TestHelper.CreateRandomPlayerWithId(TestHelper.CreateRandomNumber(1, 5)),
                           TestHelper.CreateRandomPlayerWithId(TestHelper.CreateRandomNumber(6, 10))
                       };
        }

        protected override Player CreateRandomEntity(int entityId)
        {
            return TestHelper.CreateRandomPlayerWithId(entityId);
        }

        class PlayerComparer : IEqualityComparer<Player>
        {
            public bool Equals(Player x, Player y)
            {
                return x.PlayerId.Equals(y.PlayerId);
            }

            public int GetHashCode(Player obj)
            {
                return obj.PlayerId.GetHashCode();
            }
        }
    }
}
