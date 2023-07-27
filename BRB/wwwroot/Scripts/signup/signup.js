$(document).ready(function () {

    $('#ddlStateAbbr').html("");
    $('#ddlStateAbbr').append('<option value="" selected><b>Select State</b></option>');

    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (resp) {
            debugger;
            $.each(resp.data, function (index, value) {
                $('#ddlStateAbbr').append(`<option value="${value.stateAbbr}"><b> ${value.stateName} </b></option>`);
            });
        },
        error: function (err) {
            alert(err)
        }
    });
});

$('#btnCreateUser').click(function () {
    $('#signUpForm').validate();
    if ($('#signUpForm').valid()) {
        $.ajax({
            url: '/Account/CreateUser',
            type: 'POST',
            data: $('#signUpForm').serialize(),
            success: function (response) {
                debugger
                if (response.success) {
                    swal("Account Status", response.message, "success");
                    window.location.href = response.redirect;
                } else {
                    swal("Account Status", response.message, "error");
                }


            },
            error: function (err) {
                console.log("error on ajax request")
            }
        });
    }

});

//$(document).ajaxSend(function (event, jqxhr, settings) {
//    $('#loader').fadeIn()
//    if (settings.type == 'POST') {
//        $.each($(event.delegateTarget).find('form'), (i, item) => {

//            $(item).attr('onkeypress', 'return event.keyCode != 13')
//        })
//    }
//})

//$(document).ajaxComplete(function (event, jqxhr, settings) {
//    if (settings.type == 'POST') {
//        $.each($(event.delegateTarget).find('form'), (i, item) => {

//            $(item).removeAttr('onkeypress')
//        })
//    }
//    $('#loader').fadeOut(250);
//});

