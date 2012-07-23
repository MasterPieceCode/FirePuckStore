using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.Entity;
using System.Text;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Implementation
{
    public class CardRepository : AbstractRepository, ICardRepository
    {
        public List<Card> GetCards()
        {
            return DbContext.Cards.ToList();
        }

        public List<Card> GetAllCardsWithPlayerInfo()
        {
            return DbContext.Cards.Include(c => c.Player).ToList();
        }

        public Card FindCardById(int cardId)
        {
            return DbContext.Cards.Find(cardId);
        }

        public Card FindCardByIdAsNoTracking(int cardId)
        {
            return DbContext.Cards.Where(c => c.CardId == cardId).AsNoTracking().FirstOrDefault();
        }

        public void DeleteCard(Card card)
        {
            DbContext.Cards.Remove(card);
            DbContext.SaveChanges();
        }

        public void AddCard(Card card)
        {
            DbContext.Cards.Add(card);
            DbContext.SaveChanges();
        }

        public void UpdateCard(Card card)
        {
            DbContext.Entry(card).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
    }
}
