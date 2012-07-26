using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public abstract class GenericService<T> : IGenericService<T> where T : class
    {
        #region Properties

        protected IGenericRepository<T> Repository { get; private set; }
        protected IFileService FileService { get; private set; }

        #endregion

        #region Constructor

        protected GenericService(IGenericRepository<T> repository, IFileService fileService)
        {
            Repository = repository;
            FileService = fileService;
        }

        #endregion

        #region Abstract Members

        protected abstract int GetEntityId(T entity);

        #endregion

        #region IGenericService Implementation

        public virtual List<T> GetAll()
        {
            return Repository.GetAll();
        }

        public T GetById(int id)
        {
            var result = Repository.GetById(id);

            if (result == null)
            {
                throw new KeyNotFoundException(string.Format("Entiy with id {0} not found", id));
            }

            return result;
        }

        public void Add(T entity)
        {
            var fileUploadable = entity as IFileUploadable;
            if (fileUploadable != null && fileUploadable.FileInput != null)
            {
                fileUploadable.ImageUrl = UploadImage(fileUploadable.FileInput);
            }
            Repository.Add(entity);
        }

        public void Update(T entity)
        {
            var dbCard = Repository.GetByIdAsNoTracking(GetEntityId(entity));

            if (dbCard == null)
            {
                throw new KeyNotFoundException(string.Format("Card with id {0} not found", GetEntityId(entity)));
            }

            var fileUploadable = entity as IFileUploadable;
            var dbCardIsfileUploadable = dbCard as IFileUploadable;

            if (fileUploadable != null && dbCardIsfileUploadable != null)
            {

                if (fileUploadable.FileInput == null)
                {
                    fileUploadable.ImageUrl = dbCardIsfileUploadable.ImageUrl;
                }
                else
                {
                    if (!string.IsNullOrEmpty(dbCardIsfileUploadable.ImageUrl))
                    {
                        DeleteUploadedImageFromServer(dbCardIsfileUploadable.ImageUrl);
                    }

                    fileUploadable.ImageUrl = UploadImage(fileUploadable.FileInput);
                }
            }

            Repository.Update(entity);
        }

        public void Delete(int entityId)
        {
            var entity = GetById(entityId);

            var fileUploadable = entity as IFileUploadable;

            if (fileUploadable != null && !string.IsNullOrEmpty(fileUploadable.ImageUrl))
            {
                DeleteUploadedImageFromServer(fileUploadable.ImageUrl);
            }

            Repository.Delete(GetEntityId(entity));
        }

        public void Dispose()
        {
            Repository.Dispose();
        }

        #endregion

        #region Helper Methods

        private string UploadImage(HttpPostedFileBase fileInput)
        {
            var imagePhysicalPath = FileService.GetPhysicalPath(HttpContext.Current, Constants.CardImagesServerPath);
            return Path.Combine(Constants.CardImagesServerPath, FileService.UploadToServerPath(imagePhysicalPath, fileInput));
        }

        private void DeleteUploadedImageFromServer(string imageUrl)
        {
            if (imageUrl.Equals(Constants.DefaultmageServerPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            FileService.DeleteFileFromServer(FileService.GetPhysicalPath(HttpContext.Current, imageUrl));
        }

        #endregion
    }
}
