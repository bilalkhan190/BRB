var licenseArray = [];
var certificateArray = [];
var affilationArray = [];
var positionArray = [];
var positionParentArray = [{ key: 0, data: [] }];

function showall() {
    $('#cbAffilationCurrentlyIn').prop("checked", false);
    $('#affilationform').trigger("reset");

    $('#txtAffilationEndedMonth').show();
    $('#txtAffilationEndedYear').show();
    $('#labelAffilationEndedDate').show();
    $("#pnlEndDate").show();
}
$('#cbAffilationCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#txtAffilationEndedMonth').val('');
        $('#txtAffilationEndedYear').val('');
        $('#txtAffilationEndedMonth').hide();
        $('#txtAffilationEndedYear').hide();
        $('#labelAffilationEndedDate').hide();
        $("#pnlEndDate").hide();
    } else {
        $('#txtAffilationEndedMonth').show();
        $('#txtAffilationEndedYear').show();
        $('#labelAffilationEndedDate').show();
        $("#pnlEndDate").show();

    }
});

$('#cbPositionCurrentlyIn').click(function () {
    if ($(this).is(':checked')) {
        $('#ddlPositionEndedMonth').hide();
        $('#ddlPositionEndedYear').hide();
        $('#ddlPositionEndedMonth').val('');
        $('#ddlPositionEndedYear').val('');
        $('#LabelEndedPositionDate').hide();
        $('#pnlPositionEndDate').hide();
    } else {
        $('#ddlPositionEndedMonth').show();
        $('#ddlPositionEndedYear').show();
        $('#LabelEndedPositionDate').show();
        $('#pnlPositionEndDate').show();
    }
});


$(document).ready(function () {
    LoadDropdowns();
    GetCompleteData();
})

$('#cbSectionNotApply').change(function () {
    if (this.checked) {
        $('#mainDiv').hide();
    }
    else {
        $('#mainDiv').show();
    }
});

$('#btnSaveLicense').click(function () {
    $('#licenseForm').validate();
    if ($('#licenseForm').valid()) {
        $("#noList").hide();
        $('#btnSaveLicense').prop('disabled', true)
        let license = {
            Title: $('#txtlicenseTitle').val(),
            StateAbbr: $('#ddllicenseState').val(),
            StateName: $('#ddllicenseState option:selected').text(),
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
        $('#licenseModal').modal('toggle')
        $('#btnSaveLicense').prop('disabled', false)

    }

});

$('.closeLicense').click(function () {
    $('#licenseForm').trigger('reset');
    $('#btnSaveLicense').prop('disabled', false)
})


function LoadLicenseCards() {
    let stateName = '';
    $('#divLicenseCard div.row-cstm').html('');
    licenseArray = covertArrayKeyIntoCamelCase(licenseArray)

    // console.log(licenseArray)
    $.each(licenseArray, function (index, value) {
        $("#noList").hide();
        let html = ` <div class="card p-0 mt-2 mb-2 cardWrapper">
  <div class="card-body">
    <div class="row">
      <div class="col-md-8">
        <h5 class="title-text"><span class="card-text">${value.title}</span></h5>
        <p class="text-muted"><span class="card-text">${value.receivedMonth} - ${value.receivedYear}</span></p>
        <p class="text-muted"><span class="card-text _stateName">${value.stateName}</span></p>
      </div>
      <div class="col-md-4">
        <div class="card-Btn">
          <button type="button" class="btnDeleteLicense btn custombtn w-auto ms-2 btn-outline-danger" data-item='${index}'><svg stroke="currentColor" fill="currentColor" stroke-width="0" viewbox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
          <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"></path></svg></button> <button type="button" id="btnEditLicense" class="btnEditLicense btn custombtn customBtn-light w-auto ms-1" data-item='${index}'><svg stroke="currentColor" fill="currentColor" stroke-width="0" viewbox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
          <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"></path></svg></button>
        </div>
      </div>
    </div>
  </div>
</div>`;
        $('#noLicense').hide();

        $('#divLicenseCard div.row-cstm').append(html);
        //$.ajax({
        //    url: '/Common/GetStateName?stateAbbr=' + value.stateAbbr,
        //    type: 'GET',

        //    success: function (resp) {
        //        $("._stateName").html("");
        //        $("._stateName").html(resp.data);
        //    },
        //    error: function (err) {

        //    }
        //});

    })

    if (licenseArray != null && licenseArray.length > 3) {
        $("#divLicenseCard div.row-cstm").addClass("BoxHeight");
    }
    $('#divEditSection').show();
}
function LoadCertCards() {
    $('#divCertificateCard div.row-cstm').html('');
    certificateArray = covertArrayKeyIntoCamelCase(certificateArray)
    $.each(certificateArray, function (index, value) {
        $("#noListCert").hide();
        let html = `
               <div class="card p-0 mt-2 mb-2 cardWrapper">
  <div class="card-body">
    <div class="row">
      <div class="col-md-8">
        <h5 class="title-text"><span class="card-text">${value.title}</span></h5>
        <p class="text-muted"><span class="card-text">${value.receivedMonth} - ${value.receivedYear}</span></p>
        <p class="text-muted"><span class="card-text _stateNam">${value.stateName}</span></p>
      </div>
      <div class="col-md-4">
        <div class="card-Btn">
          <button type="button" class="btnDeleteCertificate btn custombtn w-auto ms-2 btn-outline-danger" data-item="${index}"><svg stroke="currentColor" fill="currentColor" stroke-width="0" viewbox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
          <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z"></path></svg></button> <button type="button" class="btnEditCertificate btn custombtn customBtn-light w-auto ms-1" data-item='${index}'><svg stroke="currentColor" fill="currentColor" stroke-width="0" viewbox="0 0 24 24" height="1em" width="1em" xmlns="http://www.w3.org/2000/svg">
          <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z"></path></svg></button>
        </div>
      </div>
    </div>
  </div>
</div>
  `
        $('#noCertificate').hide();
        $('#divCertificateCard div.row-cstm').append(html);
        //$.ajax({
        //    url: '/Common/GetStateName?stateAbbr=' + value.stateAbbr,
        //    type: 'GET',

        //    success: function (resp) {
        //        $("._stateName").html("");
        //        $("._stateName").html(resp.data);
        //    },
        //    error: function (err) {

        //    }
        //});

    })
    if (certificateArray != null && certificateArray.length > 3) {
        $("#divCertificateCard").addClass("BoxHeight");
    }

}

function LoadaffCards() {


    $('#divAffilateCard div.row-cstm').html('');
    affilationArray = covertArrayKeyIntoCamelCase(affilationArray)
    $.each(affilationArray, function (index, affilation) {
        console.log(affilation,'---affilation ')
        // affilation.stateName = resp.data
        let endMonth = "";
        if (affilation.endedMonth && affilation.endedYear) { endMonth = affilation.endedMonth + " " + affilation.endedYear; } else { endMonth = "Present"; }
        $("#noListAff").hide();
        let PositionHtml = '';
        debugger
        let posArr = positionArray.filter(x => x.affiliationId == affilation.affiliationId);
        $.each(posArr, function (index, position) {
            let _endMonth = "";
            if (position.endedMonth && position.endedYear && position.endedMonth != "null" && position.endedYear != "null") { _endMonth = position.endedMonth + " " + position.endedYear; } else { _endMonth = "Present"; }
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
                            <button type="button"  class="btnDeletePosition btn custombtn w-auto ms-2 btn-outline-danger" data-item='${positionArray.length - 1}' data-json='${JSON.stringify(position)}'>
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                    </path>
                                </svg>
                            </button>
                            <button type="button" class='btnEditAffilationPosition btn custombtn customBtn-light w-auto ms-1' data-identity='${guid()}' data-json='${JSON.stringify(position)}' data-item='${positionArray.length - 1}' onclick="$('#PositionModel').modal('toggle')">
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
                                                        <p class="text-muted">${position.startedMonth} ${position.startedYear} - ${_endMonth}</p>
                </span>
        </div>
    </div>
    </div></div>`
            //$('.positionDiv').append(html);
        })
        let html = `
                <div class="card p-0 mt-2 mb-2 cardWrapper cardWrapper-aff"> 
                    <div class="card-body">
                       <div class="row">
                                <div class="col-md-12">
                                <span class="card-text row">
                                <div class="col-md-6">
                                    <h5 class="title-text affilationtitle">${affilation.affiliationName}</h5>
                                    </div>
                                        <div class="col-md-6">
                                 <div class="card-Btn">
                                <button type="button"  class="btnDeleteAffiliation btn custombtn w-auto ms-2 btn-outline-danger" data-json='${JSON.stringify(affilation)}' data-item='${affilationArray.length - 1}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button>
                                <button type="button" id="btnEditAffilation" class="btnEditAffilation btn custombtn customBtn-light w-auto ms-1" data-json='${JSON.stringify(affilation)}' data-item='${index}'>
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                                   </div>
                                </div>
                                    <p class="HasEndDate" class="text-muted">${affilation.startedMonth} ${affilation.startedYear} - ${endMonth}</p>
                                    <p class="_stateName" class="text-muted">${affilation.city} ,${affilation.stateName}</p>
                                </span>
                                <span>
                                ${PositionHtml ? "" : `   <p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>`}
                              
                               ${PositionHtml}
                             <button type="button" id="btnAddPositions" class= "btn custombtn customBtn-light w-auto ms-1 " data - bs - toggle="modal" onclick = "clearStorage(this)" data-item='${guid()}'
                                    data-bs-target="#PositionModel">
                                     Add an Position of ${affilation.affiliationName}
                            </button >
                            </div >
                        </div >
                    </div >
                </div >
        `
        $('#divAffilateCard div.row-cstm').append(html);





        //$.ajax({
        //    url: '/Common/GetStateName?stateAbbr=' + affilation.stateAbbr,
        //    type: 'GET',
        //    success: function (resp) {

        //    },
        //    error: function () { }
        //})


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

$(document).on("click", ".btnDeleteLicense", function () {
    $(this).closest(".cardWrapper").remove();

    let index = $(this).attr("data-item");
    let editRecord = licenseArray[index];
    licenseArray.splice(index, 1);
    LoadLicenseCards();
    $.ajax({
        url: '/professional/DeleteLicense?id=' + editRecord.licenseId,
        type: 'post',
        success: function (response) {
            if (response.success) {

            }
            if ($('#divLicenseCard').find('.row-cstm').children().length == 0) {
                $('#noList').show();
            }
        },
        error: function (error) { }
    });

});


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

$(document).on("click", ".btnDeleteCertificate", function () {
    $(this).closest(".cardWrapper").remove();
    let index = $(this).attr("data-item");
    let editRecord = certificateArray[index];
    certificateArray.splice(index, 1);
    LoadCertCards();
    console.log(editRecord)
    $.ajax({
        url: '/professional/DeleteCertificate?id=' + editRecord.certificateId,
        type: 'post',
        success: function (response) {
            if (response.success) {

            }
            if ($('#divCertificateCard').find('.row-cstm').children().length == 0) {
                $('#noListCert').show();
            }

        },
        error: function (error) { }
    });

});

$(document).on("click", ".btnEditAffilation", function () {
    let index = $(this).attr("data-item");
    let editRecord = affilationArray[index];
    console.log(editRecord)
    $('#txtAffiliationName').val(editRecord.affiliationName);
    $('#txtAffilationCity').val(editRecord.city);
    $('#txtAffilationStateAbbr').val(editRecord.stateAbbr);
    $('#txtAffilationStartedMonth').val(editRecord.startedMonth);
    $('#txtAffilationStartedYear').val(editRecord.startedYear);
    debugger;
    if (editRecord.endedMonth != null && editRecord.endedYear != null) {
        $('#txtAffilationEndedMonth').show();
        $('#txtAffilationEndedYear').show();
        $('#labelAffilationEndedDate').show();
        $('#txtAffilationEndedMonth').val(editRecord.endedMonth);
        $('#txtAffilationEndedYear').val(editRecord.endedYear);
    }
    else {
        $("#cbAffilationCurrentlyIn").prop("checked", true);
        $('#txtAffilationEndedMonth').hide();
        $('#txtAffilationEndedYear').hide();
        $('#labelAffilationEndedDate').hide();
    }
    localStorage.setItem("edit-aff", index);
    $("#AffilationModal").modal("show");
})

$(document).on("click", "button.btnEditAffilationPosition", function () {
    let index = $(this).attr("data-identity");
    let editRecord = JSON.parse($(this).attr("data-json"));
    console.log(editRecord, 'edit record')

    $('#txtpositionTitle').val(editRecord.title);
    $('#ddlPositionStartedMonth').val(editRecord.startedMonth);
    $('#ddlPositionStartedYear').val(editRecord.startedYear);

    $('#txtResponsibility1').val(editRecord.responsibility1);
    $('#txtResponsibility2').val(editRecord.responsibility2);
    $('#txtResponsibility3').val(editRecord.responsibility3);
    $('#txtOtherInfo').val(editRecord.otherInfo);
    if (editRecord.endedMonth == null && editRecord.endedYear == null || editRecord.endedMonth == "null" && editRecord.endedYear == "null") {
        $("#cbPositionCurrentlyIn").prop("checked", true);
        $('#ddlPositionEndedMonth').hide();
        $('#ddlPositionEndedYear').hide();
        $('#ddlPositionEndedMonth').val('');
        $('#ddlPositionEndedYear').val('');
        $('#LabelEndedPositionDate').hide();
        $('#pnlPositionEndDate').hide();
    } else {
        $('#ddlPositionEndedMonth').val(editRecord.endedMonth);
        $('#ddlPositionEndedYear').val(editRecord.endedYear);
        $('#ddlPositionEndedMonth').show();
        $('#ddlPositionEndedYear').show();
        $('#LabelEndedPositionDate').show();
        $('#pnlPositionEndDate').show();

    }
    localStorage.setItem("edit-position", index);
    $("#PositionModel").modal("show");
});

$(document).on("click", ".btnDeleteAffiliation", function () {
    $(this).closest(".cardWrapper-aff").remove();
    let index = $(this).attr("data-item");
    let editRecord = JSON.parse($(this).attr("data-json"));
    affilationArray.splice(index, 1);
    LoadaffCards();

    $.ajax({
        url: '/professional/DeleteAffilation?id=' + editRecord.affiliationId,
        type: 'post',
        success: function (response) {
            if (response.success) {


            }
            if ($('#divAffilateCard').find('.row-cstm').children().length == 0) {
                $('#noListAff').show();
            }
        },
        error: function (error) { }
    });

});



$(document).on("click", ".btnDeletePosition", function () {
    if ($(this).closest(".cardWrapper-aff").find(".cardWrapper-affPos").length == 1) {
        $(this).closest(".cardWrapper-affPos").before("<p class='noPosition'><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>");
    }
    $(this).closest(".cardWrapper-affPos").remove();
    let index = $(this).attr("data-item");
    console.log(index, "---index-----")
    let editRecord = JSON.parse($(this).attr("data-json"));
    console.log(editRecord, "---edit record for delete position")
    $.ajax({
        url: '/professional/DeleteAffilationPosition?id=' + editRecord.affiliationPositionId,
        type: 'post',
        success: function (response) {
            if (response.success) {
                debugger;
                positionArray.splice(index, 1);
                //let record = affilationArray.affiliationPositions.filter(x => x.affiliationPositionId == editRecord.affiliationPositionId)
                //let Affindex = affilationArray.affiliationPositions.findIndex(record);
                //affilationArray.affiliationPositions.splice(Affindex, 1);
                console.log(positionArray, '---position Array ---')
                localStorage.clear();
                /*LoadaffCards();*/
            }
        },
        error: function (error) { }
    });

});

$(document).on('click', '#btnAddPositions', function () {
    $('#positionForm').trigger('reset');
    $('#PositionModel').modal('show');


});
$('#btnSaveCertificate').click(function () {
    $('#certificateForm').validate();
    if ($('#certificateForm').valid()) {
        $("#noListCert").hide();
        $('#btnSaveCertificate').prop('disabled', true)
        let certificate = {
            Title: $('#txtCertificateTitle').val(),
            StateAbbr: $('#ddlCertificateState').val(),
            StateName: $('#ddlCertificateState option:selected').text(),
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
        $('#CertificateModal').modal('toggle')
        $('#btnSaveCertificate').prop('disabled', false)
    }

});

$('.closeCertificate').click(function () {
    $('#certificationForm').trigger('reset');
    $('#btnSaveCertificate').prop('disabled', false)
})

$('#btnSaveAffilation').click(function () {
    var sMonth = $("#txtAffilationStartedMonth").val();
    var sYear = $("#txtAffilationStartedYear").val();
    var eMonth = $("#txtAffilationEndedMonth").val();
    var eYear = $("#txtAffilationEndedYear").val();
    if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
        swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
        return
    }

    $('#affilationform').validate();
    if ($('#affilationform').valid()) {
        $("#noListAff").hide();
        let affilation = {
            affiliationName: $('#txtAffiliationName').val(),
            city: $('#txtAffilationCity').val(),
            stateAbbr: $('#txtAffilationStateAbbr').val(),
            stateName: $('#txtAffilationStateAbbr option:selected').text(),
            startedMonth: $('#txtAffilationStartedMonth').val(),
            startedYear: $('#txtAffilationStartedYear').val(),
            endedMonth: $('#txtAffilationEndedMonth').val(),
            endedYear: $('#txtAffilationEndedYear').val(),
            affiliationPositions: []
        };
        let endDate = affilation.endedMonth + " " + affilation.endedYear;
        if ($("#cbAffilationCurrentlyIn").is(":checked")) {
            affilation.endedMonth = null;
            affilation.endedYear = null;
            endDate = "Present";
        }
        $('#btnSaveAffilation').prop('disabled', true)
        if (localStorage.getItem("edit-aff") === null) {
            affilationArray.push(affilation)
            affilationArray = covertArrayKeyIntoCamelCase(affilationArray)
            $('#noAffilation').hide();
            let html = `
        <div class= "card p-0 mt-2 mb-2 cardWrapper cardWrapper-aff">
        <div class="card-body">
            <div class="row">
                <div class="col-md-12">
                    <span class="card-text row">
                        <div class="col-md-6">
                            <h5 class="title-text affilationtitle">${affilation.affiliationName}</h5>
                        </div>
                        <div class="col-md-6">
                            <div class="card-Btn">
                                <button type="button"  class="btnDeleteAffiliation btn custombtn w-auto ms-2 btn-outline-danger" data-json='${JSON.stringify(affilation)}' data-item='${affilationArray.length - 1}'>
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
                        <p class="HasEndDate" class="text-muted">${affilation.startedMonth} ${affilation.startedYear} - ${endDate}</p>
                        <p class="_stateName" class="text-muted">${affilation.city} , ${affilation.stateName}</p>
                    </span>
                    <span>
                        <p class="noPosition" class="danger-text"><em>You currently have no positions listed. Either add a position to the organization or delete the organization.</em></p>

                        <button type="button" class="btn btn-primary btn-sm custombtn w-auto mt-2" onclick="clearStorage(this);$('#PositionModel').modal('toggle')" data-item='${guid()}'
                            >
                            Add a Position at ${affilation.affiliationName}
                        </button>
                </div>
            </div>
                  </div>
                </div>
        `
            $('#divAffilateCard div.row-cstm').append(html);
        }
        else {
            let index = localStorage.getItem("edit-aff");
            affilationArray[index] = affilation;
            console.log(affilationArray[index],'affiliation edit mode array')
            $($(".cardWrapper-aff")[index]).find(".affilationtitle").html(affilation.affiliationName)
            $($(".cardWrapper-aff")[index]).find(".HasEndDate").html(`${affilation.startedMonth} ${affilation.startedYear} - ${endDate}`);
            $($(".cardWrapper-aff")[index]).find("._stateName").html(`${affilation.city} , ${affilation.stateName}`);
            $($(".cardWrapper-aff")[index]).find(".btnEditAffilation").attr("data-json", JSON.stringify(affilation));
            localStorage.clear();
        }
        $('#AffilationModal').modal('toggle')
    }
    // LoadaffCards();
});


$('.closeAffilation').click(function () {

    $('#btnSaveAffilation').prop('disabled', false)
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
                $('#cbIsComplete').prop('disabled', response.data.professionalExperience.isComplete)
                $('#cbSectionNotApply').prop('checked', response.data.professionalExperience.isOptOut).trigger('change')
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

$(document).on('click', '#btnAddPosition', function () {
    var sMonth = $("#ddlPositionStartedMonth").val();
    var sYear = $("#ddlPositionStartedYear").val();
    var eMonth = $("#ddlPositionEndedMonth").val();
    var eYear = $("#ddlPositionEndedYear").val();
    if (Date.parse(sMonth + " " + sYear) > Date.parse(eMonth + " " + eYear)) {
        swal("Invalid Date Range", "Start date cannot be greater than end date", "error");
        return;
    }


    $('#positionForm').validate();
    if ($('#positionForm').valid()) {
        let position = {
            title: $('#txtpositionTitle').val(),
            startedMonth: $('#ddlPositionStartedMonth').val(),
            startedYear: $('#ddlPositionStartedYear').val(),
            endedMonth: $('#ddlPositionEndedMonth').val(),
            endedYear: $('#ddlPositionEndedYear').val(),
            responsibility1: $('#txtResponsibility1').val(),
            responsibility2: $('#txtResponsibility2').val(),
            responsibility3: $('#txtResponsibility3').val(),
            otherInfo: $('#txtOtherInfo').val(),
        };
        $('#btnAddPosition').prop('disabled', true)
        positionArray.push(position);
        //LoadaffCards();

        $('#noPosition').hide();
        let _endMonth = "";
        if (position.endedMonth && position.endedYear) { _endMonth = position.endedMonth + " " + position.endedYear; } else { _endMonth = "Present"; }
        if (localStorage.getItem("edit-position")) {

            let html = `  <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span class="card-text row">
                                                        <div class="col-md-6">
                                                            <h5 class="title-text">${position.title}</h5>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="card-Btn">
                                                                <button type="button"  class="btnDeletePosition btn custombtn w-auto ms-2 btn-outline-danger"  data-item='${positionArray.length - 1}' data-json='${JSON.stringify(position)}'>
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                    </path>
                                </svg>
                            </button>
                                                                 <button type="button" class='btnEditAffilationPosition btn custombtn customBtn-light w-auto ms-1' data-identity='${guid()}' data-json='${JSON.stringify(position)}' data-item='${positionArray.length - 1}' onclick="$('#PositionModel').modal('toggle')">
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                    </path>
                                </svg>
                            </button>
                                                            </div>
                                                        </div>
                                                        <input type='hidden' class='Title' value='${position.title}'>
                                                            <input type='hidden' class='startMonth' value='${position.startedMonth}'>
                                                                <input type='hidden' class='startYear' value='${position.startedYear}'>
                                                                    <input type='hidden' class='endMonth' value='${position.endedMonth}'>
                                                                        <input type='hidden' class='endYear' value='${position.endedYear}'>
                                                                            <input type='hidden' class='res1' value='${position.responsibility1}'>
                                                                                <input type='hidden' class='res2' value=${position.responsibility2}>
                                                                                    <input type='hidden' class='res3' value='${position.responsibility3}'>
                                                                                        <input type='hidden' class='otherInfo' value='${position.otherInfo}'>
                                                                                            <p class="text-muted">${position.startedMonth} ${position.startedYear} - ${_endMonth}</p>
                                                                                        </span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>`;
            let id = localStorage.getItem("edit-position");

            $("button[data-identity='" + id + "']").closest(".cardWrapper-affPos").html(html);
            localStorage.clear();
        }
        else {
            $('#positionForm').trigger('reset');
            let html = `  <div class="card p-0 mt-2 mb-2 cardWrapper cardWrapper-affPos">  <div class="card-body">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <span class="card-text row">
                                                        <div class="col-md-6">
                                                            <h5 class="title-text">${position.title}</h5>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="card-Btn">
                                                                <button type="button"  class="btnDeletePosition btn custombtn w-auto ms-2 btn-outline-danger" data-item='${positionArray.length - 1} data-json='${JSON.stringify(position)}'>
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                    </path>
                                </svg>
                            </button>
                                                                 <button type="button" class='btnEditAffilationPosition btn custombtn customBtn-light w-auto ms-1' data-identity='${guid()}' data-json='${JSON.stringify(position)}' data-item='${positionArray.length - 1}' onclick="$('#PositionModel').modal('toggle')">
                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                    viewBox="0 0 24 24" height="1em" width="1em"
                                    xmlns="http://www.w3.org/2000/svg">
                                    <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                    </path>
                                </svg>
                            </button>
                                                            </div>
                                                        </div>
                                                        <input type='hidden' class='Title' value='${position.title}'>
                                                            <input type='hidden' class='startMonth' value='${position.startedMonth}'>
                                                                <input type='hidden' class='startYear' value='${position.startedYear}'>
                                                                    <input type='hidden' class='endMonth' value='${position.endedMonth}'>
                                                                        <input type='hidden' class='endYear' value='${position.endedYear}'>
                                                                            <input type='hidden' class='res1' value='${position.responsibility1}'>
                                                                                <input type='hidden' class='res2' value=${position.responsibility2}>
                                                                                    <input type='hidden' class='res3' value='${position.responsibility3}'>
                                                                                        <input type='hidden' class='otherInfo' value='${position.otherInfo}'>
                                                                                            <p class="text-muted">${position.startedMonth} ${position.startedYear} - ${_endMonth}</p>
                                                                                        </span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div></div>`;
            let id = localStorage.getItem("add-position");
            $("button[data-item='" + id + "']").prev("p").remove();
            $("button[data-item='" + id + "']").before(html);
            //$('#noPosition').parent().append(html);
        }
        $('#PositionModel').modal('toggle')
        $('#btnAddPosition').prop('disabled', false)
        console.log(positionArray);
    }
});

$('.closePosition').click(function () {
    $('#positionForm').trigger('reset');
    $('#btnAddPosition').prop('disabled', false)
})

$('.modalClose ').click(function () {
    $('#positionForm').trigger('reset');
})

$('.closeCertificate ').click(function () {
    $('#certificateForm').trigger('reset');
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
    debugger
    $.each(affilationArray, function (index, record) {
        let positions = [];
        $($(".cardWrapper-aff")[index]).find(".cardWrapper-affPos").each(function (index, value) {
            let position = {
                Title: $(this).find(".Title").val(),
                StartedMonth: $(this).find(".startMonth").val(),
                StartedYear: $(this).find(".startYear").val(),
                EndedMonth: $(this).find(".endMonth").val(),
                EndedYear: $(this).find(".endYear").val(),
                Responsibility1: $(this).find(".res1").val(),
                Responsibility2: $(this).find(".res2").val() == "null" || $(this).find(".res2").val() == null ? "" : $(this).find(".res2").val(),
                Responsibility3: $(this).find(".res3").val() == "null" || $(this).find(".res3").val() == null ? "" : $(this).find(".res3").val(),
                OtherInfo: $(this).find(".otherInfo").val() == "null" || $(this).find(".otherInfo").val() == null ? "" : $(this).find(".otherInfo").val(),
            };
            positions.push(position);
        });
        console.log(index, '--save and continue loop')

        record.affiliationPositions = positions; //this is the point


    });

    //console.log(affilationArray)
    //return;
    //affilationArray.AffiliationPositions = positionArray
    let data = {
        Licenses: licenseArray, Certificates: certificateArray, Affiliations: affilationArray,
        AffiliationPositions: positionArray,
        IsComplete: $('#cbIsComplete').is(':checked'),
        IsOptOut: $('#cbSectionNotApply').is(':checked'),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),

    };
    if (!data.IsOptOut) {
        if (licenseArray.length == 0 && certificateArray.length == 0 && (affilationArray.length == 0 || positionArray.length == 0)) {
            swal("Required", "Please fill out the any section to proceed", "error");
            return false;
        }
    }

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
    let parsedJson = JSON.parse($(this).attr("data-json"));
    $('#txtpositionTitle').val(parsedJson.Title);
    $('#ddlPositionStartedMonth').val(parsedJson.StartedMonth);
    $('#ddlPositionStartedYear').val(parsedJson.StartedYear);
    $('#ddlPositionEndedMonth').val(parsedJson.EndedMonth);
    $('#ddlPositionEndedYear').val(parsedJson.EndedYear);
    $('#txtResponsibility1').val(parsedJson.Responsibility1);
    $('#txtResponsibility2').val(parsedJson.Responsibility2 == "null" || parsedJson.Responsibility2 == null ? "" : parsedJson.responsibility2);
    $('#txtResponsibility3').val(parsedJson.Responsibility3 == "null" || parsedJson.Responsibility3 == null ? "" : parsedJson.responsibility3);
    $('#txtOtherInfo').val(parsedJson.OtherInfo == "null" || parsedJson.OtherInfo == null ? "" : parsedJson.OtherInfo);
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
    $('#cbPositionCurrentlyIn').prop('checked', false);
    $('#ddlPositionEndedMonth').show();
    $('#ddlPositionEndedYear').show();
    $('#LabelEndedPositionDate').show();
    $('#pnlPositionEndDate').show();
    $('#positionForm').trigger('reset');

}

