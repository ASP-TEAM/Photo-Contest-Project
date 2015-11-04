﻿namespace PhotoContest.Infrastructure.Interfaces
{
    using System.Collections.Generic;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Invitation;

    public interface IContestsService
    {
        IEnumerable<ContestViewModel> GetActiveContests(string userId);

        IEnumerable<ContestViewModel> GetInactiveContests();

        IEnumerable<ContestViewModel> GetMyContests(string userId);

        IEnumerable<ContestViewModel> GetAllContests(string userId);

        bool DismissContest(int id, string userId);

        bool FinalizeContest(int id, string userId);

        int InviteUser(CreateInvitationBindingModel model, string loggedUserId);

        int CreateContest(CreateContestBindingModel model, string userId);

        int JoinContest(int id, string userId);
    }
}