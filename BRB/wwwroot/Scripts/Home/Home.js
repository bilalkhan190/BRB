let userId = $('#hdfUserId').val();
let lastSelectedVisitedId = $('#hdfLastSelectedVisitedId').val();
let lastSectionCompletedId = $('#hdfLastSectionCompletedId').val();


$('#btnGenResume').click(function () {
    debugger
    $.ajax({
        url: '/Resume/GenerateResumeOnWord?font=' + $("input[name='Font']:checked").val(),
        type: 'get',
        success: function (response) {
            if (response.success) {
                swal("Resume Generated Successfullyl", response.message, "success");
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
            else {
                swal("Error", response.message, "error");
            }
        },
        error: function (err) {
            debugger
        }
    });
})