namespace PhotoContest.Models
{
    public class ContestWinners
    {
        public int Id { get; set; }

        public int ContestId { get; set; }

        public virtual Contest Contest { get; set; }

        public int Place { get; set; }

        public string WinnerId { get; set; }

        public virtual User Winner { get; set; }
    }
}