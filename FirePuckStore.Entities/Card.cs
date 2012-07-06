using System;
using System.ComponentModel.DataAnnotations;

namespace FirePuckStore.Models
{
    public class Card : ICloneable
    {
        public int CardId { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        public string Description { get; set; }

        public int PlayerId { get; set; }

        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public virtual Player Player { get; set; }

        public Card Clone()
        {
            var cloneable = (ICloneable)this;
            return (Card)cloneable.Clone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }
}