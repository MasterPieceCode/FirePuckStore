using System.Collections.Generic;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICardService
    {
        IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards();
        List<Card> GetAllCards();
    }
}
