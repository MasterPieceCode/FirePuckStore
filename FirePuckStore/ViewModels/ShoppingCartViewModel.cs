using System.Collections.Generic;
using FirePuckStore.Models;

namespace FirePuckStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Order> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}