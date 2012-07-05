using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.Controllers.Services.Interfaces
{
    public interface ICardService
    {
        IDictionary<string, List<Card>> GetOrderedByLeagueWithMixedCards();
    }
}
