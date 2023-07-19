$(function () {
    //$('.sidebar').next().addClass('xo;-md-')
    //$('.sidebar').closest('div').remove()
    SearchProductReport()
})


//
$('#FormSProductReport').submit(function (e) {
    e.preventDefault()
    let StartDate = $("[name='fromDate']").val().replaceAll('-', '');
    let ReturnDate = $('[name="toDate"]').val().replaceAll('-', '');

    if ($('[name="fromDate"]').val().trim() != '' && $('[name="fromDate"]').val() != null) {

        if ($('[name="toDate"]').val().trim() == '' || $('[name="toDate"]').val() == null) {

            swal("Error", 'Please Select To Date', "error");
        }
        else if (ReturnDate.replaceAll('-', '') < StartDate.replaceAll('-', '')) {
            swal("Error", 'To Date Can not be less than From Date', "error");
        }
        else {

            SearchProductReport($('[name="fromDate"]').val(), $('[name="toDate"]').val(), $('[name="voucher"]').val(), $('[name="email"]').val())
        }
    } else {
        SearchProductReport($('[name="fromDate"]').val(), $('[name="toDate"]').val(), $('[name="voucher"]').val(), $('[name="email"]').val())

    }

});

const SearchProductReport = (fromDate = null, toDate = null, voucher = null, email = null) => {

    $.ajax({
        url: '/Admin/SearchProductReport',
        data: {
            fromDate: fromDate,
            toDate: toDate,
            voucher: voucher,
            email: email
        },
        type: 'Get',
        success: function (response) {
            console.log(response)
            if (!response.error) {
                response.data = JSON.parse(response.data)
                if (response.data != null) {
                    $('#TblProductReport tbody').html('')
                    let tr = ''
                    response.data.map((item, index) => {
                        tr += ` <tr>
                                    <td>${item.FirstName}</td>
                                    <td>${item.LastName}</td>
                                    <td>${item.UserName}</td>
                                    <td>${item.IsActive}</td>
                                    <td>${new Date(item.CreatedDate).toISOString().split('T')[0]}</td>
                                    <td>${item.Product}</td>
                                    <td>${item.VoucherCode}</td>
                                    <td>${new Date(item.GeneratedDate).toISOString().split('T')[0]}</td>
                                    <td>${item.Domain}</td>
                                </tr>`
                    })
                    $('#TblProductReport tbody').html(tr)
                }
            }
            debugger


        }, error: function (err) {
            alert('error')
        }
    })
}

