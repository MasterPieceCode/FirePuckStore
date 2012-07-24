using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.BL.Services.Interfaces;
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

            var mockFileService = new Mock<IFileService>();
            var service = new CardService(mockRepository.Object, mockFileService.Object);

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

            var expectedCards = new List<Card>()
                                    {
                                        TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomNumber(1, 5)),
                                        TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomNumber(6, 10)),
                                    };

            var fileServiceMock = new Mock<IFileService>();

            mockRepository.Setup(repository => repository.GetAllCardsWithPlayerInfo()).Returns(expectedCards);
            var service = new CardService(mockRepository.Object, fileServiceMock.Object);

            var actualCards = service.GetAllCards();

            mockRepository.Verify(cardRepository => cardRepository.GetAllCardsWithPlayerInfo(), Times.Once());

            Assert.Equal(expectedCards.Count, actualCards.Count);
            Assert.Equal(expectedCards[0], actualCards[0], new CardComparer());
            Assert.Equal(expectedCards[1], actualCards[1], new CardComparer());
        }

        [Fact]
        public void TestGetCardById()
        {
            var mockRepository = new Mock<ICardRepository>();

            var expectedCardId = TestHelper.CreateRandomId();
            var expected = TestHelper.CreateRandomCardWithId(expectedCardId);

            mockRepository.Setup(repository => repository.FindCardById(expectedCardId)).Returns(expected);
            var fileServiceMock = new Mock<IFileService>();
            var service = new CardService(mockRepository.Object, fileServiceMock.Object);

            var actual = service.GetById(expectedCardId);

            mockRepository.Verify(cardRepository => cardRepository.FindCardById(expectedCardId), Times.Once());
            Assert.Equal(expected, actual, new CardComparer());
        }

        [Fact]
        public void TestAddCardWithImageInput()
        {
            var mockRepository = new Mock<ICardRepository>();

            var expectedCard = TestHelper.CreateRandomCardWithId(TestHelper.CreateRandomId());
            var postedFileMock = new Mock<HttpPostedFileBase>();
            expectedCard.FileInput = postedFileMock.Object;

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);

            var fileServiceMock = new Mock<IFileService>();

            SetupFileServiceForImageUploading(fileServiceMock, physicalPath, postedFileMock, uploadedFileName);

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);

            service.Add(expectedCard);

            VerifyImageWasUploadedToPhysicalPath(fileServiceMock, physicalPath, expectedCard.ImageUrl, uploadedFileName, postedFileMock);
            mockRepository.Verify(cardRepository => cardRepository.AddCard(expectedCard), Times.Once());
        }

        [Fact]
        public void TestUpdateCardWithoutImageInput()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomId();
            var expectedCard = TestHelper.CreateRandomCardWithId(cardId);
            var dbCard = TestHelper.CreateRandomCardWithId(cardId);
            dbCard.ImageUrl = TestHelper.CreateRandomString(10);

            mockRepository.Setup(repository => repository.FindCardByIdAsNoTracking(cardId)).Returns(dbCard);

            var fileServiceMock = new Mock<IFileService>();

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);
            service.Update(expectedCard);

            mockRepository.Verify(repository => repository.FindCardByIdAsNoTracking(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.UpdateCard(expectedCard), Times.Once());            
            Assert.Equal(expectedCard.ImageUrl, dbCard.ImageUrl);
        }

        [Fact]
        public void TestUpdateCardWithImageInputAndNotAssignedImageUrl()
        {
            var mockRepository = new Mock<ICardRepository>();
            var cardId = TestHelper.CreateRandomId();
            var expectedCard = TestHelper.CreateRandomCardWithId(cardId);

            var postedFileMock = new Mock<HttpPostedFileBase>();
            expectedCard.FileInput = postedFileMock.Object;

            var fileServiceMock = new Mock<IFileService>();

            mockRepository.Setup(repository => repository.FindCardByIdAsNoTracking(cardId)).Returns(expectedCard);

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);

            SetupFileServiceForImageUploading(fileServiceMock, physicalPath, postedFileMock, uploadedFileName);

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);
            service.Update(expectedCard);

            mockRepository.Verify(repository => repository.FindCardByIdAsNoTracking(cardId), Times.Once());
            VerifyImageWasUploadedToPhysicalPath(fileServiceMock, physicalPath, expectedCard.ImageUrl, uploadedFileName, postedFileMock);

            fileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(expectedCard.ImageUrl), Times.Never());
            mockRepository.Verify(cardRepository => cardRepository.UpdateCard(expectedCard), Times.Once());
        }

        [Fact]
        public void TestUpdateCardWithImageInputAndAssignedImageUrl()
        {
            var mockRepository = new Mock<ICardRepository>();
            var cardId = TestHelper.CreateRandomId();
            var expectedCard = TestHelper.CreateRandomCardWithId(cardId);
            var postedFileMock = new Mock<HttpPostedFileBase>();
            expectedCard.FileInput = postedFileMock.Object;
            var dbCard = TestHelper.CreateRandomCardWithId(cardId);
            dbCard.ImageUrl = TestHelper.CreateRandomString(15);
            var fileServiceMock = new Mock<IFileService>();

            mockRepository.Setup(repository => repository.FindCardByIdAsNoTracking(cardId)).Returns(dbCard);

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);
            var previousUploadedImagePhysicalPath = TestHelper.CreateRandomString(15);

            fileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, dbCard.ImageUrl)).Returns(previousUploadedImagePhysicalPath);

            SetupFileServiceForImageUploading(fileServiceMock, physicalPath, postedFileMock, uploadedFileName);

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);
            service.Update(expectedCard);

            mockRepository.Verify(repository => repository.FindCardByIdAsNoTracking(cardId), Times.Once());
            fileServiceMock.Verify(fileService => fileService.GetPhysicalPath(null, dbCard.ImageUrl), Times.Once());
            fileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(previousUploadedImagePhysicalPath), Times.Once());
            VerifyImageWasUploadedToPhysicalPath(fileServiceMock, physicalPath, expectedCard.ImageUrl, uploadedFileName, postedFileMock);

            mockRepository.Verify(cardRepository => cardRepository.UpdateCard(expectedCard), Times.Once());
        }

        private static void SetupFileServiceForImageUploading(Mock<IFileService> fileServiceMock, string physicalPath, Mock<HttpPostedFileBase> postedFileMock,
                                                              string imageFileName)
        {
            fileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, CardService.CardImagesServerPath)).Returns(physicalPath);
            fileServiceMock.Setup(fileService => fileService.UploadToServerPath(physicalPath, postedFileMock.Object)).Returns(imageFileName);
        }

        private static void VerifyImageWasUploadedToPhysicalPath(Mock<IFileService> fileServiceMock, string physicalPath, string actualServerPath, string uploadedFileName, Mock<HttpPostedFileBase> postedFileMock)
        {
            fileServiceMock.Verify(fileService => fileService.GetPhysicalPath(null, CardService.CardImagesServerPath), Times.Once());
            fileServiceMock.Verify(fileService => fileService.UploadToServerPath(physicalPath, postedFileMock.Object), Times.Once());
            var expectedServerPath = Path.Combine(CardService.CardImagesServerPath, uploadedFileName);
            Assert.Equal(actualServerPath, expectedServerPath);
        }

        [Fact]
        public void TestDeleteCardWithoutImageUploadedById()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var card = TestHelper.CreateRandomCardWithId(cardId);

            mockRepository.Setup(cardRepository => cardRepository.FindCardById(cardId)).Returns(card);

            var fileServiceMock = new Mock<IFileService>();

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);
            service.DeleteCard(cardId);

            mockRepository.Verify(cardRepository => cardRepository.FindCardById(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.DeleteCard(card), Times.Once());
        }

        [Fact]
        public void TestDeleteCardWithImageUploadedById()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);
            var card = TestHelper.CreateRandomCardWithId(cardId);
            card.ImageUrl = TestHelper.CreateRandomString(5);
            var cardImagePhysicalPath = TestHelper.CreateRandomString(15);
            mockRepository.Setup(cardRepository => cardRepository.FindCardById(cardId)).Returns(card);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, card.ImageUrl)).Returns(cardImagePhysicalPath);

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);
            service.DeleteCard(cardId);

            fileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(cardImagePhysicalPath), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.FindCardById(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.DeleteCard(card), Times.Once());
        }

        [Fact]
        public void TestDeleteCardThrowExceptionIfCardNotFound()
        {
            var mockRepository = new Mock<ICardRepository>();

            var cardId = TestHelper.CreateRandomNumber(1, 10);

            var fileServiceMock = new Mock<IFileService>();

            mockRepository.Setup(cardRepository => cardRepository.FindCardById(cardId)).Returns((Card)null);

            var service = new CardService(mockRepository.Object, fileServiceMock.Object);

            Assert.Throws<KeyNotFoundException>(() => service.DeleteCard(cardId));

            mockRepository.Verify(cardRepository => cardRepository.FindCardById(cardId), Times.Once());
            mockRepository.Verify(cardRepository => cardRepository.DeleteCard(null), Times.Never());
        }
    }
}
