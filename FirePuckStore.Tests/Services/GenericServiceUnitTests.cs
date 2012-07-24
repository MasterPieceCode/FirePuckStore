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
using Moq;
using Xunit;

namespace FirePuckStore.Tests.Services
{
    public abstract class GenericServiceUnitTests<TRep, TEntity>
        where TRep : class,  IGenericRepository<TEntity>
        where TEntity : IFileUploadable
    {
        private IGenericService<TEntity> _service;
        protected Mock<TRep> RepositoryMock { get; private set; }
        protected Mock<IFileService> FileServiceMock { get; private set; }

        protected IGenericService<TEntity> Service { get { return _service ?? (_service = CreateService()); } }

        protected abstract IGenericService<TEntity> CreateService();

        protected GenericServiceUnitTests()
        {
            RepositoryMock = new Mock<TRep>();
            FileServiceMock = new Mock<IFileService>();
        }


        protected abstract IEqualityComparer<TEntity> GetEntityComparer();

        protected abstract List<TEntity> CreateExpectedEntites();

        [Fact]
        public void TestGetById()
        {
            var expectedentityId = TestHelper.CreateRandomId();
            var expected = CreateRandomEntity(expectedentityId);

            RepositoryMock.Setup(repository => repository.GetById(expectedentityId)).Returns(expected);

            var actual = Service.GetById(expectedentityId);
            RepositoryMock.Verify(entityRepository => entityRepository.GetById(expectedentityId), Times.Once());
            Assert.Equal(expected, actual, GetEntityComparer());
        }

        protected abstract TEntity CreateRandomEntity(int entityId);
        
        [Fact]
        public void TestGetentityByIdThrowExceptionIfentityNotFound()
        {
            var expectedentityId = TestHelper.CreateRandomId();
            var expected = default(TEntity);

            RepositoryMock.Setup(repository => repository.GetById(expectedentityId)).Returns(expected);
            Assert.Throws<KeyNotFoundException>(() => Service.GetById(expectedentityId));
            RepositoryMock.Verify(entityRepository => entityRepository.GetById(expectedentityId), Times.Once());
        }

        
        [Fact]
        public void TestAddentityWithImageInput()
        {
            var expectedEntity = CreateRandomEntity(TestHelper.CreateRandomId());
            var postedFileMock = new Mock<HttpPostedFileBase>();

            expectedEntity.FileInput = postedFileMock.Object;

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);

            SetupFileServiceForImageUploading(physicalPath, postedFileMock, uploadedFileName);

            Service.Add(expectedEntity);

            VerifyImageWasUploadedToPhysicalPath(physicalPath, expectedEntity.ImageUrl, uploadedFileName, postedFileMock);
            RepositoryMock.Verify(entityRepository => entityRepository.Add(expectedEntity), Times.Once());
        }


        private void SetupFileServiceForImageUploading(string physicalPath, Mock<HttpPostedFileBase> postedFileMock, string imageFileName)
        {
            FileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, Constants.CardImagesServerPath)).Returns(physicalPath);
            FileServiceMock.Setup(fileService => fileService.UploadToServerPath(physicalPath, postedFileMock.Object)).Returns(imageFileName);
        }

        private  void VerifyImageWasUploadedToPhysicalPath(string physicalPath, string actualServerPath, string uploadedFileName, Mock<HttpPostedFileBase> postedFileMock)
        {
            FileServiceMock.Verify(fileService => fileService.GetPhysicalPath(null, Constants.CardImagesServerPath), Times.Once());
            FileServiceMock.Verify(fileService => fileService.UploadToServerPath(physicalPath, postedFileMock.Object), Times.Once());
            var expectedServerPath = Path.Combine(Constants.CardImagesServerPath, uploadedFileName);
            Assert.Equal(actualServerPath, expectedServerPath);
        }

        [Fact]
        public void TestUpdateentityWithoutImageInput()
        {
            var entityId = TestHelper.CreateRandomId();
            var expectedEntity = CreateRandomEntity(entityId);
            var dbentity = CreateRandomEntity(entityId);
            dbentity.ImageUrl = TestHelper.CreateRandomString(10);

            RepositoryMock.Setup(repository => repository.GetByIdAsNoTracking(entityId)).Returns(dbentity);

            Service.Update(expectedEntity);

            RepositoryMock.Verify(repository => repository.GetByIdAsNoTracking(entityId), Times.Once());
            RepositoryMock.Verify(entityRepository => entityRepository.Update(expectedEntity), Times.Once());
            Assert.Equal(expectedEntity.ImageUrl, dbentity.ImageUrl);
        }

        
        [Fact]
        public void TestUpdateentityWithImageInputAndNotAssignedImageUrl()
        {
            var entityId = TestHelper.CreateRandomId();
            var expectedEntity = CreateRandomEntity(entityId);

            var postedFileMock = new Mock<HttpPostedFileBase>();
            expectedEntity.FileInput = postedFileMock.Object;

            var fileServiceMock = new Mock<IFileService>();

            RepositoryMock.Setup(repository => repository.GetByIdAsNoTracking(entityId)).Returns(expectedEntity);

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);

            SetupFileServiceForImageUploading(physicalPath, postedFileMock, uploadedFileName);

            Service.Update(expectedEntity);

            RepositoryMock.Verify(repository => repository.GetByIdAsNoTracking(entityId), Times.Once());
            VerifyImageWasUploadedToPhysicalPath(physicalPath, (expectedEntity).ImageUrl, uploadedFileName, postedFileMock);

            fileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(expectedEntity.ImageUrl), Times.Never());
            RepositoryMock.Verify(entityRepository => entityRepository.Update(expectedEntity), Times.Once());
        }

        
        [Fact]
        public void TestUpdateentityWithImageInputAndAssignedImageUrl()
        {
            var entityId = TestHelper.CreateRandomId();
            var expectedEntity = CreateRandomEntity(entityId);
            var postedFileMock = new Mock<HttpPostedFileBase>();
            expectedEntity.FileInput = postedFileMock.Object;
            var dbentity = CreateRandomEntity(entityId);
            dbentity.ImageUrl = TestHelper.CreateRandomString(15);

            RepositoryMock.Setup(repository => repository.GetByIdAsNoTracking(entityId)).Returns(dbentity);

            var uploadedFileName = TestHelper.CreateRandomString(5);
            var physicalPath = TestHelper.CreateRandomString(15);
            var previousUploadedImagePhysicalPath = TestHelper.CreateRandomString(15);

            FileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, dbentity.ImageUrl)).Returns(previousUploadedImagePhysicalPath);

            SetupFileServiceForImageUploading(physicalPath, postedFileMock, uploadedFileName);

            Service.Update(expectedEntity);

            RepositoryMock.Verify(repository => repository.GetByIdAsNoTracking(entityId), Times.Once());
            FileServiceMock.Verify(fileService => fileService.GetPhysicalPath(null, dbentity.ImageUrl), Times.Once());
            FileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(previousUploadedImagePhysicalPath), Times.Once());
            VerifyImageWasUploadedToPhysicalPath(physicalPath, expectedEntity.ImageUrl, uploadedFileName, postedFileMock);

            RepositoryMock.Verify(entityRepository => entityRepository.Update(expectedEntity), Times.Once());
        }


        [Fact]
        public void TestUpdateentityThrowsExceptionIfentityIsNotFound()
        {
            var entityId = TestHelper.CreateRandomId();
            var expectedentity = default(TEntity);

            RepositoryMock.Setup(repository => repository.GetByIdAsNoTracking(entityId)).Returns(expectedentity);

            Assert.Throws<KeyNotFoundException>(() => Service.Update(CreateRandomEntity(entityId)));
            RepositoryMock.Verify(repository => repository.GetByIdAsNoTracking(entityId), Times.Once());
        }


        [Fact]
        public void TestDeleteentityWithoutImageUploadedById()
        {
            var entityId = TestHelper.CreateRandomNumber(1, 10);
            var entity = CreateRandomEntity(entityId);

            RepositoryMock.Setup(entityRepository => entityRepository.GetById(entityId)).Returns(entity);

            Service.Delete(entityId);

            RepositoryMock.Verify(entityRepository => entityRepository.GetById(entityId), Times.Once());
            RepositoryMock.Verify(entityRepository => entityRepository.Delete(entityId), Times.Once());
        }

        [Fact]
        public void TestDeleteentityWithImageUploadedById()
        {
            var entityId = TestHelper.CreateRandomNumber(1, 10);
            var entity = CreateRandomEntity(entityId);
            entity.ImageUrl = TestHelper.CreateRandomString(5);
            var entityImagePhysicalPath = TestHelper.CreateRandomString(15);
            RepositoryMock.Setup(entityRepository => entityRepository.GetById(entityId)).Returns(entity);

            FileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, entity.ImageUrl)).Returns(entityImagePhysicalPath);

            Service.Delete(entityId);

            FileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(entityImagePhysicalPath), Times.Once());
            RepositoryMock.Verify(entityRepository => entityRepository.GetById(entityId), Times.Once());
            RepositoryMock.Verify(entityRepository => entityRepository.Delete(entityId), Times.Once());
        }

        
        [Fact]
        public void TestDeleteentityThrowExceptionIfentityNotFound()
        {
            var entityId = TestHelper.CreateRandomNumber(1, 10);

            RepositoryMock.Setup(entityRepository => entityRepository.GetById(entityId)).Returns(default(TEntity));

            Assert.Throws<KeyNotFoundException>(() => Service.Delete(entityId));
        }


        [Fact]
        public void TestDefaultImageIsNotDeletedIfImageUrlSetToDefaultImagePath()
        {
            var entityId = TestHelper.CreateRandomNumber(1, 10);
            var entity = CreateRandomEntity(entityId);
            entity.ImageUrl = Constants.DefaultmageServerPath;
            var entityImagePhysicalPath = TestHelper.CreateRandomString(15);
            RepositoryMock.Setup(entityRepository => entityRepository.GetById(entityId)).Returns(entity);

            FileServiceMock.Setup(fileService => fileService.GetPhysicalPath(null, entity.ImageUrl)).Returns(entityImagePhysicalPath);

            Service.Delete(entityId);

            FileServiceMock.Verify(fileService => fileService.DeleteFileFromServer(entityImagePhysicalPath), Times.Never());
            RepositoryMock.Verify(entityRepository => entityRepository.GetById(entityId), Times.Once());
            RepositoryMock.Verify(entityRepository => entityRepository.Delete(entityId), Times.Once());
        }
    }
}
