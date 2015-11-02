namespace PhotoContest.Models
{
    public class Reward
    {
        public int Id { get; set; } 

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Place { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }
    }
}