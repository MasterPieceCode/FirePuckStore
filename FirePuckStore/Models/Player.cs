namespace FirePuckStore.Models
{
    public class Player
    {
        public int PlayerId { get; set; }

        public string FullName { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public virtual int LeagueId { get; set; }

        public virtual League League { get; set; }
    }
}