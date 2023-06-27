var languageArray = [];
$(document).ready(function () {
    $('#ddlAbility').html("");
    $('#ddlAbility').append('<option value="0" selected><b>Select Ability</b></option>')
    $.ajax({
        url: '/Common/GetLanguageAbilityList',
        type: 'get',
        success: function (response) {
            $.each(response.data, function (index, value) {
                $('#ddlAbility').append(`<option value="${value.languageAbilityId}"><b> ${value.languageAbilityDesc} </b></option>`);
            })
        },
        error: function (err) {
            alert(err)
        }
    });

    LoadData();
}); 

$('#btnSaveLanguage').click(function () {

    let language = {
        LanguageId: $('#hdfLanguageId').val(),
        LanguageName: $('#txtLanguageName').val(),
        LanguageAbilityId: $('#ddlAbility').val(),
        LanguageAbilityDesc : $('#ddlAbility option:selected').text(),
    };
    if (localStorage.getItem("pos-index") == null) {
        languageArray.push(language);
    }
    else {
        languageArray[parseInt(localStorage.getItem("pos-index"))] = language;
        localStorage.clear();
    }
    $('#LanguageModal').modal('toggle');
    LoadCards();
    //fill the array of position record and display the recorded data in div
  
});

$('#btnSaveAndContinue').click(function () {
    let data = {
        LanguageSkillId: $('#hdfLanguageSkillId').val(),
        LastSectionVisitedId: $('#hdfLastSectionVisitedId').val(),
        Languages: languageArray,
        IsComplete: $('#cbIsComplete').val($('#cbIsComplete').is(':checked'))[0].checked
    }
   
    $.ajax({
        url: '/Language/PostLanguageSkillData',
        type: 'POST',
        data: { languageViewModel: data },
        success: function (response) {
            languageArray = [];
            LoadData();
            window.location.href = response.redirect
        },
        error: function (error) { }
    });
});

function LoadData() {  
    $.ajax({
        url: '/Language/GetLanguageSkillsRecord',
        type: 'GET',
        success: function (response) {
            $('#hdfLanguageSkillId').val(response.data.languageSkillId);
            if (response.data.isComplete) {
                $("#cbIsComplete").val(response.data.isComplete).prop("checked",true)
            }
            if (response.data.languages.length > 0) {
                $.each(response.data.languages, function (index, value) {
                    languageArray.push(value)
                });
                LoadCards();
            }
          
        },
        error: function (error) {

        }
    });
}
$(document).on('click', '#btnEditLangauge', function () {
    var response = languageArray[$(this).attr('data-edit')];
    localStorage.setItem("pos-index", $(this).attr('data-edit'));
    $('#hdfLanguageId').val(response.languageId);
    $('#txtLanguageName').val(response.languageName);
    $('#ddlAbility').val(response.languageAbilityId);
    $('#LanguageModal').modal('show');
});

function LoadCards() {
    $("#divEditSection").html("");
    languageArray = languageArray.map(el => _.mapKeys(el, (val, key) => _.camelCase(key)));
    console.log(languageArray)
    $.each(languageArray, function (index, value) {
        let html = ` 
                <div class="card ml-4 col-md-4"> 
                    <div class="card-body">
                       <div class="row">
                            <div class="col-md-8">
                                <span class="card-text">
                                    <p>${value.languageName}</p>
                                    <p class="text-muted">${value.languageAbilityDesc}</p>
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
                                </button><button type="button" id="btnEditLangauge" data-item='${value.languageId}' data-edit='${index}' class="btn btn-outline-primary">
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

        $('#emptyListMessage').hide();
        $('#divEditSection').append(html);
    });
    
}
