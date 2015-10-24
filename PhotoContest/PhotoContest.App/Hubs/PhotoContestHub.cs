using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PhotoContest.App.Hubs
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    using Microsoft.Ajax.Utilities;
    using Microsoft.AspNet.SignalR.Hubs;

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