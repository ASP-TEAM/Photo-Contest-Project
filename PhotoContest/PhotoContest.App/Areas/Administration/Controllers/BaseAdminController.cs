namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    using PhotoContest.App.Controllers;
    using PhotoContest.Common;
    using PhotoContest.Data.Interfaces;

    [Authorize(Roles = GlobalConstants.AdminRole)]
    public abstract class BaseAdminController : BaseController
    {
        protected BaseAdminController(IPhotoContestData data)
            : base(data)
        {
        }
    }
}