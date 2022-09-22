$(document).ready(function () {
    SendOrder();
    $('#Birthdate').datetimepicker({
        format: 'DD/MM/YYYY' 
    });
})

function SendOrder() {
    $(".sendOrder").click(function () {
        var formData = new FormData();
        var fincode = $(".fincode").val();
        var surname = $(".surname").val();
        var fatherName = $(".fatherName").val();
        var baseNumber = $(".baseNumber").val();
        var name = $(".name").val();
        var address = $(".address").val();
        var workAddress = $(".workAddress").val();
        var InstagramAddress = $(".InstagramAddress").val();
        var WhoIsOkey = $(".WhoIsOkey").val();
        var baseNumber = $(".baseNumber").val();
        var firstName = $(".firstName").val();
        var firstNumber = $(".firstNumber").val();
        var secondName = $(".secondName").val();
        var secondNumber = $(".secondNumber").val();
        var thirdName = $(".thirdName").val();
        var thirdNumber = $(".thirdNumber").val();
        var dateBirth = $("#Birth_Date").val();
        if (name.length == 0) {
            alert("Ad yazmaq mecburidi")
            return
        }  
        formData.append('name', name)
        formData.append('surname', surname)
        formData.append('fatherName', fatherName)
        formData.append('baseNumber', baseNumber)
        formData.append('fincode', fincode)
        formData.append('address', address)
        formData.append('workAddress', workAddress)
        formData.append('InstagramAddress', InstagramAddress)
        formData.append('firstName', firstName)
        formData.append('firstNumber', firstNumber)
        formData.append('secondName', secondName)
        formData.append('secondNumber', secondNumber)
        formData.append('thirdName', thirdName)
        formData.append('thirdNumber', thirdNumber)
        formData.append('WhoIsOkey', WhoIsOkey)
        formData.append('dateBirth', dateBirth)
        
        if (confirm('İstifadəcini qeydə almaq istiyirsiz?')) {
            $(".sendOrder").hide();

            $.ajax({
                type: 'POST',
                url: '/Customer/CreateCustomer',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.status === "success") {
                        alert(response.message)
                        window.location.reload();
                    }
                    else {
                        alert("Xəta baş verdi,şəbəkəni yoxluyun!")
                        $(".sendOrder").show();
                    }
                }
            });
        }


    })
}
