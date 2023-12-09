var ProfileHandler = function () {

    var init = function () {
        initializeImageUpload();
    };

    var initializeImageUpload = function () {

        let fileUploader = $(".image-uploader-container");

        if (fileUploader.length) {
            fileUploader.find(".btn-upload").click(function () {
                fileUploader.find(".hidden-file").click();
            });

            fileUploader.find(".hidden-file").change(function () {
                let file = $(this)[0].files[0];

                if (file !== undefined) {
                    fileUploader.find(".img").attr("src", URL.createObjectURL(file));
                }
            });
        }
    }

    return {
        init: init
    }

}();