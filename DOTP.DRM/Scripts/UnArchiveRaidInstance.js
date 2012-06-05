var onSubmitSuccess = function (data) {
    if (data.Success && (true == data.Success)) {
        $("#successPanel").slideDown(300);
        $("#main").slideUp(300);
    }
    else {
        $("#errorContent").text(data.ErrorMessage);
        $("#errorPanel").slideDown(300);
    }
};

var onSubmitError = function (data) {
    $("#errorContent").text("There was an error while sending the data to the server. Please try again later or contact the administrator.");
    $("#errorPanel").slideDown(300);
};

$(document).ready(function () {
    $("#Cancel").click(function () {
        document.location = "/";
    });

    $("#UnArchive").click(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/UnArchive",
            data: {
                ID: $("#ID").val(),
                Name: $("#Name").val()
            },
            success: onSubmitSuccess,
            error: onSubmitError
        });

        return false;
    });
});
