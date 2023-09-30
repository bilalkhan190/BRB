$(document).ready(function () {
    let uri = '/ContactInfo/getState'
    $('#ddlStateAbbr').html("");
    $('#ddlStateAbbr').append('<option value=""><b>Select State</b></option>')
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

    
   
    $.ajax({
        url: '/ContactInfo/GetAllContactInfo' ,
        type: 'GET',
        dataType:'json',
        success: function (response) {
            if (response.data != null) {
                console.log(response)
                $('input[name="FirstName"]').val(response.data.firstName);
                $('#hdfcontactInfoId').val(response.data.contactInfoId);
                $('input[name="LastName"]').val(response.data.lastName);
                $('input[name="Phone"]').val(response.data.phone);
                $('input[name="Email"]').val(response.data.email);
                $('input[name="Address1"]').val(response.data.address1);
                $('input[name="Address2"]').val(response.data.address2);
                $('input[name="City"]').val(response.data.city);
                //$('select[name="StateAbbr"]').val(response.data.stateAbbr);
                $('input[name="ZipCode"]').val(response.data.zipCode);
                $('input[name="IsComplete"]').prop('checked', response.data.isComplete);
                $('input[name="IsComplete"]').prop('disabled', response.data.isComplete); 
            }
            
        },
        error: function (err) {
            alert('error')
        }
    });
    function getState() {
        $.get(uri, function (resp) {
            $('#ddlStateAbbr').val(resp.data)
        })
    }
    setTimeout(
        getState
    , 1000);

 
});


$('#btnSaveContactInfo').click(function () {
    $('input[name="IsComplete"]').val($('input[name="IsComplete"]').is(":checked"))[0].checked
    $('#contactForm').validate();
    if ($('#contactForm').valid()) {
        $.ajax({
            url: '/ContactInfo/Contactinfo',
            type: 'POST',
            data: $('#contactForm').serialize(),
            success: function (response) {
                console.log(response.redirect)
                window.location.href = response.redirect;
            },
            error: function (err) {
                alert('error')
            }
        });
    }
})
    
  
  
      
    
  
    
    


