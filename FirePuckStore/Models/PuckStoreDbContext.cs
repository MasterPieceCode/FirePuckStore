using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FirePuckStore.Models
{
    public class PuckStoreDbContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Order> Orders { get; set; } 
    }
}