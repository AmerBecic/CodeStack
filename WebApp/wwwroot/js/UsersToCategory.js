$(function () {

    $("#SaveSelectedUsers").prop('disabled', true);



    $('select').on('change', () => {

        var selectedCategoryId = $('#CategoryId').find(":selected").val();

        var url = "/Admin/UsersToCategory/GetUsersForCategory?categoryId=" + selectedCategoryId;

        if (selectedCategoryId != 0) {
            $.ajax(
                {
                    type: "GET",
                    url: url,
                    success: (data) => {
                        $("#UsersCheckList").html(data);
                        $("#SaveSelectedUsers").prop('disabled', false);
                    },
                    error: (xhr, ajaxOptions, thrownError) => {
                        var errorText = "Status:" + xhr.status + " - " + xhr.statusText;

                        ShowAlert("#alert_placeholder", "danger", "Error!", errorText);

                        console.error(thrownError + "/r/n" + xhr.statusText + "/r/n" + xhr.responseText);
                    }
                }
            );
        }
        else {
            $("#SaveSelectedUsers").prop('disabled', true);
            $("input[type=checkbox]").prop("checked", false);
            $("input[type=checkbox]").prop("disabled", true);

        }
    });

    $("#SaveSelectedUsers").click(() => {

        var url = "/Admin/UsersToCategory/SaveSelectedUsers";

        var categoryId = $("#CategoryId").val();

        var antiForgeryToken = $("input[name='__RequestVerificationToken']").val();

        var selectedUsers = [];

        $("#SaveSelectedUsers").prop('disabled', true);
        $("input[type=checkbox]").prop("disabled", true);
        $('select').prop('disabled', true);

        $(".progress").show("fade");

        $('input[type=checkbox]:checked').each(function () {
            var userModel = {
                Id: $(this).attr("value")
            };

            selectedUsers.push(userModel);
        });


        var selectedUsersForCategory = {
            __RequestVerificationToken: antiForgeryToken,
            CategoryId: categoryId,
            UsersSelected: selectedUsers
        };

        $.ajax(
            {
                type: "POST",
                url: url,
                data: selectedUsersForCategory,
                success: (data) => {
                    $("#UsersCheckList").html(data);

                    $(".progress").hide("fade", function () {

                        $(".alert-success").fadeTo(2000, 500).slideUp(500, function () {
                            $("#SaveSelectedUsers").prop('disabled', false);
                            $("input[type=checkbox]").prop("disabled", false);
                            $('select').prop('disabled', false);
                        });

                    });

                },
                error: (xhr, ajaxOptions, thrownError) => {
                    var errorText = "Status:" + xhr.status + " - " + xhr.statusText;

                    ShowAlert("#alert_placeholder", "danger", "Error!", errorText);

                    console.error(thrownError + "/r/n" + xhr.statusText + "/r/n" + xhr.responseText);

                    $("#SaveSelectedUsers").prop('disabled', false);
                    $("input[type=checkbox]").prop("disabled", false);
                    $('select').prop('disabled', false);
                }
            }
        );


    });

});