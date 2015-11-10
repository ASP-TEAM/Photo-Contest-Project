namespace PhotoContest.Data.Strategies.VotingStrategy
{

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    using PhotoContest.Common.Exceptions;

    public class ClosedVotingStrategy : IVotingStrategy
    {
        public void CheckPermission(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.Committee.Contains(user))
            {
                throw new BadRequestException("User is not in the voting committee.");
            }
        }
    }
}
