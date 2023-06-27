$(document).ready(function() {
    $(".sidebar ul.nav li.nav-item a.nav-link").click(function(){
        $(".sidebar ul.nav li.nav-item a.nav-link").removeClass("active");
        $(this).addClass("active");
    });
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl)
    })
})

