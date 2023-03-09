import Device from "../models/device.js";
import Software from "../models/software.js";

// Vaiable für temporäre Softwareliste
var tempSoftwareList = []

// Listbox "Geräteliste" -> Index des ausgewählten items speichern 
var deviceListBoxSelectedIndex = -1;

// Listbox "Softwareliste" -> Index des ausgewählten items speichern 
var softwareListBoxSelectedIndex = -1;

// Listenindex des aktuell bearbeiteten Geräts bei Klick auf "Bearbeiten" 
var deviceEditedIndex = -1;

// Listenindex der aktuell bearbeiteten Software bei Klick auf "Bearbeiten" 
var softwareEditedIndex = -1;

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

    for (let software of deviceList[deviceEditedIndex].Software) {
        var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + software.Name + '</button>');
        newItem.click(function () {
            $("#button-edit-software").prop("disabled", false);
            $("#button-delete-software").prop("disabled", false);
        });

        $('#software-listbox').append(newItem);
    }

    $("#add-device").show();
    $("#software-list").show();
    $("#add-software").hide();

    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
});

// Button "Geräteliste -> Löschen"
$("#button-delete-device").on("click", function () {
    $('#device-listbox .device-listbox-item').eq(deviceListBoxSelectedIndex).remove();
    deviceList.slice(deviceListBoxSelectedIndex, 1);

    $("#add-device").hide();
    $("#software-list").hide();
    $("#add-software").hide();

    $("#button-edit-device").prop("disabled", true);
    $("#button-delete-device").prop("disabled", true);
});

// Button "Gerät -> Speichern"
$("#button-devices-save").on("click", function () {
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
        deviceListBoxSelectedIndex = newItem.index();

        $("#button-edit-device").prop("disabled", false);
        $("#button-delete-device").prop("disabled", false);
    });

    $('#device-listbox').append(newItem);

    $('#software-listbox').empty()

    $('#deviceListInput').val(JSON.stringify(deviceList));
});

// Button "Gerät -> Ändern"
$("#button-devices-edit").on("click", function () {
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

    $('#deviceListInput').val(JSON.stringify(deviceList));
});

// Button "Gerät -> Abbrechen"
$("#button-devices-cancel").on("click", function () {
    $("#add-device").hide();
    $("#software-list").hide();
    $("#add-software").hide();
});

// Button "Softwareliste -> Hinzufügen"
$("#button-add-software").on("click", function () {
    $("#add-software").show();
});

// Button "Softwareliste -> Bearbeiten"
$("#button-edit-software").on("click", function () {
    $("#add-software").show();

    $("#button-edit-software").prop("disabled", true);
    $("#button-delete-software").prop("disabled", true);
});

// Button "Softwareliste -> Löschen"
$("#button-delete-software").on("click", function () {
    $('#software-listbox .software-listbox-item').eq(softwareListBoxSelectedIndex).remove();
    $("#add-software").hide();
});

// Button "Software -> Speichern"
$("#button-software-save").on("click", function () {
    $("#add-software").hide();

    var newSoftware = new Software();
    newSoftware.Name = $("#input-software-name").val();
    newSoftware.Comments = $("#input-software-comments").val();

    tempSoftwareList.push(newSoftware);

    var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + newSoftware.Name + '</button>');
    newItem.click(function () {
        $("#button-edit-software").prop("disabled", false);
        $("#button-delete-software").prop("disabled", false);
    });

    $('#software-listbox').append(newItem);

    $('#deviceListInput').val(JSON.stringify(deviceList));
});

// Button "Gerät -> Ändern"
$("#button-software-edit").on("click", function () {
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

    $('#deviceListInput').val(JSON.stringify(deviceList));
});

// Button "Software -> Abbrechen"
$("#button-software-cancel").on("click", function () {
    $("#add-software").hide();
});
