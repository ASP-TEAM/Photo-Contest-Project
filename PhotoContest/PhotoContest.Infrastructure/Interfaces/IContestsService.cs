using PhotoContest.Infrastructure.Models.BindingModels.Reward;

namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;

    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Invitation;

    public interface IContestsService
    {
        IQueryable<ContestViewModel> GetTopNewestContests(int takeCount);

        IEnumerable<ContestViewModel> GetActiveContests(string userId);

        IEnumerable<ContestViewModel> GetInactiveContests();

        IEnumerable<ContestViewModel> GetMyContests(string userId);

        IEnumerable<ContestViewModel> GetAllContests(string userId);

        BaseContestViewModel GetPreviewContest(int id, string userId = null);

        bool DismissContest(int id, string userId);

        bool FinalizeContest(int id, string userId);

        bool JoinContestCommittee(int id, string userId);

        int InviteUser(CreateInvitationBindingModel model, string loggedUserId);

        int CreateContest(CreateContestBindingModel model, string userId);

        int JoinContest(int id, string userId);

        int UpdateContest(UpdateContestBindingModel model, string userId);

        int AddRewards(int id, CreateRewardsBindingModel model, string userId);
    }
}