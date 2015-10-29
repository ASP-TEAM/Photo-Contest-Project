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

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-1.11.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/ignitejs")
                    .IncludeDirectory("~/Scripts/igniteUI", "*.js", true));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(
                new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/ignitecss")
                    .IncludeDirectory("~/Content/igniteUI", "*.css", true));
        }
    }
}