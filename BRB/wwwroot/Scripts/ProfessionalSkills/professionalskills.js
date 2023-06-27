var licenseArray = [];
var certificateArray = [];
var affilationArray = [];
var positionArray = [];
$('#cbAffilationCurrentlyIn').click(function () {
            if ($(this).is(':checked')) {
                $('#txtAffilationEndedMonth').hide();
                $('#txtAffilationEndedYear').hide();
                $('#labelAffilationEndedDate').hide();
            } else {
                $('#txtAffilationEndedMonth').show();
                $('#txtAffilationEndedYear').show();
                $('#labelAffilationEndedDate').show();
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


$(document).ready(function () {
    LoadDropdowns();
})

$('#btnSaveLicense').click(function () {
    let license = {
        Title: $('#txtlicenseTitle').val(),
        StateAbbr: $('#ddllicenseState').val(),
        ReceivedMonth: $('#ddllicenseReceivedMonth').val(),
        ReceivedYear: $('#ddllicenseReceivedYear').val(),
    };
    let html = ` 
                  <div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                           <div class="col-md-10">
                                <span class="card-text">
                                    <p>${$('#txtlicenseTitle').val()}</p>
                                    <p class="text-muted"> ${$('#ddllicenseReceivedMonth').val()} -  ${$('#ddllicenseReceivedYear').val()} </p>
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
                                <button type="button" id="btnEditLicense" data-item='' class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                   
                                </button>

                            </div>
</div></div></div>
  `
    $('#noLicense').hide();
    $('#divLicenseCard div.row').append(html);
    licenseArray.push(license);
    console.log(licenseArray);
});


$('#btnSaveCertificate').click(function () {
    let certificate = {
        Title: $('#txtCertificateTitle').val(),
        StateAbbr: $('#ddlCertificateState').val(),
        ReceivedMonth: $('#ddlCertificateReceivedMonth').val(),
        ReceivedYear: $('#ddlCertificateReceivedYear').val(),
    };

    let html = `
                <div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-10">
                                <span class="card-text">
                                    <p>${$('#txtCertificateTitle').val()}</p>
                                    <p class="text-muted"> ${$('#ddlCertificateReceivedMonth').val() } -  ${$('#ddlCertificateReceivedYear').val()} </p>
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
                                <button type="button" id="btnEditLicense" data-item='' class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                   
                                </button>

                            </div>
</div></div></div>
  `
    $('#noCertificate').hide();
    $('#divCertificateCard div.row').append(html);
    certificateArray.push(certificate)
    console.log(certificateArray);
});

$('#btnSaveAffilation').click(function () {
    let affilation = {
        AffiliationName: $('#txtAffiliationName').val(),
        City: $('#txtAffilationCity').val(),
        StateAbbr: $('#txtAffilationStateAbbr').val(),
        StartedMonth: $('#txtAffilationStartedMonth').val(),
        StartedYear: $('#txtAffilationStartedYear').val(),
        EndedMonth: $('#txtAffilationEndedMonth').val(),
        EndedYear: $('#txtAffilationEndedYear').val(),
    };

    let html = `
                <div class="card ml-4 "> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-10">
                                <span class="card-text">
                                    <p>${affilation.AffiliationName}</p>
                                    <p id="HasEndDate" class="text-muted">${affilation.StartedMonth} ${affilation.StartedYear} - ${affilation.EndedMonth} ${affilation.EndedYear } </p>
                                </span>
                                <span>
                                 <p id="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>

                                 <button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" data-bs-toggle="modal"
                                    data-bs-target="#PositionModel">
                                Add an Position of ${affilation.AffiliationName}
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
                                <button type="button" id="btnEditAffilation" data-item='' class="btn btn-outline-primary">
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
  `
    $('#noAffilation').hide();
    $('#divAffilateCard div.row').append(html);
    affilationArray.push(affilation)
    console.log(affilationArray);
});

function LoadDropdowns() {
    $('.ddl_State').html("");
    $('.ddl_State').append('<option value="0" selected><b>Select State</b></option>')
    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('.ddl_State').append(`<option value="${value.stateAbbr}"><b> ${value.stateName} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}

$('#btnAddPosition').click(function () {
    let position = {
        Title: $('#txtpositionTitle').val(),
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
    let html = `<div class="card ml-4 mt-2"> 
                    <div class="card-body">
                       <div class="row">
                        <div class="col-md-10">
                                <span class="card-text">
                                    <p>${position.Title}</p>
                                    <p class="text-muted">${position.StartedMonth} ${position.StartedYear} - ${position.EndedMonth} ${position.EndedMonth}</p>
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
    $('#noPosition').hide();
    $('#noPosition').parent().append(html)
   
});

$(document).on('click', '#cbSectionNotApply', function () {
    if (this.checked) {
        $('#professionalForm').hide();
        $('#mainDiv').hide()
    }
    else {
        $('#professionalForm').show();
        $('#mainDiv').show()
    }
});

$('#btnSaveAndContinue').click(function () {
    let data = {
        Licenses: licenseArray, Certificates: certificateArray, Affiliations: affilationArray,
        AffiliationPositions: positionArray,
        IsComplete: $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked,
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),

    };
    $.ajax({
        url: '/Professional/PostProfessionalSkillsData',
        type: 'POST',
        data: { professionalViewModel: data },
        success: function (response) { },
        error: function (error) { }
    });

});
