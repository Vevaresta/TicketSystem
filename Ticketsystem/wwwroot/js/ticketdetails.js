import cud from './cud.js'

$(document).ready(function () {
    cud.initTabs();
    cud.setViewType("Details");
    cud.initBackup();
    cud.initDevices();

    $(".bg-lightyellow").focus(function () {
        $(this).addClass("bg-lightyellow")
    });
    $(".form-control").addClass("bg-lightyellow");
});

// Changes-Listbox-Item Click
$(".changes-listbox-item").on("click", function (event) {
    cud.onClickChangesListBoxItem(event);
});