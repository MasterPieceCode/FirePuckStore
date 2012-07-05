using System.Data.Entity;
using FirePuckStore.Models;

namespace FirePuckStore.DAL
{
    public class PuckStoreDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Order> Orders { get; set; } 
    }
}