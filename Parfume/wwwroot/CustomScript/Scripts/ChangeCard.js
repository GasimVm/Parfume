$(document).ready(function () {
    AddCard();
})

function AddCard() {
    $(".changeCardCustomer").click(function () {

        var formData = new FormData();
        var customerId = $(".customerId").val();
        var newCardId = $(".cardInfo").children("option:selected").val();
        formData.append('customerId', customerId)
        formData.append('newCardId', newCardId)

        $(".changeCardCustomer").hide()
        $.ajax({
            type: 'POST',
            url: '/Card/ChangeCardCustomer',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === "success") {

                    alert("Qeydə alındı!")
                    window.location = document.referrer;
                }
                else {
                    alert("Xəta baş verdi,şəbəkəni yoxluyun!")

                }
            }
        });

    })
}

 