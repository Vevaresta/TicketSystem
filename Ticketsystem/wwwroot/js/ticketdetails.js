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

$("#button-generate-pdf").on("click", function () {
    let route = $("#hidden-generate-pdf").val();
    window.open(route, "_blank");
});

$("#button-show-pdf-signed").on("click", function () {
    let route = $("#hidden-show-pdf-signed").val();
    window.open(route, "_blank");
});

$('#file-picker-pdf-signed').on('change', function () {
    let ticketId = $("#hidden-ticket-id").val();
    var file = this.files[0];

    if (file.type != 'application/pdf') {
        alert('Keine gültige PDF-Datei!');
    }
    else {
        var formData = new FormData();
        formData.append('pdfFile', file);

    $.ajax({
        url: '/Tickets/UploadPdf?id=' + ticketId,
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            $("#button-show-pdf-signed").removeClass("collapse");
        },
        error: function (error) {
            alert(response);
        }
    });
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