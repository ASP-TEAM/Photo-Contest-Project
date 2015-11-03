using System.ComponentModel.DataAnnotations;

namespace PhotoContest.Infrastructure.Models.ViewModels.Reward
{
    public class AddRewardViewModel
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        [Required]
        public int Place { get; set; } 

        [DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }
    }
}