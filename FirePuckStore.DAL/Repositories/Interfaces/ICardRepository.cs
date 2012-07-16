using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Interfaces
{
    public interface ICardRepository
    {
        List<Card> GetCards();
        List<Card> GetCardsWithPlayerInfo();
        Card FindCardById(int cardId);
        void DeleteCard(Card card);
    }
}
