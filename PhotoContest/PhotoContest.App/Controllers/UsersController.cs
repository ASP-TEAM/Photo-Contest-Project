namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    public class UsersController : BaseController
    {
        public UsersController(IPhotoContestData data)
            : base(data)
        {
        }
    }
}