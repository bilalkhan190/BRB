﻿var positionArray = [];

$(document).ready(function () {
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

    $.ajax({
        url: '/Military/GetMilataryExperience',
        type: 'Get',
        success: function (response) {
            loadData(response)
        },
        error: function (error) {
            console.log(error)
        }
    })

});

$('#cbCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#ddlEndedMonth').hide();
        $('#ddlEndedYear').hide();
        $('#labelEndedDate').hide();
    } else {
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
    }
});


$(document).on('click', '#cbPositionCurrentlyIn', function () {
    $('#mainForm').validate();
    if ($(this).is(':checked')) {
        $('#ddlPositionEndedMonth').hide();
        $('#ddlPositionEndedYear').hide();
        $('#LabelEndedPositionDate').hide();
    } else {
        $('#ddlPositionEndedMonth').show();
        $('#ddlPositionEndedYear').show();
        $('#LabelEndedPositionDate').show();
    }
});

$(document).on('change', '#cbSectionNotApply', function () {
    if (this.checked) $('#mainForm').hide()
    else $('#mainForm').show()
});

$(document).on('click', '#btnAddOrContinue', function () {
    //let isCompleted = $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked;

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
        MilitaryPositions: positionArray
    };
    if ($('#mainForm').valid()) {
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

    $('#divEditSection div.row').html('');


    positionArray = positionArray.map(el => _.mapKeys(el, (val, key) => _.camelCase(key)));
    $.each(positionArray, function (index, value) {
        let html = ` <div class="col-md-10">
                                <span class="card-text">
                                    <p>${value.title}</p>
                                    <p class="text-muted"  id="messageCurrentlyNotIn">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear} </p>
                                    <p class="text-muted" id="messageCurrentlyIn">${value.startedMonth} ${value.startedYear} - Current </p>
                                    <p class="text-muted">Training Completed: ${value.mainTraining}</p>
                                </span>
                            </div>
                            <div class="col-md-2">
                                <button type="button"  class="btn btn-outline-danger">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditCollege" data-item='${value.militaryPositionId}' data-edit=${index} class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                            </div>`
        $('#divEditSection div.row').append(html)
        if (value.endedMonth == null && value.endedYear == null) {
            $('#messageCurrentlyNotIn').hide();
        } else {
            $('#messageCurrentlyIn').hide();
        }

    });
    $('#divEditSection').show();
});

function loadData(response) {
    $('#hdfMilitaryExperienceId').val(response.data.militaryExperienceId);
    $('#txtCity').val(response.data.city);
    $('#txtBranch').val(response.data.branch);
    $('#ddlCountry').val(response.data.countryId);
    $('#txtStartedMonth').val(response.data.startedMonth);
    $('#txtStartedYear').val(response.data.startedYear);
    $('#ddlEndedMonth').val(response.data.endedMonth);
    $('#ddlEndedYear').val(response.data.endedYear);
    $('#txtRank').val(response.data.rank);
    if (response.data.isComplete) {
        $('#cbIsComplete').prop('checked', true);
    }
    if (response.data.endedMonth == null && response.data.endedYear == null) {
        //$('#cbCurrentlyIn').prop('checked', true);
        $('#ddlEndedMonth').hide();
        $('#ddlEndedYear').hide();
        $('#labelEndedDate').hide();
    }
    $('#divEditSection div.row').html("")
    if (response.data.militaryPositions.length > 0) {
       
        $.each(response.data.militaryPositions, function (index, value) {
            positionArray.push(response.data.militaryPositions[index])

            let html = ` <div class="col-md-10">
                                <span class="card-text">
                                    <p>${value.title}</p>
                                    <p class="text-muted"  id="messageCurrentlyNotIn">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear} </p>
                                    <p class="text-muted" id="messageCurrentlyIn">${value.startedMonth} ${value.startedYear} - Current </p>
                                    <p class="text-muted">Training Completed: ${value.mainTraining}</p>
                                </span>
                            </div>
                            <div class="col-md-2">
                                <button type="button"  class="btn btn-outline-danger">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditCollege" data-item='${value.militaryPositionId}' data-edit=${index} class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                            </div>`
            $('#divEditSection div.row').append(html)
            if (value.endedMonth == null && value.endedYear == null) {
                $('#messageCurrentlyNotIn').hide();
            } else {
                $('#messageCurrentlyIn').hide();
            }

        });
    }
}

$(document).on('click', '#btnEditCollege', function () {
    //let id = $('#btnEditCollege').attr('data-item')
    //$.ajax({
    //    url: '/Military/GetPositionById?id=' + id,
    //    type: 'get',
    //    success: function (response) {
    //        $('#hdfMilitaryPositionId').val(response.data.militaryPositionId)
    //        $('#txtTitle').val(response.data.title)
    //        $('#ddlPositionStartedMonth').val(response.data.startedMonth)
    //        $('#ddlPositionStartedYear').val(response.data.startedYear)
    //        if (response.data.endedMonth == null && response.data.endedYear == null) {
    //            $('#cbPositionCurrentlyIn').prop('checked', true);
    //            $('#ddlPositionEndedMonth').hide();
    //            $('#ddlPositionEndedYear').hide();
    //            $('#LabelEndedPositionDate').hide();
    //        }
    //        $('#ddlPositionEndedMonth').val(response.data.endedMonth)
    //        $('#ddlPositionEndedYear').val(response.data.endedYear)
    //        $('#txtMainTraining').val(response.data.mainTraining)
    //        $('#txtResponsibility1').val(response.data.responsibility1)
    //        $('#txtResponsibility2').val(response.data.responsibility2)
    //        $('#txtResponsibility3').val(response.data.responsibility3)
    //        $('#txtOtherInfo').val(response.data.otherInfo)
    //        $('#SummaryModal').modal('show')
    //    },
    //    error: function (error) {

    //    }
    //})

    var response = positionArray[$(this).attr('data-edit')];
    localStorage.setItem("pos-index", $(this).attr('data-edit'))
    $('#hdfMilitaryPositionId').val(response.militaryPositionId)
            $('#txtTitle').val(response.title)
            $('#ddlPositionStartedMonth').val(response.startedMonth)
            $('#ddlPositionStartedYear').val(response.startedYear)
            if (response.endedMonth == null && response.endedYear == null) {
                $('#cbPositionCurrentlyIn').prop('checked', true);
                $('#ddlPositionEndedMonth').hide();
                $('#ddlPositionEndedYear').hide();
                $('#LabelEndedPositionDate').hide();
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
