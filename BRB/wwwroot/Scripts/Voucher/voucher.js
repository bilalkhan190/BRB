$('#voucherForm').on('submit', function (e) {
    e.preventDefault();
    $(this).validate();
    if ($(this).valid()) {
        $.ajax({
            url: '/admin/CreateVoucher',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                if (response.success) {
                    swal("Created Successful", response.message, "success");

                } else {
                    swal("Error", response.message, "error");
                }
                $('#voucherForm').trigger('reset')
            },
            error: function (err) { }
        })
    }
      
    
  
})