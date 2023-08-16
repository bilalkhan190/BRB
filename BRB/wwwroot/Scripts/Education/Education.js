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
    let input = `<input name="MinorOther" id="MinorOther" placeholder="Other Minor" type="text" class="form-control mt-2" value="" required/>`
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
    $('#btnCollageSave').prop('disabled', false)
    $('#btnSaveScholarship').prop('disabled', false)
    $('#btnSaveHonor').prop('disabled', false);
    $(document).find('input[name="OtherCertificates"]').remove();
    $(document).find('input[name="DegreeOther"]').remove();
    $(document).find('input[name="MajorOther"]').remove();
    $(document).find('input[name="MinorOther"]').remove();
    $(document).find('input[name="MajorSpecialityOther"]').remove();
    
    FormReset();
})

$('#btnCollageSave').click(function () {
   
    $('#CollageForm').validate();
    if ($('#CollageForm').valid()) {
        $('#btnCollageSave').prop('disabled', true)
        let college = {
            lastSectionVisitedId: $('#hdfLastSectionVisitedId').val(), collegeId: $('#hdfCollegeId').val(),
            collegeName: $('#txtCollegeName').val(),collegeCity: $('#CollegeCity').val(),
            collegeStateAbbr: $('#ddlState').val(), gradDate: $('#txtGradDate').val(),
            schoolName: $('#txtSchoolName').val(), degreeId: $('#ddlDegree').val(), degreeOther: $('#DegreeOther').val()
            , majorId: $('#ddlMajor').val(), majorOther: $('#MajorOther').val(), majorSpecialtyId: $('#ddlMajorSpeciality').val(),
            majorSpecialtyOther: $('#MajorSpecialityOther').val(), minorId: $('#ddlMinor').val(), minorOther: $('#MinorOther').val(),
            certificateId: $('#ddlOtherCertificates').val(), certificateOther: $('#OtherCertificates').val(), honorProgram: $('#HonorProgram').val(),
            gpa: $('#Gpa').val(), includeGpa: $('#cbkIncludeGpa').is(':checked'), isComplete: $('#cbkIsComplete').is(":checked"),
            month: $("#Month").val(), year: $("#Year").val()
        }
        if (localStorage.getItem("edit-college") === null) {
            collegeArray.push(college)
            collegeArray = covertArrayKeyIntoCamelCase(collegeArray)
            $('#CollageForm').trigger('reset');
            let html = `<div class="card ml-4 mt-4 cardWrapper crd-wrp-olg"> 
                                                           <div class="card-body">
                                                    <div class="row"> 
                                                    <div class="col-md-12">
                                               <span class="card-text">
                                               <div class="row">
                                               <div class="col-md-6">
                                                    <h5 class="title-text">${college.collegeName}</h5>
                                                     <p class="text-muted text-city">${college.collegeCity}</p>
                                                    <p class="text-muted text-gpa">GPA: ${college.gpa}</p>
                                                    </div>
                                                    <div class="col-md-6">
                                                    <div class="card-Btn">
                                                        <button type="button" id="btnDeleteCollege"  data-item='${college.collegeId}' class="btn custombtn w-auto ms-2 btn-outline-danger">
                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                         viewBox="0 0 24 24" height="1em" width="1em"
                                                         xmlns="http://www.w3.org/2000/svg">
                                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                      </path>
                                                  </svg>
                                              </button><button type="button" data-identity='${guid()}' data-json='${JSON.stringify(college)}' data-item='${collegeArray.length - 1}' class=" btnEditCollege btn custombtn customBtn-light w-auto ms-1">
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
                                                                class="btn btn-primary custombtn w-auto mt-2 mb-2" onclick='clearStorage(this);showModal("#HonorModal")' data-item='${guid()}'>
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
                                                               class="btn btn-primary custombtn w-auto mt-2" onclick="clearStorage(this, 'schol');$('#ScholarshipModal').modal('toggle');" data-item='${guid()}'>
                                                       Add an Academic
                                                        Scholarship
                                                   </button>
                                               </span>
                                           </div>
                                           
                                        </div>
                                        </div>
                                </div>`
            $('#divEditSection').append(html);
        }
        else {
            debugger;
            let index = localStorage.getItem("edit-college");
            collegeArray[index] = college;
            $($(".crd-wrp-olg")[index]).find(".title-text").html(college.collegeName)
            $($(".crd-wrp-olg")[index]).find(".text-city").html(college.collegeCity)
            $($(".crd-wrp-olg")[index]).find(".text-gpa").html("GPA: " + college.gpa)
            $($(".crd-wrp-olg")[index]).find(".btnEditCollege").attr("data-json", JSON.stringify(college));
            localStorage.clear();
        }
        $('#SummaryModal').modal('toggle');
        //LoadCards();
        $('#btnCollageSave').prop('disabled', false)
        $('#CollageForm').trigger('reset');
        $('#collegeErrMessage').hide();

    }
      
});
$('#Gpa').on('change', function () {
    this.value = parseFloat(this.value).toFixed(2);
});
function LoadCards() {
    $('#divEditSection').html("")
    console.log(collegeArray);
    collegeArray = covertArrayKeyIntoCamelCase(collegeArray)
    console.log(collegeArray);
    $.each(collegeArray, function (index, value) {
        console.log(value)
        let honorHtml = "";
        $.each(value.academicHonors, function (indexc, honor) {
            honorHtml += ` <div class="card mb-3 cardWrapper cardWrapper-hnr"> 
                                                        <div class="card-body">
                                                            <div class="row">
                                                                <div class="col-md-10">
                                                            <span class="card-text">
                                                                <p>${honor.honorName}</p>
                                                                <p class="text-muted"> ${honor.honorMonth} ${honor.honorYear}</p>
                                                            </span>
                                                        </div>
                                                        <div class="col-md-2">
                                                        <div class="card-Btn">
                                                            <button type="button" id="btnDeleteHonor" data-item='${honor.academicHonorId}' acad-edit="${indexc}" data-parent="${index}" class="btn custombtn w-auto ms-2 btn-outline-danger">
                                                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                     viewBox="0 0 24 24" height="1em" width="1em"
                                                                     xmlns="http://www.w3.org/2000/svg">
                                                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                    </path>
                                                                </svg>
                                                            </button><button type="button" data-identity='${guid()}' data-json='${JSON.stringify(honor)}' data-item="${honor.academicHonorId}" class="btn custombtn customBtn-light w-auto ms-1 btnEditHonor">
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
                                                     <input type='hidden' class='HonorName' value="${honor.honorName}">
                                                     <input type='hidden' class='HonorMonth' value="${honor.honorMonth}">
                                                     <input type='hidden' class='HonorYear' value="${honor.honorYear}">
                                                     <input type='hidden' class='AcademicHonorId' value="${honor.academicHonorId}">
                                                 </div>`;
        });

        let scholHtml = "";
        $.each(value.academicScholarships, function (indexc, schol) {
            scholHtml += ` <div class="card mb-3 cardWrapper-schols"> 
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-10">
                    <span class="card-text">
                        <p>${schol.scholarshipName}</p>
                        <p class="text-muted"> ${schol.scholarshipMonth} ${schol.scholarshipYear}</p>
                    </span>
                </div>
                <div class="col-md-2">
                <div class="card-Btn">
                    <button type="button" id="btnDeleteAcademicScholarship"  data-item='${schol.academicScholarshipId}' sch-edit="${indexc}" sch-parent="${index}" class="btn custombtn w-auto ms-2 btn-outline-danger">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                            </path>
                        </svg>
                    </button><button type="button" data-item='${schol.academicScholarshipId}' data-json='${JSON.stringify(schol)}' data-item="${schol.academicScholarshipId}" class="btn custombtn customBtn-light w-auto ms-1 btnEditAcademicScholarship">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                            </path>
                        </svg>
                    </button>
                        </div>
                    </div>
                    <input type="hidden" class="AcademicScholarshipId" value="${schol.academicScholarshipId}">
                    <input type="hidden" class="ScholarshipName" value="${schol.scholarshipName}">
                    <input type="hidden" class="ScholarshipCriteria" value="${schol.scholarshipCriteria}">
                    <input type="hidden" class="ScholarshipMonth" value="${schol.scholarshipMonth}">
                    <input type="hidden" class="ScholarshipYear" value="${schol.scholarshipYear}">
                </div>
                       </div>`;
        })


        let html = `<div class="card ml-4 mt-4 cardWrapper crd-wrp-olg"> 
                                                           <div class="card-body">
                                                    <div class="row"> 
                                                    <div class="col-md-12">
                                               <span class="card-text">
                                               <div class="row">
                                               <div class="col-md-6">
                                                    <h5 class="title-text">${value.collegeName}</h5>
                                                     <p class="text-muted text-city">${value.collegeCity}</p>
                                                    <p class="text-muted text-gpa">GPA: ${value.gpa}</p>
                                                    </div>
                                                    <div class="col-md-6">
                                                    <div class="card-Btn">
                                                        <button type="button" id="btnDeleteCollege" data-item='${value.collegeId}' data-edit=${index} class="btn custombtn w-auto ms-2 btn-outline-danger">
                                                    <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                         viewBox="0 0 24 24" height="1em" width="1em"
                                                         xmlns="http://www.w3.org/2000/svg">
                                                        <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                      </path>
                                                  </svg>
                                              </button><button type="button" data-identity='${guid()}' data-json='${JSON.stringify(value)}' data-item='${index}' class=" btnEditCollege btn custombtn customBtn-light w-auto ms-1">
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
                                                       ${honorHtml}
                                                    <button type="button"
                                                                class="btn btn-primary custombtn w-auto mt-2 mb-2" onclick='clearStorage(this);showModal("#HonorModal")' data-item='${guid()}'>
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
                                                     ${scholHtml}
                                                        <button type="button"
                                                               class="btn btn-primary custombtn w-auto mt-2" onclick="clearStorage(this, 'schol');$('#ScholarshipModal').modal('toggle');" data-item='${guid()}'>
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
  
    //$('#DivAcademicHonor').html("")
    //acadmicHonorArray = covertArrayKeyIntoCamelCase(acadmicHonorArray)
    //$.each(acadmicHonorArray, function (index, value) {
    //    let html = ` <div class="card mb-3"> 
    //                                                    <div class="card-body">
    //                                                        <div class="row">
    //                                                            <div class="col-md-10">
    //                                                        <span class="card-text">
    //                                                            <p>${value.honorName}</p>
    //                                                            <p class="text-muted"> ${value.honorMonth} ${value.honorYear}</p>
    //                                                        </span>
    //                                                    </div>
    //                                                    <div class="col-md-2">
    //                                                    <div class="card-Btn">
    //                                                        <button type="button" id="btnDeleteHonor" data-item='${value.academicHonorId}' acad-edit=${index} class="btn custombtn w-auto ms-2">
    //                                                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                                                 viewBox="0 0 24 24" height="1em" width="1em"
    //                                                                 xmlns="http://www.w3.org/2000/svg">
    //                                                                <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
    //                                                                </path>
    //                                                            </svg>
    //                                                        </button><button type="button" id="btnEditHonor" data-item='${value.academicHonorId}' acad-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
    //                                                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                                                 viewBox="0 0 24 24" height="1em" width="1em"
    //                                                                 xmlns="http://www.w3.org/2000/svg">
    //                                                                <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
    //                                                                </path>
    //                                                            </svg>
    //                                                        </button>
    //                                                        </div>
    //                                                    </div>
    //                                                     </div>
    //                                                 </div>
    //                                             </div>`
    //    $('#DivAcademicHonor').append(html)
    //});
    //if (acadmicHonorArray != null && acadmicHonorArray.length > 3) {
    //    $("#DivAcademicHonor").addClass("BoxHeight");
    //}
    //$('#divAcademicScholarship').html("")
    //acadmicScholarshipArray = covertArrayKeyIntoCamelCase(acadmicScholarshipArray)
    //$.each(acadmicScholarshipArray, function (index, value) {

    //    let html = ` <div class="card mb-3"> 
    //                                                    <div class="card-body">
    //                                                        <div class="row">
    //                                                            <div class="col-md-10">
    //                                                        <span class="card-text">
    //                                                            <p>${value.scholarshipName}</p>
    //                                                            <p class="text-muted"> ${value.scholarshipMonth} ${value.scholarshipYear}</p>
    //                                                        </span>
    //                                                    </div>
    //                                                    <div class="col-md-2">
    //                                                    <div class="card-Btn">
    //                                                        <button type="button" id="btnDeleteAcademicScholarship"  data-item='${value.academicScholarshipId}' sch-edit=${index}  class="btn custombtn w-auto ms-2">
    //                                                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                                                 viewBox="0 0 24 24" height="1em" width="1em"
    //                                                                 xmlns="http://www.w3.org/2000/svg">
    //                                                                <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
    //                                                                </path>
    //                                                            </svg>
    //                                                        </button><button type="button" id="btnEditAcademicScholarship" data-item='${value.academicScholarshipId}' sch-edit=${index} class="btn custombtn customBtn-light w-auto ms-1">
    //                                                            <svg stroke="currentColor" fill="currentColor" stroke-width="0"
    //                                                                 viewBox="0 0 24 24" height="1em" width="1em"
    //                                                                 xmlns="http://www.w3.org/2000/svg">
    //                                                                <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
    //                                                                </path>
    //                                                            </svg>
    //                                                        </button>
    //                                                         </div>
    //                                                     </div>
    //                                                 </div>
    //                                             </div>`
    //    $('#divAcademicScholarship').append(html)
    //});  
    //if (acadmicScholarshipArray != null && acadmicScholarshipArray.length > 3) {
    //    $("#divAcademicScholarship").addClass("BoxHeight");
    //}
    
}
    


$(document).on("click", "button.btnEditHonor", function () {
    debugger;
    let index = $(this).attr("data-identity");
    let editRecord = JSON.parse($(this).attr("data-json"));
    console.log(editRecord)

    $('#hdfAcademicHonorId').val(editRecord.academicHonorId);
    $('#txtAcademicHonor').val(editRecord.honorName);
    $('#txtStartedMonth').val(editRecord.honorMonth);
    $('#txtStartedYear').val(editRecord.honorYear);
    localStorage.setItem("edit-honor", index);
    $("#HonorModal").modal("show");
});

$(document).on("click", "button.btnEditAcademicScholarship", function () {
    let index = $(this).attr("data-identity");
    let editRecord = JSON.parse($(this).attr("data-json"));
    console.log(editRecord)

    $('#hdfAcademicScholarshipId').val(editRecord.academicScholarshipId);
    $('#txtScholarshipName').val(editRecord.scholarshipName);
    $('#txtScholarshipStartedMonth').val(editRecord.scholarshipMonth);
    $('#txtScholarshipStartedYear').val(editRecord.scholarshipYear);
    $('#txtScholarshipCriteria').val(editRecord.scholarshipCriteria);
    localStorage.setItem("edit-scholarship", index);
    $("#ScholarshipModal").modal("show");
});


$('#btnSaveHonor').click(function () {

    $('#formAcademicHonor').validate();
    if ($('#formAcademicHonor').valid()) {
        $('#btnSaveHonor').prop('disabled', true)
        let value = {
            collegeId: $('#hdfCollegeId').val(),
            academicHonorId: $('#hdfAcademicHonorId').val(),
            honorName: $('#txtAcademicHonor').val(),
            honorMonth: $('#txtStartedMonth').val(),
            honorYear: $('#txtStartedYear').val()
        };


        if (localStorage.getItem("edit-honor")) {
            let html = ` 
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
                                                            <button type="button" id="btnDeleteHonor" data-item='${value.academicHonorId}'  class="btn custombtn w-auto ms-2">
                                                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                     viewBox="0 0 24 24" height="1em" width="1em"
                                                                     xmlns="http://www.w3.org/2000/svg">
                                                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                    </path>
                                                                </svg>
                                                            </button><button type="button" id="btnEditHonor" data-item='${value.academicHonorId}' data-json='${JSON.stringify(value)}' data-identity="${guid()}" class="btn custombtn customBtn-light w-auto ms-1">
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
                                                          <input type='hidden' class='HonorName' value="${value.honorName}">
                                                     <input type='hidden' class='HonorMonth' value="${value.honorMonth}">
                                                     <input type='hidden' class='HonorYear' value="${value.honorYear}">
                                                     <input type='hidden' class='AcademicHonorId' value="${value.academicHonorId}">
                                                     </div>
                                                     
                                                 `
            let id = localStorage.getItem("edit-honor");
            $("button[data-identity='" + id + "']").closest(".cardWrapper-hnr").html(html);
            localStorage.clear();
        }
        else {
            $('#positionForm').trigger('reset');

            let html = ` <div class="card mb-3 cardWrapper cardWrapper-hnr"> 
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
                                                            <button type="button" id="btnDeleteHonor" data-item='${value.academicHonorId}' class="btn custombtn w-auto ms-2">
                                                                <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                                                     viewBox="0 0 24 24" height="1em" width="1em"
                                                                     xmlns="http://www.w3.org/2000/svg">
                                                                    <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                                                                    </path>
                                                                </svg>
                                                            </button><button type="button" data-identity='${guid()}' data-json='${JSON.stringify(value)}' data-item="${value.academicHonorId}" class="btn custombtn customBtn-light w-auto ms-1 btnEditHonor">
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
                                                               <input type='hidden' class='HonorName' value="${value.honorName}">
                                                     <input type='hidden' class='HonorMonth' value="${value.honorMonth}">
                                                     <input type='hidden' class='HonorYear' value="${value.honorYear}">
                                                     <input type='hidden' class='AcademicHonorId' value="${value.academicHonorId}">
                                                     </div>
                                               
                                                 </div>`
            let id = localStorage.getItem("add-honor");
            $("button[data-item='" + id + "']").before(html);
            //$('#noPosition').parent().append(html);
        }

        hideModal("#HonorModal");
        ResetHonor();
        $('#btnSaveHonor').prop('disabled', false)


    }
});


$('#btnSaveScholarship').click(function () {

    $('#formAcademicScholarship').validate();
    if ($('#formAcademicScholarship').valid()) {
        $('#btnSaveScholarship').prop('disabled', true)
        let value = {
            collegeId: $('#hdfCollegeId').val(),
            academicScholarshipId: $('#hdfAcademicScholarshipId').val(),
            scholarshipName: $('#txtScholarshipName').val(),
            scholarshipCriteria: $('#txtScholarshipCriteria').val(),
            scholarshipMonth: $('#txtScholarshipStartedMonth').val(),
            scholarshipYear: $('#txtScholarshipStartedYear').val()
        };

        if (localStorage.getItem("edit-scholarship")) {
            let html = `
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
                    <button type="button" id="btnDeleteAcademicScholarship"  data-item='${value.academicScholarshipId}' class="btn custombtn w-auto ms-2">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                            </path>
                        </svg>
                    </button><button type="button" id="btnEditAcademicScholarship" data-item='${value.academicScholarshipId}' data-json='${JSON.stringify(value)}' data-identity="${guid()}" class="btn custombtn customBtn-light w-auto ms-1">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                            </path>
                        </svg>
                    </button>
                        </div>
                    </div>
                    <input type="hidden" class="AcademicScholarshipId" value="${value.academicScholarshipId}">
                    <input type="hidden" class="ScholarshipName" value="${value.scholarshipName}">
                    <input type="hidden" class="ScholarshipCriteria" value="${value.scholarshipCriteria}">
                    <input type="hidden" class="ScholarshipMonth" value="${value.scholarshipMonth}">
                    <input type="hidden" class="ScholarshipYear" value="${value.scholarshipYear}">
                </div>
                      `
            let id = localStorage.getItem("edit-scholarship");
            $("button[data-identity='" + id + "']").closest(".cardWrapper-schols").html(html);
            localStorage.clear();
        }
        else {
            $('#formAcademicScholarship').trigger('reset');

            let html = ` <div class="card mb-3 cardWrapper-schols"> 
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
                    <button type="button" id="btnDeleteAcademicScholarship"  data-item='${value.academicScholarshipId}' class="btn custombtn w-auto ms-2">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M6 19c0 1.1.9 2 2 2h8c1.1 0 2-.9 2-2V7H6v12zM19 4h-3.5l-1-1h-5l-1 1H5v2h14V4z">
                            </path>
                        </svg>
                    </button><button type="button" data-item='${value.academicScholarshipId}' data-json='${JSON.stringify(value)}' data-identity="${guid()}" class="btn btnEditAcademicScholarship custombtn customBtn-light w-auto ms-1">
                        <svg stroke="currentColor" fill="currentColor" stroke-width="0"
                                viewBox="0 0 24 24" height="1em" width="1em"
                                xmlns="http://www.w3.org/2000/svg">
                            <path d="M3 17.25V21h3.75L17.81 9.94l-3.75-3.75L3 17.25zM20.71 7.04c.39-.39.39-1.02 0-1.41l-2.34-2.34c-.39-.39-1.02-.39-1.41 0l-1.83 1.83 3.75 3.75 1.83-1.83z">
                            </path>
                        </svg>
                    </button>
                        </div>
                    </div>
                    <input type="hidden" class="AcademicScholarshipId" value="${value.academicScholarshipId}">
                    <input type="hidden" class="ScholarshipName" value="${value.scholarshipName}">
                    <input type="hidden" class="ScholarshipCriteria" value="${value.scholarshipCriteria}">
                    <input type="hidden" class="ScholarshipMonth" value="${value.scholarshipMonth}">
                    <input type="hidden" class="ScholarshipYear" value="${value.scholarshipYear}">
                </div>
                       </div>`
            let id = localStorage.getItem("add-schol");
            $("button[data-item='" + id + "']").before(html);
            //$('#noPosition').parent().append(html);
        }

        $("#ScholarshipModal").modal('toggle');
        ResetScholarship();
        $('#btnSaveScholarship').prop('disabled', false)
    }
});
$(document).ready(function () {
    FillDropdowns();
    $.ajax({
        url: '/Education/GetData',
        type: 'Get',
        success: function (response) {
            console.log(response)
          
           
            if (response.data.length > 0) {
                $('#cbkIsComplete').prop('checked', response.data[0].education.isComplete)
                $('#cbkIsComplete').prop('disabled', response.data[0].education.isComplete)
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
            } else if (response.data != null)
            {
                $('#cbkIsComplete').prop('checked', response.data.isComplete)
                $('#cbkIsComplete').prop('disabled', response.data.isComplete)
            }
            

        }, error: function (err) {
            alert('error')
        }
    })

});


function getMonth(index) {
    var mL = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return mL[index];
}
$(document).on('click', '.btnEditCollege', function () {
    $('#btnCollageSave').prop('disabled', false)
    var response = JSON.parse($(this).attr("data-json"));
    console.log(response)
    console.log(response.gradDate)
    localStorage.setItem("col-index", $(this).attr('data-identity'))
    localStorage.setItem("edit-college", $(this).attr('data-item'));
   // let today = new Date(response.gradDate).toISOString().split('T')[0];
    $('input[name="CollegeId"]').val(response.collegeId)
    $('input[name="CollegeName"]').val(response.collegeName)
    $('input[name="CollegeCity"]').val(response.collegeCity)
    if (typeof (response.gradDate) == 'undefined') {
        $('select[name="Month"]').val(response.month)
        $('select[name="Year"]').val(response.year)
    }
    else {
        $('select[name="Month"]').val(getMonth(new Date(response.gradDate).getMonth()))
        $('select[name="Year"]').val(new Date(response.gradDate).getFullYear())
    }
    //$('input[name="GradDate"]').val(today)
    $('input[name="HonorProgram"]').val(response.honorProgram)
    $('select[name="CollegeStateAbbr"]').val(response.collegeStateAbbr)
    $('input[name="SchoolName"]').val(response.schoolName)
    $('#ddlDegree').val(response.degreeId).trigger('change')
    $('#DegreeOther').val(response.degreeOther);
    $('#ddlMajor').val(response.majorId).trigger('change')
    $('#MajorOther').val(response.majorOther);
    $('select[name="MajorSpecialtyId"]').val(response.majorSpecialtyId).trigger('change')
    $('input[name="MajorSpecialityOther"]').val(response.majorSpecialtyOther);
    $('select[name="MinorId"]').val(response.minorId).trigger('change')
    $('input[name="MinorOther"]').val(response.minorOther);
    if (response.includeGpa != null || response.includeGpa != false) {
        $('input[name="IncludeGpa"]').prop('checked', true);
    }
    $('select[name="CertificateId"]').val(response.certificateId).trigger('change')
    $('input[name="CertificateOther"]').val(response.certificateOther);
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
    };
    debugger;

    if (collegeArray == 0) {
        
        swal("Education Required", "Please fill out the education to proceed", "error");
    }
    else {
        $.each(collegeArray, function (index, record) {
            let honors = [];
            $($(".crd-wrp-olg")[index]).find(".cardWrapper").each(function (index, value) {
                let honor = {
                    HonorName: $(this).find(".HonorName").val(),
                    HonorMonth: $(this).find(".HonorMonth").val(),
                    HonorYear: $(this).find(".HonorYear").val(),
                    AcademicHonorId: $(this).find(".AcademicHonorId").val(),
                };

                honors.push(honor);
            });
            record.AcademicHonors = honors;

            let schols = [];
            $($(".crd-wrp-olg")[index]).find(".cardWrapper-schols").each(function (index, value) {
                let schol = {
                    AcademicScholarshipId: $(this).find(".AcademicScholarshipId").val(),
                    ScholarshipName: $(this).find(".ScholarshipName").val(),
                    ScholarshipCriteria: $(this).find(".ScholarshipCriteria").val(),
                    ScholarshipMonth: $(this).find(".ScholarshipMonth").val(),
                    ScholarshipYear: $(this).find(".ScholarshipYear").val(),
                };

                schols.push(schol);
            });
            record.AcademicScholarships = schols;
        });
        console.log(collegeArray)

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
    }
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
    $('#ddlMajor').append('<option value="" selected><b>Select Major</b></option>')
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
    $('#ddlMajorSpeciality').append('<option value="" selected><b>Select Major specialty</b></option>')
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
    $('#ddlMinor').append('<option value="" selected><b>Select Minors</b></option>')
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
    $('#ddlState').append('<option value="" selected><b>Select State</b></option>')
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
    $('#ddlOtherCertificates').append('<option value="" selected><b>Select Certificates</b></option>')
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



//$(document).on("click", "button.btnEditPosition", function () {
//    parsedJson = JSON.parse($(this).attr("data-json"));
//    $('#txtpositionTitle').val(parsedJson.Title);
//    $('#ddlPositionStartedMonth').val(parsedJson.StartedMonth);
//    $('#ddlPositionStartedYear').val(parsedJson.StartedYear);
//    $('#ddlPositionEndedMonth').val(parsedJson.EndedMonth);
//    $('#ddlPositionEndedYear').val(parsedJson.EndedYear);
//    $('#txtResponsibility1').val(parsedJson.Responsibility1);
//    $('#txtResponsibility2').val(parsedJson.Responsibility2);
//    $('#txtResponsibility3').val(parsedJson.Responsibility3);
//    $('#txtOtherInfo').val(parsedJson.OtherInfo);
//    localStorage.setItem("edit-position", $(this).attr("data-identity"));
//})
function hideModal(modalId) {
    $(modalId).modal('toggle');
}

function showModal(modalId) {
    $(modalId).modal('show');
}



//$(document).on('click', '#btnEditHonor', function () {
//    $('#btnSaveHonor').prop('disabled', false)
//    var response = acadmicHonorArray[$(this).attr('acad-edit')];
//    localStorage.setItem("acad-index", $(this).attr('acad-edit'))
//    $('#hdfAcademicHonorId').val(response.academicHonorId);
//    $('#txtAcademicHonor').val(response.honorName);
//    $('#txtStartedMonth').val(response.honorMonth);
//    $('#txtStartedYear').val(response.honorYear);
//    $('#HonorModal').modal('show');
//});

//$(document).on('click', '#btnEditAcademicScholarship', function () {
//    $('#btnSaveScholarship').prop('disabled', false)
//    var response = acadmicScholarshipArray[$(this).attr('sch-edit')];
//    localStorage.setItem("sch-index", $(this).attr('sch-edit'))
//    $('#hdfAcademicScholarshipId').val(response.academicScholarshipId);
//    $('#txtScholarshipName').val(response.scholarshipName);
//    $('#txtScholarshipCriteria').val(response.scholarshipCriteria);
//    $('#txtScholarshipStartedMonth').val(response.scholarshipMonth);
//    $('#txtScholarshipStartedYear').val(response.scholarshipYear);
//    $('#ScholarshipModal').modal('show');
//});


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
    $('select').val("");
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
    localStorage.setItem("acad-parent-index", $(this).attr('data-parent'));
    let id = $(this).attr('data-item')
    debugger;
    $.ajax({
        url: '/Education/DeleteCollege?academicId=' + id,
        type: 'post',
        success: function (response) {
            console.log(acadmicHonorArray)
            let index = parseInt(localStorage.getItem("acad-index"));
            let parent = parseInt(localStorage.getItem("acad-parent-index"));
            acadmicHonorArray.splice(index, 1);
            
            collegeArray[parent].academicHonors.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    });
});

$(document).on('click', '#btnDeleteAcademicScholarship', function () {

    localStorage.setItem("sch-index", $(this).attr('sch-edit'));
    localStorage.setItem("sch-parent", $(this).attr('sch-parent'));
    let id = $(this).attr('data-item')
    $.ajax({
        url: '/Education/DeleteCollege?scholarshipid=' + id,
        type: 'post',
        success: function (response) {
            let index = parseInt(localStorage.getItem("sch-index"));
            let parent = parseInt(localStorage.getItem("sch-parent"));
            collegeArray[parent].academicScholarships.splice(index, 1);
            LoadCards();
        },
        error: function (err) {

        }
    });
});



function guid() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}


function clearStorage(btn, type = "honors") {
    localStorage.clear();
    if (type == "schol") {
        localStorage.setItem("add-schol", $(btn).attr("data-item"));
    }
    else {
        localStorage.setItem("add-honor", $(btn).attr("data-item"));
    }
}

