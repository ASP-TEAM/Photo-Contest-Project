namespace PhotoContest.Data.Strategies.DeadlineStrategy
{
    using System;

    using PhotoContest.Data.Interfaces;

    using PhotoContest.Models;

    public class ByEndTimeStrategy : IDeadlineStrategy
    {
        public void Deadline(IPhotoContestData data, Contest contest, User user)
        {
            if (this.CheckDeadline(contest.EndDate))
            {
                if (contest.isOpenForSubmissions == false)
                {
                    contest.isOpenForSubmissions = true;
                    data.Contests.Update(contest);
                    data.SaveChanges();
                }

                throw new Exception("The contest is closed for submissions");
            }
        }

        private bool CheckDeadline(DateTime deadlineDate)
        {
            if (deadlineDate > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}