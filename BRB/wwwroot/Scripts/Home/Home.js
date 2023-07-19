let userId = $('#hdfUserId').val();
let lastSelectedVisitedId = $('#hdfLastSelectedVisitedId').val();
let lastSectionCompletedId = $('#hdfLastSectionCompletedId').val();


$('#btnGenResume').click(function () {
    debugger
    $.ajax({
        url: '/Resume/GenerateResumeOnWord',
        type: 'get',
        success: function (response) {
            if (response.success) {
                alert(response.message);
                let a = document.createElement("a");
                a.href = "/downloads/" + response.data;
                a.download = response.data;
                a.click();
                a.innerText = "Download";
                a.classList.add("btn");
                a.classList.add("btn-primary");
                a.classList.add("custombtn");
                a.classList.add("w-auto");
                $("#btnGenResume").remove();
                document.getElementById("panel").append(a);


            }
        },
        error: function (err) {
            debugger
        }
    });
})