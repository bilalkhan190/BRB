let userId = $('#hdfUserId').val();
let lastSelectedVisitedId = $('#hdfLastSelectedVisitedId').val();
let lastSectionCompletedId = $('#hdfLastSectionCompletedId').val();

$('#btnGenerateResume').click(function () {
    $.ajax({
        url: '/Resume/GenerateResume',
        type: 'get',
        success: function (response) {
          
        },
        error: function (err) { }
    });
})