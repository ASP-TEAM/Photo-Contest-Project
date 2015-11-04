using PhotoContest.Common.Exceptions;

namespace PhotoContest.Infrastructure.Services
{
    using PhotoContest.Infrastructure.Interfaces;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper.QueryableExtensions;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Infrastructure.Models.ViewModels.Contest;
    using PhotoContest.Models.Enums;
    using System;
    using PhotoContest.Data.Strategies;
    using PhotoContest.Infrastructure.Models.BindingModels.Contest;
    using PhotoContest.Infrastructure.Models.BindingModels.Invitation;
    using PhotoContest.Models;

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

        public bool DismissContest(int id, string userId)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                throw new NotFoundException(string.Format("Contest with id {0} not found", id));
            }

            if (contest.Status != ContestStatus.Active)
            {
                throw new BadRequestException(string.Format("Contest with id {0} is not active", id));
            }

            var loggedUser = this.Data.Users.Find(userId);

            if (contest.OrganizatorId != loggedUser.Id)
            {
                throw new UnauthorizedException("Only the contest organizator can dismiss it.");
            }

            contest.IsOpenForSubmissions = false;
            contest.Status = ContestStatus.Dismissed;
            contest.EndDate = DateTime.Now;

            this.Data.SaveChanges();

            return true;
        }

        public bool FinalizeContest(int id, string userId)
        {
            var contest = this.Data.Contests.Find(id);

            if (contest == null)
            {
                throw new NotFoundException(string.Format("Contest with id {0} not found", id));
            }

            if (contest.Status != ContestStatus.Active)
            {
                throw new BadRequestException(string.Format("Contest with id {0} is not active", id));
            }

            var loggedUser = this.Data.Users.Find(userId);

            if (contest.OrganizatorId != loggedUser.Id)
            {
                throw new UnauthorizedException("Only the contest organizator can finalize it.");
            }

            contest.IsOpenForSubmissions = false;
            contest.Status = ContestStatus.Finalized;
            contest.EndDate = DateTime.Now;

            this.Data.SaveChanges();

            var rewardStrategy =
                    StrategyFactory.GetRewardStrategy(contest.RewardStrategy.RewardStrategyType);

            rewardStrategy.ApplyReward(this.Data, contest);

            return true;
        }

        public int InviteUser(CreateInvitationBindingModel model, string loggerUserId)
        {
            var contest = this.Data.Contests.Find(model.ContestId);

            if (contest == null)
            {
                throw new NotFoundException(string.Format("Contest with id {0} not found", model.ContestId));
            }

            var loggedUser = this.Data.Users.Find(loggerUserId);

            if (contest.OrganizatorId != loggedUser.Id)
            {
                throw new BadRequestException("Only the contest organizator can invite users.");
            }

            if (model.Type == InvitationType.Committee
                && contest.VotingStrategy.VotingStrategyType != VotingStrategyType.Closed)
            {
                throw new BadRequestException("The contest voting strategy type is not 'CLOSED'.");
            }

            if (model.Type == InvitationType.ClosedContest
                && contest.ParticipationStrategy.ParticipationStrategyType != ParticipationStrategyType.Closed)
            {
                throw new BadRequestException("The contest participation strategy type is not 'CLOSED'.");
            }

            if (!contest.IsOpenForSubmissions)
            {
                throw new BadRequestException("The contest is closed for submissions/registrations.");
            }

            var userToInvite = this.Data.Users.All().FirstOrDefault(u => u.UserName == model.Username);

            if (userToInvite == null)
            {
                throw new NotFoundException(string.Format("User with username {0} not found", model.Username));
            }

            if (userToInvite.UserName == loggedUser.UserName)
            {
                throw new BadRequestException("Users cannot invite themselves.");
            }

            if (userToInvite.PendingInvitations.Any(i => i.ContestId == model.ContestId && i.Type == model.Type))
            {
                throw new BadRequestException(string.Format("User is already invited to contest with id {0}", contest.Id));
            }

            var invitation = new Invitation
            {
                ContestId = model.ContestId,
                InviterId = loggedUser.Id,
                InvitedId = userToInvite.Id,
                DateOfInvitation = DateTime.Now,
                Type = model.Type,
                Status = InvitationStatus.Neutral
            };

            if (model.Type == InvitationType.ClosedContest)
            {
                contest.InvitedUsers.Add(userToInvite);
            }

            userToInvite.PendingInvitations.Add(invitation);
            loggedUser.SendedInvitations.Add(invitation);

            this.Data.SaveChanges();

            return invitation.Id;
        }

        public int CreateContest(CreateContestBindingModel model, string userId)
        {
            if (this.Data.RewardStrategies.Find(model.RewardStrategyId) == null)
            {
                throw new NotFoundException("Not existing reward strategy");
            }

            if (this.Data.ParticipationStrategies.Find(model.ParticipationStrategyId) == null)
            {
                throw new NotFoundException("Not existing participation strategy");
            }

            if (this.Data.VotingStrategies.Find(model.VotingStrategyId) == null)
            {
                throw new NotFoundException("Not existing voting strategy");
            }

            if (this.Data.DeadlineStrategies.Find(model.DeadlineStrategyId) == null)
            {
                throw new NotFoundException("Not existing deadline strategy");
            }

            var loggedUserId = userId;

            var contest = new Contest
            {
                Title = model.Title,
                Description = model.Description,
                Status = ContestStatus.Active,
                RewardStrategyId = model.RewardStrategyId,
                VotingStrategyId = model.VotingStrategyId,
                ParticipationStrategyId = model.ParticipationStrategyId,
                DeadlineStrategyId = model.DeadlineStrategyId,
                ParticipantsLimit = model.ParticipantsLimit,
                TopNPlaces = model.TopNPlaces,
                SubmissionDeadline = model.SubmissionDeadline,
                IsOpenForSubmissions = true,
                StartDate = DateTime.Now,
                OrganizatorId = loggedUserId,
            };

            this.Data.Contests.Add(contest);
            this.Data.SaveChanges();

            return contest.Id;
        }

        public int JoinContest(int id, string userId)
        {
            var user = this.Data.Users.Find(userId);
            var contest = this.Data.Contests.Find(id);

            if (contest.OrganizatorId == user.Id)
            {
                throw new BadRequestException("You cannot join contest created by you");
            }

            var deadlineStrategy =
                StrategyFactory.GetDeadlineStrategy(contest.DeadlineStrategy.DeadlineStrategyType);

            deadlineStrategy.CheckDeadline(this.Data, contest, user);

            var participationStrategy =
                StrategyFactory.GetParticipationStrategy(contest.ParticipationStrategy.ParticipationStrategyType);

            participationStrategy.CheckPermission(this.Data, user, contest);

            if (contest.Participants.Contains(user))
            {
                throw new BadRequestException("You already participate in this contest");
            }

            if (contest.Committee.Contains(user))
            {
                throw new UnauthorizedException("You cannot participate in this contest, you are in the committee");
            }

            contest.Participants.Add(user);
            this.Data.Contests.Update(contest);
            this.Data.SaveChanges();

            return contest.Id;
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