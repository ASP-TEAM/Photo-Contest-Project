using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Infrastructure.Models.ViewModels.Strategy.Reward
{
    public class TopNPrizesViewModel : AbstractRewardStrategyViewModel
    {
        [Required]
        [Range(1, 15)]
        public int TopNPlaces { get; set; }
    }
}