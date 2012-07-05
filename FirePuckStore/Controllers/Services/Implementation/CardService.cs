using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FirePuckStore.Controllers.Services.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.Controllers.Services.Implementation
{
    public class CardService : ICardService
    {
        private const int MixedCardsCount = 5;
        private readonly PuckStoreDbContext _dbContext;

        public CardService()
        {
            _dbContext = new PuckStoreDbContext();
        }

        public IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards()
        {
            var cards =_dbContext.Cards.ToList();

            var result = new Dictionary<string, List<Card>>{{"Mixed", new List<Card>()}};

            var orderedCards = cards.GroupBy(c => c.Category);
            foreach(var card in orderedCards)
            {
                result[card.Key] = card.ToList();
                // get 5 cards from every category to mixed category
                result["Mixed"].AddRange(card.Take(MixedCardsCount).Select(ConverToMixedLeagueCard));
            }
            return result;
        }

        private Card ConverToMixedLeagueCard(Card card)
        {
            var result = new Card
                             {
                                 CardId = card.CardId,
                                 Category = "Mixed",
                                 Description = card.Description,
                                 ImageUrl = card.ImageUrl,
                                 Player = card.Player,
                                 Price = card.Price,
                                 Quantity = card.Quantity
                             };
            return result;
        }
    }
}