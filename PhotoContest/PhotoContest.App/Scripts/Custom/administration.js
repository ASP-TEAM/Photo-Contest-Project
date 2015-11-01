var adminManager = (function() {
    function onUserSearchData() {
        return {
            text: $("#searchUsers").val()
        }
    }

    function showSuccessMessage(data) {
        return notificationHelper.showSuccessMessage(data);
    }

    function onManagePicturesData() {
        console.log($("#contestPictures").data("kendoDropDownList").dataItem());
    }

    function onPictureRemovedSuccessfully(data) {
        $("#viewDetails").trigger("click");
        return notificationHelper.showSuccessMessage(data);
    }

    function onPictureNotRemoved(error, code) {
        return notificationHelper.showErrorMessage(error);
    }

    return {
        onUserSearchData: onUserSearchData,
        showSuccessMessage: showSuccessMessage,
        onManagePicturesData: onManagePicturesData,
        onPictureRemovedSuccessfully: onPictureRemovedSuccessfully,
        onPictureNotRemoved: onPictureNotRemoved
    }
})();