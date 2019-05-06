
//Dashboard.cshtml Mobile sidebar Javascript

function w3_open() {
    document.getElementById("mySidebar").style.display = "block";
}

function w3_close() {
    document.getElementById("mySidebar").style.display = "none";
}


//---------------------------- Oasis -------------------------------
//When loged out back button wont work

    window.history.forward();
     function noBack() {window.history.forward(); }

//Edit Tables popover for the dashboard
$(function () {
    $('[data-toggle="popover"]').popover()
})