using System.ComponentModel.DataAnnotations;

namespace FirePuckStore.Models
{
    public class League
    {
        public int LeagueId { get; set; }

        [Display(Name = "League")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string LogoPath { get; set; }

        public string WebSite { get; set; }
    }
}