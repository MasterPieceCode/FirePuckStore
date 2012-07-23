using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Text;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Implementation
{
    public class CartRepository : AbstractRepository, ICartRepository
    {
        public List<Order> GetCartOrder()
        {
            return DbContext.Orders.Include(o => o.Card).ToList();
        }

        public Order GetOrderForCardId(int cardId)
        {
            return DbContext.Orders.AsNoTracking().FirstOrDefault(o => o.CardId == cardId);
        }

        public void AddOrder(Order orderForCard)
        {
            DbContext.Orders.Add(orderForCard);
            DbContext.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            DbContext.Entry(order).State = EntityState.Modified;
            DbContext.SaveChanges();   
        }
    }
}
