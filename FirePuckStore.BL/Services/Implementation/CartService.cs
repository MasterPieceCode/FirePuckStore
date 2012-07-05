using System.Collections.Generic;
using System.Linq;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly PuckStoreDbContext _dbContext;

        public CartService()
        {
            _dbContext = new PuckStoreDbContext();
        }

        public List<Order> GetCart()
        {
            return _dbContext.Orders.ToList();
        }
    }
}