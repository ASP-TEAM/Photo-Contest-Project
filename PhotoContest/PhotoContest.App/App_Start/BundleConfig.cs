namespace PhotoContest.App
{
    #region

    using System.Web.Optimization;

    #endregion

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.form.js",
                "~/Scripts/jquery.noty.packaged.js",
                "~/Scripts/jquery.signalR*",
                "~/Scripts/Custom/notificationHelper.js",
                "~/Scripts/Custom/ajaxHelper.js",
                "~/Scripts/Custom/picturesUpload.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                        "~/Scripts/kendo/kendo.all.min.js",
                        "~/Scripts/kendo/kendo.aspnetmvc.min.js",
                        "~/Scripts/kendo/kendo.culture.en.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/administration").Include("~/Scripts/Custom/administration.js"));

            bundles.Add(new ScriptBundle("~/bundles/createContest").Include("~/Scripts/Custom/createContest.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                    "~/Content/kendo/kendo.common.core.min.css",
                    "~/Content/kendo/kendo.common.min.css",
                    "~/Content/kendo/kendo.black.min.css"));
        }
    }
}