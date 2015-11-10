namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    using PhotoContest.Common.Exceptions;

    public class ByEndTimeStrategy : IDeadlineStrategy
    {
        public void CheckDeadline(IPhotoContestData data, Contest contest, User user)
        {
            if (contest.SubmissionDeadline < DateTime.Now)
            {
                if (contest.IsOpenForSubmissions)
                {
                    contest.IsOpenForSubmissions = false;
                    data.Contests.Update(contest);
                    data.SaveChanges();
                }

                throw new BadRequestException("The contest is closed for submissions/registrations");
            }
        }
    }
}