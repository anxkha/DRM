var currentClassSelect = null;
var currentPrimarySpecializationSelect = null;
var currentSecondarySpecializationSelect = null;

var getCurrentRace = function () {
    return $("#Race").val().replace(" ", "");
};

var getCurrentClass = function () {
    return $("#" + currentClassSelect).val().replace(" ", "");
};

var showClassesByRace = function (race) {
    if (currentClassSelect) {
        $("#" + currentClassSelect).hide();
    }

    currentClassSelect = race + "Classes";

    $("#" + currentClassSelect).show();

    showPrimarySpecializationByClass(getCurrentClass());
    showSecondarySpecializationByClass(getCurrentClass());
};

var showPrimarySpecializationByClass = function (clss) {
    if (currentPrimarySpecializationSelect) {
        $("#" + currentPrimarySpecializationSelect).hide();
    }

    currentPrimarySpecializationSelect = clss + "PriSpec";

    $("#" + currentPrimarySpecializationSelect).show();
};

var showSecondarySpecializationByClass = function (clss) {
    if (currentSecondarySpecializationSelect) {
        $("#" + currentSecondarySpecializationSelect).hide();
    }

    currentSecondarySpecializationSelect = clss + "SecSpec";

    $("#" + currentSecondarySpecializationSelect).show();
};

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
    showClassesByRace(getCurrentRace());

    $("#Race").change(function () {
        showClassesByRace(getCurrentRace());
    });

    $("[name*='Classes']").change(function () {
        showPrimarySpecializationByClass(getCurrentClass());
        showSecondarySpecializationByClass(getCurrentClass());
    });

    $("#addForm").submit(function () {
        $("#errorPanel").slideUp(300);

        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/Characters/Add",
            data: {
                Name: $("#Name").val(),
                Level: $("#Level").val(),
                Race: $("#Race").val(),
                Class: $("#" + currentClassSelect).val(),
                PrimarySpecialization: $("#" + currentPrimarySpecializationSelect).val(),
                SecondarySpecialization: $("#" + currentSecondarySpecializationSelect).val()
            },
            success: onSubmitSuccess,
            error: onSubmitError
        });

        return false;
    });
});
