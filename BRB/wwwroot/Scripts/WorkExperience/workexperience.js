var companyArray = [];
var jobArray = [];
let url = '/WorkExperience/GetJobResponsibility';
$(document).on('click', '#btnAddJob', function () {
    let companyId = $(this).attr("data-company-id");
    localStorage.setItem("CompanyId", companyId);
    if ($('#divJobForm').children().length > 0) {
        $('#mainDisplayPage').hide();
        $('#divJobForm').show()
    } else {
        $('#mainDisplayPage').hide();
        $('#divJobForm').load(url);
        fillDropdown();
        //setTimeout(function () { fillDropdown() }, 2000)
    }
});

$(document).ready(function () {
    $('#ddlStateAbbr').html("");
    $('#ddlStateAbbr').append('<option value="" selected><b>Select State</b></option>')
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
});


$(document).on("click", "#btnSavePosition", function () {
    let companyId = localStorage.getItem("CompanyId");
    $.ajax({
        url: '/WorkExperience/AddPosition?CompanyId=' + companyId + "&" + $('#WorkPositionForm').serialize(),
        type: 'get',
        success: function (response) {
            console.log(response);
            if (response.success) {
                $("#btnCancelJob").click();
                $('#' + response.fieldName).html(response.data);
                $('#WorkPositionForm').trigger("reset");


            }
        },
        error: function (err) {
            alert(err)
        }
    });
});
    $('#btnAddCompany').click(function () {

    $.ajax({
        url: '/WorkExperience/AddCompany?' + $('#companyForm').serialize(),
        type: 'get',
        success: function (response) {
            console.log(response);
            if (response.success) {
                $('#companiesPenal').html(response.data);
                $('#companyForm').trigger("reset");
               
            }
        },
        error: function (err) {
            alert(err)
        }
    });


    //companyArray.push(company);
    ////fill the array of position record and display the recorded data in div
    //let html = `
    //            <div class="card ml-10 col-md-10">
    //                <div class="card-body">
    //                   <div class="row">
    //                        <div class="col-md-8">
    //                            <span class="card-text">
    //                                <p></p>
    //                                <p class="text-muted"></p>
    //                            </span>
    //                        </div>
    //                        <div class="col-md-4">
    //                            <button type="button"  class="btn btn-outline-danger">
    //                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                     viewBox="0 0 24 24" height="1em" width="1em"
    //                                     xmlns="http://www.w3.org/2000/svg">
    //                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
    //                                    </path>
    //                                </svg>
    //                            </button><button type="button" id="btnEditLangauge" data-item='' class="btn btn-outline-primary">
    //                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                     viewBox="0 0 24 24" height="1em" width="1em"
    //                                     xmlns="http://www.w3.org/2000/svg">
    //                                    <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
    //                                    </path>
    //                                </svg>
    //                            </button>
    //                        </div>
    //                          <button type="button" id="btnAddJob" class="btn btn-secondary custombtn w-auto">Add a Job in </button>
    //                    </div>
    //                  </div>
    //            </div>`

    //$('#emptyListMessage').hide();
    //$('#divEditSection div.row').append(html)




      });

$(document).on('click', '#btnCancelJob', function () {
    console.log('cancel click')
    $('#divJobForm').html('');
    $('#mainDisplayPage').show();
});

$('#btnSaveOrContinue').click(function () {
    $.ajax({
        url: '/WorkExperience/UpdateExperienceMaster?isComplete=' + $('#cbIsComplete').is(":checked"),
        type: 'get',
        success: function (response) {
            if (response.success) {
                window.location.href = response.redirect;
            }
        },
        error: function (err) {
            alert(err)
        }
    });
})


function fillDropdown(val) {
    debugger;
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
                    console.log(value)
                    let html = ` <div id="ooption-${index}" class="form-check">
                    <input id="${index}-option" name="responsibilityOptions[${index}].ResponsibilityOption" value="${value.respOptionId}"
                           type="checkbox" class="form-check-input"><label for="${index}-option"
                                                                           class="">${value.caption}</label>
                </div>`;
                    $('#optionsDiv').append(html)

                });
            }
            if (response.data.responsibilityQuestions.length > 0) {
                $('#divQuestions').html("")
                $.each(response.data.responsibilityQuestions, function (index, value) {
                    console.log(value);
                    let html = ` <div class="form-group">
                    <label for="t" class="">
                       ${value.caption}
                       
                       <input type="hidden" name="workRespQuestions[${index}].RespQuestionId" value="${value.respQuestionId}"/>
                    </label><input name="workRespQuestions[${index}].Answer" id="t" type="text" class="form-control"
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

$('#btnAddAward').click(function () {
    let positionId = $(this).attr("data-position-id");
    localStorage.setItem("PositionId", positionId);
    $('#awardModal').modal('show');
  
});

//hit kr k check krna hai 
$('#btnSaveAward').click(function () {
    let positionId = localStorage.getItem("PositionId");
    console.log($('#formAward').serialize());
      $.ajax({
          url: '/WorkExperience/AddAward?' + $('#formAward').serialize() + '&CompanyJobId=' + positionId,
        type: 'get',
        success: function (response) {
            console.log(response);
            if (response.success) {
                console.log(response.fieldName)
                $('#' + response.fieldName).html(response.data);
                $('#formAward').trigger("reset");

            }
        },
        error: function (err) {
            alert(err)
        }
    });
    //'/WorkExperience/AddAward?CompanyJobId=' + positionId + "&" + $('#formAward').serialize(),
});


$(document).on("click", "button.btnEditCompany", function () {
    var json = JSON.parse($(this).attr("data-item"));
    $("input[name='CompanyId']").val(json.companyId);
    $("input[name='CompanyName']").val(json.companyName);
    $("input[name='City']").val(json.city);
    $("select[name='State']").val(json.state);
    $("select[name='StartMonth']").val(json.startMonth);
    $("select[name='StartYear']").val(json.startYear);
    $("select[name='EndMonth']").val(json.endMonth);
    $("select[name='EndYear']").val(json.endYear);
    $("#SummaryModal").modal('show');
})

$(document).on("click", "button.btnEditAward", function () {
    var json = JSON.parse($(this).attr("data-item"));
    console.log(json)
    $("input[name='JobAwardId']").val(json.jobAwardId);
    $("input[name='AwardDesc']").val(json.awardDesc);
    $("select[name='AwardedMonth']").val(json.awardedMonth);
    $("select[name='AwardedYear']").val(json.awardedYear);
    $("#hdfPositionId").val(json.companyJobId);
    $('#awardModal').modal('show');
})

$(document).on("click", "button.btnEditPosition", function () {
    $('#loader').fadeIn();
    var json = JSON.parse($(this).attr("data-item"));
    console.log(json)
    localStorage.setItem("CompanyId", json.companyId);
    //if ($('#divJobForm').children().length > 0) {
    //    $('#mainDisplayPage').hide();
    //    $('#divJobForm').show()
    //} else {
    //    $('#mainDisplayPage').hide();
    //    $('#divJobForm').load(url);
    //    fillDropdown();
    //}
    //debugger;
    $.get(url, function (response) {
        console.log(response)
        $('#mainDisplayPage').hide();
        $('#divJobForm').html(response);
            $(document).find("#ddlJobResponsibility").val(json.jobResponsibilityId);
            $(document).find("#ddlJobResponsibility").trigger("change");
      
            $("input[name='PositionId']").val(json.positionId);
      
            $(`input[name='Title']`).val(json.title);
            $(`select[name='StartMonth']`).val(json.startMonth);
            $(`select[name='StartYear']`).val(json.startYear);
            $(`select[name='EndMonth']`).val(json.endMonth);
            $(`select[name='EndYear']`).val(json.endYear);
            $(`input[name='Project1']`).val(json.project1);
            $(`input[name='Project2']`).val(json.project2);
            $(`input[name='ImproveProductivity']`).val(json.improveProductivity);
            $(`input[name='IncreaseRevenue']`).val(json.increaseRevenue);
            $(`input[name='PercentageImprovement']`).val(json.percentageImprovement);

            setTimeout(function () {
                if (json.workRespQuestions) {
                    $.each(json.workRespQuestions, function (index, value) {

                        $(`input[name='workRespQuestions[${index}].Answer']`).val(value.answer);
                    })
                }

                if (json.responsibilityOptions) {
                    $.each(json.responsibilityOptions, function (index, value) {

                        $(`input[name='responsibilityOptions[${index}].ResponsibilityOption'][value='${value.responsibilityOption}']`).prop("checked", true);
                    })
                }
                $('#loader').fadeOut(250)
            }, 1000);

      
    })
    
   
   
    
})




//$(document).on('click', 'input[type="checkbox"]', function () {
    
//});

