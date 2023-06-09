﻿import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';
import cud from './cud.js'

// Wird beim Laden der Seite ausgeführt
$(document).ready(function () {
    cud.initTabs();
    cud.setViewType("Create");
    cud.initDevices();
    cud.disableDeviceButtons();

    let ticketType = $("#hidden-ticket-type").val();
    if (ticketType == "Special") {
        changeTicketType($("#radio-special"));
    }

    let backupByClient = $("#hidden-backup-by-client").val();
    let backupByStaff = $("#hidden-backup-by-staff").val();

    $("#radio-backup-client").prop("checked", backupByClient);
    $("#radio-backup-staff").prop("checked", backupByStaff);
});

// Umschalten der Ticketart durch Klick auf Radio-Button oder Label
$(".ticket-type-input").on("click", function (event) {
    var radio = $(event.target);
    changeTicketType(radio);
});

// Umschalten der Ticketart durch Klick auf Radio-Button oder Label
$(".ticket-type-label").on("click", function (event) {
    var label = $(event.target);
    var radio = label.prev('input[type=radio]');
    changeTicketType(radio);
});

function changeTicketType(radio) {
    cud.radioTicketTypeSelectedValue = radio.val();
    radio.prop("checked", true);

    var singleDevice = $("#single-device");
    var devicesTab = $("#devices-tab");
    var backup = $("#backup")

    if (cud.radioTicketTypeSelectedValue == TicketTypes.Repair || cud.radioTicketTypeSelectedValue == TicketTypes.DataRecovery) {
        singleDevice.show();
        devicesTab.hide();
        backup.show();
        if ($('#switch-backup').prop('checked')) {
            $("#backup-choices").removeClass("collapse");
        }
        $('#input-client-email').rules('add', 'required');
        $('#input-device-name').rules('add', 'required');
        $('#tabs a[href="#tab1"]').tab('show');
    }
    else if (cud.radioTicketTypeSelectedValue == TicketTypes.Consultation) {
        singleDevice.hide();
        devicesTab.hide();
        backup.hide();
        $("#backup-choices").addClass("collapse");
        $('#input-client-email').rules('remove', 'required');
        $('#input-device-name').rules('remove', 'required');
        $('#tabs a[href="#tab1"]').tab('show');
    }
    else {
        if ($('#switch-backup').prop('checked')) {
            $("#backup-choices").removeClass("collapse");
        }
        singleDevice.hide();
        devicesTab.show();
        backup.show();
        $('#input-client-email').rules('add', 'required');
        $('#input-device-name').rules('remove', 'required');
    }
}

$("#switch-send-email").on("change", function () {
    if ($("#switch-send-email").prop("checked")) {
        $("#button-send-email").show();
    }
    else {

        $("#button-send-email").hide();
    }
});

// Switch für "Datensicherung?"
$("#switch-backup").on('change', function () {
    cud.onClickBackupSwitch();
});

// Erlaube die Auswahl des Backup-Durchführenden durch Klick auf Label
$(".label-backup-choice").on("click", function (event) {
    cud.onClickBackupChoicesLabel(event);
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
$("#button-main-save-ticket").on("click", function () {
    cud.onClickButtonMainSave();
});


