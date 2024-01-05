var Site = (function () {
    var initializeJoinButton = function joinButton(buttonSelector, url) {
        $("body").on("click", buttonSelector, function () {
            $.ajax({
                url: url,
                dataType: "json",
                method: "POST",
                data: {
                    groupId: $(this).attr("data-id")
                },
                success: function (data) {

                    if (data.statusCode != null && data.statusCode === 200) {
                        window.location.reload();
                    }
                }
            });
        });
    }

    var initializeLeaveButton = function leaveButton(buttonSelector, url) {
        $("body").on("click", buttonSelector, function () {
            $.ajax({
                url: url,
                dataType: "json",
                method: "POST",
                data: {
                    mappingId: $(this).attr("data-id")
                },
                success: function (data) {
                    if (data.statusCode != null && data.statusCode === 200) {
                        window.location.reload();
                    }
                }
            });
        });
    }


    return {
        InitializeJoinButton: initializeJoinButton,
        InitializeLeaveButton: initializeLeaveButton
    }

})();
