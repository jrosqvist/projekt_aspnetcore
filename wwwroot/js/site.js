// Materialize-funktioner
$(document).ready(function () {
    $(".sidenav").sidenav();
    $('.parallax').parallax();
    $(".tabs").tabs();
    $(".date-picker").datepicker({
        minDate: new Date()
    });
    $(".modal").modal();
    $(".scrollspy").scrollSpy();
    $(".materialboxed").materialbox();
    $('select').formSelect();
    $('.timepicker').timepicker({
        twelveHour: false,
        defaultTime: "18:00",
        disabledHours: [0, 1, 2, 3, 4, 5, 23],
        i18n: {
            cancel: "Stäng",
            done: "Ok"
        }
    });
});


// Ändrar utseendet på huvudmenyn vid scroll
window.onscroll = function () { scroll() };
function scroll() {
    if (document.body.scrollTop > 50 || document.documentElement.scrollTop > 50) {
        document.querySelector(".nav-wraper").style.position = "fixed";
        document.querySelector(".nav-wraper").style.zIndex = "10";
        document.querySelector(".nav-wraper").style.opacity = "0.95";
        document.querySelector(".nav-wraper").style.top = "0";
        //document.querySelector(".nav-wraper").style.backgroundColor = "rgb(24, 24, 24)";
    } else {
        document.querySelector(".nav-wraper").style.position = "relative";
        document.querySelector(".nav-wraper").style.opacity = "1";
        // document.querySelector(".nav-wraper").style.backgroundColor = "transparent";
        //document.querySelector(".nav-wraper").style.backgroundColor = "rgb(24, 24, 24)";
    }
}


// Plockar fram alla klassnamn med counter
const counters = document.querySelectorAll(".counter");
const speed = 400;

/* Uppdaterar företagsinformationen */
counters.forEach(function (counter) {
    const updateCount = () => {
        // + renderar ett nummer ist för textsträng
        const target = +counter.getAttribute("data-target");
        // Plockar fram texten (0)
        const count = +counter.innerText;
        // Sätter ökningshastigheten
        const increment = target / speed;

        // Körs tills slutsumman är uppnådd
        if (count < target) {
            // Ändrar innehållet
            counter.innerText = Math.ceil(count + increment);
            // Uppdaterar varje millisekund
            setTimeout(updateCount, 10);
        } else {
            // Slutliga antalet nått
            counter.innerText = Math.round(target);
        }
    }
    updateCount();
});

// Skriver ut rubiken på startsidan
const pitchTitle = document.getElementById("pitch-title");
const newText = "Pelican Palace Hotell & Restaurang";
let index = 1;

if (pitchTitle != null) {
    setInterval(writeText, 100);
}

function writeText() {
    pitchTitle.innerText = newText.slice(0, index);

    index++;

    /*
    if (index > newText.length) {
        index = 1;
    }
    */
}

// Ändra temat

function addDarkTheme() {
    $("p").addClass("white-text");
    $(".nav-wraper a").addClass("white-text");
    $("h1").addClass("white-text");
    $("h2").addClass("white-text");
    $("h3").addClass("white-text");
    $("h4").addClass("white-text");
    $("h5").addClass("white-text");
    $("h6").addClass("white-text");
    $("i").addClass("white-text");
    $("span").addClass("white-text");
    $("body").addClass("grey darken-4");
    $(".nav-wraper").addClass("grey darken-4");
    $(".card").addClass("grey darken-4");
    $(".tab").addClass("grey darken-4");
    $("input").addClass("white-text");
    $("textarea").addClass("white-text");
    $(".select-dropdown").addClass("white-text");
    $(".select-wrapper li").addClass("grey darken-4");
    $("th").addClass("white-text");
    $("tr").addClass("white-text");
    $(".sidenav").addClass("grey darken-4");
    $(".sidenav a").addClass("white-text");
}

function removeDarkTheme() {
    $("p").removeClass("white-text");
    $(".nav-wraper a").removeClass("white-text");
    $("h1").removeClass("white-text");
    $("h2").removeClass("white-text");
    $("h3").removeClass("white-text");
    $("h4").removeClass("white-text");
    $("h5").removeClass("white-text");
    $("h6").removeClass("white-text");
    $("i").removeClass("white-text");
    $("span").removeClass("white-text");
    $("body").removeClass("grey darken-4");
    $(".nav-wraper").removeClass("grey darken-4");
    $(".card").removeClass("grey darken-4");
    $(".tab").removeClass("grey darken-4");
    $("input").removeClass("white-text");
    $("textarea").removeClass("white-text");
    $(".select-dropdown").removeClass("white-text");
    $(".select-wrapper li").removeClass("grey darken-4");
    $("th").removeClass("white-text");
    $("tr").removeClass("white-text");
}

// Behåller temat mha local storage

$('#theme-box').change(function (e) {
    if (e.target.checked) {
        localStorage.checked = "true";
        addDarkTheme();
    } else {
        localStorage.checked = "false";
        removeDarkTheme();
    }
})

$(document).ready(function () {
    let check = localStorage.checked;
    if (check === "true") {
        $("#theme-box").prop("checked", true);
        addDarkTheme();
    } else {
        $("#theme-box").prop("checked", false);
        removeDarkTheme();
    }
});




// AJAX live-sökning efter hotellbokningar
$(function () {

    $("#hotel-searchbox").keyup(function (e) {
        var hs = $("#hotel-searchbox").val();
        $.get("/HotelBooking/Ajax?SearchString=" + hs, function (r) {
            //Uppdaterar infon

            $("#hotel-table").html(r);
        });

    });

});

// AJAX live-sökning efter bordsbokningar
$(function () {

    $("#restaurant-searchbox").keyup(function (e) {
        var rs = $("#restaurant-searchbox").val();
        $.get("/RestaurantBooking/Ajax?SearchString=" + rs, function (r) {
            //Uppdaterar infon

            $("#restaurant-table").html(r);
        });

    });

});




/*
function toggleTheme() {
    $("p").toggleClass("white-text");
    $(".nav-wraper a").toggleClass("white-text");
    $("h1").toggleClass("white-text");
    $("h2").toggleClass("white-text");
    $("h3").toggleClass("white-text");
    $("h4").toggleClass("white-text");
    $("h5").toggleClass("white-text");
    $("h6").toggleClass("white-text");
    $("i").toggleClass("white-text");
    $("input").toggleClass("white-text");
    $("th").toggleClass("white-text");
    $("td").toggleClass("white-text");
    $("tr").toggleClass("white-text");
    $("span").toggleClass("white-text");
    $("body").toggleClass("grey darken-4");
    $(".nav-wraper").toggleClass("grey darken-4");
    $(".card").toggleClass("grey darken-4");
}
*/