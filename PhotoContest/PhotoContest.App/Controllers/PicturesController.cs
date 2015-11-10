namespace PhotoContest.App.Controllers
{
    using System.Web.Mvc;

    using Microsoft.AspNet.Identity;

    using PhotoContest.Data.Interfaces;
    using System.Net;
    using PhotoContest.Common.Exceptions;
    using PhotoContest.Infrastructure.Services;

    public class PicturesController : BaseController
    {
        private PictureService _service;

        public PicturesController(IPhotoContestData data)
            : base(data)
        {
            this._service = new PictureService(data);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Vote(int id)
        {
            try
            {
                var votesCount = this._service.Vote(id, this.User.Identity.GetUserId());

                return this.Content(votesCount.ToString());
            }
            catch (BadRequestException exception)
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return this.Json(new { ErrorMessage = exception.Message });
            }
        }
    }
}