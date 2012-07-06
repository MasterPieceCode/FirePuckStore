using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
