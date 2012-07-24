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
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository, IFileService fileService) : base(repository, fileService)
        {
            _repository = repository;
        }

        public List<League> GetLeagues()
        {
            return _repository.GetLeagues();
        }

        protected override int GetEntityId(Player entity)
        {
            return entity.PlayerId;
        }
    }
}
