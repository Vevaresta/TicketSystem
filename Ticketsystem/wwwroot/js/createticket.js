const radioButtons = document.querySelectorAll(".radio-ticket-type");

function handleRadioChange(event) {
    const selectedValue = event.target.value;

    var einzelgeraet = document.getElementById("einzelgeraet");
    var geraetetab = document.getElementById("geraetetab");

    if (selectedValue == "reparatur" || selectedValue == "datenrettung") {
        $('#tabs a[href="#tab1"]').tab('show');
        einzelgeraet.hidden = false;
        geraetetab.hidden = true;
    }
    else if (selectedValue == "beratung") {
        $('#tabs a[href="#tab1"]').tab('show');
        einzelgeraet.hidden = true;
        geraetetab.hidden = true;
    }
    else {
        einzelgeraet.hidden = true;
        geraetetab.hidden = false;
    }
}

radioButtons.forEach(function (radioButton) {
    radioButton.addEventListener('change', handleRadioChange);
});

$(function () {
    $('#tabs a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });
});

// Vaiable f�r Ger�teliste
var deviceList = []

// Vaiable f�r tempor�re Softwareliste
var tempSoftwareList = []

class Device {
    Id;
    Name;
    DeviceType;
    Manufacturer;
    SerialNumber;
    Accessories;
    Comments;
    Software = [];
}

class Software {
    Id;
    Name;
    Comments;
}

// Listbox "Ger�teliste" -> Index des ausgew�hlten items speichern 
var deviceListBoxSelectedIndex = -1;
$('.device-listbox-item').click(function () {
    deviceListBoxSelectedIndex = $(this).index();
});

// Listbox "Softwareliste" -> Index des ausgew�hlten items speichern 
var softwareListBoxSelectedIndex = -1;
$('.software-listbox-item').click(function () {
    softwareListBoxSelectedIndex = $(this).index();
});

// Button "Ger�teliste -> Hinzuf�gen"
$("#button-add-device").on("click", function () {
    $("#add-device").attr("hidden", false);
    $("#software-list").attr("hidden", false);
});

// Button "Ger�teliste -> L�schen"
$("#button-delete-device").on("click", function () {
    $('#device-listbox .device-listbox-item').eq(deviceListBoxSelectedIndex).remove();
    deviceList.slice(deviceListBoxSelectedIndex, 1);
});

// Button "Ger�t -> Speichern"
$("#button-device-save").on("click", function () {
    $("#add-device").attr("hidden", true);
    $("#software-list").attr("hidden", true);
    $("#add-software").attr("hidden", true);

    var newDevice = new Device();
    newDevice.Name = $("#input-device-name").val();
    newDevice.DeviceType = $("#input-device-type").val();
    newDevice.Manufacturer = $("#input-device-manufacturer").val();
    newDevice.SerialNumber = $("#input-device-serialnumber").val();
    newDevice.Accessories = $("#input-device-accessories").val();
    newDevice.Comments = $("#input-device-comments").val();
    newDevice.Software = tempSoftwareList;

    deviceList.push(newDevice);

    var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + newDevice.Name + '</button>');
    $('#device-listbox').append(newItem);

    $('#deviceListInput').val(JSON.stringify(deviceList));
});

// Button "Ger�t -> Abbrechen"
$("#button-device-cancel").on("click", function () {
    $("#add-device").attr("hidden", true);
    $("#software-list").attr("hidden", true);
    $("#add-software").attr("hidden", true);
});

// Button "Softwareliste -> Hinzuf�gen"
$("#button-add-software").on("click", function () {
    $("#add-software").attr("hidden", false);
});

// Button "Softwareliste -> L�schen"
$("#button-delete-software").on("click", function () {
    $('#software-listbox .software-listbox-item').eq(softwareListBoxSelectedIndex).remove();
});

// Button "Software -> Speichern"
$("#button-software-save").on("click", function () {
    $("#add-software").attr("hidden", true);

    var newSoftware = new Software();
    newSoftware.Name = $("#input-software-name").val();
    newSoftware.Comments = $("#input-software-comments").val();

    tempSoftwareList.push(newSoftware);

    var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + newSoftware.Name + '</button>');
    $('#software-listbox').append(newItem);
});

// Button "Software -> Abbrechen"
$("#button-software-cancel").on("click", function () {
    $("#add-software").attr("hidden", true);
});