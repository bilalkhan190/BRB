var companyArray = [];
var jobArray = [];
let url = '/WorkExperience/GetJobResponsibility';
$(document).on('click', '#btnAddJob', function () {
    if ($('#divJobForm').children().length > 0) {
        $('#mainDisplayPage').hide();
        $('#divJobForm').show()
    } else {
        $('#mainDisplayPage').hide();
        $('#divJobForm').load(url);
        setTimeout(function () { fillDropdown() }, 1000)
    }
});


$('#btnAddCompany').click(function () {

    let company = {
        LanguageName: $('#txtLanguageName').val(),
        LanguageAbilityId: $('#ddlAbility').val(),
    };
    companyArray.push(company);
    //fill the array of position record and display the recorded data in div

    let html = ` 
                <div class="card ml-10 col-md-10"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-8">
                                <span class="card-text">
                                    <p></p>
                                    <p class="text-muted"></p>
                                </span>
                            </div>
                            <div class="col-md-4">
                                <button type="button"  class="btn btn-outline-danger">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                        </path>
                                    </svg>
                                </button><button type="button" id="btnEditLangauge" data-item='' class="btn btn-outline-primary">
                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                         viewBox="0 0 24 24" height="1em" width="1em"
                                         xmlns="http://www.w3.org/2000/svg">
                                        <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                                        </path>
                                    </svg>
                                </button>
                            </div>
                              <button type="button" id="btnAddJob" class="btn btn-secondary custombtn w-auto">Add a Job in </button>
                        </div>
                      </div>
                </div>`

    $('#emptyListMessage').hide();
    $('#divEditSection div.row').append(html)


});

$(document).on('click', '#btnCancelJob', function () {
    console.log('cancel click')
    $('#divJobForm').hide();
    $('#mainDisplayPage').show();
});


function fillDropdown() {
    $("#ddlJobResponsibility").html("")
    $('#ddlJobResponsibility').append('<option value="0" selected><b>Select Responsibility</b></option>')
    $.ajax({
        url: '/WorkExperience/GetResponsibilities',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlJobResponsibility').append(`<option value="${value.jobCategoryId}"><b> ${value.jobCategoryDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}

$(document).on('change', '#ddlJobResponsibility', function () {
    let id = $(this).val();
    $.ajax({
        url: '/WorkExperience/GetResponsibilityFAQ',
        type: 'get',
        data: { jobCategoryId: id },
        success: function (response) {
            if (response.data.responsibilityOptions.length > 0) {
                $('#optionsDiv').html("");
                $.each(response.data.responsibilityOptions, function (index, value) {
                    let html = ` <div id="ooption-${index}" class="form-check">
                    <input id="${index}-option" name="${index}-option"
                           type="checkbox" class="form-check-input"><label for="${index}-option"
                                                                           class="">${value.caption}</label>
                </div>`;
                    $('#optionsDiv').append(html)

                });
            }
            if (response.data.responsibilityQuestions.length > 0) {
                $('#divQuestions').html("")
                $.each(response.data.responsibilityQuestions, function (index, value) {
                    let html = ` <div class="form-group">
                    <label for="t" class="">
                       ${value.caption}
                    </label><input name="t" id="t" type="text" class="form-control"
                                   value="">
                </div>`;

                    $('#divQuestions').append(html);
                });
            }



        },
        error: function (err) {
            alert(err)
        }
    });
})

//$(document).on('click', 'input[type="checkbox"]', function () {
    
//});

