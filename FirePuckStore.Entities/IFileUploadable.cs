using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FirePuckStore.Models
{
    public interface IFileUploadable
    {
        string ImageUrl { get; set; }

        HttpPostedFileBase FileInput { get; set; }
    }
}
