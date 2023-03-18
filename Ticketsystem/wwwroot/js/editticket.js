import Device from './models/device.js';
import Software from './models/software.js';
import TicketTypes from './models/tickettypes.js';

$(document).ready(function () {
    $(".bg-lightyellow").focus(function () {
        $(this).addClass("bg-lightyellow")
    });
    $(".form-control").addClass("bg-lightyellow");

    var ticketType = $("#ticket-type").val();

    var einzelgeraet = $("#einzelgeraet");
    var geraetetab = $("#geraetetab");
    var backup = $("#backup")

    if (ticketType == TicketTypes.Repair || ticketType == TicketTypes.DataRecovery) {
        $('#tabs a[href="#tab1"]').tab('show');
        einzelgeraet.show();
        geraetetab.hide();
        backup.show();
        $('#input-device-name').rules('add', 'required');
    }
    else if (ticketType == TicketTypes.Consultation) {
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