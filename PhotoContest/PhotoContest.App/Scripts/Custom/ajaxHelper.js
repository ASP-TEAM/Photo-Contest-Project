var ajaxHelper = (function() {

    var bar = $('.progress-bar');
    var percent = $('.progress-bar');
    var status = $('#status');

    $('form[name="upload-picture-form"]').ajaxForm({
        beforeSend: function() {
            status.empty();
            var percentVal = '0%';
            bar.width(percentVal);
            percent.html(percentVal);
        },
        uploadProgress: function(event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            bar.width(percentVal)
            percent.html(percentVal);
        },
        success: function(picture) {
            var percentVal = '100%';
            bar.width(percentVal);
            percent.html(percentVal);
            $('.pictures').append(picture);
        },
        error: function(xhr) {
            bar.width('0%');
            percent.html('0%');
            status.html(xhr.responseText);
        }
    });

    $('#invite-user-username').keyup(function(e) {
        var searchTerm = e.target.value;

        $.ajax({
            url: "/users/AutoCompleteUsername?searchTerm=" + searchTerm,
            method: "GET",
            success: function (results) {
                if (results.length > 0) {
                    $('#suggestions').html('');
                    $('#suggestions').css('display', 'initial');

                    for (var result in results) {
                        $('#suggestions')
                            .append('<li data-toggle="tab" onclick=ajaxHelper.autoComplete("' + results[result] + '")>' + results[result] + '</li>');
                    }
                } else {
                    $('#suggestions').css('display', 'none');
                }
            },
        });
    });

    $("#registerForm #Username").keyup(function (input) {
        $("#usernameCheckResult").html("");

        if ($(input.target).val().length > 0) {
            $.get("/Users/IsUsernameAvailable/?username=" + $(input.target).val(), function (result) {
                if (result !== true) {
                    $("#usernameCheckResult").html(result);
                }
            });
        }
    });

    function autoComplete(value) {
        $('#invite-user-username').val(value);
        $('#suggestions').html('');
        $('#suggestions').css('display', 'none');
    }

    function onSuccess() {
        notificationHelper.showSuccessMessage("Operation successfull");
    }

    function onError() {
        notificationHelper.showErrorMessage('Error occured');
    }

    return {
        onSuccess: onSuccess,
        onError: onError,
        autoComplete: autoComplete
    }

})();