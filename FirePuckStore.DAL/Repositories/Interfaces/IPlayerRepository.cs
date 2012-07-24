using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Interfaces
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        List<League> GetLeagues();
    }
}
