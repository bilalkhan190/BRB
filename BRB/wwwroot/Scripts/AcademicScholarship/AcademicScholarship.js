$('#btnSaveScholarship').click(function () {
    let obj = { CollegeId: $('#hdfCollegeId').val(), ScholarshipName: $('#txtScholarshipCriteria').val(), ScholarshipCriteria: $('#txtDateReceived').val(), Date: $('#txtSDate').val() }
    $.ajax({
        url: '/Education/PostDataAcademicScholarship',
        type: 'POST',
        data: obj,
        success: function (resp) {
            console.log(response.data.message)
        },
        error: function (err) { }
    });
});