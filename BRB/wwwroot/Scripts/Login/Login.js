
$('#btnLogin').click(function () {
    $('#loginForm').validate();
    if ($('#loginForm').valid()) {
        $.ajax({
            url: '/Account/Login',
            type: 'POST',
            data: { UserName: $('#email').val(), Password: $('#pwd').val() },
            success: function (response) {
                if (response.success) {
                    swal("Login Successfull", response.message, "success");
                    window.location.href = response.redirect;
                } else {
                    swal("Invalid Username or Password", response.message, "error");
                }


            },
            error: function (err) {
                console.log("error on ajax request")
            }
        });
    }

});

$('#FormVoucherVerfication').on('submit', function (e) {
    e.preventDefault();

    $.ajax({
        url: '/Resume/VerifyVoucher',
        type: 'POST',
        data: { voucherCode: $('#txtVoucher').val() },
        success: function (response) {
            if (response.success) {
                swal("Success", response.message, "success");
                setTimeout(function () { window.location.href = response.redirect; }, 1000);
               
            } else {
                swal("Error", response.message, "error");
            }


        },
        error: function (err) {
            console.log("error on ajax request")
        }
    });

})

const togglePassword = document
    .querySelector('#togglePassword');
const password = document.querySelector('#pwd');
togglePassword.addEventListener('click', () => {
    // Toggle the type attribute using
    // getAttribure() method
    const type = password
        .getAttribute('type') === 'password' ?
        'text' : 'password';
    password.setAttribute('type', type);
    // Toggle the eye and bi-eye icon
    this.classList.toggle('bi-eye');
});

$(document).on('keypress', function (e) {
    if (e.keyCode === 13 || e.which === 13) {
        $('#btnLogin').trigger('click');
    }

})

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
});

