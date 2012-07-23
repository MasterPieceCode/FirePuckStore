using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.Tests
{
    public class TestHelper
    {
        private const string RandomStringLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static readonly Random _random = new Random();

        public static string CreateRandomString(int length)
        {
            var sb = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                sb.Append(RandomStringLetters[_random.Next(RandomStringLetters.Length)]);
            }
            return sb.ToString();
        }

        public static int CreateRandomNumber(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        public static int CreateRandomId()
        {
            return CreateRandomNumber(1, 10);
        }

        public static Dictionary<string, List<Card>> GetRandomDictionaryCardData()
        {
            var result = new Dictionary<string, List<Card>>
                             {{"Category1", new List<Card>()}, {"Category2", new List<Card>()}};
            return result;
        }

        public static List<Order> GetRandomOrders()
        {
            return new List<Order>() {new Order(), new Order()};
        }

        public static List<Card> Create2RandomCardsWithDifferentCategories()
        {
            return new List<Card>
                       {
                           CreateRandomCardWithId(1),
                           CreateRandomCardWithId(2),
                       };
        }

        public static Card CreateRandomCardWithId(int cardId)
        {
            return new Card { CardId = cardId, Category = CreateRandomString(10), Quantity = CreateRandomNumber(1, 20) };
        }

        public static Player CreateRandomPlayerWithId(int playerId)
        {
            return new Player { PlayerId = playerId};
        }

        public static Order CreateRandomOrderForCardId(int cardId)
        {
            var result = new Order
                             {
                                 OrderId = CreateRandomNumber(1, 10),
                                 Quantity = CreateRandomNumber(1, 20)
                             };

            var card = CreateRandomCardWithId(CreateRandomNumber(1, 10));
            
            card.Price = CreateRandomNumber(20, 100);
            result.Card = card;

            return result;
        }
    }
}
