using System.Collections.Generic;
using System.Linq;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private const int MixedCardsCount = 5;

        public CardService(ICardRepository cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards()
        {
            var cards = _cardRepository.GetCards().ToList();

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

        public List<Card> GetAllCards()
        {
            return _cardRepository.GetCardsWithPlayerInfo();
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