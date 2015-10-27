namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using PhotoContest.App.Controllers;
    using PhotoContest.Data.Interfaces;

    [Authorize(Roles = "Administrator")]
    public class PicturesController : BaseController
    {
        public PicturesController(IPhotoContestData data) : base(data)
        {
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var picture = this.Data.Pictures.Find(id);

            if (picture == null)
            {
                this.Response.StatusCode = 400;
                return this.Content("Picture not found");
            }

            picture.Contest.Pictures.Remove(picture);

            picture.User.Pictures.Remove(picture);

            this.Data.Pictures.Delete(picture);

            this.Data.SaveChanges();

            this.Response.StatusCode = 200;

            return this.Content("Operation successful");
        }
    }
}