namespace PhotoContest.App.Areas.Administration.Controllers
{
    using System.Collections;
    using System.Data.Entity;
    using System.Web.Mvc;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using PhotoContest.Data.Interfaces;

    public abstract class KendoAdminController : BaseAdminController
    {
        protected KendoAdminController(IPhotoContestData data)
            : base(data)
        {
        }

        protected abstract IEnumerable GetData();

        protected abstract T GetById<T>(object id) where T : class;

        [HttpPost]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var ads =
                this.GetData()
                .ToDataSourceResult(request);

            return this.Json(ads);
        }

        protected JsonResult GridOperation<T>(T model, [DataSourceRequest]DataSourceRequest request)
        {
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        protected void ChangeEntityStateAndSave(object dbModel, EntityState state)
        {
            var entry = this.Context.Entry(dbModel);
            entry.State = state;
            this.Data.SaveChanges();
        }
    }
}