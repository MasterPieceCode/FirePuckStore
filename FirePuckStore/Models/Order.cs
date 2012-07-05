using System.Collections.Generic;

namespace FirePuckStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public virtual Card Card { get; set; }

        public int CardId { get; set; }

        public int Quantity { get; set; }
    }
}