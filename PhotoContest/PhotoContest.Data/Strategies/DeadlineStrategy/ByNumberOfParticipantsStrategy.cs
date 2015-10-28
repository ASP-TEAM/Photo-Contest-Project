namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    public class ByNumberOfParticipantsStrategy : IDeadlineStrategy
    {
        public void CheckDeadline(IPhotoContestData data, Contest contest, User user)
        {
            if (contest.Participants.Count >= contest.ParticipantsLimit)
            {
                if (contest.IsOpenForSubmissions)
                {
                    contest.IsOpenForSubmissions = false;
                    data.Contests.Update(contest);
                    data.SaveChanges();
                }

                throw new InvalidOperationException("The contest is closed for submissions/registrations");
            }
        }
    }
}