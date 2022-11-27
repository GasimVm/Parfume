var selectPlaceholder = 'Seçin';
var inputTooShort = 'Axtarış üçün minimum 3 hərf daxil edin.';
var searching = 'Axtarılır...';
var noResults = 'Uyğun nəticə tapılmadı.';
$(document).ready(function () {
    Select2Plugin();
    SendOrder();
    $('#Birthdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    $('#datetimepicker3').datetimepicker({
        format: 'DD/MM/YYYY',
        sideBySide: false
    });
    
   

})
function SendOrder() {
    $(".price").keyup(function () {

        var amount = $(".amount").val();
        var price = $(".price").val();
        var total = amount * price;
        $(".totalPrice").val(total)

    })
    $(".amount").change(function () {

        var amount = $(".amount").val();
        var price = $(".price").val();
        var total = amount * price;
        $(".totalPrice").val(total)

    })
    $(".duration").change(function () {
        var duration = $(".duration option:selected").val();
        var firstPrice = $(".firstPrice").val();
        var total = $(".totalPrice").val() - firstPrice;
        var monthlyPayment = total / duration;
        $(".monthlyPayment").val(monthlyPayment)
    })
    $(".firstPrice").change(function () {
        var firstPrice = $(".firstPrice").val();
        var duration = $(".duration option:selected").val();

        var total = $(".totalPrice").val() ;
        var monthlyPayment = (total - firstPrice)/ duration;
        $(".monthlyPayment").val(monthlyPayment)
    })

    $(".sendOrder").click(function () {
        var formData = new FormData();
        var fincode = $("#UsersDirectly option:selected").text();
       
        var dateCreate = $("#Create_Date").val();
        var dateBirth = $("#Birth_Date").val();
        
        var WhoIsOkey = $(".WhoIsOkey").val();
        var CustomerId = $("#UsersDirectly").val();
        var surname = $(".surname").val();
        var duration = $(".duration").children("option:selected").val();
        var cardId = $(".cardInfo").children("option:selected").val();
        var fatherName = $(".fatherName").val();
        var baseNumber = $(".baseNumber").val();
        var name = $(".name").val();
        var firstName = $(".firstName").val();
        var firstNumber = $(".firstNumber").val();
        var secondName = $(".secondName").val();
        var secondNumber = $(".secondNumber").val();
        var thirdName = $(".thirdName").val();
        var thirdNumber = $(".thirdNumber").val();
        var productName = $(".productName").val();
        var quantity = $(".quantity").val();
        var price = $(".price").val();
        var cost = $(".cost").val();
        var firstPrice = $(".firstPrice").val();
        var monthlyPayment = $(".monthlyPayment").val();
        var totalPrice = $(".totalPrice").val();
        var amount = $(".amount").val();
        var address = $(".address").val();
        var workAddress = $(".workAddress").val();
        var InstagramAddress = $(".InstagramAddress").val();
        

        if (name.length == 0) {
            alert("Ad yazmaq mecburidi")
            return
        } else if (productName.length == 0) {
            alert("Parfumun adı yazmaq mecburidi")
            return
        } else if (fincode.length == 0) {
            alert("Fin yazmaq mecburidi")
            return
        } else if (totalPrice.length == 0) {
            alert("Toplam qiymət boş ola bilməz!")
            return
        } else if (price.length == 0) {
            alert("Qiymət boş ola bilməz!")
            return
        } else if (cost.length == 0) {
            alert("Maya dəyəri boş ola bilməz!")
            return
        } else if (amount.length == 0) {
            alert("Sayı boş ola bilməz!")
            return
        }
        formData.append('name', name)
        formData.append('surname', surname)
        formData.append('duration', duration)
        formData.append('fatherName', fatherName)
        formData.append('baseNumber', baseNumber)
        formData.append('fincode', fincode)
        formData.append('firstName', firstName)
        formData.append('firstNumber', firstNumber)
        formData.append('secondName', secondName)
        formData.append('secondNumber', secondNumber)
        formData.append('thirdName', thirdName)
        formData.append('thirdNumber', thirdNumber)
        formData.append('quantity', quantity)
        formData.append('price', price)
        formData.append('cost', cost)
        formData.append('firstPrice', firstPrice)
        formData.append('amount', amount)
        formData.append('monthlyPayment', monthlyPayment)
        formData.append('productName', productName)
        formData.append('totalPrice', totalPrice)
        formData.append('address', address)
        formData.append('dateCreate', dateCreate)
        formData.append('CustomerId', CustomerId)
        formData.append('workAddress', workAddress)
        formData.append('WhoIsOkey', WhoIsOkey)
        formData.append('dateBirth', dateBirth)
        formData.append('cardId', cardId)
        formData.append('InstagramAddress', InstagramAddress)

        if (confirm('Sifarişi qeydə almaq istiyirsiz?')) {
            $(".sendOrder").hide();
            $.ajax({
                type: 'POST',
                url: '/Order/CreateOrder',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.status === "success") {

                        alert("Qeydə alındı!")
                        window.location.reload();
                    }
                    else {
                        $(".sendOrder").show();
                        alert("Xəta baş verdi,şəbəkəni yoxluyun!")
                        
                    }
                }
            });
        }
        
    })
}

function Select2Plugin() {
    
    $('.users-Directly').select2({
        placeholder: selectPlaceholder,
        allowClear: true,
        //minimumInputLength: 3,
        language: {
            inputTooShort: function () {
                return inputTooShort;
            },
            searching: function () {
                return searching;
            },
            noResults: function () {
                return;
            },
            loadingMore: function () {
                return noResults;
            }
        },
        tags: true,
        createTag: function (params) {
            var term = $.trim(params.term);

            if (term === '') {
                return null;
            }

            return {
                id: 'new_item//' + term,
                text: term,
                newTag: true // add additional parameters
            };
        },
        ajax: {
            delay: 500,
            url: '/Customer/Users',
            data: function (params) {
                var query = {
                    search: params.term,
                    page: params.page || 1
                };
                // Query parameters will be ?search=[term]&page=[page]
                return query;
            },
            processResults: function (data, params) {
                
                $('.users-Directly').change("select2:closing", function (e) {
                    
                    var selectedUserId = $(".users-Directly").val();
                    console.log(data.results)
                    $.each(data.results, function (i, item) {
                         
                        if (selectedUserId == item['id']) {
                            console.log(data.results[i])
                            $(".surname").val(data.results[i]['surname'])   
                            $(".fatherName").val(data.results[i]['fatherName'])
                            $(".name").val(data.results[i]['name'])
                            $(".address").val(data.results[i]['address'])
                            $(".workAddress").val(data.results[i]['workAddress'])
                            $(".InstagramAddress").val(data.results[i]['instagramAddress'])
                            $(".baseNumber").val(data.results[i]['baseNumber'])
                            $(".firstName").val(data.results[i]['firstNumberWho'])
                            $(".firstNumber").val(data.results[i]['firstNumber'])
                            $(".secondName").val(data.results[i]['secondNumberWho'])
                            $(".secondNumber").val(data.results[i]['secondNumber'])
                            $(".thirdName").val(data.results[i]['thirdNumberWho'])
                            $(".thirdNumber").val(data.results[i]['thirdNumber'])
                            $(".WhoIsOkey").val(data.results[i]['whoIsOkey'])
                            $(".cardInfo").val(data.results[i]['cardId']).prop("selected", true)
                        }
                        

                    });
                    data.results = []
                });
                params.page = params.page || 1;
                
                return {

                    results: data.results,
                    pagination: {
                        more: (params.page * 20) < data.count_filtered

                    }
                };
            }

        } 
    });
 
    
    
     
}