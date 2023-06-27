﻿var organizationArr = [];
var positionArray = []


$(document).ready(function () {
    LoadDropdowns();
    getFormData()
});

function getFormData() {
    $.ajax({
        url: '/organization/GetData',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                organizationArr.push(value.organization)
                for (var i = 0; i < value.orgPositions.length; i++) {
                    positionArray.push(value.orgPositions[i]);
                }
            });
            LoadCards()
        },
        error: function (err) {

        }
    })
}

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






$('#btnSaveOrganization').click(function () {
    let orginazation = {
        OrgExperienceId: $('#hdfOrgExperienceId').val(),
        OrganizationId: $('#hdfOrganizationId').val(),
        OrgName: $('#txtOrgName').val(),
        City: $('#txtCity').val(),
        StateAbbr: $('#ddlStateAbbr').val(),
        StartedMonth: $('#ddlStartedMonth').val(),
        StartedYear: $('#ddlStartedYear').val(),
        EndedMonth: $('#ddlEndedMonth').val(),
        EndedYear: $('#ddlEndedYear').val(),
    };
    if (localStorage.getItem("org-index") == null) {
        organizationArr.push(language);
    }
    else {
        organizationArr[parseInt(localStorage.getItem("org-index"))] = orginazation;
        localStorage.clear();
    }
    LoadCards();

});

$(document).on('click', '#btnAddPosition', function () {
    let position = {
        OrganizationId: $('#hdfOrganizationId').val(),
        OrgPositionId: $('#hdfOrgPositionId').val(),
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
    if (localStorage.getItem("pos-index") == null) {
        positionArray.push(language);
    }
    else {
        positionArray[parseInt(localStorage.getItem("pos-index"))] = position;
        localStorage.clear();
    }
    LoadCards();
});


$('#btnSaveAndContinue').click(function () {
    obj = {
        OrgExperienceId: $('#hdfOrgExperienceId').val(),
        OrgPositions: positionArray,
        Organizations: organizationArr,
        IsComplete: $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked
    };
    $.ajax({
        url: '/organization/PostOrganizationData',
        type: 'POST',
        data: { organizationViewModel: obj },
        success: function (response) {
            LoadCards();
            window.location.href = response.redirect;
        },
        error: function (error) {

        }
    });
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

function LoadCards() {
    $('#divEditSection div.row').html("")
    organizationArr = covertArrayKeyIntoCamelCase(organizationArr)
    $.each(organizationArr, function (index, value) {
        let html = ` <div class="col-md-10">
                                <span class="card-text">
                                    <p>${value.orgName}</p>
                                    <p class="text-muted">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear} </p>
                                    <p class="text-muted"> ${value.city}</p>
                                </span>
                                <div id="positionDiv">
                <div class="row"></div>
                        </div>
                         <button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" data-bs-toggle="modal"
                                    data-bs-target="#PositionModel">
                                Add an Position of ${value.orgName}
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
                                <button type="button" id="btnEditOrg" data-item='${value.organizationId}' org-index="${index}" class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                   
                                </button>

                            </div>
         `
        $('#divEditSection div.row').append(html)
    });

    $('#positionDiv div.row').html("");
    positionArray = covertArrayKeyIntoCamelCase(positionArray)
    $.each(positionArray, function (index, value) {
        let html = `<div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                        <div class="col-md-10">
                                <span class="card-text">
                                    <p>${value.title}</p>
                                    <p class="text-muted">${value.startedMonth} ${value.endedMonth} - ${value.endedMonth} ${value.endedYear}</p>
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
                                <button type="button" id="btnEditPosition" data-item='${value.orgPositionId}' pos-index='${index}' class="btn btn-outline-primary">
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
}


$(document).on('click', '#btnEditOrg', function () {
    var response = organizationArr[$(this).attr('org-index')];
    localStorage.setItem("org-index", $(this).attr('org-index'));
    $('#hdfOrgExperienceId').val(response.orgExperienceId)
    $('#hdfOrganizationId').val(response.organizationId)
    $('#txtOrgName').val(response.orgName)
    $('#txtCity').val(response.city)
    $('#ddlStateAbbr').val(response.stateAbbr)
    $('#ddlStartedMonth').val(response.startedMonth)
    $('#ddlStartedYear').val(response.startedYear)
    $('#ddlEndedMonth').val(response.endedMonth)
    $('#ddlEndedYear').val(response.endedYear);
    $('#SummaryModal').modal('show');
});

$(document).on('click', '#btnEditPosition', function () {
    var response = positionArray[$(this).attr('pos-index')];
    console.log(response)
        localStorage.setItem("pos-index", $(this).attr('pos-index'));
        $('#hdfOrganizationId').val(response.organizationId),
            $('#hdfOrgPositionId').val(response.orgPositionId),
        $('#txtTitle').val(response.title),
        $('#ddlPositionStartedMonth').val(response.startedMonth),
        $('#ddlPositionStartedYear').val(response.startedYear),
        $('#ddlPositionEndedMonth').val(response.endedMonth),
        $('#ddlPositionEndedYear').val(response.endedYear),
        $('#txtResponsibility1').val(response.responsibility1),
        $('#txtResponsibility2').val(response.responsibility2),
        $('#txtResponsibility3').val(response.responsibility3),
        $('#txtOtherInfo').val(response.otherInfo),
        $('#PositionModel').modal('show');
})