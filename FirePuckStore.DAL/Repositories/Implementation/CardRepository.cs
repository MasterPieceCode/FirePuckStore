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
        public List<Card> GetAll()
        {
            return DbContext.Cards.ToList();
        }

        public List<Card> GetAllCardsWithPlayerInfo()
        {
            return DbContext.Cards.Include(c => c.Player).ToList();
        }

        public Card GetById(int cardId)
        {
            return DbContext.Cards.Find(cardId);
        }

        public void Delete(int entityId)
        {
            var card = DbContext.Cards.Find(entityId); 
            DbContext.Cards.Remove(card);
            DbContext.SaveChanges();
        }

        public Card GetByIdAsNoTracking(int playerId)
        {
            return DbContext.Cards.Where(c => c.CardId == playerId).AsNoTracking().FirstOrDefault();
        }

        public void Delete(Card card)
        {
            DbContext.Cards.Remove(card);
            DbContext.SaveChanges();
        }

        public void Add(Card card)
        {
            DbContext.Cards.Add(card);
            DbContext.SaveChanges();
        }

        public void Update(Card card)
        {
            DbContext.Entry(card).State = EntityState.Modified;
            DbContext.SaveChanges();
        }
    }
}
