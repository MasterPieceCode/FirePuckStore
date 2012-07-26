using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirePuckStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public virtual Card Card { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }

        [NotMapped]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
    }
}