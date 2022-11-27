$(document).ready(function () {
    DeleteCard();
    AddCard();
})

function AddCard() {
    $(".addCardCustomer").click(function () {

        var formData = new FormData();
        var CustomerId = $(".customerId").val();
        var cardId = $(".cardInfo").children("option:selected").val();
        formData.append('CustomerId', CustomerId)
        formData.append('cardId', cardId)
         
            $(".addCardCustomer").hide()
            $.ajax({
                type: 'POST',
                url: '/Card/AddCard',
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

function DeleteCard() {
    $(".cardDelete").click(function () {

        var formData = new FormData();

        var cardId = $(this).attr("data-cardId");
         
        formData.append('cardId', cardId)
        $.ajax({
            type: 'POST',
            url: '/Card/DeleteCard',
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
                     
                }
            }
        });
    })
}