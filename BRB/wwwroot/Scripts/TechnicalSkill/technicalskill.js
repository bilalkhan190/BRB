$('#btnSaveAndContinue').click(function () {
    let data = {
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        TechnicalSkillId: $('#hdfTechnicalSkillId').val(),
        Msword: $('input[name="Msword"]').is(":checked"),
        Msexcel: $('input[name="Msexcel"]').is(":checked"),
        MspowerPoint: $('input[name="MspowerPoint"]').is(":checked"),
        //MswordPerfect: $('input[name="MswordPerfect"]').val(),
        GoogleSuite: $('input[name="GoogleSuite"]').is(":checked"),
        GoogleDocs: $('input[name="GoogleDocs"]').is(":checked"),
        Msoutlook: $('input[name="Msoutlook"]').is(":checked"),
        MacPages: $('input[name="MacPages"]').is(":checked"),
        MacNumbers: $('input[name="MacNumbers"]').is(":checked"),
        MacKeynote: $('input[name="MacKeynote"]').is(":checked"),
        AdobeAcrobat: $('input[name="AdobeAcrobat"]').is(":checked"),
        AdobePublisher: $('input[name="AdobePublisher"]').is(":checked"),
        AdobeIllustrator: $('input[name="AdobeIllustrator"]').is(":checked"),
        AdobePhotoshop: $('input[name="AdobePhotoshop"]').is(":checked"),
        otherPrograms: $('#otherSkill').val(),
        //OtherDesc: $('input[name="OtherDesc"]').val(),
        //OtherPrograms: $('input[name="OtherPrograms"]').val(),
        IsComplete: $('input[name="IsComplete"]').is(":checked"),
        /*IsOptOut: $('input[name="Msword"]').is(":checked")*/
    }
    console.log(data)
    $.ajax({
        url: '/technicalskills/postdata',
        type: 'POST',
        data: data,
        success: function (response) {
            LoadData();
            window.location.href = response.redirect;
        },
        error: function (error) {

        }
    });
});


$(document).ready(function () {
    LoadData()
});

function LoadData() {
    $.ajax({
        url: '/TechnicalSkills/GetAllData',
        type: 'get',
        success: function (response) {
            if (response.data != null) {
                console.log(response.data)
                $('#hdfTechnicalSkillId').val(response.data.technicalSkillId)
                if (response.data.msword) {
                    $('input[name="Msword"]').prop('checked', true)
                }
                if (response.data.msexcel) {
                    $('input[name="Msexcel"]').prop('checked', true)
                }
                if (response.data.mspowerPoint) {
                    $('input[name="MspowerPoint"]').prop('checked', true)
                }
                if (response.data.msoutlook) {
                    $('input[name="Msoutlook"]').prop('checked', true)
                }
                if (response.data.macPages) {
                    $('input[name="MacPages"]').prop('checked', true)
                }
                if (response.data.macKeynote) {
                    $('input[name="MacKeynote"]').prop('checked', true)
                }
                if (response.data.macNumbers) {
                    $('input[name="MacNumbers"]').prop('checked', true)
                }
                if (response.data.adobeAcrobat) {
                    $('input[name="AdobeAcrobat"]').prop('checked', true)
                }
                if (response.data.adobeIllustrator) {
                    $('input[name="AdobeIllustrator"]').prop('checked', true)
                }
                if (response.data.adobePhotoshop) {
                    $('input[name="AdobePhotoshop"]').prop('checked', true)
                }
                if (response.data.adobePublisher) {
                    $('input[name="AdobePublisher"]').prop('checked', true)
                }
                if (response.data.googleDocs) {
                    $('input[name="GoogleDocs"]').prop('checked', true)
                }
                if (response.data.googleSuite) {
                    $('input[name="GoogleSuite"]').prop('checked', true)
                }
                
                $('input[name="IsComplete"]').prop('checked', response.data.isComplete)
                $('input[name="IsComplete"]').prop('disabled', response.data.isComplete)
                
                $('#otherSkill').val(response.data.otherPrograms);
            }
           
        },
        error: function () {

        }
    })
}