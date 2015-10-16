namespace PhotoContest.App.Controllers
{
    using PhotoContest.Data.Interfaces;

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }
    }
}