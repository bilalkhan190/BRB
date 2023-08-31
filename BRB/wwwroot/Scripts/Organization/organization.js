var organizationArr = [];
var positionArray = []


$(document).ready(function () {
    LoadDropdowns();
    getFormData();
   

});

function getFormData() {
    $.ajax({
        url: '/organization/GetData',
        type: 'get',
        success: function (response) {
            console.log(response)
            $('#hdfOrgExperienceId').val(response.data[0].orgExperience.orgExperienceId);
            $('#cbSectionNotApply').prop("checked", response.data[0].orgExperience.isOptOut).trigger('change');
            $('#cbIsComplete').prop("checked", response.data[0].orgExperience.isComplete);
            $('#cbIsComplete').prop("disabled", response.data[0].orgExperience.isComplete);
            console.log(response.data)
            if (response.data != null) {
                if (response.data.length > 0) {
                    $.each(response.data, function (index, value) {
                        if (value.organization != null) {
                            organizationArr.push(value.organization)
                        }
                        if (value.orgPositions.length > 0) {
                            for (var i = 0; i < value.orgPositions.length; i++) {
                                positionArray.push(value.orgPositions[i]);
                            }
                        }

                    });
                }
                LoadCards()
            }

        },
        error: function (err) {

        }
    })
}

$('#cbSectionNotApply').change(function () {
    if (this.checked) {
        $('#isOptSection').hide();
    }
    else {
        $('#isOptSection').show();
    }
});

$('#cbCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').val('');
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
    if ($("#cbPositionCurrentlyIn").is(':checked')) {
        $('#ddlPositionEndedMonth').val('');
        $('#ddlPositionEndedYear').val('');
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
    $('#orgForm').validate()
    if ($('#orgForm').valid()) {
        $("#noList").hide();
        var sMonth = $("#ddlStartedMonth").val();
        var sYear = $("#ddlStartedYear").val();
        var eMonth = $("#ddlEndedMonth").val();
        var eYear = $("#ddlEndedYear").val();

        if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
            swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
            return false;
        }
        let organization = {
            OrgExperienceId: $('#hdfOrgExperienceId').val(),
            OrganizationId: $('#hdfOrganizationId').val(),
            OrgName: $('#txtOrgName').val(),
            City: $('#txtCity').val(),
            StateAbbr: $('#ddlStateAbbr').val(),
            StartedMonth: $('#ddlStartedMonth').val(),
            StartedYear: $('#ddlStartedYear').val(),
            EndedMonth: $('#ddlEndedMonth').val(),
            orgPositions: [],
            EndedYear: $('#ddlEndedYear').val(),
        };
        $('#btnSaveOrganization').prop('disabled', true)
        if (localStorage.getItem("org-index") == null) {
            organizationArr.push(organization);
        }
        else {
            let positions = organizationArr[parseInt(localStorage.getItem("org-index"))] ? organizationArr[parseInt(localStorage.getItem("org-index"))].orgPositions : [];
            organization.orgPositions = positions;
            organizationArr[parseInt(localStorage.getItem("org-index"))] = organization;
            localStorage.clear();
        }
        $('#orgForm').trigger('reset');
        $('#SummaryModal').modal('toggle')
        $('#btnSaveOrganization').prop('disabled', false)
        LoadCards();
        console.log(organizationArr);
    }


});

$('.closeModal').click(function () {
    $('#orgForm').trigger('reset')
    $('#btnSaveOrganization').prop('disabled', false)
})
$('.modelPosition').click(function () {
    $('#orgPositionForm').trigger('reset');
    $('#btnAddPosition').prop('disabled', false)
})


$(document).on('click', '#btnAddPosition', function () {
    $('#orgPositionForm').validate();
    if ($('#orgPositionForm').valid()) {
        var sMonth = $("#ddlPositionStartedMonth").val();
        var sYear = $("#ddlPositionStartedYear").val();
        var eMonth = $("#ddlPositionEndedMonth").val();
        var eYear = $("#ddlPositionEndedYear").val();

        if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
            swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
            return false;
        }
        $('#btnAddPosition').prop('disabled', true)
        let position = {
            organizationId: $('#hdfOrganizationId').val(),
            orgPositionId: $('#hdfOrgPositionId').val(),
            title: $('#txtTitle').val(),
            startedMonth: $('#ddlPositionStartedMonth').val(),
            startedYear: $('#ddlPositionStartedYear').val(),
            endedMonth: $('#ddlPositionEndedMonth').val(),
            endedYear: $('#ddlPositionEndedYear').val(),
            responsibility1: $('#txtResponsibility1').val(),
            responsibility2: $('#txtResponsibility2').val(),
            responsibility3: $('#txtResponsibility3').val(),
            otherInfo: $('#txtOtherInfo').val(),
        };
        debugger;
        if (localStorage.getItem("pos-index") == null) {
            organizationArr.filter(x => x.organizationId == position.organizationId)[0].orgPositions.push(position);
            positionArray.push(position);
        }
        else {
            organizationArr.filter(x => x.organizationId == position.organizationId)[0].orgPositions[localStorage.getItem("pos-index")] = position;
            positionArray[parseInt(localStorage.getItem("pos-index"))] = position;
            localStorage.clear();
        }
        $('#orgPositionForm').trigger('reset');
       
        LoadCards();
        $('#PositionModel').modal('toggle')
        $('#btnAddPosition').prop('disabled', false)

    }

});


$('#btnSaveAndContinue').click(function () {
    debugger;
    if (!(organizationArr.length > 0 && positionArray.length > 0)) {

        $("#cbIsComplete").prop("checked", false);
    }
    //organizationArr.map((item, index) => {
    //    let res = positionArray.filter((x) => x.organizationId == item.organizationId)
    //    if (res) {
    //        item.OrgPositions = res
    //    }

    //})

    let obj = {
        OrgExperienceId: $('#hdfOrgExperienceId').val(),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        //OrgPositions: positionArray,
        Organizations: organizationArr,
        IsComplete: $('#cbIsComplete').is(':checked'),
        IsOptOut: $('#cbSectionNotApply').is(':checked')
    };

    if (!obj.IsOptOut) {
        let positionsLength = organizationArr.filter(x => x.orgPositions.length > 0) ? organizationArr.filter(x => x.orgPositions.length > 0).length : 0;
        if (organizationArr.length == 0 || positionsLength != organizationArr.length) {
            $("#cbIsComplete").prop("checked", false);          
        }
    }

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
    $('#divEditSection').html("")
    organizationArr = covertArrayKeyIntoCamelCase(organizationArr)
    console.log(organizationArr)
    let pos_index = 0;
    $.each(organizationArr, function (index, value) {
        $("#noList").hide();
        let endMonth = "";
        if (value.endedMonth && value.endedYear) { endMonth = value.endedMonth + " " + value.endedYear; } else { endMonth = "Present"; }
        let htmlPosition = ''
        positionArray = covertArrayKeyIntoCamelCase(positionArray)
        debugger
        let posArr = positionArray.filter(x => x.organizationId == value.organizationId);
        
        $.each(value.orgPositions, function (_index, _value) {
            let _endMonth = "";
            if (_value.endedMonth && _value.endedYear) { _endMonth = _value.endedMonth + " " + _value.endedYear; } else { _endMonth = "Present"; }
            htmlPosition += `<div class="card col-md-12 p-0 mb-3 cardWrapper mt-3"> 
                    <div class="card-body">
                       <div class="row mx-auto">
                        <div class="col-md-12 p-0">
                                <span class="card-text">
                                <div class="row">
                                <div class="col-md-6">
                                <h5 class="title-text">${_value.title}</h5>
                                    <p class="text-muted">${_value.startedMonth} ${_value.startedYear} - ${_endMonth}</p>
                                </div>
                                  <div class="col-md-6">
                                    <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2 btn-outline-danger" id="btnDeletePosition" data-org-id="${value.organizationId}" data-item='${_value.orgPositionId}' pos-index='${_index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" data-item='${_value.orgPositionId}' data-json='${JSON.stringify(_value)}' pos-index='${_index}' class="btnEditPosition btn custombtn customBtn-light w-auto ms-1">
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
                </div> </div>`;
            pos_index++;

            //$('#positionDiv').append(html)
        });




        let html = `<div class="card col-md-12 p-0 mb-3 cardWrapper mt-3">
    <div class="card-body">
        <div class="row mx-auto">
            <div class="col-md-12 p-0">
                <span class="card-text">
                    <div class="row">
                        <div class="col-md-6">
                            <h5 class="title-text">${value.orgName}</h5>
                            <p class="text-muted">
                                ${value.startedMonth} ${value.startedYear} - ${endMonth}
                            </p>
                            <p class="text-muted">${value.city}</p>
                        </div>
                        <div class="col-md-6">
                            <div class="card-Btn">
                                <button type="button" class="btn custombtn w-auto ms-2 btn-outline-danger" id="btnDeleteOrg"
                                    data-item="${value.organizationId}" org-index="${index}">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24"
                                        height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                        <path
                                            d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditOrg" data-item="${value.organizationId}"
                                    org-index="${index}" class="btn custombtn customBtn-light w-auto ms-1">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0" viewBox="0 0 24 24"
                                        height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
                                        <path
                                            d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                            </div>
                        </div>
                    </div>
                </span>
            </div>
            ${ htmlPosition ? ``: `<p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>` }
             <div id="positionDiv">${htmlPosition}</div>
        <!--<button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" onclick="$('#PositionModel').modal('toggle')">-->
        <button type="button" class="btn btn-primary btn-sm custombtn w-auto" onclick="OpenPositionModel('${value.organizationId}');">

            Add a Position at ${value.orgName}
        </button>
        </div>
       
    </div>
</div>
         `
       
        $('#divEditSection').append(html)
    });
   // $('#positionDiv').before(`<p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>`)
    // #region junk
    //$('#positionDiv div.row').html("");
    //positionArray = covertArrayKeyIntoCamelCase(positionArray)
    //$.each(positionArray, function (index, value) {
    //    let endMonth = "";
    //    if (value.endedMonth && value.endedYear) { endMonth = value.endedMonth + " " + value.endedYear; } else { endMonth = "Present"; }
    //    let htmlPosition = `<div class="card col-md-12 p-0 mb-3 cardWrapper mt-3">
    //                <div class="card-body">
    //                   <div class="row mx-auto">
    //                    <div class="col-md-12 p-0">
    //                            <span class="card-text">
    //                            <div class="row">
    //                            <div class="col-md-6">
    //                            <h5 class="title-text">${value.title}</h5>
    //                                <p class="text-muted">${value.startedMonth} ${value.endedMonth} - ${endMonth}</p>
    //                            </div>
    //                              <div class="col-md-6">
    //                                <div class="card-Btn">
    //                            <button type="button"  class="btn custombtn w-auto ms-2" id="btnDeletePosition" data-item='${value.orgPositionId}' pos-index='${index}'>
    //                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                     viewBox="0 0 24 24" height="1em" width="1em"
    //                                     xmlns="http://www.w3.org/2000/svg">
    //                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
    //                                    </path>
    //                                </svg>
    //                            </button>
    //                            <button type="button" id="btnEditPosition" data-item='${value.orgPositionId}' pos-index='${index}' class="btn custombtn customBtn-light w-auto ms-1">
    //                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                     viewBox="0 0 24 24" height="1em" width="1em"
    //                                     xmlns="http://www.w3.org/2000/svg">
    //                                    <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
    //                                    </path>
    //                                </svg>

    //                            </button>
    //                            </div>
    //                              </div>
    //                            </div>

    //                            </span>
    //                        </div>
    //                </div>
    //            </div>`

    //    //$('#positionDiv').append(html)
    //});

    // #endregion

    if (positionArray != null && positionArray.length > 3) {
        $("#positionDiv").addClass("BoxHeight");
    }
}

function guid() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}


const OpenOrgModel = () => {
    $('#hdfOrganizationId').val(guid())
    $('#SummaryModal').modal('toggle')
    $('#ddlEndedMonth').show();
    $('#ddlEndedYear').show();
    $('#labelEndedDate').show();
    $('#orgForm').trigger('reset');
}

const OpenPositionModel = (id) => {
    $('#hdfOrganizationId').val(id)
    $('#PositionModel').modal('toggle')

    $('#orgPositionForm')[0].reset()
    $('#ddlPositionEndedMonth').show();
    $('#ddlPositionEndedYear').show();
    $('#LabelEndedPositionDate').show();
}



$(document).on('click', '#btnEditOrg', function () {
    var response = organizationArr[$(this).attr('org-index')];
    localStorage.setItem("org-index", $(this).attr('org-index'));
    console.log(response)
    $('#hdfOrgExperienceId').val(response.orgExperienceId)
    $('#hdfOrganizationId').val(response.organizationId)
    $('#txtOrgName').val(response.orgName)
    $('#txtCity').val(response.city)
    $('#ddlStateAbbr').val(response.stateAbbr)
    $('#ddlStartedMonth').val(response.startedMonth)
    $('#ddlStartedYear').val(response.startedYear)
    $('#ddlEndedMonth').val(response.endedMonth)
    $('#ddlEndedYear').val(response.endedYear);
    if (!(response.endedMonth && response.endedYear)) {
        $("#cbCurrentlyIn").prop("checked", true);

    }
    else {
        $("#cbCurrentlyIn").prop("checked", false);
    }

    if ($("#cbCurrentlyIn").is(':checked')) {
        $('#ddlEndedMonth').val('');
        $('#ddlEndedYear').val('');
        $('#ddlEndedMonth').hide();
        $('#ddlEndedYear').hide();
        $('#labelEndedDate').hide();
    } else {
        $('#ddlEndedMonth').show();
        $('#ddlEndedYear').show();
        $('#labelEndedDate').show();
    }
    $('#SummaryModal').modal('show');
});

$(document).on('click', '.btnEditPosition', function () {
    var response = JSON.parse($(this).attr("data-json"));
    localStorage.setItem("pos-index", $(this).attr('pos-index'));
    $('#hdfOrganizationId').val(response.organizationId);
        $('#hdfOrgPositionId').val(response.orgPositionId);
        $('#txtTitle').val(response.title);
        $('#ddlPositionStartedMonth').val(response.startedMonth);
        $('#ddlPositionStartedYear').val(response.startedYear);
        $('#ddlPositionEndedMonth').val(response.endedMonth);
        $('#ddlPositionEndedYear').val(response.endedYear);
        $('#txtResponsibility1').val(response.responsibility1);
        $('#txtResponsibility2').val(response.responsibility2);
        $('#txtResponsibility3').val(response.responsibility3);
        $('#txtOtherInfo').val(response.otherInfo);
    if (!(response.endedMonth && response.endedYear)) {
        $("#cbPositionCurrentlyIn").prop("checked", true);
   
    }
    else {
        $("#cbPositionCurrentlyIn").prop("checked", false);
    }
    if ($("#cbPositionCurrentlyIn").is(':checked')) {
        $('#ddlPositionEndedMonth').val('');
        $('#ddlPositionEndedYear').val('');
        $('#ddlPositionEndedMonth').hide();
        $('#ddlPositionEndedYear').hide();
        $('#LabelEndedPositionDate').hide();
    } else {
        $('#ddlPositionEndedMonth').show();
        $('#ddlPositionEndedYear').show();
        $('#LabelEndedPositionDate').show();
    }
        $('#PositionModel').modal('show');
});


$(document).on('click', '#btnDeleteOrg', function () {

    localStorage.setItem("org-index", $(this).attr('org-index'));
    let id = $(this).attr('data-item')
    
    $.ajax({
        url: '/Organization/delete?id=' + id,
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

    let index = $(this).attr('pos-index');
    let id = $(this).attr('data-item');
    let Orgid = $(this).attr('data-org-id');
    $.ajax({
        url: '/Organization/delete?positionId=' + id,
        type: 'post',
        success: function (response) {

            organizationArr.filter(x => x.organizationId == Orgid)[0].orgPositions.splice(parseInt(index), 1);
           
            LoadCards();
        },
        error: function (err) {

        }
    });
});