using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FirePuckStore.Models
{
    public class Player : IFileUploadable
    {
        public int PlayerId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]
        public HttpPostedFileBase FileInput { get; set; }

        [Required(ErrorMessage = "The League field is required")]
        public virtual int LeagueId { get; set; }

        public virtual League League { get; set; }

        public Player()
        {
            ImageUrl = Constants.DefaultmageServerPath;
        }
    }
}