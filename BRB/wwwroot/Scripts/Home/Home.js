﻿let userId = $('#hdfUserId').val();
let lastSelectedVisitedId = $('#hdfLastSelectedVisitedId').val();
let lastSectionCompletedId = $('#hdfLastSectionCompletedId').val();


$('#btnGenResume').click(function () {
    debugger
    $.ajax({
        url: '/Resume/GenerateResume',
        type: 'get',
        success: function (response) {
          
        },
        error: function (err) {
            debugger
        }
    });
})