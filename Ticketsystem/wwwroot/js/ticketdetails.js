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
    let id = $("#hidden-ticketid").val();
    window.open("/Tickets/ShowPdfNewTicket/" + id + "?", "_blank");
});