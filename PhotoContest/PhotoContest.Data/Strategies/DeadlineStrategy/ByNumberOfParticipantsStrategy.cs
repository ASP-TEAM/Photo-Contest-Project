namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;
    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    public class ByNumberOfParticipantsStrategy : IDeadlineStrategy
    {
        public void Deadline(IPhotoContestData data, Contest contest, User user)
        {
            if (contest.Participants.Count >= contest.ParticipantsLimit)
            {
                if (contest.IsOpenForSubmissions)
                {
                    contest.IsOpenForSubmissions = false;
                    data.Contests.Update(contest);
                    data.SaveChanges();
                }

                throw new Exception("The contest is closed for submissions");
            }
        }
    }
}