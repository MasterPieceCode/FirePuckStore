using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FirePuckStore.Models;

namespace FirePuckStore.DAL
{
    public class SampleDataInitializer : DropCreateDatabaseIfModelChanges<PuckStoreDbContext>
    {
        protected override void Seed(PuckStoreDbContext context)
        {
            var leagues = new List<League>
                              {
                                  new League {Name = "NHL", Description = "The best league in the North Americe"},
                                  new League {Name = "KHL", Description = "The best league in the Europe"}
                              };

            leagues.ForEach(l => context.Leagues.Add(l));

            var players = new List<Player>
                              {
                                  new Player{FullName = "Alex Ovechkin", League = leagues.Single(x => x.Name == "NHL")},

                                  new Player{FullName = "Evgeny Malkin", League = leagues.Single(x => x.Name == "NHL")},
                                  new Player{FullName = "Pavel Datsyk", League = leagues.Single(x => x.Name == "NHL")},
                                  new Player{FullName = "Alex Morozov", League = leagues.Single(x => x.Name == "KHL")},
                                  new Player{FullName = "Branko Radivoevich",League = leagues.Single(x => x.Name == "KHL")},

                              };
            players.ForEach(p => context.Players.Add(p));

            var cards = new List<Card>
                            {
                                new Card{Player = players.Single(p => p.FullName == "Alex Ovechkin"), Category = "NHL", Quantity = 15, Price = 25.5m, ImageUrl = "/Content/images/Ovechkin1.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Alex Ovechkin"), Category = "NHL", Quantity = 5, Price = 37.4m, ImageUrl = "/Content/images/Ovechkin2.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Alex Ovechkin"), Category = "NHL", Quantity = 12, Price = 30.5m, ImageUrl = "/Content/images/Ovechkin3.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Evgeny Malkin"), Category = "NHL", Quantity = 20, Price = 40.5m, ImageUrl = "/Content/images/Malkin1.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Evgeny Malkin"), Category = "NHL", Quantity = 17, Price = 25.5m, ImageUrl = "/Content/images/Malkin2.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Pavel Datsyk"), Category = "NHL", Quantity = 9, Price = 24.5m, ImageUrl = "/Content/images/Datsyk1.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Pavel Datsyk"), Category = "NHL", Quantity = 23, Price = 29.5m, ImageUrl = "/Content/images/Datsyk2.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Alex Morozov"), Category = "KHL", Quantity = 14, Price = 25.7m, ImageUrl = "/Content/images/Morozov1.jpg"},
                                new Card{Player = players.Single(p => p.FullName == "Branko Radivoevich"), Category = "KHL", Quantity = 17, Price = 75.6m, ImageUrl = "/Content/images/Radivoevich1.jpg"},

                            };
           cards.ForEach(c => context.Cards.Add(c));

            var orders = new List<Order>
                            {
                                new Order {Card = cards.First(c => c.Player.FullName == "Alex Ovechkin"), Quantity = 2},
                                new Order {Card = cards.First(c => c.Player.FullName == "Alex Morozov"), Quantity = 3}
                            };

            orders.ForEach(o => context.Orders.Add(o));

            /*context.SaveChanges();*/
        }
    }
}