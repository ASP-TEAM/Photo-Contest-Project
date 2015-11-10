namespace PhotoContest.App.Hubs
{
    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.AspNet.SignalR;

    [HubName("PhotoContestHub")]
    public class PhotoContestHub : Hub
    {
        public void SendNotification(string username, string notificationType)
        {
            string userId = this.Context.User.Identity.Name;

            if (!userId.IsNullOrWhiteSpace())
            {
                this.Clients.User(username).notificationReceived(notificationType);
            }
        }
    }
}