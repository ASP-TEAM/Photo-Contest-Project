namespace PhotoContest.Models
{
    using PhotoContest.Models.Enums;

    public class ParticipationStrategy : AbstractStrategy
    {
        public int Id { get; set; }

        public ParticipationStrategyType ParticipationStrategyType { get; set; }
    }
}