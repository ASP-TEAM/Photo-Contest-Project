namespace PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline
{
    using System.ComponentModel.DataAnnotations;

    public class ByNumberOfParticipantsViewModel : AbstractDeadlineStrategyViewModel
    {
        [Required]
        [Range(2, 1000)]
        public int ParticipantsLimit { get; set; }
    }
}