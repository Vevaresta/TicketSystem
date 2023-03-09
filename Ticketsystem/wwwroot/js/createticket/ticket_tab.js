import Device from "../models/device.js";
import "./devices_tab.js";

// Variable für Geräteliste
window.deviceList = [];

// Ticketart als string (wird bei jedem Radiobutton-Klick geändert)
window.radioTicketTypeSelectedValue = $('input[name="ticket-type"]:checked').val();

// Liste der Radiobuttons zum Wählen der Ticketart
var radioButtons = $(".radio-ticket-type");

// Wird beim Laden der Seite ausgeführt
$(document).ready(function () {
    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);

    radioButtons.change(function handleRadioChange(event) {
        radioTicketTypeSelectedValue = event.target.value;

        var einzelgeraet = $("#einzelgeraet");
        var geraetetab = $("#geraetetab");
        var backup = $("#backup")

        if (radioTicketTypeSelectedValue == "Reparatur" || radioTicketTypeSelectedValue == "Datenrettung") {
            $('#tabs a[href="#tab1"]').tab('show');
            einzelgeraet.show();
            geraetetab.hide();
            backup.show();
        }
        else if (radioTicketTypeSelectedValue == "Beratung") {
            $('#tabs a[href="#tab1"]').tab('show');
            einzelgeraet.hide();
            geraetetab.hide();
            backup.hide();
        }
        else {
            einzelgeraet.hide();
            geraetetab.show();
            backup.show();
        }
    });

    $('.software-listbox-item').click(function () {
        softwareListBoxSelectedIndex = $(this).index();
    });
});

// Switch für "Datensicherung?"
$("#switch-backup").on('change', function () {
    var state = $("#switch-backup").prop("checked");
    if (state == true) {
        $("#backup-choices").show();
    }
    else {
        $("#backup-choices").hide();
    }
});

// Tabliste initialisieren ("Daten" | "Geräte")
$(function () {
    $('#tabs a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
});

// Button "Hauptseite -> Speichern"
$("#button-main-save-ticket").on("click", function () {
    var tickettype = radioTicketTypeSelectedValue;

    $('#ticketTypeInput').val(JSON.stringify(tickettype));

    if (radioTicketTypeSelectedValue == "Reparatur") {
        var newDevice = new Device();
        if ($("#input-device-name").val() != "") {
            newDevice.Name = $("#input-device-name").val();
            newDevice.DeviceType = $("#input-device-type").val();
            newDevice.Manufacturer = $("#input-device-manufacturer").val();
            newDevice.SerialNumber = $("#input-device-serialnumber").val();
            newDevice.Accessories = $("#input-device-accessories").val();
            newDevice.Comments = $("#input-device-comments").val();
            newDevice.Software = [];
        }

        deviceList = [];
        deviceList.push(newDevice);        

        $('#deviceListInput').val(JSON.stringify(deviceList));
    }
});
