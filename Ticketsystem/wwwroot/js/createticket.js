﻿import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';
import cud from './cud.js'

// Wird beim Laden der Seite ausgeführt
$(document).ready(function () {
    cud.initTabs();
    cud.setViewType("Create");

    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);

    // Umschalten der Ticketart durch Klick auf Radio-Button oder Label
    $(".ticket-type-container").on("click", function (event) {
        var label = $(event.target);
        var radio = label.prev('input[type=radio]');

        cud.radioTicketTypeSelectedValue = radio.val();
        radio.prop("checked", true);

        var singleDevice = $("#single-device");
        var devicesTab = $("#devices-tab");
        var backup = $("#backup")

        if (cud.radioTicketTypeSelectedValue == TicketTypes.Repair || cud.radioTicketTypeSelectedValue == TicketTypes.DataRecovery) {
            singleDevice.show();
            devicesTab.hide();
            backup.show();
            $('#input-device-name').rules('add', 'required');
            $('#tabs a[href="#tab1"]').tab('show');
        }
        else if (cud.radioTicketTypeSelectedValue == TicketTypes.Consultation) {
            singleDevice.hide();
            devicesTab.hide();
            backup.hide();
            $('#input-device-name').rules('remove', 'required');
            $('#tabs a[href="#tab1"]').tab('show');
        }
        else {
            singleDevice.hide();
            devicesTab.show();
            backup.show();
            $('#input-device-name').rules('remove', 'required');
        }
    });
});

// Erlaube die Auswahl des Backup-Durchführenden durch Klick auf Label
$(".label-backup-choice").on("click", function (event) {
    cud.onClickBackupChoicesLabel();
});

// Switch für "Datensicherung?"
$("#switch-backup").on('change', function () {
    cud.onClickBackupSwitch();
});

// Button "Geräteliste -> Hinzufügen"
$("#button-add-device").on("click", function () {
    cud.onClickButtonDevicesListNew();
});

// Button "Geräteliste -> Bearbeiten"
$("#button-edit-device").on("click", function () {
    cud.onClickButtonDevicesListEdit();
});

// Button "Geräteliste -> Löschen"
$("#button-delete-device").on("click", function () {
    cud.onClickButtonDeviceDelete();
});

// Button "Gerät -> Speichern"
$("#button-devices-save").on("click", function () {
    cud.onClickButtonDeviceSave();
});

// Button "Gerät -> Ändern"
$("#button-devices-edit").on("click", function () {
    cud.onClickButtonDeviceUpdate();
});

// Button "Gerät -> Abbrechen"
$("#button-devices-cancel").on("click", function () {
    cud.onClickButtonDeviceCancel();
});

// Button "Softwareliste -> Hinzufügen"
$("#button-add-software").on("click", function () {
    cud.onClickButtonSoftwareListNew();
});

// Button "Softwareliste -> Bearbeiten"
$("#button-edit-software").on("click", function () {
    cud.onClickButtonSoftwareListEdit();
});

// Button "Softwareliste -> Löschen"
$("#button-delete-software").on("click", function () {
    cud.onClickButtonSoftwareListDelete();
});

// Button "Software -> Speichern"
$("#button-software-save").on("click", function () {
    cud.onClickButtonSoftwareSave();
});

// Button "Software -> Ändern"
$("#button-software-edit").on("click", function () {
    cud.onClickButtonSoftwareUpdate();
});

// Button "Software -> Abbrechen"
$("#button-software-cancel").on("click", function () {
    cud.onClickButtonSoftwareCancel();
});

// Button "Hauptseite -> Speichern"
$("#button-main-save-ticket").on("click", function (event) {
    var ticketType = cud.radioTicketTypeSelectedValue;

    $('#ticketTypeInput').val(ticketType);

    if (cud.radioTicketTypeSelectedValue == TicketTypes.Repair || cud.radioTicketTypeSelectedValue == TicketTypes.DataRecovery) {
        if ($("#input-device-name").val() != "") {
            var newDevice = new Device();
            newDevice.Name = $("#input-device-name").val();
            newDevice.DeviceType = $("#input-device-type").val();
            newDevice.Manufacturer = $("#input-device-manufacturer").val();
            newDevice.SerialNumber = $("#input-device-serialnumber").val();
            newDevice.Accessories = $("#input-device-accessories").val();
            newDevice.Comments = $("#input-device-comments").val();

            newDevice.Software = [];
            cud.deviceList = [];

            cud.deviceList.push(newDevice);
        }
    }

    $('#deviceListInput').val(JSON.stringify(cud.deviceList));
});


