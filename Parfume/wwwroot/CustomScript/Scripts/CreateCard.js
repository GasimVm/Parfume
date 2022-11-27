$(document).ready(function () {
    
    AddCard();
})

function AddCard() {
    $(".addCard").click(function () {

        var formData = new FormData();
        var CardName = $(".CardName").val();
        formData.append('CardName', CardName)
        $(".addCard").hide();
        $.ajax({
            type: 'POST',
            url: '/Card/CreateCard',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.status === "success") {
                   
                    alert("Qeydə alındı!")
                    window.location.reload();
                }
                else {
                    $(".addCard").show();
                    alert("Xəta baş verdi,şəbəkəni yoxluyun!")
                    $("#gAnalize").show()
                }
            }
        });
    })
}