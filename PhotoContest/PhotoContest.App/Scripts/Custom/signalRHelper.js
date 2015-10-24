var signalRHelper = (function() {
    var hub = $.connection.PhotoContestHub;
    $.connection.hub.start();

    (function() {
        hub.client.notificationReceived = function (invitation) {
            $('.notifications').append(invitation);
        }
    }());

    function sendNotification(username, type) {
        hub.server.sendNotification(username, type);
    }

    return {
        sendInvitation: sendNotification
    }
}())