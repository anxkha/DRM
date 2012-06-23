var onSignupSuccess = function (data) {
    if (data.Success && (true == data.Success)) {
        $("#successPanel").slideDown(300);
        $("#main").slideUp(300);
    }
    else {
        $(window).scrollTop(100);
        $("#errorContent").text(data.ErrorMessage);
        $("#errorPanel").slideDown(300);
    }
};

var onCancelSuccess = function (data) {
    if (data.Success && (true == data.Success)) {
        $("#successPanel").slideDown(300);
        $("#successContent").text("You have successfully cancelled that signup.");
        $("#main").slideUp(300);
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
    $(".drmCancelSignupButton").click(function () {
        $("#errorPanel").slideUp(300);

        var character = $(this).attr("id").replace("Cancel", "");

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/CancelSignup",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Character: character
            },
            success: onCancelSuccess,
            error: onSubmitError
        });
    });

    $("#signupForm").submit(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/NewSignup",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Character: $("#Character").val(),
                Comment: $("#Comment").val()
            },
            success: onSignupSuccess,
            error: onSubmitError
        });

        return false;
    });
});
