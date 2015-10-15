namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        public void Participate(IPhotoContestData data, User user, Contest contest)
        {
            if (!contest.IsOpenForSubmissions)
            {
                throw new InvalidOperationException("The contest registration is closed.");
            }
        }
    }
}
