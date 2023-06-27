$(document).ready(function () {


});

$('#txtConfirmPassword').change(function () {
    CheckConfirmPassword(this);
})

$('#btnCreateAccount').click(function () {
    let firstName = $('#txtFirstName').val();
    let lastName = $('#txtLastName').val();
    let address = $('#txtAddress').val();
    let address2 = $('#txtAddress2').val();
    let email = $('#txtEmail').val();
    let city = $('#ddlCity :selected').text();
    let state = $('#ddlState :selected').text();
    let zip = $('#ddlZip :selected').text();
    let phoneNumber = $('#txtPhoneNumber').val();
    let password = $('#txtPassword').val();
    if (ValidateInputs()) {
        if (ValidateDropdowns()) {
            alert('form submited')
        }
    //     $.ajax({
    //    url: 'Account/CreateUser',
    //    type: 'POST',
    //    data: {
    //        FirstName: firstName,
    //        LastName: lastName,
    //        UserName: email,
    //        UserPassword: password,
    //        Address1: address,
    //        Address2: address2,
    //        City: city,
    //        ZipCode: zip,
    //        StateAbbr: state,
    //        Phone: phoneNumber
    //    },
    //    success: function (resp) {
    //        alert(resp.Message)
    //    },
    //    error: function (err) {
    //    }
    //});
    }
   
});


function ValidateInputs() {  
    $('#RegisterForm .input-validation').each(function () {
        const warning = `<small class="text-danger">this field is required</small>`
        if (this.value == "") {
            $(this).css('border-color', 'red')
            $(this).after(warning);
        } else {
            $(this).next().remove();
            $(this).css('border-color', '')
            return false;
        }
    });

    //$('select').each(function () {
    //    $(this).next().remove();
    //    $(this).css('border-color', '')
    //    return true;
    //})  
    
}

function ValidateDropdowns() {
    $('#RegisterForm .select-validation').each(function () {
        const warning = `<small class="text-danger">this field is required</small>`
        if (this.value == "") {
            $(this).css('border-color', 'red')
            $(this).after(warning);
            return false;
        } else {
            $(this).next().remove();
            $(this).css('border-color', '')
            return true;
        }
    });
}





                function CheckConfirmPassword(confirmPassword) {
                    console.log(confirmPassword)
                    const matched = `<small class="text-success">password mateched</small>`;
                    const notMateched = `<small class="text-danger">password is not matching.. Enter again</small>`;
                    const password = $('#txtPassword').val();
                    console.log(password)
                    if (password != confirmPassword.value) {
                        $(confirmPassword).after(notMateched);
                        return false;
                    } else {
                        $(confirmPassword).after(matched);
                        return true
                    }
                }
