using System;
using System.Collections.Generic;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface ICardService : IGenericService<Card>
    {
        IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards();
    }
}
