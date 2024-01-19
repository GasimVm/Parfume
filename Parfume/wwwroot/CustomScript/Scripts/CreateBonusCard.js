var selectPlaceholder = 'Seçin';
var inputTooShort = 'Axtarış üçün minimum 3 hərf daxil edin.';
var searching = 'Axtarılır...';
var noResults = 'Uyğun nəticə tapılmadı.';
$(document).ready(function () {

    CreateBonusCard();
    Select2Plugin()
    

})

function CreateBonusCard() {
    $(".addBonusCard").click(function () {
        var formData = new FormData();
        var name = $(".name").val();
        var surname = $(".surname").val();
        var fatherName = $(".fatherName").val();
        var baseNumber = $(".baseNumber").val();
        var fincode = $("#UsersDirectly option:selected").text();
        var CustomerId = $("#UsersDirectly").val();
        var cardNumber = $(".cardNumber").val();
        var cardTypeId = $(".cardType option:selected").val();
        var balans = $(".cardType option:selected").text();
        if (name.length == 0) {
            alert("Ad yazmaq mecburidi")
            return
        } else if (cardNumber.length == 0) {
            alert("Kartın nömrəsini yazmaq mecburidi")
            return
        } else if (baseNumber.length == 0) {
            alert("Nömrə yazmaq mecburidi!")
            return
        } else if (cardTypeId.length == 0) {
            alert("Bonus kart secilmelidir  boş ola bilməz!")
            return
        }  
        formData.append('name', name)
        formData.append('surname', surname)
        formData.append('fatherName', fatherName)
        formData.append('baseNumber', baseNumber)
        formData.append('fincode', fincode)
        formData.append('CustomerId', CustomerId)
        formData.append('cardTypeId', cardTypeId)
        formData.append('cardNumber', cardNumber)
        formData.append('balans', balans)

        if (confirm('Karti qeydə almaq istiyirsiz?')) {
            $(".addBonusCard").hide();

            $.ajax({
                type: 'POST',
                url: '/BonusCard/CreateBonusCard',
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
                        $(".addBonusCard").show();
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
                            $(".name").val(data.results[i]['name'])
                            $(".surname").val(data.results[i]['surname'])
                            $(".fatherName").val(data.results[i]['fatherName'])
                            $(".baseNumber").val(data.results[i]['baseNumber'])
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