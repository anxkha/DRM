var onSubmitSuccess = function (data) {
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

var onSubmitError = function (data) {
    $(window).scrollTop(100);
    $("#errorContent").text("There was an error while sending the data to the server. Please try again later or contact the administrator.");
    $("#errorPanel").slideDown(300);
};

$(document).ready(function () {
    $("#signupForm").submit(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/NewSignup",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Character: $("#Character").val(),
                Comment: $("#Comment").val(),
            },
            success: onSubmitSuccess,
            error: onSubmitError
        });

        return false;
    });
});
