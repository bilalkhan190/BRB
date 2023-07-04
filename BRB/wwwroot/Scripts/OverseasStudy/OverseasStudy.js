var overseasArray = [];
function GenerateRadio() {
    $.ajax({
        url: '/OverseasStudy/GetLivingSituationData',
        type: 'Get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                let html = `<div class="form-check">
                                    <label class="form-check-label">
                                        <input name="LivingSituationId"
                                               type="radio" class="form-check-input" value="${value.livingSituationId}" checked=""/>   ${value.livingSituationDesc}
                                    </label>
                                         
                                </div>`;
                $('#livingSituationSection').after(html);
            });
        },
        error: function (err) {
            alert(err)
        }
    });
}

$(document).ready(function () {

    //filling the dropdown
    $('#ddlCountry').html("");
    $('#ddlCountry').append('<option value="0" selected><b>Select Country</b></option>')
    $.ajax({
        url: '/Common/GetCountryList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlCountry').append(`<option value="${value.countryId}"><b> ${value.countryName} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    /*LoadData();*/

    //get submitted data
    $.ajax({
        url: '/OverseasStudy/GetDataById',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                overseasArray.push(value)
            });
            LoadData();
        },
        error: function (err) {

        }
    })


    GenerateRadio();
}); 
$('#btnSaveOverseasStudy').click(function () {
    $('#OverseasForm').validate();
    if ($('#OverseasForm').valid()) {
        let overseas = {
            overseasStudyId: $('#hdfOverseasStudyId').val(),
            collegeName: $('input[name="CollegeName"]').val(),
            City: $('input[name="City"]').val(),
            countryId: $('select[name="CountryId"]').val(),
            startedDate: $('input[name="StartedDate"]').val(),
            endDate: $('input[name="EndedDate"]').val(),
            classesCompleted: $('input[name="ClassesCompleted"]').val(),
            otherInfo: $('input[name="OtherInfo"]').val(),
            livingSituationOther: $('input[name="LivingSituationOther"]').val(),
            livingSituationId: $('input[name="LivingSituationId"]').val()
        }
        if (localStorage.getItem("pos-index") == null) {
            overseasArray.push(overseas);
        }
        else {
            overseasArray[parseInt(localStorage.getItem("pos-index"))] = overseas;
            localStorage.clear();
        }
        $("#DivSection").html("");
        LoadData();
      
    }
 
   
});


$(document).on('change', 'input[type="radio"]', function () {
    let input = "<input type='text' name='LivingSituationOther' class='form-control mt-2 mb-2' required id='txtLivingSituationOther'/>";
    if (this.value == 3) {
        $('#cbDiv').after(input);
    } else {
        $('input[name="LivingSituationOther"]').remove();
    }
});

$('#cbNotApply').click(function () {
    if (this.checked) {
        alert('checked')
        $('#cbkFinished').attr("disabled", true);
        $('#btnSaveAndContinue').attr("disabled", true);
    }
    else {
        alert('not checked')
        $('#cbkFinished').prop('disabled', false);
        $('#btnSaveAndContinue').prop('disabled', false);
    }
  
});

$('#btnClose').click(function () {
    ResetForm();
});

function ResetForm() {
    $('#hdfOverseasStudyId').val('');
    $('input[type="CollegeName"]').val('')
    $('input[type="City"]').val('')
    $('select[type="CountryId"]').val(0)
    $('input[type="StartedDate"]').val('')
    $('input[type="EndedDate"]').val('')
    $('input[type="ClassesCompleted"]').val('')
    $('input[type="OtherInfo"]').val('')
    $('input[type="LivingSituationOther"]').val('')
}

$(document).on('click', '#btnEditOverseas', function () {
    var response = overseasArray[$(this).attr('data-edit')];
    localStorage.setItem("pos-index", $(this).attr('data-edit'));
      let startDate = new Date(response.startedDate).toISOString().split('T')[0];
      let endDate = new Date(response.endedDate).toISOString().split('T')[0];
      console.log(response.collegeName)
    $('#hdfOverseasStudyId').val(response.overseasStudyId);
    $('#txtcollegeName').val(response.collegeName)
    $('#City').val(response.city);
    $('#ddlCountry').val(response.countryId)
    $('#txtStartDate').val(startDate)
    $('#txtEndDate').val(endDate)
    $('#txtClassSectionCompleted').val(response.classesCompleted)
    $('#txtOtherInfo').val(response.otherInfo)
    $('input[type="LivingSituationId"]').val(response.livingSituationId);
    $('#SummaryModal').modal('show')
});

$('#btnSaveAndContinue').click(function () {
    let data = {
        OverseasStudyId: $('#hdfOverseasStudyId').val(),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        OverseasStudies: overseasArray,
        IsComplete: $('#cbIsComplete').is(":checked")
    };
    $.ajax({
        url: '/OverseasStudy/postdata',
        type: 'post',
        data: data,
        success: function (response)
        {
            LoadData();
            if (response.redirect != null) {
                window.location.href = response.redirect
            }
           
        },
        error: function () { }
    })
});

function LoadData() {
    $("#DivSection").html("");
    $.each(overseasArray, function (index, value) {
        let html = ` 
                <div class="card col-md-12 p-0 mb-3 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-8">
                                <span class="card-text">
                                    <h5 class="title-text">${value.collegeName}</h5>
                                    <p class="text-muted">${value.startedDate}-${value.endDate}</p>
                                </span>
                            </div>
                            <div class="col-md-4">
                            <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2" id="btnDeleteOverseas" data-item='${value.overseasStudyId}' data-edit='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditOverseas" data-item='${value.overseasStudyId}' data-edit='${index}' class="btn custombtn customBtn-light w-auto ms-1">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                </div>
                            </div>
                        </div>
                      </div>
                </div>`
        $("#DivSection").append(html);
    });
    if (overseasArray != null && overseasArray.length > 3) {
        $("#DivSection").addClass("BoxHeight");
    }
}

$(document).on('click', '#btnDeleteOverseas', function () {

    localStorage.setItem("over-index", $(this).attr('data-edit'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/OverseasStudy/Delete?id=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("over-index"));
            overseasArray.splice(index, 1);
            LoadData();
        },
        error: function (err) {

        }
    });
});