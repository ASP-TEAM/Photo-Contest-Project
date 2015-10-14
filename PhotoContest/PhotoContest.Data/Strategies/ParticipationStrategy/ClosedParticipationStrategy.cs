using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoContest.Data.Strategies.ParticipationStrategy
{
    using PhotoContest.Data.Interfaces;
    using PhotoContest.Models;

    public class ClosedParticipationStrategy : IParticipationStrategy
    {
        public void SubmitPicture(Picture picture, IPhotoContestData data, User user, Contest contest)
        {
            if (contest.Participants.Contains(user))
            {
                contest.Pictures.Add(picture);
                data.SaveChanges();
            }
            else
            {
                throw new ArgumentException("The user is not selected to participate.");
            }
        }
    }
}
