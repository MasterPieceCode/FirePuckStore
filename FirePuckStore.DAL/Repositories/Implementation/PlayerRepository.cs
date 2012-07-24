using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using FirePuckStore.DAL.Repositories.Interfaces;
using FirePuckStore.Models;

namespace FirePuckStore.DAL.Repositories.Implementation
{
    public class PlayerRepository : AbstractRepository, IPlayerRepository
    {
        public List<Player> GetAll()
        {
            return DbContext.Players.Include(p => p.League).ToList();
        }

        public void Delete(int playerId)
        {
            var player = DbContext.Players.Find(playerId);
            DbContext.Players.Remove(player);
            DbContext.SaveChanges();
        }

        public Player GetByIdAsNoTracking(int playerId)
        {
            return DbContext.Players.Where(player => player.PlayerId == playerId).AsNoTracking().FirstOrDefault();
        }

        public Player GetById(int id)
        {
            return DbContext.Players.Find(id);
        }

        public void Add(Player entity)
        {
            DbContext.Players.Add(entity);
            DbContext.SaveChanges();
        }

        public void Update(Player player)
        {
            DbContext.Entry(player).State = EntityState.Modified;
            DbContext.SaveChanges();
        }

        public List<League> GetLeagues()
        {
            return DbContext.Leagues.ToList();
        }
    }
}
