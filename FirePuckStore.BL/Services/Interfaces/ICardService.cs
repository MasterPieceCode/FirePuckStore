using System;
using System.Collections.Generic;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICardService : IGenericService<Card>, IDisposable
    {
        IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards();
        /*List<Card> GetAll();*/
/*
        void Delete(int cardId);
        Card GetById(int cardId);
        void Add(Card card);
        void Update(Card card);
*/
    }
}
