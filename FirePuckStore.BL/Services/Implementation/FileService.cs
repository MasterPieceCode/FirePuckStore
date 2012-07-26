using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FirePuckStore.BL.Services.Interfaces;

namespace FirePuckStore.BL.Services.Implementation
{
    public class FileService : IFileService
    {
        #region Consts and Fields

        private const string RandomStringLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly Random _random = new Random();

        #endregion

        #region IFileService Implementation

        public string UploadToServerPath(string physicalPath, HttpPostedFileBase fileInput)
        {
            string imageFileName;
            string imageFullPath;

            do
            {
                imageFullPath = GetImagePath(physicalPath, imageFileName = GetImageFileName(fileInput));
            } 
            while (File.Exists(imageFullPath));

            var directoryInfo = Directory.GetParent(imageFullPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            fileInput.SaveAs(imageFullPath);
            return imageFileName;
        }

        public void DeleteFileFromServer(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }

        public string GetPhysicalPath(HttpContext context, string serverPath)
        {
            return context.Server.MapPath(serverPath);
        }

        #endregion

        #region Helper Methods

        private static string GetImagePath(string serverPath, string imageFileName)
        {
            return Path.Combine(serverPath, imageFileName);
        }

        private static string GetImageFileName(HttpPostedFileBase fileInput)

        {
            return string.Format("{0}{1}", CreateRandomString(10), Path.GetExtension(fileInput.FileName));
        }

        private static string CreateRandomString(int length)
        {
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                sb.Append(RandomStringLetters[_random.Next(RandomStringLetters.Length)]);
            }
            return sb.ToString();
        }

        #endregion
    }
}
