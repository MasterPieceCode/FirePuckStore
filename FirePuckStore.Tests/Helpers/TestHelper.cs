using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.Tests
{
    public class TestHelper
    {
        public static Dictionary<string, List<Card>> GetRandomDictionaryCardData()
        {
            var result = new Dictionary<string, List<Card>>{{"Category1", new List<Card>()}, {"Category2", new List<Card>()}};
            return result;
        }

        public static List<Order> GetRandomOrders()
        {
            return new List<Order>() {new Order(), new Order()};
        }
    }
}
