using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace FirePuckStore.Models
{
    public class Card : IFileUploadable
    {
        public int CardId { get; set; }

        [Display(Name = "Category")]
        [Required]
        public string Category { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "The Player field is required")]
        public int PlayerId { get; set; }

        [Required]
        [Range(0, 100)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        public virtual Player Player { get; set; }

        public string ImageUrl { get; set; }

        [NotMapped]
        public HttpPostedFileBase FileInput { get; set; }

        public Card()
        {
            ImageUrl = Constants.DefaultmageServerPath;
        }
    }
}