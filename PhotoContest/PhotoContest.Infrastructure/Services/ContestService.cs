namespace PhotoContest.Infrastructure.Services
{
    using PhotoContest.Infrastructure.Interfaces;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Models.Enums;

    public class ContestService : BaseService, IContestsService
    {
        public ContestService(IPhotoContestData data)
            :base(data)
        {
        }

        public IEnumerable<ContestViewModel> GetActiveContests(string userId)
        {
            var activeContests = this.Data.Contests.All()
                .Where(c => c.Status == ContestStatus.Active)
                .OrderByDescending(c => c.StartDate)
                .ProjectTo<ContestViewModel>()
                .ToList();

            this.ApplyRights(activeContests, userId);

            return activeContests;
        }

        public IEnumerable<ContestViewModel> GetInactiveContests()
        {
            var inactiveContests = this.Data.Contests.All()
                .Where(c => c.Status != ContestStatus.Active)
                .OrderByDescending(c => c.StartDate)
                .ProjectTo<ContestViewModel>()
                .ToList();

            return inactiveContests;
        }

        public IEnumerable<ContestViewModel> GetMyContests(string userId = null)
        {
            var myContests = this.Data.Contests.All()
               .Where(c => c.OrganizatorId == userId)
               .OrderByDescending(c => c.StartDate)
               .Project()
               .To<ContestViewModel>()
               .ToList();

            return myContests;
        }

        public IEnumerable<ContestViewModel> GetAllContests(string userId = null)
        {
            var allContests = this.Data.Contests.All()
                    .OrderByDescending(c => c.StartDate)
                    .ProjectTo<ContestViewModel>()
                    .ToList();

            this.ApplyRights(allContests, userId);

            return allContests;
        }

        private void ApplyRights(IList<ContestViewModel> contests, string userId = null)
        {
            if (userId != null)
            {
                var user = this.Data.Users.Find(userId);

                for (int i = 0; i < contests.Count(); i++)
                {
                    if (user.Id == contests[i].OrganizatorId)
                    {
                        contests[i].CanManage = true;
                        continue;
                    }

                    if (contests[i].ParticipationStrategyType != ParticipationStrategyType.Closed
                        && !user.InContests.Any(c => c.Id == contests[i].Id)
                        && !user.CommitteeInContests.Any(c => c.Id == contests[i].Id))
                    {
                        contests[i].CanParticipate = true;
                    }
                }
            }
        }
    }
}