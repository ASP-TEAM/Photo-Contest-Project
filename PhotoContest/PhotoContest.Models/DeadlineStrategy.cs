namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;

    public class DeadlineStrategy : AbstractStrategy
    {
        public int Id { get; set; }

        public DeadlineStrategyType DeadlineStrategyType { get; set; }
    }
}