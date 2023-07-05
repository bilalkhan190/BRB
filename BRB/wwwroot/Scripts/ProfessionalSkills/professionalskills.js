var licenseArray = [];
var certificateArray = [];
var affilationArray = [];
var positionArray = [];
var positionParentArray = [{key: 0, data: []}];
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
    if (localStorage.getItem("edit-license") === null) {
        licenseArray.push(license);
       
    }
    else {
        let index = localStorage.getItem("edit-license");
        licenseArray[index] = license;
        localStorage.clear();
    }
   
    LoadCards();
});



function LoadCards() {
    $('#divLicenseCard div.row-cstm').html('');
    $.each(licenseArray, function (index, value) {
        console.log(value);
        let html = ` 
                  <div class="card p-0 mt-2 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                           <div class="col-md-8">
                                <span class="card-text">
                                    <h5 class="title-text">${value.Title}</h5>
                                    <p class="text-muted"> ${value.ReceivedYear} -  ${value.ReceivedMonth} </p>
                                </span>
                            </div>
                            <div class="col-md-4">
                            <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditLicense" class="btnEditLicense btn custombtn customBtn-light w-auto ms-1" data-item='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                   
                                </button>
                                </div>
                            </div>
</div></div></div> `;
        $('#noLicense').hide();
        $('#divLicenseCard div.row-cstm').append(html);
    })

    if (licenseArray != null && licenseArray.length > 3) {
        $("#divLicenseCard div.row-cstm").addClass("BoxHeight");
    }
    $('#divEditSection').show();
}
function LoadCertCards() {
    $('#divCertificateCard div.row-cstm').html('');
    $.each(certificateArray, function (index, value) {
        console.log(value);
        let html = `
                <div class="card p-0 mt-2 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-8">
                                <span class="card-text">
                                    <h5 class="title-text">${value.Title}</h5>
                                    <p class="text-muted"> ${value.ReceivedMonth} -  ${value.ReceivedYear} </p>
                                </span>
                            </div>
                            <div class="col-md-4">
                            <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" class="btnEditCertificate btn custombtn customBtn-light w-auto ms-1" data-item='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                   
                                </button>
                                </div>
                            </div>
</div></div></div>
  `
        $('#noCertificate').hide();
        $('#divCertificateCard div.row-cstm').append(html);
    })
    if (certificateArray != null && certificateArray.length > 3) {
        $("#divCertificateCard").addClass("BoxHeight");
    }

}

function LoadaffCards() {
    $('#divAffilateCard div.row-cstm').html('');
    $.each(affilationArray, function (index, affilation) {
        let html = `
                <div class="card p-0 mt-2 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-12">
                                <span class="card-text row">
                                <div class="col-md-6">
                                    <h5 class="title-text">${affilation.AffiliationName}</h5>
                                    </div>
                                        <div class="col-md-6">
                                 <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditAffilation" class="btnEditAffilation btn custombtn customBtn-light w-auto ms-1" data-item='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                   </div>
                                </div>
                                    <p id="HasEndDate" class="text-muted">${affilation.StartedMonth} ${affilation.StartedYear} - ${affilation.EndedMonth} ${affilation.EndedYear} </p>
                                </span>
                                <span>
                                 <p id="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>

                                 <button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" data-bs-toggle="modal"
                                    data-bs-target="#PositionModel">
                                Add an Position of ${affilation.AffiliationName}
                            </button>
                            </div>
                        </div>
                    </div>
                </div>
  `
        $('#noAffilation').hide();
        $('#divAffilateCard div.row-cstm').append(html);
    })
    if (affilationArray != null && affilationArray.length > 3) {
        $("#divAffilateCard").addClass("BoxHeight");
    }
}

$(document).on("click", ".btnEditLicense", function () {
    let index = $(this).attr("data-item");    
    let editRecord = licenseArray[index];
    $('#txtlicenseTitle').val(editRecord.Title),
        $('#ddllicenseState').val(editRecord.StateAbbr),
        $('#ddllicenseReceivedMonth').val(editRecord.ReceivedMonth),
        $('#ddllicenseReceivedYear').val(editRecord.ReceivedYear),
        localStorage.setItem("edit-license", index);
    $("#licenseModal").modal("show");
})


$(document).on("click", ".btnEditCertificate", function () {
    let index = $(this).attr("data-item");
    let editRecord = certificateArray[index];
    $('#txtCertificateTitle').val(editRecord.Title),
        $('#ddlCertificateState').val(editRecord.StateAbbr),
        $('#ddlCertificateReceivedMonth').val(editRecord.ReceivedMonth),
        $('#ddlCertificateReceivedYear').val(editRecord.ReceivedYear),
    localStorage.setItem("edit-cert", index);
    $("#CertificateModal").modal("show");
})

$(document).on("click", ".btnEditAffilation", function () {
    let index = $(this).attr("data-item");
    let editRecord = affilationArray[index];
    $('#txtAffiliationName').val(editRecord.AffiliationName),
        $('#txtAffilationCity').val(editRecord.City),
        $('#txtAffilationStateAbbr').val(editRecord.StateAbbr),
        $('#txtAffilationStartedMonth').val(editRecord.StartedMonth),
        $('#txtAffilationStartedYear').val(editRecord.StartedYear),
        $('#txtAffilationEndedMonth').val(editRecord.EndedMonth),
        $('#txtAffilationEndedYear').val(editRecord.EndedYear),
      localStorage.setItem("edit-aff", index);
    $("#AffilationModal").modal("show");
})

$('#btnSaveCertificate').click(function () {
    let certificate = {
        Title: $('#txtCertificateTitle').val(),
        StateAbbr: $('#ddlCertificateState').val(),
        ReceivedMonth: $('#ddlCertificateReceivedMonth').val(),
        ReceivedYear: $('#ddlCertificateReceivedYear').val(),
    };
    if (localStorage.getItem("edit-cert") === null) {
        certificateArray.push(certificate)
    }
    else {
        let index = localStorage.getItem("edit-cert");
        certificateArray[index] = certificate;
        localStorage.clear();
    }
   

    LoadCertCards();
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
    if (localStorage.getItem("edit-aff") === null) {
        affilationArray.push(affilation)
    }
    else {
        let index = localStorage.getItem("edit-aff");
        affilationArray[index] = affilation;
        localStorage.clear();
    }

    LoadaffCards();
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
    let html = `<div class="card p-0 mt-4 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                        <div class="col-md-12">
                                <span class="card-text row">
                                    <div class="col-md-6">
                                    <h5 class="title-text">${position.Title}</h5>
                                    </div>
                                    <div class="col-md-6">
                                       <div class="card-Btn">
                                <button type="button"  class="btn custombtn w-auto ms-2">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditCollege" data-item='' class="btn custombtn customBtn-light w-auto ms-1">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                </div>
                                    </div>
                                    <p class="text-muted">${position.StartedMonth} ${position.StartedYear} - ${position.EndedMonth} ${position.EndedMonth}</p>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>`
    $('#noPosition').hide();
    $('#noPosition').parent().append(html)
    console.log(positionArray);
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
