import cud from './cud.js'

$(document).ready(function () {
    cud.initTabs();
    cud.setViewType("Details");
    cud.initDevices();

    $(".bg-lightyellow").focus(function () {
        $(this).addClass("bg-lightyellow")
    });
    $(".form-control").addClass("bg-lightyellow");
});

// Changes-Listbox-Item Click
$(".changes-listbox-item").on("click", function (event) {
    cud.onClickChangesListBoxItem(event);
});

// Button "Gerät -> Schließen"
$("#button-devices-close").on("click", function () {
    $("#show-device").hide();
    $("#show-software").hide();
    $("#software-list").hide();
});

// Button "Software -> Schließen"
$("#button-software-close").on("click", function () {
    $("#show-software").hide();
});

// Button "Änderungen -> Schließen"
$("#button-changes-close").on("click", function () {
    $("#changes-per-date").hide();
});

$("#button-show-pdf-new").on("click", function () {
    let route = $("#hidden-generate-pdf").val();
    window.open(route, "_blank");
});

$("#button-send-email").on("click", function (event) {
    event.preventDefault();
    let buttonSendEmail = $("#button-send-email");
    let checkIcon = $("#icon-send-email-check");
    let ticketId = $("#hidden-ticket-id").val();
    buttonSendEmail.addClass("disabled");
    buttonSendEmail.addClass("btn-space");
    checkIcon.addClass("collapse");

    $.ajax(
        {
            type: 'POST',
            dataType: 'JSON',
            url: '/Tickets/SendEmail',
            data: { id: ticketId },
            success:
                function (response) {
                    console.log(response);
                    setTimeout(function () {
                        buttonSendEmail.removeClass("disabled");
                        buttonSendEmail.removeClass("btn-space");
                        checkIcon.removeClass("collapse");
                    }, 1000);
                },
            error:
                function (response) {
                    alert("Error: " + response.responseText);
                    console.log(response);
                    setTimeout(function () {
                        buttonSendEmail.removeClass("disabled");
                    }, 1000);
                }
        }
    );
});