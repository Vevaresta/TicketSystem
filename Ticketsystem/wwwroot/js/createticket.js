import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';

// Ticketart als string (wird bei jedem Radiobutton-Klick geändert)
var radioTicketTypeSelectedValue = $('input[name="ticket-type"]:checked').val();

// Variable für Geräteliste
var deviceList = [];

// Vaiable für temporäre Softwareliste
var tempSoftwareList = [];

// Listbox "Geräteliste" -> Index des ausgewählten items speichern
var deviceListBoxSelectedIndex = -1;

// Listbox "Softwareliste" -> Index des ausgewählten items speichern
var softwareListBoxSelectedIndex = -1;

// Listenindex des aktuell bearbeiteten Geräts bei Klick auf "Bearbeiten"
var deviceEditedIndex = -1;

// Listenindex der aktuell bearbeiteten Software bei Klick auf "Bearbeiten"
var softwareEditedIndex = -1;

// Wird beim Laden der Seite ausgeführt
$(document).ready(function () {
    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);


    // Umschalten der Ticketart durch Klick auf Radio-Button oder Label
    $(".ticket-type-container").on("click", function (event) {
        var label = $(event.target);
        var radio = label.prev('input[type=radio]');

        radioTicketTypeSelectedValue = radio.val();
        radio.prop("checked", true);

        var einzelgeraet = $("#einzelgeraet");
        var geraetetab = $("#geraetetab");
        var backup = $("#backup")

        if (radioTicketTypeSelectedValue == TicketTypes.Repair || radioTicketTypeSelectedValue == TicketTypes.DataRecovery) {
            $('#tabs a[href="#tab1"]').tab('show');
            einzelgeraet.show();
            geraetetab.hide();
            backup.show();
            $('#input-device-name').rules('add', 'required');
        }
        else if (radioTicketTypeSelectedValue == TicketTypes.Consultation) {
            $('#tabs a[href="#tab1"]').tab('show');
            einzelgeraet.hide();
            geraetetab.hide();
            backup.hide();
            $('#input-device-name').rules('remove', 'required');
        }
        else {
            einzelgeraet.hide();
            geraetetab.show();
            backup.show();
            $('#input-device-name').rules('remove', 'required');
        }
    });

    // Erlaube die Auswahl des Backup-Durchführenden durch Klick auf Label
    $(".label-backup-choice").on("click", function (event) {
        let id = event.target.id;
        if (String(id).includes("client")) {
            $("#radio-backup-client").prop("checked", true);
        }
        if (String(id).includes("staff")) {
            $("#radio-backup-staff").prop("checked", true);
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

// Button "Geräteliste -> Hinzufügen"
$("#button-add-device").on("click", function () {
    tempSoftwareList = [];
    $("#software-listbox").empty();

    $("#input-devices-name").val("");
    $("#input-devices-type").val("");
    $("#input-devices-manufacturer").val("");
    $("#input-devices-serialnumber").val("");
    $("#input-devices-accessories").val("");
    $("#input-devices-comments").val("");

    $("#button-devices-save").removeClass("collapse");
    $("#button-devices-edit").addClass("collapse");

    $("#add-device").show();
    $("#software-list").show();
    $("#validate-input-devices-name").hide();
});

// Button "Geräteliste -> Bearbeiten"
$("#button-edit-device").on("click", function () {
    deviceEditedIndex = deviceListBoxSelectedIndex;

    $("#input-devices-name").val(deviceList[deviceListBoxSelectedIndex].Name);
    $("#input-devices-type").val(deviceList[deviceListBoxSelectedIndex].DeviceType);
    $("#input-devices-manufacturer").val(deviceList[deviceListBoxSelectedIndex].Manufacturer);
    $("#input-devices-serialnumber").val(deviceList[deviceListBoxSelectedIndex].SerialNumber);
    $("#input-devices-accessories").val(deviceList[deviceListBoxSelectedIndex].Accessories);
    $("#input-devices-comments").val(deviceList[deviceListBoxSelectedIndex].Comments);

    $("#button-devices-save").addClass("collapse");
    $("#button-devices-edit").removeClass("collapse");

    $("#validate-input-devices-name").hide();

    var softwareListBox = $('#software-listbox');
    softwareListBox.empty();

    for (let software of deviceList[deviceEditedIndex].Software) {
        var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + software.Name + '</button>');
        newItem.click(function () {
            softwareListBoxSelectedIndex = $(this).index();
            $("#button-edit-software").prop("disabled", false);
            $("#button-delete-software").prop("disabled", false);
        });

        $('#software-listbox').append(newItem);
    }

    tempSoftwareList = deviceList[deviceEditedIndex].Software;

    $("#add-device").show();
    $("#software-list").show();
    $("#add-software").hide();

    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
});

// Button "Geräteliste -> Löschen"
$("#button-delete-device").on("click", function () {
    $('#device-listbox .device-listbox-item').eq(deviceListBoxSelectedIndex).remove();
    deviceList.splice(deviceListBoxSelectedIndex, 1);

    $("#add-device").hide();
    $("#software-list").hide();
    $("#add-software").hide();

    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
});

// Button "Gerät -> Speichern"
$("#button-devices-save").on("click", function () {
    if ($("#input-devices-name").val() == "") {
        $("#validate-input-devices-name").show();
    }
    else {
        $("#add-device").hide();
        $("#software-list").hide();
        $("#add-software").hide();

        var newDevice = new Device();
        newDevice.Name = $("#input-devices-name").val();
        newDevice.DeviceType = $("#input-devices-type").val();
        newDevice.Manufacturer = $("#input-devices-manufacturer").val();
        newDevice.SerialNumber = $("#input-devices-serialnumber").val();
        newDevice.Accessories = $("#input-devices-accessories").val();
        newDevice.Comments = $("#input-devices-comments").val();
        newDevice.Software = tempSoftwareList;

        deviceList.push(newDevice);

        var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + newDevice.Name + '</button>');
        newItem.click(function () {
            deviceListBoxSelectedIndex = $(this).index();
            $("#button-edit-device").prop("disabled", false);
            $("#button-delete-device").prop("disabled", false);
        });

        $('#device-listbox').append(newItem);

        $('#software-listbox').empty()
    }
});

// Button "Gerät -> Ändern"
$("#button-devices-edit").on("click", function () {
    if ($("#input-devices-name").val() == "") {
        $("#validate-input-devices-name").show();
    }
    else {
        $("#add-device").hide();
        $("#software-list").hide();
        $("#add-software").hide();

        var oldDevice = deviceList[deviceEditedIndex];
        oldDevice.Name = $("#input-devices-name").val();
        oldDevice.DeviceType = $("#input-devices-type").val();
        oldDevice.Manufacturer = $("#input-devices-manufacturer").val();
        oldDevice.SerialNumber = $("#input-devices-serialnumber").val();
        oldDevice.Accessories = $("#input-devices-accessories").val();
        oldDevice.Comments = $("#input-devices-comments").val();

        $('#device-listbox .device-listbox-item').eq(deviceEditedIndex).text(oldDevice.Name);

        $('#software-listbox').empty()
    }
});

// Button "Gerät -> Abbrechen"
$("#button-devices-cancel").on("click", function () {
    $("#add-device").hide();
    $("#software-list").hide();
    $("#add-software").hide();
});

// Button "Softwareliste -> Hinzufügen"
$("#button-add-software").on("click", function () {
    $("#input-software-name").val("");
    $("#input-software-comments").val("");

    $("#validate-input-software-name").hide();

    $("#button-software-save").removeClass("collapse");
    $("#button-software-edit").addClass("collapse");

    $("#add-software").show();
});

// Button "Softwareliste -> Bearbeiten"
$("#button-edit-software").on("click", function () {
    $("#add-software").show();

    $("#validate-input-software-name").hide();

    $("#button-software-save").addClass("collapse");
    $("#button-software-edit").removeClass("collapse");

    softwareEditedIndex = softwareListBoxSelectedIndex;

    $("#input-software-name").val(tempSoftwareList[softwareListBoxSelectedIndex].Name);
    $("#input-software-comments").val(tempSoftwareList[softwareListBoxSelectedIndex].Comments);

    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);
});

// Button "Softwareliste -> Löschen"
$("#button-delete-software").on("click", function () {
    $('#software-listbox .software-listbox-item').eq(softwareListBoxSelectedIndex).remove();
    $("#add-software").hide();
    tempSoftwareList.splice(softwareListBoxSelectedIndex, 1);

    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);
});

// Button "Software -> Speichern"
$("#button-software-save").on("click", function () {
    if ($("#input-software-name").val() == "") {
        $("#validate-input-software-name").show();
    }
    else {
        $("#add-software").hide();

        var newSoftware = new Software();
        newSoftware.Name = $("#input-software-name").val();
        newSoftware.Comments = $("#input-software-comments").val();

        tempSoftwareList.push(newSoftware);

        var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + newSoftware.Name + '</button>');
        newItem.click(function () {
            softwareListBoxSelectedIndex = $(this).index();
            $("#button-edit-software").prop("disabled", false);
            $("#button-delete-software").prop("disabled", false);
        });

        $("#button-edit-software").prop("disabled", true);
        $("#button-delete-software").prop("disabled", true);

        $('#software-listbox').append(newItem);
    }
});

// Button "Software -> Ändern"
$("#button-software-edit").on("click", function () {
    if ($("#input-software-name").val() == "") {
        $("#validate-input-software-name").show();
    }
    else {
        $("#add-software").hide();

        var oldSoftware = tempSoftwareList[softwareEditedIndex];
        oldSoftware.Name = $("#input-software-name").val();
        oldSoftware.Comments = $("#input-software-comments").val();

        $('#software-listbox .software-listbox-item').eq(softwareEditedIndex).text(oldSoftware.Name);
    }
});

// Button "Software -> Abbrechen"
$("#button-software-cancel").on("click", function () {
    $("#add-software").hide();
});

// Button "Hauptseite -> Speichern"
$("#button-main-save-ticket").on("click", function (event) {
    var ticketType = radioTicketTypeSelectedValue;

    $('#ticketTypeInput').val(ticketType);

    if (radioTicketTypeSelectedValue == TicketTypes.Repair || radioTicketTypeSelectedValue == TicketTypes.DataRecovery) {
        if ($("#input-device-name").val() != "") {
            var newDevice = new Device();
            newDevice.Name = $("#input-device-name").val();
            newDevice.DeviceType = $("#input-device-type").val();
            newDevice.Manufacturer = $("#input-device-manufacturer").val();
            newDevice.SerialNumber = $("#input-device-serialnumber").val();
            newDevice.Accessories = $("#input-device-accessories").val();
            newDevice.Comments = $("#input-device-comments").val();

            newDevice.Software = [];
            deviceList = [];

            deviceList.push(newDevice);
        }
    }

    $('#deviceListInput').val(JSON.stringify(deviceList));
});


