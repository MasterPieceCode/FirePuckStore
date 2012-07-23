using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Interfaces
{
    public interface ICartRepository : IDisposable
    {
        List<Order> GetCartOrder();
        Order GetOrderForCardId(int cardId);
        void AddOrder(Order orderForCard);
        void UpdateOrder(Order order);
        void DeleteOrder(int order);
    }
}
