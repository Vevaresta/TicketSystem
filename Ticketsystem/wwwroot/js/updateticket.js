import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';
import cud from './cud.js'

$(document).ready(function () {
    cud.initTabs();
    cud.setViewType("Update");
    cud.disableDeviceButtons();
    cud.initDevices();
    cud.initBackup();
    let backupSwitch = $("#switch-backup");
    if (backupSwitch.prop("checked")) {
        $("#backup-choices").show();
    }

    let isTicketChangeError = $("#hidden-ticket-change").val();
    if (isTicketChangeError) {
        $('#tabs a[href="#tab3"]').tab('show');
    }
});

// Switch für "Datensicherung?"
$("#switch-backup").on('change', function () {
    cud.onClickBackupSwitch();
});

// Erlaube die Auswahl des Backup-Durchführenden durch Klick auf Label
$(".label-backup-choice").on("click", function (event) {
    cud.onClickBackupChoicesLabel();
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