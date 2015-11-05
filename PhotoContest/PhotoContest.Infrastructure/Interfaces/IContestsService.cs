namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Invitation;
    using System.Web;
    using PhotoContest.Infrastructure.Models.BindingModels.Reward;

    public interface IContestsService
    {
        IQueryable<ContestViewModel> GetTopNewestContests(int takeCount);

        IEnumerable<ContestViewModel> GetActiveContests(string userId);

        IEnumerable<ContestViewModel> GetInactiveContests();

        IEnumerable<ContestViewModel> GetMyContests(string userId);

        IEnumerable<ContestViewModel> GetAllContests(string userId);

        UpdateContestBindingModel GetManageContest(int id, string userId);

        BaseContestViewModel GetPreviewContest(int id, string userId = null);

        CreateContestViewModel GetCreateContest();

        bool DismissContest(int id, string userId);

        bool FinalizeContest(int id, string userId);

        bool JoinContestCommittee(int id, string userId);

        int InviteUser(CreateInvitationBindingModel model, string loggedUserId);

        int CreateContest(CreateContestBindingModel model, string userId);

        bool JoinContest(int id, IEnumerable<HttpPostedFileBase> files, string userId);

        bool UpdateContest(UpdateContestBindingModel model, string userId);

        bool AddRewards(int id, CreateRewardsBindingModel model, string userId);
    }
}