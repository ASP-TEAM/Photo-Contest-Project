namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class OpenParticipationStrategy : IParticipationStrategy
    {
        public void Participate(IPhotoContestData data, User user, Contest contest)
        {
            if (contest.Participants.Contains(user))
            {
                throw new ArgumentException("You already participate in this contest");
            }
            
            contest.Participants.Add(user);

            data.Contests.Update(contest);

            data.SaveChanges();
        }
    }
}
