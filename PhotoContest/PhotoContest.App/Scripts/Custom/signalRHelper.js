var signalRHelper = (function() {
    var hub = $.connection.PhotoContestHub;
    $.connection.hub.start();

    (function() {
        hub.client.notificationReceived = function (id) {
            var notifications = $('#notifications');
            notifications.html('');

            $.ajax({
                url: '/users/GetNotification/' + id,
                method: 'GET',
                success: function (invitation) {
                    notifications.prepend(invitation);
                    notifications.show();
                },
                error: function(xhr) {
                    notificationHelper.showErrorMessage(xhr.responseText);
                }
            });
        }
    }());

    function sendNotification(username, type) {
        hub.server.sendNotification(username, type);
    }

    return {
        sendInvitation: sendNotification
    }
}())