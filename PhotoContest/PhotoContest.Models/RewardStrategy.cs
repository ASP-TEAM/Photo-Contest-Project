namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;

    public class RewardStrategy
    {
        public int Id { get; set; }

        public RewardStrategyType RewardStrategyType { get; set; }
    }
}