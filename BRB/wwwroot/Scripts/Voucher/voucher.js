$('#voucherForm').on('submit', function (e) {
    e.preventDefault();
    $.ajax({
        url: '/admin/createvoucher',
        type: 'POST',
        data: $(this).serialize(),
        success: function (response) {

        },
        error: function (err) { }
    })
})