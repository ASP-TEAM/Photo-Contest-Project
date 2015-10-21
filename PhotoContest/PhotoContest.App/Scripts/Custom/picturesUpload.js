$("#uploadPictureInput").change(function (input) {
    if (input.target.files) {
        var reader = new FileReader();

        $('#contestUploadPicturesContainer').html("");

        reader.onload = function (e) {
            $('#contestUploadPicturesContainer').append("<img class=\"col-lg-4 picturePreview\" src=\"" + e.target.result + "\" />");
        }

        for (var i = 0; i < input.target.files.length; i++) {
            reader.readAsDataURL(input.target.files[i]);
        }
    }
});