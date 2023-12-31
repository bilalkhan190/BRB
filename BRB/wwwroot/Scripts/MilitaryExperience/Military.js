﻿var positionArray = [];

$(document).ready(function () {

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
        url: '/Military/GetMilataryExperience',
        type: 'Get',
        success: function (response) {
            if (response.data != null) {
                console.log(response.data)
                $('#hdfMilitaryExperienceId').val(response.data.militaryExperienceId);
                //$('.isoptOut').prop("checked", response.data.isOptOut).trigger('change');
                $('#cbSectionNotApply').prop("checked", response.data.isOptOut).trigger('change');
                //$('#cbCurrentlyIn').prop("checked", response.data.currentlyIn).trigger('change');
                $('#cbIsComplete').prop("disabled", response.data.isComplete);
                loadData(response)


            }

        },
        error: function (error) {
            console.log(error)
        }
    })


});

$('#cbCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').val('');
        $('#ddlEndedMonth').hide();
        $('#ddlEndedYear').hide();
        $('#labelEndedDate').hide();
        $('#pnlMilatExp').hide();

    } else {
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
        $('#pnlMilatExp').show();
    }
});


$(document).on('click', '#cbPositionCurrentlyIn', function () {
    $('#mainForm').validate();
    if ($(this).is(':checked')) {
        
        $('#ddlPositionEndedMonth').val('');
        $('#ddlPositionEndedYear').val('');
        $('#pnlEndDate').hide();
    } else {
        
        $('#pnlEndDate').show();
    }
});

$(document).on('change', '#cbSectionNotApply', function () {
    if (this.checked) $('#isOptSection').hide()
    else $('#isOptSection').show()
});

$(document).on('click', '#btnAddOrContinue', function () {
    //let isCompleted = $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked;
    debugger
    let masterData = {
        MilitaryExperienceId: $('#hdfMilitaryExperienceId').val(),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        Branch: $('#txtBranch').val(),
        City: $('#txtCity').val(),
        CountryId: $('#ddlCountry').val(),
        StartedMonth: $('#txtStartedMonth').val(),
        StartedYear: $('#txtStartedYear').val(),
        EndedMonth: $('#ddlEndedMonth').val(),
        EndedYear: $('#ddlEndedYear').val(),
        Rank: $('#txtRank').val(),
        IsComplete: $('#cbIsComplete').is(':checked'),
        IsOptOut: $('#cbSectionNotApply').is(':checked'),
        MilitaryPositions: positionArray
    };
    if (!$('#cbSectionNotApply').prop('checked')) {
        if ($('#mainForm').valid()) {

            var sMonth = $("#txtStartedMonth").val();
            var sYear = $("#txtStartedYear").val();
            var eMonth = $("#ddlEndedMonth").val();
            var eYear = $("#ddlEndedYear").val();

            if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
                swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
                return false;
            }

            if (!masterData.IsOptOut) {
                if (!positionArray.length > 0) {
                    swal("Positions Requried", "Please add a military position to proceed", "error");
                    return false;
                }
            }
         
            $.ajax({
                url: '/Military/PostMilitaryData',
                type: 'POST',
                dataType: 'json',
                data: { militaryViewModel: masterData, },
                success: function (response) {
                    positionArray = [];
                    loadData(response)
                    window.location.href = response.redirect;
                },
                error: function (error) {
                    console.log(error)
                }
            });
        }

    } else {
        $.ajax({
            url: '/Military/PostMilitaryData',
            type: 'POST',
            dataType: 'json',
            data: { militaryViewModel: masterData, },
            success: function (response) {
                positionArray = [];
                loadData(response)
                window.location.href = response.redirect;
            },
            error: function (error) {
                console.log(error)
            }
        });
    }

});

$('#btnAddPosition').click(function () {
    $('#militaryPositionForm').validate();
    if ($('#militaryPositionForm').valid()) {
        var sMonth = $("#ddlPositionStartedMonth").val();
        var sYear = $("#ddlPositionStartedYear").val();
        var eMonth = $("#ddlPositionEndedMonth").val();
        var eYear = $("#ddlPositionEndedYear").val();

        if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
            swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
            return false;
        }
        $('#btnAddPosition').prop('disabled', true);
        let position = {
            MilitaryPositionId: $('#hdfMilitaryPositionId').val(),
            Title: $('#txtTitle').val(),
            StartedMonth: $('#ddlPositionStartedMonth').val(),
            StartedYear: $('#ddlPositionStartedYear').val(),
            EndedMonth: $('#ddlPositionEndedMonth').val(),
            EndedYear: $('#ddlPositionEndedYear').val(),
            MainTraining: $('#txtMainTraining').val(),
            Responsibility1: $('#txtResponsibility1').val(),
            Responsibility2: $('#txtResponsibility2').val(),
            Responsibility3: $('#txtResponsibility3').val(),
            OtherInfo: $('#txtOtherInfo').val(),
        };
        if (localStorage.getItem("pos-index") == null) {
            positionArray.push(position);
        }
        else {
            positionArray[parseInt(localStorage.getItem("pos-index"))] = position;
            localStorage.clear();
        }

        //fill the array of position record and display the recorded data in div
        $('#militaryPositionForm').trigger('reset');
        $('#SummaryModal').modal('toggle')
        $('#btnAddPosition').prop('disabled', false);
        $('#divEditSection div.row').html('');


        positionArray = positionArray.map(el => _.mapKeys(el, (val, key) => _.camelCase(key)));
        //$.each(positionArray, function (index, value) {
        //    let endDate = '';
        //    if (!(value.endedMonth && value.endedYear)) {
        //        endDate = "Present";
        //    }
        //    else {
        //        endDate = value.endedMonth + " " + value.endedYear;
        //    }
        //    let html = ` <div class="col-md-12 positionInnerBox">
        //                        <span class="card-text row pt-3">
        //                        <div class="col-md-8">
        //                         <p class="text-muted"> ${value.title}</p>
        //                            <p class="text-muted" >${value.startedMonth} ${value.startedYear} - ${endDate} </p>
        //                            <p class="text-muted">Training Completed: ${value.mainTraining}</p>
        //                            </div>
        //                            <div class="col-md-4">
        //                            <div class="card-Btn">
        //                       <button type="button" id="btnDelete"  data-item='${value.militaryPositionId}' data-edit=${index} class="btn btn-primary btn-sm custombtn w-auto">
        //                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
        //                                 viewBox="0 0 24 24" height="1em" width="1em"
        //                                 xmlns="http://www.w3.org/2000/svg">
        //                                <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
        //                                </path>
        //                            </svg>
        //                        </button><button type="button" id="btnEditMilitary" data-item='${value.militaryPositionId}' data-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
        //                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
        //                                 viewBox="0 0 24 24" height="1em" width="1em"
        //                                 xmlns="http://www.w3.org/2000/svg">
        //                                <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
        //                                </path>
        //                            </svg>
        //                        </button>
        //                        </div>
        //                        </div>
                                   
        //                        </span>
        //                    </div>`
        //    $('#divEditSection div.row').append(html)
        //    if (value.endedMonth == null || value.endedMonth == "" && value.endedYear == null || value.endedYear == "") {
        //        $('#messageCurrentlyNotIn').hide();
        //    } else {
        //        $('#messageCurrentlyIn').hide();
        //    }

        //});
        //if (positionArray != null && positionArray.length > 3) {
        //    $("#divEditSection").addClass("BoxHeight");
        //}
        LoadCards();
    }

});

$('.closeModal').click(function () {
    $('#militaryPositionForm').trigger('reset')
    $('#btnAddPosition').prop('disabled', false);
})
function loadData(response) {
    //if (response.data.militaryPositions.length > 0) {
    //   $()
    //}
    //else {
    //    let msg = `  <p class="danger-text" id="noList">
    //                    <i>
    //                        You currently have no Military Positions listed. Click the Add button below to
    //                        add one.
    //                    </i>
    //                </p>`;
    //    $('.btnAddPos').before(msg);
    //}
    if (response.data != null) {
        $('#hdfMilitaryExperienceId').val(response.data.militaryExperienceId);
        $('#txtCity').val(response.data.city);
        $('#txtBranch').val(response.data.branch);
        $('#ddlCountry').val(response.data.countryId);
        $('#txtStartedMonth').val(response.data.startedMonth);
        $('#txtStartedYear').val(response.data.startedYear);
        $('#ddlEndedMonth').val(response.data.endedMonth);
        $('#ddlEndedYear').val(response.data.endedYear);
        $('#txtRank').val(response.data.rank);
        $('#cbIsComplete').prop('checked', response.data.isComplete);
        $('#cbSectionNotApply').prop("checked", response.data.isOptOut)
        if (response.data.endedMonth == null && response.data.endedYear == null) {
            $('#cbCurrentlyIn').prop('checked', true);
            $('#ddlEndedMonth').hide();
            $('#ddlEndedYear').hide();
            $('#labelEndedDate').hide();
        }
        $('#divEditSection div.row').html("")
        if (response.data.militaryPositions.length > 0) {
            $.each(response.data.militaryPositions, function (index, value) {
                positionArray.push(response.data.militaryPositions[index])
            });
        }
        LoadCards();
    }

}



function LoadCards() {
    $('#divEditSection div.row').html("")
    $.each(positionArray, function (index, value) {
        $('#noList').hide();
        let endDate = '';
        if (!(value.endedMonth && value.endedYear)) {
            endDate = 'Present';
        }
        else {
            endDate = value.endedMonth + " " + value.endedYear;
        }
        let html = ` <div class="col-md-12 positionInnerBox">
                                <span class="card-text row pt-3">
                                <div class="col-md-8">
                                 <p class="text-muted"> ${value.title}</p>
                                    <p class="text-muted">${value.startedMonth} ${value.startedYear} - ${endDate} </p>
                                    
                                    <p class="text-muted">Training Completed: ${value.mainTraining}</p>
                                    </div>
                                    <div class="col-md-4">
                                    <div class="card-Btn">
                               <button type="button" id="btnDelete"  data-item='${value.militaryPositionId}' data-edit=${index} class="btn btn-primary btn-sm custombtn w-auto btn-outline-danger">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditMilitary" data-item='${value.militaryPositionId}' data-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                </div>
                                </div>
                            </div>`
        $('#divEditSection div.row').append(html)
        if (value.endedMonth == null && value.endedYear == null) {
            $('#messageCurrentlyNotIn').hide();
        } else {
            $('#messageCurrentlyIn').hide();
        }
    })

}
$(document).on('click', '#btnEditMilitary', function () {
    var response = positionArray[$(this).attr('data-edit')];
    localStorage.setItem("pos-index", $(this).attr('data-edit'))
    $('#hdfMilitaryPositionId').val(response.militaryPositionId)
    $('#txtTitle').val(response.title)
    $('#ddlPositionStartedMonth').val(response.startedMonth)
    $('#ddlPositionStartedYear').val(response.startedYear)
    if (response.endedMonth == null || response.endedMonth == "" && response.endedYear == null || response.endedYear == "" ) {
        $('#cbPositionCurrentlyIn').prop('checked', true);
        $('#pnlEndDate').hide();
    } else {
        $('#pnlEndDate').show();
    }
    $('#ddlPositionEndedMonth').val(response.endedMonth)
    $('#ddlPositionEndedYear').val(response.endedYear)
    $('#txtMainTraining').val(response.mainTraining)
    $('#txtResponsibility1').val(response.responsibility1)
    $('#txtResponsibility2').val(response.responsibility2)
    $('#txtResponsibility3').val(response.responsibility3)
    $('#txtOtherInfo').val(response.otherInfo)
    $('#SummaryModal').modal('show')

});

function clearPosition() {
    $("#militaryPositionForm").trigger("reset");
    localStorage.clear();
    $('#pnlEndDate').show();
    $('#cbPositionCurrentlyIn').prop('checked', false);     
    $('#SummaryModal').modal('toggle');
}

$(document).on('click', '#btnDelete', function () {
      
    localStorage.setItem("mil-index", $(this).attr('data-edit'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/Military/delete?id=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("mil-index"));
            positionArray.splice(index, 1);
            if (positionArray.length == 0) $('#noList').show();
            LoadCards();
        },
        error: function (err) {

        }
    });
});
