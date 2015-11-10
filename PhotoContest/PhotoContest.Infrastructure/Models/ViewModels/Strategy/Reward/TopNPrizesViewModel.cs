namespace PhotoContest.Infrastructure.Models.ViewModels.Strategy.Reward
{
    using System.ComponentModel.DataAnnotations;

    public class TopNPrizesViewModel : AbstractRewardStrategyViewModel
    {
        [Required]
        [Range(1, 15)]
        public int TopNPlaces { get; set; }
    }
}