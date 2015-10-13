namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;

    public class VotingStrategy : AbstractStrategy
    {
        public int Id { get; set; }

        public VotingStrategyType VotingStrategyType { get; set; }
    }
}