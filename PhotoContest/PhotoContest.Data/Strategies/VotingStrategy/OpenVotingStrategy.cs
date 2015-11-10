namespace PhotoContest.Data.Strategies.VotingStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    using PhotoContest.Common.Exceptions;

    public class OpenVotingStrategy : IVotingStrategy
    {
        public void CheckPermission(IPhotoContestData data, User user, Contest contest)
        {
            if (contest.Participants.Contains(user))
            {
                throw new BadRequestException("You cannot vote for contest that you currently participate in.");
            }
        }
    }
}