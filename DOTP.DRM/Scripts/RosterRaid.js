var onUpdateSuccess = function (data) {
    if (data.Success && (true == data.Success)) {
        document.location.reload();
    }
    else {
        $(window).scrollTop(100);
        $("#errorContent").text(data.ErrorMessage);
        $("#errorPanel").slideDown(300);
    }
};

var onSubmitError = function (data) {
    $(window).scrollTop(100);
    $("#errorContent").text("There was an error while sending the data to the server. Please try again later or contact the administrator.");
    $("#errorPanel").slideDown(300);
};

$(document).ready(function () {
    $(".drmQueuedCharacterCheckbox").click(function () {
        $("#errorPanel").slideUp(300);

        var name = $(this).attr("name");

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/RosterCharacter",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Character: name
            },
            success: onUpdateSuccess,
            error: onSubmitError
        });
    });

    $(".drmRosteredCharacterCheckbox").click(function () {
        $("#errorPanel").slideUp(300);

        var name = $(this).attr("name");

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/UnrosterCharacter",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Character: name
            },
            success: onUpdateSuccess,
            error: onSubmitError
        });
    });
});
