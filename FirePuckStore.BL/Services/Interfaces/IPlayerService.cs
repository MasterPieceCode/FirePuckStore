using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Implementation;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Interfaces
{
    public interface IPlayerService : IGenericService<Player>
    {
        List<League> GetLeagues();
    }
}
