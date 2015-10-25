namespace PhotoContest.App.Models.ViewModels.Contest
{
    public class ContestWinnerViewModel
    {
        public int Id { get; set; }

        public string ContestTitle { get; set; }

        public string ContestDescription { get; set; }

        public string Winner { get; set; }

        public int Place { get; set; } 
    }
}