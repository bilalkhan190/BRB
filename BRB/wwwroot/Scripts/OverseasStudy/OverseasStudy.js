var overseasArray = [];
function GenerateRadio() {
    $.ajax({
        url: '/OverseasStudy/GetLivingSituationData',
        type: 'Get',
        success: function (response) {
            console.log(response.data)
            $.each(response.data, function (index, value) {
                console.log(value.livingSituationId)
                let html = `<div class="form-check">
                                    <label class="form-check-label">
                                        <input name="LivingSituationId"
                                               type="radio" class="form-check-input" ${value.livingSituationId == 1 ? `checked` : ``} value="${value.livingSituationId}" required/>   ${value.livingSituationDesc}
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
   
    $('#btnSaveOverseasStudy').click(function () {
        var sMonth = $("#ddlStartedMonth").val();
        var sYear = $("#ddlStartedYear").val();
        var eMonth = $("#ddlEndedMonth").val();
        var eYear = $("#ddlEndedYear").val();
        if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
            swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
            return;
        }
        $('#OverseasForm').validate();
        if ($('#OverseasForm').valid()) {
            //var from = $("#txtStartDate").val();
            //var to = $("#txtEndDate").val();

            //if (Date.parse(from) > Date.parse(to)) {
            //    swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
            //    return false;
            //}
            let overseas = {
                overseasStudyId: $('#hdfOverseasStudyId').val(),
                collegeName: $('input[name="CollegeName"]').val(),
                City: $('input[name="City"]').val(),
                countryId: $('select[name="CountryId"]').val(),
                CountryName: $('select[name="CountryId"] option:selected').text(),
                //startedDate: $('input[name="StartedDate"]').val(),
                //EndedDate: $('#txtEndDate').val(),
                StartedMonth:$('#ddlStartedMonth').val(),
                StartedYear :$('#ddlStartedYear').val(),
                EndedMonth : $('#ddlEndedMonth').val(),
                EndedYear: $('#ddlEndedYear').val(),
                StartedDate: new Date($('#ddlStartedMonth').val() + " " + $('#ddlStartedYear').val()),
                EndedDate: $('#ddlEndedMonth').val() != "" ? new Date($('#ddlEndedMonth').val() + " " + $('#ddlEndedYear').val()) : "",
                classesCompleted: $('input[name="ClassesCompleted"]').val(),
                otherInfo: $('input[name="OtherInfo"]').val(),
                livingSituationOther: $('input[name="LivingSituationOther"]').val(),
                livingSituationId: $('input[name="LivingSituationId"]:checked').val()
            }

            if ($(".cbCurrentlyIn").is(":checked")) {
                overseas.EndedDate = null;
            }
            if (localStorage.getItem("pos-index") == null) {
                overseasArray.push(overseas);
            }
            else {
                overseasArray[parseInt(localStorage.getItem("pos-index"))] = overseas;
                localStorage.clear();
            }
            ResetForm();
            console.log(overseasArray,'--add array')
            //$("#SummaryModal").removeClass('show').css("display", "none");
            //$(".modal-backdrop").css({
            //    zindex: "-1",
            //    position: "relative"
            //});
            //$('body').css("overflow", "auto");

          /*  $("#SummaryModal").hide().removeClass("show");*/
            //$(".modal-backdrop").css({
            //    display: "none",
            //    visibility: "hidden"
            //});
            $("#DivSection").html("");
            $('#SummaryModal').modal('toggle')
            LoadData();
            $("#OverseasForm").trigger("reset");
            $("#noList").hide();
            
        }
        //$("#summaryBtn").click(function () {
        //    //$("#SummaryModal").css("display", "block");
        //    $(".modal-backdrop").css({
        //        zindex: "1",
        //        position: "fixed"
        //    });
        //});

    });
    //filling the dropdown
    $('#ddlCountry').html("");
    $('#ddlCountry').append('<option value="" selected><b>Select Country</b></option>')
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

    $.ajax({
        url: '/OverseasStudy/GetMasterdata',
        type: 'get',
        success: function (response) {
            console.log(response.data)
            if (response.data != null) {
                $('#hdfOverseasExperienceId').val(response.data.overseasExperienceId);
                $('#sectionCheckBox').prop("checked", response.data.isOptOut).trigger('change');
                $('#cbkFinished').prop("checked", response.data.isComplete);
                $('#cbkFinished').prop("disabled", response.data.isComplete);
                if (response.data.overseasStudies.length > 0) {
                    $.each(response.data.overseasStudies, function (index, value) {
                        overseasArray.push(value)
                       
                    })
                    console.log(overseasArray,'--load array on load')
                }
            }
            LoadData();
        },
        error: function (err) {

        }
    });
    /*LoadData();*/

    //get submitted data
    
    //$.ajax({
    //    url: '/OverseasStudy/GetDataById',
    //    type: 'get',
    //    success: function (response) {
    //        if (response.data != null) {
    //            $.each(response.data, function (index, value) {
    //                overseasArray.push(value)
    //            });

    //        }
    //        LoadData();
    //    },
    //    error: function (err) {

    //    }
    //});

    GenerateRadio();
}); 



function clearField() {
    localStorage.clear();
    $('#OverseasForm').trigger('reset');
    //$('input[type="radio"]').trigger('change');
        $(document).find('#txtLivingSituationOther').remove();

    $('#SummaryModal').modal('toggle');
    $(".cbCurrentlyIn").prop("checked", false);
    if ($(".cbCurrentlyIn").is(':checked')) {
        $('.lblEndedDate').hide();
        $('#ddlEndedMonth').hide();
        $('#ddlEndedYear').hide();
      
        //$('#txtEndDate').val('');
    } else {
        $('.lblEndedDate').show();
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
       // $('#txtEndDate').show();
    }
}
$(document).on('change', 'input[type="radio"]', function () {
    generateOtherTextBox($(this));
});

function generateOtherTextBox(e) {
    let input = "<input type='text' name='LivingSituationOther' class='form-control mt-2 mb-2' required id='txtLivingSituationOther'/>";
    $('#otherpanel').html('');
    if (e.val() == 3) {
     
        $('#otherpanel').append(input);
    } else {
        $('#otherpanel').html();
    }
}
$('#sectionCheckBox').change(function () {
    if (this.checked) {
        $('#isOptSection').hide()
    }
    else {
        $('#isOptSection').show()
    }
  
});

$('#btnClose').click(function () {
    $('#OverseasForm').trigger('reset')
    $(document).find('#txtLivingSituationOther').remove();
    //$('input[name="LivingSituationOther"]').remove();
  
});

$('.cbCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#ddlEndedMonth').hide();
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').hide();
        $('#ddlEndedYear').val('');
        $('#labelEndedDate').hide();
    } else {
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
    }
});


function ResetForm() {
    $('#hdfOverseasStudyId').val('');
    $('input[type="CollegeName"]').val('')
    $('input[type="City"]').val('')
    $('select[type="CountryId"]').val('')
    $('input[type="StartedDate"]').val('')
    $('input[type="EndedDate"]').val('')
    $('input[type="ClassesCompleted"]').val('')
    $('input[type="OtherInfo"]').val('')
    $('input[type="LivingSituationOther"]').val('')
}

$(document).on('click', '#btnEditOverseas', function () {
    var response = overseasArray[$(this).attr('data-edit')];
    localStorage.setItem("pos-index", $(this).attr('data-edit'));
    let endDate;
    //let startDate = new Date(response.startedDate).toISOString().split('T')[0];
    console.log(response,'--overseas edit response')
    $('#hdfOverseasStudyId').val(response.overseasStudyId);
    $('#txtcollegeName').val(response.collegeName)
    $('#City').val(response.city);
    $('#ddlCountry').val(response.countryId);
    $('#ddlStartedMonth').val(response.startedMonth);
    $('#ddlStartedYear').val(response.startedYear);
    $('#ddlEndedMonth').val(response.endedMonth);
    $('#ddlEndedYear').val(response.endedYear);
    //$('#txtStartDate').val(startDate)
    if (response.endedMonth == "" || response.endedMonth == null  && response.endedYear == "" || response.endedYear == null) {
        //endDate = new Date(response.endedDate).toISOString().split('T')[0];
        //$('#txtEndDate').val(endDate)
        $(".cbCurrentlyIn").prop("checked", true);
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').val('');

    } else {
        $('#ddlEndedMonth').val(response.endedMonth);
        $('#ddlEndedYear').val(response.endedYear);
    }
  
       
    
    if ($('.cbCurrentlyIn').is(':checked')) {
        $('#ddlEndedMonth').hide();
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').hide();
        $('#ddlEndedYear').val('');
        $('#labelEndedDate').hide();
    }else {
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
    }
    $('#txtClassSectionCompleted').val(response.classesCompleted)
    $('#txtOtherInfo').val(response.otherInfo)
   
    $('input[name="LivingSituationId"][value="' + response.livingSituationId + '"]').prop("checked", true);
    generateOtherTextBox($('input[name="LivingSituationId"][value="' + response.livingSituationId + '"]'));
    $('#txtLivingSituationOther').val(response.livingSituationOther)
    $('#SummaryModal').modal('show')
});

$('#btnSaveAndContinue').click(function () {
    let data = {
        OverseasStudyId: $('#hdfOverseasStudyId').val(),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        OverseasStudies: overseasArray,
        IsComplete: $('#cbkFinished').is(":checked"),
        IsOptOut: $('#sectionCheckBox').is(":checked")
    };

    if (!data.IsOptOut) {
        if (overseasArray.length == 0) {
            swal("Positions Requried", "Please fill out studies to proceed", "error");
            return false;
        }
    }
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
function getMonth(index) {
    var mL = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return mL[index];
}
function LoadData() {
    $("#DivSection").html("");
    if (overseasArray.length > 0) {
        $("#noList").hide();
    }
    else {
        $("#noList").show();
    }
    overseasArray.reverse();
    overseasArray = covertArrayKeyIntoCamelCase(overseasArray)
   
       // < div class="text-muted" > ${ getMonth(new Date(value.startedDate).getMonth()) + " " + new Date(value.startedDate).getFullYear() } - ${ _endMonth }</div >
    $.each(overseasArray, function (index, value) {
        let _endMonth = "";
        let _countryName = "";
        let html = "";
        if (value.endedDate) { _endMonth = getMonth(new Date(value.endedDate).getMonth()) + " " + new Date(value.endedDate).getFullYear(); } else { _endMonth = "Present"; }
        if (value.countryName) {
            _countryName = value.countryName
        } else { _countryName = $('#ddlCountry option:selected').text() }
        //if (value.startedDate == null || value.startedDate == '' && value.endedDate == null || value.endedDate == '') {
        if (value.endedDate == null || value.endedDate == '') {
             html = ` 
                <div class="card col-md-12 p-0 mb-3 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-8">
                                <span class="card-text">
                                    <div class="title-text">${value.collegeName}</div>
                                    <div class="text-muted">${value.city},${_countryName}</div>
                                      <div class="text-muted">${value.startedMonth} ${value.startedYear} - ${_endMonth}</div>
                                </span>
                            </div>
                            <div class="col-md-4">
                            <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2 btn-outline-danger" id="btnDeleteOverseas" data-item='${value.overseasStudyId}' data-edit='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditOverseas" data-item='${value.overseasStudyId}' data-edit='${index}' class="btn custombtn customBtn-light w-auto ms-1 ">
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
        } else {
             html = ` 
                <div class="card col-md-12 p-0 mb-3 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-8">
                                <span class="card-text">
                                    <div class="title-text">${value.collegeName}</div>
                                    <div class="text-muted">${value.city},${value.countryName}</div>
                                    <div class="text-muted"> ${ getMonth(new Date(value.startedDate).getMonth()) + " " + new Date(value.startedDate).getFullYear()} - ${getMonth(new Date(value.endedDate).getMonth()) + " " + new Date(value.endedDate).getFullYear()}
</div>
                                </span>
                            </div>
                            <div class="col-md-4">
                            <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2 btn-outline-danger" id="btnDeleteOverseas" data-item='${value.overseasStudyId}' data-edit='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditOverseas" data-item='${value.overseasStudyId}' data-edit='${index}' class="btn custombtn customBtn-light w-auto ms-1 ">
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
        }
     
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