using System;
using System.Collections.Generic;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICardService : IDisposable
    {
        IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards();
        List<Card> GetAllCards();
        void DeleteCard(int cardId);
        Card GetById(int cardId);
        void Add(Card card);
        void Update(Card card);
    }
}
