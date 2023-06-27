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

var organizationArr = [];
var positionArray = []

$('#cbPositionCurrentlyIn').click(function () {
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

$(document).ready(function () {
    LoadDropdowns();
});

function LoadDropdowns() {
    $('#ddlStateAbbr').html("");
    $('#ddlStateAbbr').append('<option value="0" selected><b>Select State</b></option>')
    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlStateAbbr').append(`<option value="${value.stateAbbr}"><b> ${value.stateName} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}


$('#btnSaveOrganization').click(function () {
    let orginazation = {
        VolunteerOrg1: $('#txtOrgName').val(),
        City: $('#txtCity').val(),
        StateAbbr: $('#ddlStateAbbr').val(),
        StartedMonth: $('#ddlStartedMonth').val(),
        StartedYear: $('#ddlStartedYear').val(),
        EndedMonth: $('#ddlEndedMonth').val(),
        EndedYear: $('#ddlEndedYear').val(),
    };
    organizationArr.push(orginazation);
    console.log(organizationArr)
    $.each(organizationArr, function (index, value) {

        let html = `
                <div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-10">
                                <span class="card-text">
                                    <p>${value.VolunteerOrg1}</p>
                                    <p class="text-muted">${value.StartedMonth} ${value.StartedYear} - ${value.EndedMonth} ${value.EndedYear} </p>
                                    <p class="text-muted"> ${value.City}</p>
                                </span>
                         <button type="button" class="btn btn-primary btn-sm custombtn w-auto" data-bs-toggle="modal"
                                    data-bs-target="#PositionModel">
                                Add an Position of ${value.VolunteerOrg1}
                            </button>
                            </div>
                            <div class="col-md-2">
                                <button type="button"  class="btn btn-outline-danger">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditCollege" data-item='' class="btn btn-outline-primary">
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
 <div id="positionDiv">
                <div class="row"></div>
            </div>
                </div>
 `
        $('#divEditSection div.row').append(html)
        //$('#divEditSection').show();
    });


});

$(document).on('click', '#btnAddPosition', function () {
    let position = {
        VolunteerPositionId: $('#hdfVolunteerExperienceId').val(),
        Title: $('#txtTitle').val(),
        StartedMonth: $('#ddlPositionStartedMonth').val(),
        StartedYear: $('#ddlPositionStartedYear').val(),
        EndedMonth: $('#ddlPositionEndedMonth').val(),
        EndedYear: $('#ddlPositionEndedYear').val(),
        Responsibility1: $('#txtResponsibility1').val(),
        Responsibility2: $('#txtResponsibility2').val(),
        Responsibility3: $('#txtResponsibility3').val(),
        OtherInfo: $('#txtOtherInfo').val(),
    };
    positionArray.push(position);
    let html = `<div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                        <div class="col-md-10">
                                <span class="card-text">
                                    <p>${$("#txtTitle").val()}</p>
                                    <p class="text-muted">${$('#ddlPositionStartedMonth').val()} ${$('#ddlPositionEndedYear').val()} - ${$('#ddlPositionEndedMonth').val()} ${$('#ddlPositionEndedYear').val()}</p>
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
                                </button>
                                <button type="button" id="btnEditCollege" data-item='' class="btn btn-outline-primary">
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
                </div>`
    $('#positionDiv div.row').append(html)
});

$('#btnSaveAndContinue').click(function () {
    obj = {
        VolunteerPositions: positionArray,
        VolunteerOrgs: organizationArr,
        IsComplete: $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked,
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val()
    };
    $.ajax({
        url: '/communityservice/PostCommunityService',
        type: 'POST',
        data: { communityViewModel: obj },
        success: function (response) { },
        error: function (error) {

        }
    });
});