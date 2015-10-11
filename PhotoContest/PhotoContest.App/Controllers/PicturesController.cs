namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using PhotoContest.Data.Interfaces;

    public class PicturesController : BaseController
    {
        public PicturesController(IPhotoContestData data)
            : base(data)
        {
        }
    }
}