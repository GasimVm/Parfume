$(document).ready(function () {
    $('#datetimepicker3').datetimepicker({
        format: 'DD/MM/YYYY',
        sideBySide: false
    });
    ChangePay();
    ChangeDate();
    Pay();
    DeletePay();
    ChangeCost();
})

function ChangePay() {
    
    $(document).on("click", ".changePay", function () {
        window.location.href = $(this).attr('data-href');
         
    })
}
function ChangeDate() {

    $(".changeDate").click(function () {
        var changeDay = $(".changeDay").val();
        var OrderId = $("#OrderId").val();
        var formData = new FormData();
        formData.append('changeDay', changeDay)
        formData.append('OrderId', OrderId)
        if (confirm('Ödəmə tarixini dəyişmək istiyrsiz?')) {
            $.ajax({
                type: 'POST',
                url: '/Order/ChangePaymentDate',
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
        }

    })
}
function Pay() {
    $(".PayOrder").click(function () {

        var OrderId = $("#OrderId").val();
        var price = $(".price").val();
        var note = $(".note").val();
        var dateCreate = $("#Create_Date").val();
        if (price.length == 0) {
            alert("Qiymət boş ola bilməz!")
            return
        } else if (dateCreate == undefined) {
            alert("Ödəniş tarixi boş ola bilməz!")
            return
        }
        var formData = new FormData();
        formData.append('OrderId', OrderId)
        formData.append('price', price)
        formData.append('note', note)
        formData.append('dateCreate', dateCreate)
        if (confirm('Ödəməni qeydə almaq istiyrsiz?')) {
            $(".PayOrder").hide();
            $.ajax({
                type: 'POST',
                url: '/Order/PayOrder',
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
                        $(".PayOrder").show();

                    }
                }
            });
        }

    })
}

function DeletePay() {

    $(document).on("click", ".deletePay", function () {
        var payHistoryId = $(this).attr("data-payHistoryId");
        var OrderId = $("#OrderId").val();
        var formData = new FormData();
        formData.append('payHistoryId', payHistoryId)
        formData.append('OrderId', OrderId)
        if (confirm('Ödənişi silmək istiyrsiz?')) {
            $.ajax({
                type: 'POST',
                url: '/Order/DeletePaymentPay',
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
        }
    })
}

function ChangeCost() {
    $(".changeCostbtn").click(function () {
        var changeCost = $(".changeCostValue").val();
        var OrderId = $("#OrderId").val();
        var formData = new FormData();
        formData.append('changeCost', changeCost)
        formData.append('OrderId', OrderId)
        if (confirm('Maya dəyərini dəyişmək istiyrsiz?')) {
            $.ajax({
                type: 'POST',
                url: '/Order/ChangeCost',
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
        }

    })
}