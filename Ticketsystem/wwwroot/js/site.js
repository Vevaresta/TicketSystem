// $(document).ready(showWidth);

// Fenster- / Dokumentengröße am unteren rechten Rand anzeigen
// Snippet geklaut von: https://stackoverflow.com/a/14184672
//
// Zum aktivieren obere Zeile ändern zu:
//     $(document).ready(showWidth);
function showWidth() {
    var MEASUREMENTS_ID = 'measurements';
    $("body").append('<div id="' + MEASUREMENTS_ID + '"></div>');
    $("#" + MEASUREMENTS_ID).css({
        'position': 'fixed',
        'bottom': '0',
        'right': '0',
        'background-color': 'black',
        'color': 'white',
        'padding': '5px',
        'font-size': '12px',
        'opacity': '0.7'
    });
    getDimensions = function () {
        return $(window).width() + ' (' + $(document).width() + ') x ' + $(window).height() + ' (' + $(document).height() + ')';
    }
    $("#" + MEASUREMENTS_ID).text(getDimensions());
    $(window).on("resize", function () {
        $("#" + MEASUREMENTS_ID).text(getDimensions());
    });
};