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
    getFormData();
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
    $('#orgForm').validate()
    if ($('#orgForm').valid()) {
        let orginazation = {
            VolunteerOrgId: $('#hdfVolunteerOrgId').val(),
            VolunteerOrg1: $('#txtOrgName').val(),
            City: $('#txtCity').val(),
            StateAbbr: $('#ddlStateAbbr').val(),
            StartedMonth: $('#ddlStartedMonth').val(),
            StartedYear: $('#ddlStartedYear').val(),
            EndedMonth: $('#ddlEndedMonth').val(),
            EndedYear: $('#ddlEndedYear').val(),
        };
        $('#btnSaveOrganization').prop('disabled',true)
        if (localStorage.getItem("org-index") == null) {
            organizationArr.push(orginazation);
        }
        else {
            organizationArr[parseInt(localStorage.getItem("org-index"))] = orginazation;
            localStorage.clear();
        }
        $('#orgForm').trigger('reset');
        LoadCards();

    }

});

$('.closeModal').click(function () {
    $('#orgForm').trigger('reset')
    $('#btnSaveOrganization').prop('disabled', false)
})

$('.modelPosition').click(function () {
    $('#orgPositionForm').trigger('reset');
    $('#btnAddPosition').prop('disabled', false)
});

function getFormData() {
    $.ajax({
        url: '/CommunityService/GetData',
        type: 'get',
        success: function (response) {
            if (response.data.length > 0) {
                console.log(response.data)
                $('#hdfVolunteerExperienceId').val(response.data[0].volunteerExperience.volunteerExperienceId);
                $('#cbSectionNotApply').prop("checked", response.data[0].volunteerExperience.isOptOut).trigger('change');
                $('#cbIsComplete').prop("checked", response.data[0].volunteerExperience.isComplete);
                if (response.data.length > 0) {
                    $.each(response.data, function (index, value) {
                        organizationArr.push(response.data[index].volunteerOrg)
                        if (response.data[index].volunteerPositions.length > 0) {
                            for (var i = 0; i < response.data[index].volunteerPositions.length; i++) {
                                positionArray.push(response.data[index].volunteerPositions[i]);
                            }
                        }

                    });
                }
                if (organizationArr.length > 0) {
                    $('#hdfVolunteerExperienceId').val(organizationArr[0].volunteerExperienceId)
                } else {
                    $('#hdfVolunteerExperienceId').val(response.data.volunteerExperienceId)
                }
                LoadCards()
            }
          
        },
        error: function (err) {

        }
    })
}

$(document).on('click', '#btnAddPosition', function () {
    $('#orgPositionForm').validate();
    if ($('#orgPositionForm').valid()) {
        let position = {
            VolunteerPositionId: $('#hdfVolunteerPositionId').val(),
            VolunteerOrgId: $('#hdfVolunteerOrgId').val(),
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
        $('#btnAddPosition').prop('disabled',true)
        if (localStorage.getItem("pos-index") == null) {
            positionArray.push(position);
        }
        else {
            positionArray[parseInt(localStorage.getItem("pos-index"))] = position;
            localStorage.clear();
        }
        LoadCards();
    }
});

$('#btnSaveAndContinue').click(function () {
    obj = {
        VolunteerExperienceId: $('#hdfVolunteerExperienceId').val(),
        VolunteerPositions: positionArray,
        VolunteerOrgs: organizationArr,
        IsComplete: $('#cbIsComplete').is(":checked"),
        IsOptOut: $('#cbSectionNotApply').is(":checked"),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val()
    };
    $.ajax({
        url: '/communityservice/PostCommunityService',
        type: 'POST',
        data: { communityViewModel: obj },
        success: function (response) {
            window.location.href = response.redirect;
        },
        error: function (error) {

        }
    });
});


$('#cbSectionNotApply').change(function () {
    if (this.checked) {
        $('#isOptSection').hide();
    }
    else {
        $('#isOptSection').show();
    }
});
function LoadCards() {
    $('#divEditSection div.row').html("");
    organizationArr = covertArrayKeyIntoCamelCase(organizationArr)
    $.each(organizationArr, function (index, value) {
        let html = `
                <div class="card ml-4 mt-3 p-0"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-12">
                             <span class="card-text">
                             <div class="row">
                             <div class="col-md-6">
                                    <h5 class="title-text">${value.volunteerOrg1}</h5>
                                    <p class="text-muted">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear} </p>
                                    <p class="text-muted"> ${value.city}</p>
                            </div>
                            <div class="col-md-6">
                                <div class="card-Btn">
                                    <button type="button" id="btnDeleteOrg" data-item='${value.volunteerOrgId}' org-index=${index}   class="btn custombtn w-auto ms-2">
                                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                             viewBox="0 0 24 24" height="1em" width="1em"
                                             xmlns="http://www.w3.org/2000/svg">
                                            <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                            </path>
                                        </svg>
                                    </button>
                                    <button type="button" id="btnEditOrg" data-item='${value.volunteerOrgId}' org-index=${index} class="btn custombtn customBtn-light w-auto ms-1">
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
                        </span>
                           
                                <div id="positionDiv">
                                     </div>  
                         <button type="button" class="btn btn-primary btn-sm custombtn w-auto" data-bs-toggle="modal"
                                    data-bs-target="#PositionModel">
                                Add an Position of ${value.volunteerOrg1}
                            </button>
                            </div>
                         </div>
                    </div>

                </div>
 `
        $('#divEditSection div.row').append(html)
    });
    $('').html("");
    positionArray = covertArrayKeyIntoCamelCase(positionArray);
    console.log(positionArray)
    $.each(positionArray, function (index, value) {
        let html = `<div class="card col-md-12 p-0 mb-3 cardWrapper mt-3"> 
                    <div class="card-body">
                       <div class="row mx-auto">
                        <div class="col-md-12 p-0">
                                <span class="card-text">
                                <div class="row">
                                <div class="col-md-6">
                                <h5 class="title-text">${value.title}</h5>
                                    <p class="text-muted">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear}</p>
                                </div>
                                  <div class="col-md-6">
                                    <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2" id="btnDeletePosition" data-item='${value.volunteerOrgId}' pos-index='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditPosition" data-item='${value.volunteerOrgId}' pos-index='${index}' class="btn custombtn customBtn-light w-auto ms-1">
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
                                    
                                </span>
                            </div>
                    </div>
                </div>`
        //let html = `<div class="card ml-4 "> 
        //            <div class="card-body">
        //               <div class="row">
        //                <div class="col-md-10">
        //                        <span class="card-text">
        //                            <h5 class="title-text">${value.title}</h5>
        //                            <p class="text-muted">${value.startedMonth} ${value.startedYear} - ${value.endedMonth} ${value.endedYear}</p>
        //                        </span>
        //                    </div>
        //                    <div class="col-md-2">
        //                    <div class="card-Btn">
        //                        <button type="button" id="btnDeletePosition" data-item='${value.volunteerPositionId}' pos-index=${index} class="btn custombtn w-auto ms-2">
        //                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
        //                                 viewBox="0 0 24 24" height="1em" width="1em"
        //                                 xmlns="http://www.w3.org/2000/svg">
        //                                <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
        //                                </path>
        //                            </svg>
        //                        </button>
        //                        <button type="button" id="btnEditPosition" data-item='${value.volunteerPositionId}' pos-index=${index} class="btn custombtn customBtn-light w-auto ms-1">
        //                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
        //                                 viewBox="0 0 24 24" height="1em" width="1em"
        //                                 xmlns="http://www.w3.org/2000/svg">
        //                                <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
        //                                </path>
        //                            </svg>
                                   
        //                        </button>
        //                        </div>
        //                    </div>
        //                </div>
        //            </div>
        //        </div>`
        $('#positionDiv').append(html)
    });
}

$(document).on('click', '#btnEditOrg', function () {
    var response = organizationArr[$(this).attr('org-index')];
    localStorage.setItem("org-index", $(this).attr('org-index'));
        $('#hdfVolunteerOrgId').val(response.volunteerOrgId),
        $('#txtOrgName').val(response.volunteerOrg1),
        $('#txtCity').val(response.city),
        $('#ddlStateAbbr').val(response.stateAbbr),
        $('#ddlStartedMonth').val(response.startedMonth),
        $('#ddlStartedYear').val(response.startedYear),
        $('#ddlEndedMonth').val(response.endedMonth),
        $('#ddlEndedYear').val(response.endedYear),
        $('#SummaryModal').modal('show');
});

$(document).on('click', '#btnEditPosition', function () {
        var response = positionArray[$(this).attr('pos-index')];
        localStorage.setItem("pos-index", $(this).attr('pos-index'));
        $('#hdfVolunteerPositionId').val(response.volunteerPositionId),
        $('#txtTitle').val(response.title),
        $('#ddlPositionStartedMonth').val(response.startedMonth),
        $('#ddlPositionStartedYear').val(response.startedYear),
        $('#ddlPositionEndedMonth').val(response.endedMonth),
        $('#ddlPositionEndedYear').val(response.endedYear),
        $('#txtResponsibility1').val(response.responsibility1),
        $('#txtResponsibility2').val(response.responsibility2),
        $('#txtResponsibility3').val(response.responsibility3),
        $('#txtOtherInfo').val(response.otherInfo)
        $('#PositionModel').modal('show')
});



$(document).on('click', '#btnDeleteOrg', function () {

    localStorage.setItem("org-index", $(this).attr('org-index'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/CommunityService/delete?id=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("org-index"));
            organizationArr.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    })
});

$(document).on('click', '#btnDeletePosition', function () {

    localStorage.setItem("pos-index", $(this).attr('pos-index'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/CommunityService/delete?positionId=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("pos-index"));
            positionArray.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    });
});