var ajaxHelper = (function () {

    var bar = $('.progress-bar');
    var percent = $('.progress-bar');
    var status = $('#status');
   
    $('form[name="upload-picture-form"]').ajaxForm({
        beforeSend: function () {
            status.empty();
            var percentVal = '0%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        uploadProgress: function (event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            bar.width(percentVal)
            percent.html(percentVal);
        },
        success: function (picture) {
            var percentVal = '100%';
            bar.width(percentVal);
            percent.html(percentVal);
            $('.pictures').append(picture);
        },
        error: function (xhr) {
            bar.width('0%');
            percent.html('0%');
            status.html(xhr.responseText);
        }
    });

    function onContestCreated() {
        notificationHelper.showSuccessMessage("Contest created");
    }

    function onContestNotCreated() {
        notificationHelper.showErrorMessage('Please fill correctly all required fields');
    }

    return {
        onContestCreated: onContestCreated,
        onContestNotCreated: onContestNotCreated
    }

})();