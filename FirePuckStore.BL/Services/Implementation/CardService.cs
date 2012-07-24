using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class CardService : GenericService<Card>, ICardService
    {
        private readonly ICardRepository _cardRepository;
        private const int MixedCardsCount = 5;

        public CardService(ICardRepository cardRepository, IFileService fileService) : base(cardRepository, fileService)
        {
            _cardRepository = cardRepository;
        }

        public IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards()
        {
            var cards = Repository.GetAll().ToList();

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

        
        public override  List<Card> GetAll()
        {
            return _cardRepository.GetAllCardsWithPlayerInfo();
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

        public void Dispose()
        {
            Repository.Dispose();
        }

        protected override int GetEntityId(Card entity)
        {
            return entity.CardId;
        }
    }
}