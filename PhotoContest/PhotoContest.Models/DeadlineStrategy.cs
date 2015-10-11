namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;

    public class DeadlineStrategy
    {
        public int Id { get; set; }

        public DeadlineStrategyType DeadlineStrategyType { get; set; }
    }
}