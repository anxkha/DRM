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
    $("#deleteButton").click(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Characters/Delete",
            data: {
                Name: $("#Name").val(),
                AccountID: $("#AccountID").val()
            },
            success: onSubmitSuccess,
            error: onSubmitError
        });

        return false;
    });

    $("#noButton").click(function () {
        document.location = "/Characters/";
    });
});
