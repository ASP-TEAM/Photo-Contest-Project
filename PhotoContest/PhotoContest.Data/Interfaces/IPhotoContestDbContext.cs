namespace PhotoContest.Data.Interfaces
{
    using System.Data.Entity;

    using PhotoContest.Models;

    public interface IPhotoContestDbContext
    {
        IDbSet<RewardStrategy> RewardStrategies { get; }

        IDbSet<VotingStrategy> VotingStrategies { get; }

        IDbSet<ParticipationStrategy> ParticipationStrategies { get; }

        IDbSet<DeadlineStrategy> DeadlineStrategies { get;  }

        IDbSet<Contest> Contests { get; }

        IDbSet<Picture> Pictures { get; }

        IDbSet<Vote> Votes { get; }

        IDbSet<ContestWinners> ContestWinners { get; }

        IDbSet<Invitation> Invitations { get; }
    }
}