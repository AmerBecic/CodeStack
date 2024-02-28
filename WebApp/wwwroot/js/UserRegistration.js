$(function () {
    var registerButton = $("#UserRegistrationModal button[name='register']").click(onUserRegisterClick);

    function onUserRegisterClick() {

        var url = "UserAuth/Register"

        var antiForgeryToken = $("#UserRegistrationModal input[name='__RequestVerificationToken']").val();

        var email = $("#UserRegistrationModal input[name='Email']").val();
        var password = $("#UserRegistrationModal input[name='Password']").val();
        var confirmPassword = $("#UserRegistrationModal input[name='ConfirmPassword']").val();
        var firstName = $("#UserRegistrationModal input[name='FirstName']").val();
        var lastName = $("#UserRegistrationModal input[name='LastName']").val();
        var address1 = $("#UserRegistrationModal input[name='Address1']").val();
        var address2 = $("#UserRegistrationModal input[name='Address2']").val();
        var postCode = $("#UserRegistrationModal input[name='PostCode']").val();
        var phoneNumber = $("#UserRegistrationModal input[name='PhoneNumber']").val();
        var acceptUserAgreement = $("#UserLoginModal input[name='AcceptUserAgreement']").prop('checked');

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            Address1: address1,
            Address2: address2,
            PostCode: postCode,
            PhoneNumber: phoneNumber,
            AcceptUserAgreement: acceptUserAgreement
        };

        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='LoginInValid']").val() == "true";

                if (hasErrors == true) {
                    $("#UserRegistrationModal").html(data);

                    loginButton = $("#UserRegistrationModal button[name='register']").click(onUserLoginClick);

                    var form = $("#UserRegistrationModal");

                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    location.href = 'Home/Index';
                }
            },

            error: function (xhr, ajaxOptions, thrownError) {
                console.error(thrownError + "/r/n" + xhr.statusText + "/r/n" + xhr.responseText);
            }
        });

    }
});