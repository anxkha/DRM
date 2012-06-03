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
    $("#Date").datepicker();

    $("#scheduleForm").submit(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/Schedule",
            data: {
                Raid: $("#Raids").val(),
                Name: $("#Name").val(),
                Description: $("#Description").val(),
                InviteTime: $("#Date").val() + " " + $("#InviteTimeHour").val() + ":" + $("#InviteTimeMinute").val() + ":00",
                StartTime: $("#Date").val() + " " + $("#StartTimeHour").val() + ":" + $("#StartTimeMinute").val() + ":00"
            },
            success: onSubmitSuccess,
            error: onSubmitError
        });

        return false;
    });
});
