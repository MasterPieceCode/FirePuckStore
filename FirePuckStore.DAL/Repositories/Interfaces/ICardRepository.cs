using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Interfaces
{
    public interface ICardRepository : IDisposable
    {
        List<Card> GetCards();
        List<Card> GetAllCardsWithPlayerInfo();
        Card FindCardById(int cardId);
        Card FindCardByIdAsNoTracking(int cardId);
        void DeleteCard(Card card);
        void AddCard(Card card);
        void UpdateCard(Card card);
    }
}
