$(document).ready(function () {
    $("#btn-search").on("click", function () {
        if ($("#search-form").hasClass("collapse")) {
            $("#search-form").removeClass("collapse");
            $("#btn-search").text("Schließen");
        }
        else {
            $("#search-form").addClass("collapse");
            $("#btn-search").text("Suchen");
        }
    });
});