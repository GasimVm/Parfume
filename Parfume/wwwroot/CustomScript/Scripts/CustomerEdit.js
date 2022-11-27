$(document).ready(function () {
    $('#Birthdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    ChangeBirth();
})

function ChangeBirth() {
    $(".addNoteCustomer").click(function () {
         
        var formData = new FormData();
        var CustomerId = $(".customerId").val();
        var cardId = $(".cardInfo").children("option:selected").val();
        var noteCustomer = $(".noteCustomer").val();
        var dateBirth = $("#Birth_Date").val();
        formData.append('CustomerId', CustomerId)
        formData.append('noteCustomer', noteCustomer)
        formData.append('dateBirth', dateBirth)
        formData.append('cardId', cardId)
        $.ajax({
            type: 'POST',
            url: '/Customer/AddNote',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === "success") {

                    alert("Qeydə alındı!")
                    window.location.reload();
                }
                else {
                    alert("Xəta baş verdi,şəbəkəni yoxluyun!")
                    $("#gAnalize").show()
                }
            }
        });
    })
}