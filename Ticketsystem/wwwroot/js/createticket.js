$(document).ready(function () {
    var lastname = $('.lastname');
    lastname.on('input', function () {
        var currentValue = $(this).val();
        lastname.not(this).val(currentValue);
    });

    var firstname = $('.firstname');
    firstname.on('input', function () {
        var currentValue = $(this).val();
        firstname.not(this).val(currentValue);
    });

    var rehanumber = $('.rehanumber');
    rehanumber.on('input', function () {
        var currentValue = $(this).val();
        rehanumber.not(this).val(currentValue);
    });

    var coursenumber = $('.coursenumber');
    coursenumber.on('input', function () {
        var currentValue = $(this).val();
        coursenumber.not(this).val(currentValue);
    });

    var street = $('.street');
    street.on('input', function () {
        var currentValue = $(this).val();
        street.not(this).val(currentValue);
    });

    var postalcode = $('.postalcode');
    postalcode.on('input', function () {
        var currentValue = $(this).val();
        postalcode.not(this).val(currentValue);
    });

    var city = $('.city');
    city.on('input', function () {
        var currentValue = $(this).val();
        city.not(this).val(currentValue);
    });

    var email = $('.email');
    email.on('input', function () {
        var currentValue = $(this).val();
        email.not(this).val(currentValue);
    });

    var phonenumber = $('.phonenumber');
    phonenumber.on('input', function () {
        var currentValue = $(this).val();
        phonenumber.not(this).val(currentValue);
    });
});


var counter = 1;

function duplicate() {
    var original = document.getElementById('ProduktDaten');
    var clone = original.cloneNode(true);

    // Hinzufügen des Lösch-Buttons
    var deleteButton = document.createElement("button");
    deleteButton.innerHTML = "Produkt Löschen";
    deleteButton.classList.add("btn", "btn-lg", "btn-primary");
    deleteButton.onclick = function () { clone.remove(); counter--; };

    clone.appendChild(deleteButton);

    // Erhöhen der Zählvariable und Hinzufügen des Zählers
    counter++;
    var counterSpan = document.createElement("h5");
    counterSpan.innerHTML = counter + ". " + "Produkt:";
    clone.insertBefore(counterSpan, clone.firstChild);

    original.parentNode.appendChild(clone);
}


$(function () {
    $('#myTab a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show');
    });


});