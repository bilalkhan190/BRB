$(document).ready(function () {
     FillDropdowns();
    GenerateCheckboxes()
    LoadData()
});

function LoadData() {
    $.ajax({
        url: '/ObjectiveSummary/GetAllData',
        type: 'get',
        success: function (response)
        {
            if (response.data != null) {
                console.log(response.data)
                $('#hdfobjectiveSummaryId').val(response.data.objectiveSummaryId)
                $('#ddlExperienceOfYears').val(response.data.yearsOfExperienceId)
                $('#ddlObjectiveType').val(response.data.objectiveType)
                $('#ddlPositionType').val(response.data.positionTypeId)
                $('#CurrentCompanyType').val(response.data.currentCompanyType)
                $('#ddlChangeTypeList').val(response.data.changeTypeId).trigger('change')
                $('#txtChangeTypeList').val(response.data.changeTypeOther)
                $('#FieldsOfExperience').val(response.data.fieldsOfExperience);
                $('#ckbIsComplete').prop('checked',response.data.isComplete);
                $('#ckbIsComplete').val(response.data.isComplete);
                $('#ckbIsComplete').attr('readonly', 'readonly');

              
            }
            
        },
        error: function (err) { }
    });
}

function GenerateCheckboxes() {
    let checkboxRowDiv = $('.cb-row');
    let newColumn = `<div class="col-sm-3"><div> `
    let count = 0;
    $.ajax({
        url: '/ObjectiveSummary/GetObjectives',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                let checkboxHtml = ` <div class="custom-checkbox custom-control">
                                        <input type="checkbox" id="${value.objectiveDesc}" ${value.checked ? 'checked' : ''}
                                 class="custom-control-input customcb" value="${value.objectiveId}"/>
                                <label class="custom-control-label"
                                               for="${value.objectiveDesc}">${value.objectiveDesc}</label>
                                </div>`;

                checkboxRowDiv.append(checkboxHtml);
                count++
            });

        },
        error: function (err) {
            alert('checkbox generate error')
        }
    });
}
$(document).on('change', '.customcb', function (e) {

    let count = 0;
    $(document).find(".customcb").each(function () {
        if ($(this).is(":checked")) {
            count++;
        }
        
    })

    debugger;
    if (count > 3) {
        $(this).prop('checked', false);
        $('#checkboxlimitMessage').hide();
       // $('input:checkbox:not(:checked)').prop('disabled', true);
        $(document).find(".customcb").each(function () {
            if (!$(this).is(":checked")) {
                $(this).prop("disabled", true);
               
            }
        })
       
        e.preventDefault();
        return false;

    }
    else {
        $('input:checkbox:not(:checked)').prop('disabled', false);
        $('#checkboxlimitMessage').show();
    }
   
  
    //if (checkboxCount == 3) {

    //    $(':checkbox:not(:checked)').prop('disabled', true);
    //    $('#checkboxlimitMessage').hide();
    //} else {
    //    $(':checkbox:not(:checked)').prop('disabled', false);
    //    $('#checkboxlimitMessage').show();
    //}
    //console.log(checkboxCount)

    //console.log(this)
});

function AppendNameAttributeForCheckboxes(listOfCheckboxes) {
    if (listOfCheckboxes.length == 3) {
        $(listOfCheckboxes[0]).attr('name', 'Objective1Id')
        $(listOfCheckboxes[0]).prop('checked',true)
        $(listOfCheckboxes[1]).attr('name', 'Objective2Id')
        $(listOfCheckboxes[1]).prop('checked', true)
        $(listOfCheckboxes[2]).attr('name', 'Objective3Id')
        $(listOfCheckboxes[2]).prop('checked', true)
    } else {
        $(listOfCheckboxes[0]).attr('name', 'Objective1Id')
        $(listOfCheckboxes[0]).prop('checked', true)
        $(listOfCheckboxes[1]).attr('name', 'Objective2Id')
        $(listOfCheckboxes[1]).prop('checked', true)
    }


    console.log(listOfCheckboxes);
}


$('#ckbIsComplete').click(function () {
    if ($(this).is(':checked')) {
        $(this).attr('value', 'true')
        $(this).prop('checked', true)
    }
    
})

function FillDropdowns() {
    $('#ddlExperienceOfYears').html("");
    $('#ddlExperienceOfYears').append('<option value="" selected><b>Select years</b></option>')
    $.ajax({
        url: '/ObjectiveSummary/GetExperienceYears',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlExperienceOfYears').append(`<option value="${value.yearsOfExperienceId}"><b> ${value.yearsOfExperienceDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    $('#ddlChangeTypeList').html("");
    $('#ddlChangeTypeList').append('<option value="" selected><b>Select Change Type</b></option>')
    $.ajax({
        url: '/ObjectiveSummary/GetChangeType',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlChangeTypeList').append(`<option value="${value.changeTypeId}"><b> ${value.changeTypeDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });


    $('#ddlPositionType').html("");
    $('#ddlPositionType').append('<option value="" selected><b>Select position Type</b></option>')
    $.ajax({
        url: '/ObjectiveSummary/GetPositionType',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlPositionType').append(`<option value="${value.positionTypeId}"><b> ${value.positionTypeDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });
}

$('#ddlPositionType').change(function () {
    let value = $('#ddlPositionType :selected').val();
    let input = "<input type='text' name='PositionTypeOther' class='form-control mt-2' required id='txtOtherPositionType'/>";
    if (value == 5) {
        $(this).after(input);
    }
    else {
        $('#txtOtherPositionType').remove();
    }
});

$('#ddlChangeTypeList').change(function () {
    let value = $('#ddlChangeTypeList :selected').val();
    let input = "<input type='text' name='ChangeTypeOther' class='form-control mt-2' required id='txtChangeTypeList'/>";
    if (value == 6) {
        $(this).after(input);
    }
    else {
        $('#txtChangeTypeList').remove();
    }
});


$('#btnSaveObjectives').click(function () {
    $('#objectiveForm').validate();
    if ($('#objectiveForm').valid()) {
        AppendNameAttributeForCheckboxes($('.customcb:checked'));
        $('#ckbIsComplete').val($('#ckbIsComplete').is(":checked"))[0].checked
        $.ajax({
            url: '/ObjectiveSummary/PostData',
            type: 'POST',
            data: $('#objectiveForm').serialize(),
            success: function (response) {
                LoadData();
                window.location.href = response.redirect;
            },
            error: function (err) {
                console.log(err)
            }
        })
    }
  
})


