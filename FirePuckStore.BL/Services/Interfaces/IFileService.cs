using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface IFileService
    {
        string UploadToServerPath(string physicalPath, HttpPostedFileBase fileInput);
        void DeleteFileFromServer(string fileName);
        string GetPhysicalPath(HttpContext context, string serverPath);
    }
}
