namespace PhotoContest.App.Models.ViewModels.Contest
{
    using System.ComponentModel.DataAnnotations;

    using System.Collections.Generic;
    using PhotoContest.App.Models.ViewModels.Strategy;

    public class CreateContestViewModel
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Title { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        [UIHint("TextField")]
        public string Description { get; set; }

        [Required]
        public int RewardStrategyId { get; set; }

        [Required]
        public int VotingStrategyId { get; set; }

        [Required]
        public int ParticipationStrategyId { get; set; }

        [Required]
        public int DeadlineStrategyId { get; set; }

        public IEnumerable<StrategyViewModel> RewardStrategies { get; set; } 

        public IEnumerable<StrategyViewModel> VotingStrategies { get; set; } 

        public IEnumerable<StrategyViewModel> ParticipationStrategies { get; set; } 

        public IEnumerable<StrategyViewModel> DeadlineStrategies { get; set; } 
    }
}