var selectPlaceholder = 'Seçin';
var inputTooShort = 'Axtarış üçün minimum 3 hərf daxil edin.';
var searching = 'Axtarılır...';
var noResults = 'Uyğun nəticə tapılmadı.';
$(document).ready(function () {

    Change();
    SendOrder();
    Select2Plugin()
    $('#Birthdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    $('#datetimepicker3').datetimepicker({
        format: 'DD/MM/YYYY',
        sideBySide: false
    });
    
})

function SendOrder() {
    $(".sendOrder").click(function () {
        var formData = new FormData();
        var fincode = $("#UsersDirectly option:selected").text();
        var referencesId = $(".references option:selected").val();
        var bonusPrice = $(".bonusPrice").val();
        var CustomerId = $("#UsersDirectly").val();
        var surname = $(".surname").val();
        var fatherName = $(".fatherName").val();
        var baseNumber = $(".baseNumber").val();
        var name = $(".name").val();
        var productName = $(".productName").val();
        var quantity = $(".quantity").val();
        var price = $(".price").val();
        var totalPrice = $(".totalPrice").val();
        var amount = $(".amount").val();
        var cost = $(".cost").val();
        var dateCreate = $("#Create_Date").val();
        var InstagramAddress = $(".InstagramAddress").val();
        var dateBirth = $("#Birth_Date").val();


        if (name.length == 0) {
            alert("Ad yazmaq mecburidi")
            return
        } else if (productName.length == 0) {
            alert("Parfumun adı yazmaq mecburidi")
            return
        }  else if (totalPrice.length == 0) {
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
        formData.append('fatherName', fatherName)
        formData.append('baseNumber', baseNumber)
        formData.append('fincode', fincode)
        formData.append('quantity', quantity)
        formData.append('price', price)
        formData.append('amount', amount)
        formData.append('productName', productName)
        formData.append('totalPrice', totalPrice)
        formData.append('InstagramAddress', InstagramAddress)
        formData.append('CustomerId', CustomerId)
        formData.append('cost', cost)
        formData.append('dateCreate', dateCreate)
        formData.append('dateBirth', dateBirth)
        formData.append('referencesId', referencesId)
        formData.append('bonusPrice', bonusPrice)

        if (confirm('Sifarişi qeydə almaq istiyirsiz?')) {
            $(".sendOrder").hide();

            $.ajax({
                type: 'POST',
                url: '/Order/CreateOrderCash',
                data: formData,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.status === "success") {

                        alert("Qeydə alındı!")
                        window.location.reload();
                    }
                    else {
                        alert(response.message)
                        $(".sendOrder").show();
                    }
                }
            });
        }

      
    })
}

function Change() {
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
            url: '/Customer/UsersWithPhone',
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
                            $(".references").val(data.results[i]['referencesId']).prop("selected", true)

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