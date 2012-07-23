using System;
using System.Collections.Generic;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICartService : IDisposable
    {
        List<Order> GetCart();
        Order Place1QuantityOrderAndReturnSummaryOrderForCard(int cardId);
    }
}
