var onUpdateSuccess = function (data) {
    if (data.Success && (true == data.Success)) {
        $("#successPanel").slideDown(300);
        $("#successContent").text("You have successfully updated the roster.");
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
    $(".updateButton").click(function () {
        $("#errorPanel").slideUp(300);

        var characters = "";

        $(".characterCheckbox").each(function () {
            var name = $(this).attr("name");
            var checked = $(this).attr("checked");

            if ("checked" === checked) {
                if ("" !== characters)
                    characters = characters + ",";

                characters = characters + name;
            }
        });

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Raid/UpdateRoster",
            data: {
                RaidInstanceID: $("#RaidInstanceID").val(),
                Characters: characters
            },
            success: onUpdateSuccess,
            error: onSubmitError
        });
    });
});
