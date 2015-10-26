namespace PhotoContest.Data.Interfaces
{
    using PhotoContest.Data.Repositories;
    using PhotoContest.Models;

    public interface IPhotoContestData
    {
        IRepository<User> Users { get; }

        IRepository<RewardStrategy> RewardStrategies { get; }

        IRepository<VotingStrategy> VotingStrategies { get; }

        IRepository<ParticipationStrategy> ParticipationStrategies { get; }

        IRepository<DeadlineStrategy> DeadlineStrategies { get; }

        IRepository<Contest> Contests { get; }

        IRepository<Picture> Pictures { get; }

        IRepository<Vote> Votes { get; }

        IRepository<ContestWinners> ContestWinners { get; }

        IRepository<Invitation> Invitations { get; }

        int SaveChanges();
    }
}