namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    using PhotoContest.Common.Exceptions;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        public void CheckPermission(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.IsOpenForSubmissions)
            {
                throw new BadRequestException("The contest registration is closed.");
            }
        }
    }
}