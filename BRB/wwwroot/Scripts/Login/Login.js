
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
})