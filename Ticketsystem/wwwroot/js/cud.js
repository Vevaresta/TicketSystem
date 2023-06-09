﻿// JS-Modul mit gemeinsam genutzten Variablen und Funktionen für die Views:
//    /Tickets/Create
//    /Tickets/Update
//    /Tickets/Details

import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';

const cud = {
    radioTicketTypeSelectedValue: $('input[name="ticket-type"]:checked').val(),
    deviceList: [],
    tempSoftwareList: [],
    deviceListBoxSelectedIndex: undefined,
    softwareListBoxSelectedIndex: undefined,
    deviceEditedIndex: -1,
    softwareEditedIndex: -1,
    currentTabId: "",
    viewName: "",

    setViewType: function (vt) {
        this.viewName = vt;
    },

    initTabs: function () {
        $('#tabs a').on('click', function (e) {
            e.preventDefault();

            let tab = e.target.id;
            if (tab == "tab3-tab" && cud.currentTabId != "tab3-tab") {
                $("#changes-per-date").hide();
            }

            cud.currentTabId = e.target.id;

            $(this).tab('show');
        });

    },

    initDevices: function () {
        var devicesJson = $("#hidden-devices").val();
        if (devicesJson != "") {
            this.deviceList = JSON.parse(devicesJson)

            if (this.deviceList.length > 0) {
                this.deviceList.sort(this.sortListByNames);
            }

            var ticketType = $("#hidden-ticket-type").val();

            if (ticketType == TicketTypes.Repair || ticketType == TicketTypes.DataRecovery) {
                $('#tabs a[href="#tab1"]').tab('show');
                $("#input-device-name").val(this.deviceList[0].Name);
                $("#input-device-type").val(this.deviceList[0].DeviceType);
                $("#input-device-manufacturer").val(this.deviceList[0].Manufacturer);
                $("#input-device-serialnumber").val(this.deviceList[0].SerialNumber);
                $("#input-device-accessories").val(this.deviceList[0].Accessories);
                $("#input-device-comments").val(this.deviceList[0].Comments);
            }
            else if (ticketType == TicketTypes.Consultation) {
                $('#tabs a[href="#tab1"]').tab('show');
            }
            else {
                let cud = this;
                for (let device of this.deviceList) {
                    var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + device.Name + '</button>');
                    newItem.click(function () {
                        cud.onClickDevice(this);
                        cud.onClickButtonDevicesListEdit();
                    });
                    $('#device-listbox').append(newItem);

                    var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + device.Name + '</button>');
                }
            }
        }
    },

    disableDeviceButtons: function () {
        $("#button-delete-device").prop("disabled", true);
        $("#button-delete-software").prop("disabled", true);
    },

    onClickBackupSwitch: function () {
        var state = $("#switch-backup").prop("checked");
        if (state == true) {
            $("#backup-choices").removeClass("collapse");
        }
        else {
            $("#backup-choices").addClass("collapse");
        }
    },

    onClickBackupChoicesLabel: function (event) {
        let id = event.target.id;
        if (String(id).includes("client")) {
            $("#radio-backup-client").prop("checked", true);
        }
        if (String(id).includes("staff")) {
            $("#radio-backup-staff").prop("checked", true);
        }
    },

    onClickDevice: function (newItem) {
        cud.deviceListBoxSelectedIndex = $(newItem).index();
        $("#button-delete-device").prop("disabled", false);
        $(newItem).focusout(function () {
            setTimeout(function () {
                var selectedItem = $('#device-listbox .device-listbox-item:focus');
                if (selectedItem.length === 0) {
                    $("#button-delete-device").prop("disabled", true);
                }
            }, 350)
        });
    },

    onClickSoftware: function (newItem) {
        cud.softwareListBoxSelectedIndex = $(newItem).index();
        $("#button-delete-software").prop("disabled", false);
        $(newItem).focusout(function () {
            setTimeout(function () {
                var selectedItem = $('#software-listbox .software-listbox-item:focus');
                if (selectedItem.length === 0) {
                    $("#button-delete-software").prop("disabled", true);
                }
            }, 350)
        });
    },

    sortListByNames: function (a, b) {
        const nameA = a.Name.toUpperCase();
        const nameB = b.Name.toUpperCase();
        if (nameA < nameB) {
            return -1;
        }
        if (nameA > nameB) {
            return 1;
        }
        return 0;
    },

    onClickButtonDevicesListNew: function () {
        this.tempSoftwareList = [];
        $("#software-listbox").empty();

        $("#input-devices-name").val("");
        $("#input-devices-type").val("");
        $("#input-devices-manufacturer").val("");
        $("#input-devices-serialnumber").val("");
        $("#input-devices-accessories").val("");
        $("#input-devices-comments").val("");

        $("#button-devices-save").removeClass("collapse");
        $("#button-devices-edit").addClass("collapse");

        $("#add-software").hide();
        $("#add-device").show();
        $("#software-list").show();
        $("#validate-input-devices-name").hide();

        if ($(window).width() < 768) {
            $('html, body').animate({
                scrollTop: $('#add-device').offset().top
            }, 1000);
        }
    },

    onClickButtonDevicesListEdit: function () {
        this.deviceEditedIndex = this.deviceListBoxSelectedIndex;

        $("#input-devices-name").val(this.deviceList[this.deviceEditedIndex].Name);
        $("#input-devices-type").val(this.deviceList[this.deviceEditedIndex].DeviceType);
        $("#input-devices-manufacturer").val(this.deviceList[this.deviceEditedIndex].Manufacturer);
        $("#input-devices-serialnumber").val(this.deviceList[this.deviceEditedIndex].SerialNumber);
        $("#input-devices-accessories").val(this.deviceList[this.deviceEditedIndex].Accessories);
        $("#input-devices-comments").val(this.deviceList[this.deviceEditedIndex].Comments);

        $("#button-devices-save").addClass("collapse");
        $("#button-devices-edit").removeClass("collapse");

        $("#validate-input-devices-name").hide();

        var softwareListBox = $('#software-listbox');
        softwareListBox.empty();
        this.tempSoftwareList = this.deviceList[this.deviceEditedIndex].Software;

        if (this.tempSoftwareList.length > 0) {

            this.tempSoftwareList.sort(this.sortListByNames);

            let cud = this;
            for (let software of this.tempSoftwareList) {
                var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + software.Name + '</button>');
                newItem.click(function () {
                    cud.onClickSoftware(this);
                    cud.onClickButtonSoftwareListEdit();
                });

                $('#software-listbox').append(newItem);
            }
        }

        if (this.viewName != "Details") {
            $("#add-device").show();
            $("#add-software").hide();

            if ($(window).width() < 768) {
                $('html, body').animate({
                    scrollTop: $('#add-device').offset().top
                }, 1000);
            }
        }
        else {
            $("#show-device").show();
            $("#show-software").hide();

            if ($(window).width() < 768) {
                $('html, body').animate({
                    scrollTop: $('#show-device').offset().top
                }, 1000);
            }
        }

        $("#software-list").show();
    },

    onClickButtonDeviceDelete: function () {
        $('#device-listbox .device-listbox-item').eq(this.deviceListBoxSelectedIndex).remove();
        this.deviceList.splice(this.deviceListBoxSelectedIndex, 1);

        $("#add-device").hide();
        $("#software-list").hide();
        $("#add-software").hide();

        $("#button-delete-device").prop("disabled", true);
    },

    onClickButtonDeviceSave: function () {
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
            newDevice.Software = this.tempSoftwareList;

            this.deviceList.push(newDevice);

            let cud = this;
            var newItem = $('<button type="button" class="list-group-item list-group-item-action device-listbox-item">' + newDevice.Name + '</button>');
            newItem.click(function () {
                cud.onClickDevice(this);
                cud.onClickButtonDevicesListEdit();
            });

            $('#device-listbox').append(newItem);

            $('#software-listbox').empty()
        }
    },

    onClickButtonDeviceUpdate: function () {
        if ($("#input-devices-name").val() == "") {
            $("#validate-input-devices-name").show();
        }
        else {
            $("#add-device").hide();
            $("#software-list").hide();
            $("#add-software").hide();

            var oldDevice = this.deviceList[this.deviceEditedIndex];
            oldDevice.Name = $("#input-devices-name").val();
            oldDevice.DeviceType = $("#input-devices-type").val();
            oldDevice.Manufacturer = $("#input-devices-manufacturer").val();
            oldDevice.SerialNumber = $("#input-devices-serialnumber").val();
            oldDevice.Accessories = $("#input-devices-accessories").val();
            oldDevice.Comments = $("#input-devices-comments").val();

            $('#device-listbox .device-listbox-item').eq(this.deviceEditedIndex).text(oldDevice.Name);

            $('#software-listbox').empty()
        }
    },

    onClickButtonDeviceCancel: function () {
        $("#add-device").hide();
        $("#software-list").hide();
        $("#add-software").hide();
    },

    onClickButtonSoftwareListNew: function () {
        $("#input-software-name").val("");
        $("#input-software-comments").val("");

        $("#validate-input-software-name").hide();

        $("#button-software-save").removeClass("collapse");
        $("#button-software-edit").addClass("collapse");

        $("#add-software").show();

        if ($(window).width() < 768) {
            $('html, body').animate({
                scrollTop: $('#add-software').offset().top
            }, 1000);
        }
    },

    onClickButtonSoftwareListEdit: function () {
        this.softwareEditedIndex = this.softwareListBoxSelectedIndex;

        $("#input-software-name").val(this.tempSoftwareList[this.softwareEditedIndex].Name);
        $("#input-software-comments").val(this.tempSoftwareList[this.softwareEditedIndex].Comments);

        if (this.viewName != "Details") {

            $("#validate-input-software-name").hide();
            $("#button-software-save").addClass("collapse");
            $("#button-software-edit").removeClass("collapse");
            $("#add-software").show();

            if ($(window).width() < 768) {
                $('html, body').animate({
                    scrollTop: $('#add-software').offset().top
                }, 1000);
            }
        }
        else {
            $("#show-software").show();

            if ($(window).width() < 768) {
                $('html, body').animate({
                    scrollTop: $('#show-software').offset().top
                }, 1000);
            }
        }
    },

    onClickButtonSoftwareListDelete: function () {
        $('#software-listbox .software-listbox-item').eq(this.softwareListBoxSelectedIndex).remove();
        $("#add-software").hide();
        this.tempSoftwareList.splice(this.softwareListBoxSelectedIndex, 1);

        $("#button-delete-software").prop("disabled", true);
    },

    onClickButtonSoftwareSave: function () {
        if ($("#input-software-name").val() == "") {
            $("#validate-input-software-name").show();
        }
        else {
            $("#add-software").hide();

            var newSoftware = new Software();
            newSoftware.Name = $("#input-software-name").val();
            newSoftware.Comments = $("#input-software-comments").val();

            this.tempSoftwareList.push(newSoftware);

            let cud = this;
            var newItem = $('<button type="button" class="list-group-item list-group-item-action software-listbox-item">' + newSoftware.Name + '</button>');
            newItem.click(function () {
                cud.onClickSoftware(this);
                cud.onClickButtonSoftwareListEdit();
            });

            $("#button-delete-software").prop("disabled", true);

            $('#software-listbox').append(newItem);
        }
    },

    onClickButtonSoftwareUpdate: function () {
        if ($("#input-software-name").val() == "") {
            $("#validate-input-software-name").show();
        }
        else {
            $("#add-software").hide();

            var oldSoftware = this.tempSoftwareList[this.softwareEditedIndex];
            oldSoftware.Name = $("#input-software-name").val();
            oldSoftware.Comments = $("#input-software-comments").val();

            $('#software-listbox .software-listbox-item').eq(this.softwareEditedIndex).text(oldSoftware.Name);
        }
    },

    onClickButtonSoftwareCancel: function () {
        $("#add-software").hide();
    },

    onClickButtonMainSave: function () {
        var ticketType;

        if (this.viewName == "Update") {
            ticketType = $("#hidden-ticket-type").val();
        }
        else {
            ticketType = this.radioTicketTypeSelectedValue;
        }

        if (ticketType == TicketTypes.Repair || ticketType == TicketTypes.DataRecovery) {
            if ($("#input-device-name").val() != "") {
                var newDevice = new Device();
                newDevice.Id = $("#hidden-device-id").val();
                newDevice.Name = $("#input-device-name").val();
                newDevice.DeviceType = $("#input-device-type").val();
                newDevice.Manufacturer = $("#input-device-manufacturer").val();
                newDevice.SerialNumber = $("#input-device-serialnumber").val();
                newDevice.Accessories = $("#input-device-accessories").val();
                newDevice.Comments = $("#input-device-comments").val();

                newDevice.Software = [];
                this.deviceList = [];

                this.deviceList.push(newDevice);
            }
        }

        $('#ticketTypeInput').val(ticketType);
        $('#deviceListInput').val(JSON.stringify(this.deviceList));
    },

    onClickChangesListBoxItem(event) {
        let jsonString = $(event.target).val();
        let ticketChange = JSON.parse(jsonString);

        if (ticketChange.NewStatus != "") {
            $("#changes-input-oldstatus").val(ticketChange.OldStatus);
            $("#changes-input-newstatus").val(ticketChange.NewStatus);
            $("#changes-status-changed").removeClass("collapse");
            $("#changes-status-unchanged").addClass("collapse");
        }
        else {
            $("#changes-input-unchanged").val(ticketChange.OldStatus);
            $("#changes-status-changed").addClass("collapse");
            $("#changes-status-unchanged").removeClass("collapse");
        }
        let button = $(event.target);
        let date = button.text();
        $("#changes-per-date-header").text(date);
        $("#changes-input-username").val(ticketChange.UserName);
        $("#changes-input-email").val(ticketChange.Email);
        $("#changes-input-comment").val(ticketChange.Comment);
        $("#changes-per-date").show();

        if ($(window).width() < 768) {
            $('html, body').animate({
                scrollTop: $('#changes-per-date').offset().top
            }, 1000);
        }
    }
};

export default cud;
