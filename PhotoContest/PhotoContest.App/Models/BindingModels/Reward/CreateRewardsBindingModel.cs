namespace PhotoContest.App.Models.BindingModels.Reward
{
    public class CreateRewardsBindingModel
    {
        public string[] Name { get; set; }

        public string[] Description { get; set; }

        public int[] Place { get; set; }

        public string[] ImageUrl { get; set; }
    }
}