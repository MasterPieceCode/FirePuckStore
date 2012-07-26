using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirePuckStore.BL.Services.Interfaces;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.BL.Services.Implementation
{
    public class PlayerService : GenericService<Player>, IPlayerService
    {
        #region Fieds

        private readonly IPlayerRepository _repository;

        #endregion

        #region Constructor

        public PlayerService(IPlayerRepository repository, IFileService fileService) : base(repository, fileService)
        {
            _repository = repository;
        }

        #endregion

        #region IPlayerService Implementation

        public List<League> GetLeagues()
        {
            return _repository.GetLeagues();
        }

        #endregion

        #region GenericService Overriden

        protected override int GetEntityId(Player entity)
        {
            return entity.PlayerId;
        }

        #endregion
    }
}
