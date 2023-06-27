$('#btnSaveHonor').click(function () {
    let obj = { CollegeId:  $('#hdfCollegeId').val() , HonorName: $('#txtAcademicHonor').val(), Date: $('#txtDateReceived').val() }
    $.ajax({
        url: '/Education/PostDataAcademicHonor',
        type: 'POST',
        data: obj,
        success: function (resp) {
            console.log(resp.data.message)
        },
        error: function (err) { }
        });
})