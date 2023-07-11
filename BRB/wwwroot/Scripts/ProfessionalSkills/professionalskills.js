var licenseArray = [];
var certificateArray = [];
var affilationArray = [];
var positionArray = [];
var positionParentArray = [{ key: 0, data: [] }];
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
    GetCompleteData();
})

$('#btnSaveLicense').click(function () {
    $('#licenseForm').validate();
    if ($('#licenseForm').valid()) {
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
        $('#licenseForm').trigger('reset');
        LoadLicenseCards();


    }

});

$('.closeLicense').click(function () {
    $('#licenseForm').trigger('reset');
})


function LoadLicenseCards() {
    $('#divLicenseCard div.row-cstm').html('');
   licenseArray = covertArrayKeyIntoCamelCase(licenseArray)
    console.log(licenseArray)
    $.each(licenseArray, function (index, value) {
        let html = ` 
                  <div class="card p-0 mt-2 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                           <div class="col-md-8">
                                <span class="card-text">
                                    <h5 class="title-text">${value.title}</h5>
                                    <p class="text-muted"> ${value.receivedYear} -  ${value.receivedMonth} </p>
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
   certificateArray = covertArrayKeyIntoCamelCase(certificateArray)
    console.log(certificateArray)
    $.each(certificateArray, function (index, value) {
        let html = `
                <div class="card p-0 mt-2 mb-2 cardWrapper"> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-8">
                                <span class="card-text">
                                    <h5 class="title-text">${value.title}</h5>
                                    <p class="text-muted"> ${value.receivedMonth} -  ${value.receivedYear} </p>
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
 affilationArray = covertArrayKeyIntoCamelCase(affilationArray)
    $.each(affilationArray, function (index, affilation) {
        let PositionHtml = '';
        $.each(positionArray, function (index, position) {
            PositionHtml += `
         <div class="card p-0 mt-2 mb-2 cardWrapper cardWrapper-affPos"> 
    <div class= "card-body">
       <div class="row">
            <div class="col-md-12">
                <span class="card-text row">
                    <div class="col-md-6">
                        <h5 class="title-text">${position.title}</h5>
                    </div>
                    <div class="col-md-6">
                        <div class="card-Btn">
                            <button type="button" class="btn custombtn w-auto ms-2">
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                    </path>
                                </svg>
                            </button>
                            <button type="button" class='btnEditAffilationPosition custombtn customBtn-light w-auto ms-1' data-identity='${guid()}' data-json='${JSON.stringify(position)}' data-item='${positionArray.length - 1}' data-bs-toggle="modal"
                                data-bs-target="#PositionModel">
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                    </path>
                                </svg>
                            </button>
                        </div>
                    </div>
                    <input type='hidden' class='Title' value='${position.title}'/>
                        <input type='hidden' class='startMonth' value='${position.startedMonth}'/>
                            <input type='hidden' class='startYear' value='${position.startedYear}'/>
                                <input type='hidden' class='endMonth' value='${position.endedMonth}'/>
                                    <input type='hidden' class='endYear' value='${position.endedYear}'/>
                                        <input type='hidden' class='res1' value='${position.responsibility1}'/>
                                            <input type='hidden' class='res2' value='${position.responsibility2}'/>
                                                <input type='hidden' class='res3' value='${position.responsibility3}'/>
                                                    <input type='hidden' class='otherInfo' value='${position.otherInfo}'/>
                                                        <p class="text-muted">${position.startedMonth} ${position.startedYear} - ${position.endedMonth} ${position.endedYear}</p>
                </span>
                </div>
            </div>
        </div>
    </div>
    </div>
                                    `
            //$('.positionDiv').append(html);
        })
        let html = `
                <div class="card p-0 mt-2 mb-2 cardWrapper cardWrapper-aff"> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-12">
                                <span class="card-text row">
                                <div class="col-md-6">
                                    <h5 class="title-text">${affilation.affiliationName}</h5>
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
                                <button type="button" id="btnEditAffilation" class="btnEditAffilation btn custombtn customBtn-light w-auto ms-1" data-json='${JSON.stringify(affilation)}' data-item='${affilationArray.length - 1}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                   </div>
                                </div>
                                    <p class="HasEndDate" class="text-muted">${affilation.startedMonth} ${affilation.startedYear} - ${affilation.endedMonth} ${affilation.endedYear} </p>
                                </span>
                                <span>
                                 <p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>
                               ${PositionHtml}
                             <button type = "button" id="btnAddPositions" class= "btn custombtn customBtn-light w-auto ms-1 " data - bs - toggle="modal" onclick = "clearStorage(this)" data-item='${guid()}'
                                    data - bs - target="#PositionModel">
                                     Add an Position of ${affilation.affiliationName}
                            </button >
                            </div >
                        </div >
                    </div >
                </div >
        `
        $('#divAffilateCard div.row-cstm').append(html);
        
    });
   // $('.positionDiv').html('');
   
    if (affilationArray != null && affilationArray.length > 3) {
        $("#divAffilateCard").addClass("BoxHeight");
    }
    if (positionArray != null && positionArray.length > 3) {
        $(".positionDiv").addClass("BoxHeight");
    }
}

$(document).on("click", ".btnEditLicense", function () {
    let index = $(this).attr("data-item");
    let editRecord = licenseArray[index];
    console.log(editRecord)
       $('#txtlicenseTitle').val(editRecord.title),
        $('#ddllicenseState').val(editRecord.stateAbbr),
        $('#ddllicenseReceivedMonth').val(editRecord.receivedMonth),
        $('#ddllicenseReceivedYear').val(editRecord.receivedYear),
        localStorage.setItem("edit-license", index);
    $("#licenseModal").modal("show");
})


$(document).on("click", ".btnEditCertificate", function () {
    let index = $(this).attr("data-item");
    let editRecord = certificateArray[index];
    $('#txtCertificateTitle').val(editRecord.title),
        $('#ddlCertificateState').val(editRecord.stateAbbr),
        $('#ddlCertificateReceivedMonth').val(editRecord.receivedMonth),
        $('#ddlCertificateReceivedYear').val(editRecord.receivedYear),
        localStorage.setItem("edit-cert", index);
    $("#CertificateModal").modal("show");
})

$(document).on("click", ".btnEditAffilation", function () {
    let index = $(this).attr("data-item");
    let editRecord = JSON.parse($(this).attr("data-json"));
    console.log(editRecord)
    $('#txtAffiliationName').val(editRecord.AffiliationName);
    $('#txtAffilationCity').val(editRecord.City);
    $('#txtAffilationStateAbbr').val(editRecord.StateAbbr);
    $('#txtAffilationStartedMonth').val(editRecord.StartedMonth);
    $('#txtAffilationStartedYear').val(editRecord.StartedYear);
    $('#txtAffilationEndedMonth').val(editRecord.EndedMonth);
    $('#txtAffilationEndedYear').val(editRecord.EndedYear);
    localStorage.setItem("edit-aff", index);
    $("#AffilationModal").modal("show");
})

//$(document).on("click", "button.btnEditAffilationPosition", function () {
//    let index = $(this).attr("data-item");
//    let editRecord = JSON.parse($(this).attr("data-json"));
//    console.log(editRecord)
//    $('#txtpositionTitle').val(editRecord.title);
//    $('#ddlPositionStartedMonth').val(editRecord.startedMonth);
//    $('#ddlPositionStartedYear').val(editRecord.startedYear);
//    $('#ddlPositionEndedMonth').val(editRecord.endedMonth);
//    $('#ddlPositionEndedYear').val(editRecord.endedYear);
//    $('#txtResponsibility1').val(editRecord.responsibility1);
//    $('#txtResponsibility2').val(editRecord.responsibility2);
//    $('#txtResponsibility3').val(editRecord.responsibility3);
//    $('#txtOtherInfo').val(editRecord.otherInfo);
//    localStorage.setItem("edit-position", index);
//    $("#PositionModel").modal("show");
//});

$(document).on('click', '#btnAddPositions', function () {
    $('#PositionModel').modal('show')
});
$('#btnSaveCertificate').click(function () {
    $('#certificateForm').validate();
    if ($('#certificateForm').valid()) {
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

        $('#certificateForm').trigger('reset');
        LoadCertCards();
    }

});

$('.closeCertificate').click(function () {
    $('#certificationForm').trigger('reset');
})

$('#btnSaveAffilation').click(function () {
    $('#affilationform').validate();
    if ($('#affilationform').valid()) {
        let affilation = {
            AffiliationName: $('#txtAffiliationName').val(),
            City: $('#txtAffilationCity').val(),
            StateAbbr: $('#txtAffilationStateAbbr').val(),
            StartedMonth: $('#txtAffilationStartedMonth').val(),
            StartedYear: $('#txtAffilationStartedYear').val(),
            EndedMonth: $('#txtAffilationEndedMonth').val(),
            EndedYear: $('#txtAffilationEndedYear').val(),
            AffiliationPositions: []
        };

        if (localStorage.getItem("edit-aff") === null) {
            affilationArray.push(affilation)
            affilationArray = covertArrayKeyIntoCamelCase(affilationArray)
            $('#noAffilation').hide();
            $('#affilationform').trigger('reset');
            let html = `
        <div class= "card p-0 mt-2 mb-2 cardWrapper cardWrapper-aff">
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <span class="card-text row">
                        <div class="col-md-6">
                            <h5 class="title-text">${affilation.AffiliationName}</h5>
                        </div>
                        <div class="col-md-6">
                            <div class="card-Btn">
                                <button type="button" class="btn custombtn w-auto ms-2">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                        viewBox="0 0 24 24" height="1em" width="1em"
                                        xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditAffilation" class="btnEditAffilation btn custombtn customBtn-light w-auto ms-1" data-json='${JSON.stringify(affilation)}' data-item='${affilationArray.length - 1}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                        viewBox="0 0 24 24" height="1em" width="1em"
                                        xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                            </div>
                        </div>
                        <p class="HasEndDate" class="text-muted">${affilation.StartedMonth} ${affilation.StartedYear} - ${affilation.EndedMonth} ${affilation.EndedYear} </p>
                    </span>
                    <span>
                        <p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>

                        <button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" data-bs-toggle="modal" onclick="clearStorage(this)" data-item='${guid()}'
                            data-bs-target="#PositionModel">
                            Add an Position of ${affilation.AffiliationName}
                        </button>
                </div>
            </div>
                  </div>
                </div>
        `
            $('#divAffilateCard div.row-cstm').append(html);
        }
        else {
            debugger;
            let index = localStorage.getItem("edit-aff");
            affilationArray[index] = affilation;
            $($(".cardWrapper-aff")[index]).find(".title-text").html(affilation.AffiliationName)
            $($(".cardWrapper-aff")[index]).find(".HasEndDate").html(`${affilation.StartedMonth} ${affilation.StartedYear} - ${affilation.EndedMonth} ${affilation.EndedYear}`);
            $($(".cardWrapper-aff")[index]).find(".btnEditAffilation").attr("data-json", JSON.stringify(affilation));
            localStorage.clear();
        }
    }
    // LoadaffCards();
});


$('.closeAffilation').click(function () {
    $('#affilationform').trigger('reset');
})
function GetCompleteData() {
    $.ajax({
        url: '/professional/getdata',
        type: 'get',
        success: function (response) {
            if (response.data != null) {
                console.log(response.data)
                console.log(response.data.professionalExperience.isComplete)
                $('#cbIsComplete').prop('checked', response.data.professionalExperience.isComplete)
                if (response.data.licenses.length > 0) {
                    $.each(response.data.licenses, function (index, value) {
                        licenseArray.push(value)
                    });
                   
                    LoadLicenseCards();
                }
                if (response.data.certificates.length > 0) {
                    $.each(response.data.certificates, function (index, value) {
                        certificateArray.push(value)
                    });
                   
                    LoadCertCards();
                }
                if (response.data.affilationWithPositions.length > 0) {
                    $.each(response.data.affilationWithPositions, function (index, value) {
                        affilationArray.push(value)
                        $.each(response.data.affilationWithPositions[index].affiliationPositions, function (i, v) {
                            positionArray.push(v)
                        })
                    });
                   
                    LoadaffCards();
                }

            }
          
        },
        error: function (err) {
            alert(err)
        }
    });
}

function LoadDropdowns() {
    $('#ddllicenseState').html("");
    $('#ddllicenseState').append('<option value="" selected><b>Select State</b></option>')
    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddllicenseState').append(`<option value = "${value.stateAbbr}"> <b> ${value.stateName} </b></option > `);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlCertificateState').html("");
    $('#ddlCertificateState').append('<option value="" selected><b>Select State</b></option>')
    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlCertificateState').append(`<option value = "${value.stateAbbr}"> <b> ${value.stateName} </b></option > `);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#txtAffilationStateAbbr').html("");
    $('#txtAffilationStateAbbr').append('<option value="" selected><b>Select State</b></option>')
    $.ajax({
        url: '/Common/GetStateList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#txtAffilationStateAbbr').append(`<option value = "${value.stateAbbr}"> <b> ${value.stateName} </b></option > `);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}

$(document).on('click','#btnAddPosition',function () {
    $('#positionForm').validate();
    if ($('#positionForm').valid()) {
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
        //LoadaffCards();

        $('#noPosition').hide();
        if (localStorage.getItem("edit-position")) {
            let html = `  <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span class="card-text row">
                                                        <div class="col-md-6">
                                                            <h5 class="title-text">${position.Title}</h5>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="card-Btn">
                                                                <button type="button" class="btn custombtn w-auto ms-2">
                                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                        viewBox="0 0 24 24" height="1em" width="1em"
                                                                        xmlns="http://www.w3.org/2000/svg">
                                                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                        </path>
                                                                    </svg>
                                                                </button>
                                                                <button type="button" class='btnEditPosition' data-identity='${guid()}' data-json='${JSON.stringify(position)}' class="btn custombtn customBtn-light w-auto ms-1" data-bs-toggle="modal"
                                                                    data-bs-target="#PositionModel">
                                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                        viewBox="0 0 24 24" height="1em" width="1em"
                                                                        xmlns="http://www.w3.org/2000/svg">
                                                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                                                        </path>
                                                                    </svg>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <input type='hidden' class='Title' value='${position.Title}'>
                                                            <input type='hidden' class='startMonth' value='${position.StartedMonth}'>
                                                                <input type='hidden' class='startYear' value='${position.StartedYear}'>
                                                                    <input type='hidden' class='endMonth' value='${position.EndedMonth}'>
                                                                        <input type='hidden' class='endYear' value='${position.EndedYear}'>
                                                                            <input type='hidden' class='res1' value='${position.Responsibility1}'>
                                                                                <input type='hidden' class='res2' value=${position.Responsibility2}>
                                                                                    <input type='hidden' class='res3' value='${position.Responsibility3}'>
                                                                                        <input type='hidden' class='otherInfo' value='${position.OtherInfo}'>
                                                                                            <p class="text-muted">${position.StartedMonth} ${position.StartedYear} - ${position.EndedMonth} ${position.EndedYear}</p>
                                                                                        </span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>`;
            let id = localStorage.getItem("edit-position");
            $("button[data-identity='" + id + "']").closest(".cardWrapper").html(html);
            localStorage.clear();
        }
        else {
            $('#positionForm').trigger('reset');

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
                                                                <button type="button" class="btn custombtn w-auto ms-2">
                                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                        viewBox="0 0 24 24" height="1em" width="1em"
                                                                        xmlns="http://www.w3.org/2000/svg">
                                                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                        </path>
                                                                    </svg>
                                                                </button>
                                                                <button type="button" class='btnEditPosition' data-identity='${guid()}' data-json='${JSON.stringify(position)}' class="btn custombtn customBtn-light w-auto ms-1" data-bs-toggle="modal"
                                                                    data-bs-target="#PositionModel">
                                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                        viewBox="0 0 24 24" height="1em" width="1em"
                                                                        xmlns="http://www.w3.org/2000/svg">
                                                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                                                        </path>
                                                                    </svg>
                                                                </button>
                                                            </div>
                                                        </div>
                                                        <input type='hidden' class='Title' value='${position.Title}'>
                                                            <input type='hidden' class='startMonth' value='${position.StartedMonth}'>
                                                                <input type='hidden' class='startYear' value='${position.StartedYear}'>
                                                                    <input type='hidden' class='endMonth' value='${position.EndedMonth}'>
                                                                        <input type='hidden' class='endYear' value='${position.EndedYear}'>
                                                                            <input type='hidden' class='res1' value='${position.Responsibility1}'>
                                                                                <input type='hidden' class='res2' value=${position.Responsibility2}>
                                                                                    <input type='hidden' class='res3' value='${position.Responsibility3}'>
                                                                                        <input type='hidden' class='otherInfo' value='${position.OtherInfo}'>
                                                                                            <p class="text-muted">${position.StartedMonth} ${position.StartedYear} - ${position.EndedMonth} ${position.EndedYear}</p>
                                                                                        </span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div> </div>`;
            let id = localStorage.getItem("add-position");
            $("button[data-item='" + id + "']").before(html);
            //$('#noPosition').parent().append(html);
        }

        console.log(positionArray);
    }
});

$('.closePosition').click(function () {
    $('#positionForm').trigger('reset');
})
//$(document).on('click', '#cbSectionNotApply', function () {
//    if (this.checked) {
//        $('#professionalForm').hide();
//        $('#mainDiv').hide()
//    }
//    else {
//        $('#professionalForm').show();
//        $('#mainDiv').show()
//    }
//});

$('#btnSaveAndContinue').click(function () {
    $.each(affilationArray, function (index, record) {
        let positions = [];
        $($(".cardWrapper-aff")[index]).find(".cardWrapper").each(function (index, value) {
            let position = {
                Title: $(this).find(".Title").val(),
                StartedMonth: $(this).find(".startMonth").val(),
                StartedYear: $(this).find(".startYear").val(),
                EndedMonth: $(this).find(".endMonth").val(),
                EndedYear: $(this).find(".endYear").val(),
                Responsibility1: $(this).find(".res1").val(),
                Responsibility2: $(this).find(".res2").val(),
                Responsibility3: $(this).find(".res3").val(),
                OtherInfo: $(this).find(".otherInfo").val(),
            };

            positions.push(position);
        });
        record.AffiliationPositions = positions;
    });
    let data = {
        Licenses: licenseArray, Certificates: certificateArray, Affiliations: affilationArray,
        AffiliationPositions: positionArray,
        IsComplete: $('#cbIsComplete').is(':checked'),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),

    };
    $.ajax({
        url: '/Professional/PostProfessionalSkillsData',
        type: 'POST',
        data: { professionalViewModel: data },
        success: function (response) {
            console.log(response)
            window.location.href = response.redirect;
        },
        error: function (error) { }
    });

});


function editPosition(json) {

}

$(document).on("click", "button.btnEditPosition", function () {
    parsedJson = JSON.parse($(this).attr("data-json"));
    $('#txtpositionTitle').val(parsedJson.Title);
    $('#ddlPositionStartedMonth').val(parsedJson.StartedMonth);
    $('#ddlPositionStartedYear').val(parsedJson.StartedYear);
    $('#ddlPositionEndedMonth').val(parsedJson.EndedMonth);
    $('#ddlPositionEndedYear').val(parsedJson.EndedYear);
    $('#txtResponsibility1').val(parsedJson.Responsibility1);
    $('#txtResponsibility2').val(parsedJson.Responsibility2);
    $('#txtResponsibility3').val(parsedJson.Responsibility3);
    $('#txtOtherInfo').val(parsedJson.OtherInfo);
    localStorage.setItem("edit-position", $(this).attr("data-identity"));
})


function guid() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}


function clearStorage(btn) {
    localStorage.clear();
    localStorage.setItem("add-position", $(btn).attr("data-item"));
}

