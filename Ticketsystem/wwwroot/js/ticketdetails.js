import TicketTypes from './models/tickettypes.js';

// Variable für Geräte
var devices = [];

// Variable für temporäre Softwareliste
var tempSoftwareList = [];

// Listbox "Geräteliste" -> Index des ausgewählten items speichern
var deviceListBoxSelectedIndex = -1;

// Listbox "Softwareliste" -> Index des ausgewählten items speichern
var softwareListBoxSelectedIndex = -1;

// Listenindex des aktuell bearbeiteten Geräts bei Klick auf "Bearbeiten"
var deviceShownIndex = -1;

// Listenindex der aktuell bearbeiteten Software bei Klick auf "Bearbeiten"
var softwareShownIndex = -1;

$(document).ready(function () {
    $(".bg-lightyellow").focus(function () {
        $(this).addClass("bg-lightyellow")
    });
    $(".form-control").addClass("bg-lightyellow");

    var ticketType = $("#hidden-ticket-type").val();
    var devicesJson = $("#hidden-devices").val();

    devices = JSON.parse(devicesJson)

    if (devices.length > 0) {
        devices.sort(sortListByNames);
    }

    var singleDevice = $("#single-device");
    var devicesTab = $("#devices-tab");
    var backup = $("#backup")

    if (ticketType == TicketTypes.Repair || ticketType == TicketTypes.DataRecovery) {
        $('#tabs a[href="#tab1"]').tab('show');
        $("#input-device-name").val(devices[0].Name);
        $("#input-device-type").val(devices[0].DeviceType);
        $("#input-device-manufacturer").val(devices[0].Manufacturer);
        $("#input-device-serialnumber").val(devices[0].SerialNumber);
        $("#input-device-accessories").val(devices[0].Accessories);
        $("#input-device-comments").val(devices[0].Comments);

        singleDevice.show();
        devicesTab.hide();
        backup.show();
    }
    else if (ticketType == TicketTypes.Consultation) {
        $('#tabs a[href="#tab1"]').tab('show');
        singleDevice.hide();
        devicesTab.hide();
        backup.hide();
    }
    else {
        for (let device of devices) {
            var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + device.Name + '</button>');
            newItem.click(function () {
                deviceListBoxSelectedIndex = $(this).index();
                onClickDevice();
            });

            $('#device-listbox').append(newItem);
        }
        singleDevice.hide();
        devicesTab.show();
        backup.show();
    }
});

// Tabliste initialisieren ("Daten" | "Geräte")
$(function () {
    $('#tabs a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show');
        $("#show-device").hide();
        $("#software-list").hide();
        $("#show-software").hide();
    });
});

// Geräte-Listbox -> Klick auf Gerät
function onClickDevice() {
    deviceShownIndex = deviceListBoxSelectedIndex;

    $("#software-listbox").empty();

    $("#input-devices-name").val(devices[deviceShownIndex].Name);
    $("#input-devices-type").val(devices[deviceShownIndex].DeviceType);
    $("#input-devices-manufacturer").val(devices[deviceShownIndex].Manufacturer);
    $("#input-devices-serialnumber").val(devices[deviceShownIndex].SerialNumber);
    $("#input-devices-accessories").val(devices[deviceShownIndex].Accessories);
    $("#input-devices-comments").val(devices[deviceShownIndex].Comments);

    if (devices[deviceShownIndex].Software.length > 0) {
        tempSoftwareList = devices[deviceShownIndex].Software;

        tempSoftwareList.sort(sortListByNames);

        for (let software of tempSoftwareList) {
            var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + software.Name + '</button>');
            newItem.click(function () {
                softwareListBoxSelectedIndex = $(this).index();
                onClickSoftware();
            });

            $('#software-listbox').append(newItem);
        }
    }

    $("#show-device").show();
    $("#software-list").show();
    $("#show-software").hide();
}

function onClickSoftware() {
    softwareShownIndex = softwareListBoxSelectedIndex;

    $("#input-software-name").val(tempSoftwareList[softwareShownIndex].Name);
    $("#input-software-comments").val(tempSoftwareList[softwareShownIndex].Comments);

    $("#show-software").show();
}

function sortListByNames(a, b) {
    const nameA = a.Name.toUpperCase();
    const nameB = b.Name.toUpperCase();
    if (nameA < nameB) {
        return -1;
    }
    if (nameA > nameB) {
        return 1;
    }
    return 0;
}