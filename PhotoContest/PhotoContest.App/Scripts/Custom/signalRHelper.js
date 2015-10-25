var signalRHelper = (function() {
    var hub = $.connection.PhotoContestHub;
    $.connection.hub.start();

    (function() {
        hub.client.notificationReceived = function (invitation) {
            var notifications = $('#notifications');
            notifications.prepend(invitation);
            notifications.children().first().css("background-color", "#4ebf50");
            notifications.show();
        }
    }());

    function sendNotification(username, type) {
        hub.server.sendNotification(username, type);
    }

    return {
        sendInvitation: sendNotification
    }
}())