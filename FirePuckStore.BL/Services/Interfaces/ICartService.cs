using System.Collections.Generic;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICartService
    {
        List<Order> GetCart();
    }
}
