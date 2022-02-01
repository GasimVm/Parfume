$(document).ready(function () {
    $(".ChangePass").click(function () {
        var formData = new FormData();
        var oldPassword = $(".oldPass").val();
        var newPassword = $(".newPass").val();
        formData.append('oldPassword', oldPassword)
        formData.append('newPassword', newPassword)
        $.ajax({
            type: 'POST',
            url: '/Account/ChangePassword',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === "success") {

                    alert(response.message)
                    window.location.reload();
                }
                else {
                    alert(response.message)
                }
            }
        });
    })
})