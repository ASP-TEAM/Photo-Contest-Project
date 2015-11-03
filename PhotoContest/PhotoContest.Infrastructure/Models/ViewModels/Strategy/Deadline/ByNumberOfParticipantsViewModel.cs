using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Infrastructure.Models.ViewModels.Strategy.Deadline
{
    public class ByNumberOfParticipantsViewModel : AbstractDeadlineStrategyViewModel
    {
        [Required]
        [Range(2, 1000)]
        public int ParticipantsLimit { get; set; }
    }
}