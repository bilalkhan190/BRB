var acadmicScholarshipArray = [];
var acadmicHonorArray = [];
var collegeArray = [];
var objArray = [];

$('#ddlDegree').change(function () {
    let input = `<input name="DegreeOther" id="DegreeOther" placeholder="Other Degree" type="text" class="form-control mt-2" value="" required/>`
    let ddlSelectedValue = $('#ddlDegree option:selected').val()
    if (ddlSelectedValue == 8) {
        $(this).after(input)

    } else {
        $('#DegreeOther').remove();
    }
})

$('#ddlMajor').change(function () {
    let input = `<input name="MajorOther" id="MajorOther" placeholder="Other Major" type="text" class="form-control mt-2" value="" required/>`
    let ddlSelectedValue = $('#ddlMajor option:selected').val()
    if (ddlSelectedValue == 160) {
        $(this).after(input)

    } else {
        $('#MajorOther').remove();
    }
})


$('#ddlMajorSpeciality').change(function () {
    let input = `<input name="MajorSpecialityOther" id="MajorSpecialityOther" placeholder="Other Specialty In Major" type="text" class="form-control mt-2" value="" required/>`
    let ddlSelectedValue = $('#ddlMajorSpeciality option:selected').val()
    if (ddlSelectedValue == 111) {
        $(this).after(input)

    } else {
        $('#MajorSpecialityOther').remove();
    }
});


$('#ddlMinor').change(function () {
    let input = `<input name="MinorOther" id="MinorOther" placeholder="Other Certificates" type="text" class="form-control mt-2" value="" required/>`
    let ddlSelectedValue = $('#ddlMinor option:selected').val()
    if (ddlSelectedValue == 160) {
        $(this).after(input)

    } else {
        $('#MinorOther').remove();
    }
});


$('#ddlOtherCertificates').change(function () {
    let input = `<input name="OtherCertificates" id="OtherCertificates" placeholder="Other Certificates" type="text" class="form-control mt-2" value="" required/>`
    let ddlSelectedValue = $('#ddlOtherCertificates option:selected').val()
    if (ddlSelectedValue == 47) {
        $(this).after(input)

    } else {
        $('#OtherCertificates').remove();
    }
});
$('.modalClose').click(function () {
    FormReset();
})

$('#btnCollageSave').click(function () {
    $('#CollageForm').validate();
    if ($('#CollageForm').valid()) {
        let college = {
            LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(), CollegeId: $('#hdfCollegeId').val(),
            CollegeName: $('#txtCollegeName').val(), CollegeCity: $('#CollegeCity').val(),
            CollegeStateAbbr: $('#ddlState').val(), GradDate: $('#txtGradDate').val(),
            SchoolName: $('#txtSchoolName').val(), DegreeId: $('#ddlDegree').val(), DegreeOther: $('#DegreeOther').val()
            , MajorId: $('#ddlMajor').val(), MajorOther: $('#MajorOther').val(), MajorSpecialtyId: $('#ddlMajorSpeciality').val(),
            MajorSpecialtyOther: $('#MajorSpecialityOther').val(), MinorId: $('#ddlMinor').val(), MinorOther: $('#MinorOther').val(),
            CertificateId: $('#ddlOtherCertificates').val(), CertificateOther: $('#OtherCertificates').val(), HonorProgram: $('#HonorProgram').val(),
            Gpa: $('#Gpa').val(), IncludeGpa: $('#cbkIncludeGpa').is(':checked'), IsComplete: $('#cbkIsComplete').is(":checked")
        }
        if (localStorage.getItem("col-index") == null) {
            collegeArray.push(college);
        }
        else {
            collegeArray[parseInt(localStorage.getItem("col-index"))] = college;
            localStorage.clear();
        }
      
        LoadCards();
    }
      
});

function LoadCards() {
    $('#divEditSection').html("")
    collegeArray = covertArrayKeyIntoCamelCase(collegeArray)
    $.each(collegeArray, function (index, value) {
        let html = `<div class="card ml-4 mt-4 mb-4 cardWrapper"> 
                                                           <div class="card-body">
                                                    <div class="row"> 
                                                    <div class="col-md-12">
                                               <span class="card-text">
                                               <div class="row">
                                               <div class="col-md-6">
                                                    <h5 class="title-text">${value.collegeName}</h5>
                                                     <p class="text-muted">${value.collegeCity}</p>
                                                    <p class="text-muted">GPA: ${value.gpa}</p>
                                                    </div>
                                                    <div class="col-md-6">
                                                    <div class="card-Btn">
                                                        <button type="button" id="btnDeleteCollege" data-item='${value.collegeId}' data-edit=${index} class="btn custombtn w-auto ms-2">
                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                         viewBox="0 0 24 24" height="1em" width="1em"
                                                         xmlns="http://www.w3.org/2000/svg">
                                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                      </path>
                                                  </svg>
                                              </button><button type="button" id="btnEditCollege" data-item='${value.collegeId}' data-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
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
                                                   
                                                    <h5>Academic Honors</h5>
                                                    <p class="ErrMessage">
                                                       <em>
                                                            You currently have no Academic Honors listed. Click the Add
                                                            button below to add one.
                                                        </em>
                                                    </p>
                                                       <div id="DivAcademicHonor">
                           
                                                       </div>

                                                    <button type="button"
                                                                class="btn btn-primary custombtn w-auto mt-2 mb-2" data-bs-toggle="modal"
                                                               data-bs-target="#HonorModal">
                                                        Add an Academic Honor
                                                    </button>
                                                    <h5 class="mt-2">Scholarships</h5>
                                                   <p class="ErrMessageScholarship">
                                                        <em>
                                                            You currently have no Academic Scholarships listed. Click the
                                                          Add button below to add one.
                                                      </em>
                                                   </p>
                                                     <div id="divAcademicScholarship">
                           
                                                     </div>
                                                        <button type="button"
                                                               class="btn btn-primary custombtn w-auto mt-2" data-bs-toggle="modal"
                                                               data-bs-target="#ScholarshipModal">
                                                       Add an Academic
                                                        Scholarship
                                                   </button>
                                               </span>
                                           </div>
                                           
                                        </div>
                                        </div>
                                </div>`
                     
        $('#divEditSection').append(html);
        $('#collegeErrMessage').hide();
    })
  
    $('#DivAcademicHonor').html("")
    acadmicHonorArray = covertArrayKeyIntoCamelCase(acadmicHonorArray)
    $.each(acadmicHonorArray, function (index, value) {
        let html = ` <div class="card mb-3"> 
                                                        <div class="card-body">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                            <span class="card-text">
                                                                <p>${value.honorName}</p>
                                                                <p class="text-muted"> ${value.honorMonth} ${value.honorYear}</p>
                                                            </span>
                                                        </div>
                                                        <div class="col-md-2">
                                                        <div class="card-Btn">
                                                            <button type="button" id="btnDeleteHonor" data-item='${value.academicHonorId}' acad-edit=${index} class="btn custombtn w-auto ms-2">
                                                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                     viewBox="0 0 24 24" height="1em" width="1em"
                                                                     xmlns="http://www.w3.org/2000/svg">
                                                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                    </path>
                                                                </svg>
                                                            </button><button type="button" id="btnEditHonor" data-item='${value.academicHonorId}' acad-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
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
                                                 </div>`
        $('#DivAcademicHonor').append(html)
    });
    if (acadmicHonorArray != null && acadmicHonorArray.length > 3) {
        $("#DivAcademicHonor").addClass("BoxHeight");
    }
    $('#divAcademicScholarship').html("")
    acadmicScholarshipArray = covertArrayKeyIntoCamelCase(acadmicScholarshipArray)
    $.each(acadmicScholarshipArray, function (index, value) {

        let html = ` <div class="card mb-3"> 
                                                        <div class="card-body">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                            <span class="card-text">
                                                                <p>${value.scholarshipName}</p>
                                                                <p class="text-muted"> ${value.scholarshipMonth} ${value.scholarshipYear}</p>
                                                            </span>
                                                        </div>
                                                        <div class="col-md-2">
                                                        <div class="card-Btn">
                                                            <button type="button" id="btnDeleteAcademicScholarship"  data-item='${value.academicScholarshipId}' sch-edit=${index}  class="btn custombtn w-auto ms-2">
                                                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                     viewBox="0 0 24 24" height="1em" width="1em"
                                                                     xmlns="http://www.w3.org/2000/svg">
                                                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                    </path>
                                                                </svg>
                                                            </button><button type="button" id="btnEditAcademicScholarship" data-item='${value.academicScholarshipId}' sch-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
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
        $('#divAcademicScholarship').append(html)
    });  
    if (acadmicScholarshipArray != null && acadmicScholarshipArray.length > 3) {
        $("#divAcademicScholarship").addClass("BoxHeight");
    }
    
}
    




$(document).ready(function () {
    FillDropdowns();
    $.ajax({
        url: '/Education/GetData',
        type: 'Get',
        success: function (response) {
          
            for (var i = 0; i < response.data.length; i++) {
                collegeArray.push(response.data[i].college)
                $.each(response.data[i].academicHonors, function (index, value) {
                    acadmicHonorArray.push(value)
                });
                $.each(response.data[i].academicScholarships, function (index, value) {
                    acadmicScholarshipArray.push(value)
                });
            }
           
          
            LoadCards();

        }, error: function (err) {
            alert('error')
        }
    })

});


$(document).on('click', '#btnEditCollege', function () {
    var response = collegeArray[$(this).attr('data-edit')];
    console.log(response)
    localStorage.setItem("col-index", $(this).attr('data-edit'))
    let today = new Date(response.gradDate).toISOString().split('T')[0];
    $('input[name="CollegeId"]').val(response.collegeId)
    $('input[name="CollegeName"]').val(response.collegeName)
    $('input[name="CollegeCity"]').val(response.collegeCity)
    $('input[name="GradDate"]').val(today)
    $('input[name="HonorProgram"]').val(response.honorProgram)
    $('select[name="CollegeStateAbbr"]').val(response.collegeStateAbbr)
    $('input[name="SchoolName"]').val(response.schoolName)
    $('input[name="DegreeId"]').val(response.degreeId)
    $('input[name="MajorId"]').val(response.majorId)
    $('select[name="MajorSpecialtyId"]').val(response.majorSpecialtyId)
    $('select[name="MinorId"]').val(response.minorId)
    if (response.includeGpa != null || response.includeGpa == false) {
        $('input[name="IncludeGpa"]').prop('checked', true);
    }
    $('select[name="CertificateId"]').val(response.certificateId)
    $('input[name="Gpa"]').val(response.gpa)
    $('.collegeModal').modal('show')
});
function arrayLookup(array, prop, val) {
    console.log(array)
    for (var i = 0, len = array.length; i < len; i++) {
        if (array[i].hasOwnProperty(prop) && array[i][prop] === val) {
            return array[i];
        }
    }
    return null;
}

$('#btnSaveAndContinue').click(function () {
    let obj = {
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        IsComplete: $('#cbkIsComplete').is(':checked'),
        College: collegeArray,
        AcademicHonors: acadmicHonorArray,
        AcademicScholarships: acadmicScholarshipArray
    };


    $.ajax({
        url: '/Education/PostEducationData',
        type: 'POST',
        data: { educationViewModel: obj },
        success: function (response) {
            FormReset();
            LoadCards();
            window.location.href = response.redirect;
        },
        error: function (err) {
            alert('error')
        }
    });
})

function FillDropdowns() {
    $('#ddlDegree').html("");
    $('#ddlDegree').append('<option value="0" selected><b>Select Degree</b></option>')
    $.ajax({
        url: '/Education/GetAllDegrees',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlDegree').append(`<option value="${value.degreeId}"><b> ${value.degreeDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlMajor').html("");
    $('#ddlMajor').append('<option value="0" selected><b>Select Major</b></option>')
    $.ajax({
        url: '/Education/GetAllMajor',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlMajor').append(`<option value="${value.majorId}"><b> ${value.majorDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlMajorSpeciality').html("");
    $('#ddlMajorSpeciality').append('<option value="0" selected><b>Select Major specilties</b></option>')
    $.ajax({
        url: '/Education/GetAllMajorSpecilties',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlMajorSpeciality').append(`<option value="${value.majorSpecialtyId}"><b> ${value.majorSpecialtyDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlMinor').html("");
    $('#ddlMinor').append('<option value="0" selected><b>Select Minors</b></option>')
    $.ajax({
        url: '/Education/GetAllMinors',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlMinor').append(`<option value="${value.minorId}"><b> ${value.minorDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlState').html("");
    $('#ddlState').append('<option value="0" selected><b>Select States</b></option>')
    $.ajax({
        url: '/Education/GetAllStates',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlState').append(`<option value="${value.stateAbbr}"><b> ${value.stateName} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlOtherCertificates').html("");
    $('#ddlOtherCertificates').append('<option value="0" selected><b>Select Certificates</b></option>')
    $.ajax({
        url: '/Education/GetAllCertificates',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlOtherCertificates').append(`<option value="${value.certificateId}"><b> ${value.certificateDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}


$('#btnSaveHonor').click(function () {
    let acadmicHonor = {
        CollegeId: $('#hdfCollegeId').val(),
        AcademicHonorId: $('#hdfAcademicHonorId').val(),
        HonorName: $('#txtAcademicHonor').val(),
        HonorMonth: $('#txtStartedMonth').val(),
        HonorYear: $('#txtStartedYear').val()
    };
    if (localStorage.getItem("acad-index") == null) {
        acadmicHonorArray.push(acadmicHonor);
    }
    else {
        acadmicHonorArray[parseInt(localStorage.getItem("acad-index"))] = acadmicHonor;
        localStorage.clear();
    }
    
    LoadCards();
    ResetHonor();
   
});

$('#btnSaveScholarship').click(function () {
    let acadmicScholarship = {
        CollegeId: $('#hdfCollegeId').val(),
        AcademicScholarshipId: $('#hdfAcademicScholarshipId').val(),
        ScholarshipName: $('#txtScholarshipName').val(),
        ScholarshipCriteria: $('#txtScholarshipCriteria').val(),
        ScholarshipMonth: $('#txtScholarshipStartedMonth').val(),
        ScholarshipYear: $('#txtScholarshipStartedYear').val()
    };
    if (localStorage.getItem("sch-index") == null) {
        acadmicScholarshipArray.push(acadmicScholarship);
    }
    else {
        acadmicScholarshipArray[parseInt(localStorage.getItem("sch-index"))] = acadmicScholarship;
        localStorage.clear();
    }
   LoadCards()
    ResetScholarship();
 
});


$(document).on('click', '#btnEditHonor', function () {
    var response = acadmicHonorArray[$(this).attr('acad-edit')];
    localStorage.setItem("acad-index", $(this).attr('acad-edit'))
    $('#hdfAcademicHonorId').val(response.academicHonorId);
    $('#txtAcademicHonor').val(response.honorName);
    $('#txtStartedMonth').val(response.honorMonth);
    $('#txtStartedYear').val(response.honorYear);
    $('#HonorModal').modal('show');
});

$(document).on('click', '#btnEditAcademicScholarship', function () {
    var response = acadmicScholarshipArray[$(this).attr('sch-edit')];
    localStorage.setItem("sch-index", $(this).attr('sch-edit'))
    $('#hdfAcademicScholarshipId').val(response.academicScholarshipId);
    $('#txtScholarshipName').val(response.scholarshipName);
    $('#txtScholarshipCriteria').val(response.scholarshipCriteria);
    $('#txtScholarshipStartedMonth').val(response.scholarshipMonth);
    $('#txtScholarshipStartedYear').val(response.scholarshipYear);
    $('#ScholarshipModal').modal('show');
});


function ResetHonor() {
    $('#hdfAcademicHonorId').val("")
    $('#txtAcaddemicHonor').val("")
    $('#txtAcademicHonor').val("");
    $('#txtStartedMonth').val("");
    $('#txtStartedYear').val("");
}

function FormReset() {
    $('input[type="text"]').val("");
    $('#hdfCollegeId').val("");
    $('select').val(0);
    $('input[type="date"]').val(new Date())
}


function ResetScholarship() {
    $('#hdfAcademicScholarshipId').val("");
    $('#txtScholarshipName').val("");
    $('#txtScholarshipCriteria').val("");
    $('#txtScholarshipStartedMonth').val("");
    $('#txtScholarshipStartedYear').val("")
}

$(document).on('click', '#btnDeleteCollege', function () {

    localStorage.setItem("col-index", $(this).attr('data-edit'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/Education/DeleteCollege?id=' + id,
        type: 'post',
        success: function (response) {
            console.log(collegeArray)
            let index = parseInt(localStorage.getItem("col-index"));
            collegeArray.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    })
});

$(document).on('click', '#btnDeleteHonor', function () {

    localStorage.setItem("acad-index", $(this).attr('acad-edit'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/Education/DeleteCollege?academicId=' + id,
        type: 'post',
        success: function (response) {
            console.log(acadmicHonorArray)
            let index = parseInt(localStorage.getItem("acad-index"));
            acadmicHonorArray.splice(index, 1);
            console.log(acadmicHonorArray)
            LoadCards();
        },
        error: function (err) {

        }
    });
});

$(document).on('click', '#btnDeleteAcademicScholarship', function () {

    localStorage.setItem("sch-index", $(this).attr('sch-edit'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/Education/DeleteCollege?scholarshipid=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("sch-index"));
            acadmicScholarshipArray.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    });
});