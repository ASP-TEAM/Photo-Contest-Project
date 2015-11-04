namespace PhotoContest.Infrastructure.Models.ViewModels.Contest
{
    using System.Collections.Generic;

    public class PreviewInactiveContestViewModel : BaseContestViewModel
    {
         public IEnumerable<ContestWinnerViewModel> Winners { get; set; }  
    }
}