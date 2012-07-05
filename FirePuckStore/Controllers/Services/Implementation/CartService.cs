using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FirePuckStore.Controllers.Services.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.Controllers.Services.Implementation
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