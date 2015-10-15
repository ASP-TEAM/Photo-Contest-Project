namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    public class ByEndTimeStrategy : IDeadlineStrategy
    {
        public void Deadline(IPhotoContestData data, Contest contest, User user)
        {
            if (contest.SubmissionDate < DateTime.Now)
            {
                if (contest.IsOpenForSubmissions == false)
                {
                    contest.IsOpenForSubmissions = true;
                    data.Contests.Update(contest);
                    data.SaveChanges();
                }

                throw new Exception("The contest is closed for submissions/registrations");
            }
        }
    }
}