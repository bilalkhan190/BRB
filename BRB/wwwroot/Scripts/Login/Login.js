﻿
$('#btnLogin').click(function () {
    $.ajax({
        url: '/Account/Login',
        type: 'POST',
        data: { UserName: $('#email').val(), Password: $('#pwd').val() },
        success: function (response) {
            //$.notify(response.message, { position: "center" });
            window.location.href = response.redirect;

        },
        error: function (err) {
            console.log("error on ajax request")
        }
    });
});

$(document).ajaxSend(function (event, jqxhr, settings) {
    $('#loader').fadeIn()
    if (settings.type == 'POST') {
        $.each($(event.delegateTarget).find('form'), (i, item) => {

            $(item).attr('onkeypress', 'return event.keyCode != 13')
        })
    }
})

$(document).ajaxComplete(function (event, jqxhr, settings) {
    if (settings.type == 'POST') {
        $.each($(event.delegateTarget).find('form'), (i, item) => {

            $(item).removeAttr('onkeypress')
        })
    }
    $('#loader').fadeOut(250);
})