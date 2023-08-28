let userId = $('#hdfUserId').val();
let lastSelectedVisitedId = $('#hdfLastSelectedVisitedId').val();
let lastSectionCompletedId = $('#hdfLastSectionCompletedId').val();


$(document).on("click", '#btnGenResume', function () {
    debugger
    $.ajax({
        url: '/Resume/GenerateWordDocument?font=' + $("input[name='Font']:checked").val(),
        type: 'get',
        success: function (response) {
            if (response.success) {
                swal("Resume Generated Successfully!", response.message, "success");
                let a = document.createElement("a");
                a.href = "/downloads/" + response.data;
                a.download = response.data;
                a.click();
                a.innerText = "Download";
                a.classList.add("btn");
                a.classList.add("btn-primary");
                a.classList.add("custombtn");
                a.classList.add("w-auto");
                a.id = "btnDownloadResume";
                $("#btnGenResume").hide();
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

$('input[name="Font"]').change(function () {    
    $("#btnDownloadResume").remove();
    let btn = document.createElement("button");
    btn.classList.add("btn");
    btn.classList.add("btn-primary");
    btn.classList.add("custombtn");
    btn.classList.add("w-auto");
    btn.id = "btnGenResume";
    btn.innerText = "Generate Resume";
    $("#panel").html('')
    document.getElementById("panel").append(btn);

});


