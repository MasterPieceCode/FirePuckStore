using System.Collections.Generic;
using System.Linq;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService()
        {
            
        }

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<Order> GetCart()
        {
            return _cartRepository.GetOrders();
        }
    }
}