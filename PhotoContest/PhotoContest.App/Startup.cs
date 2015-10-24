#region

using Microsoft.Owin;

#endregion

[assembly: OwinStartup(typeof(PhotoContest.App.Startup))]

namespace PhotoContest.App
{
    #region

    using Owin;

    #endregion

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}