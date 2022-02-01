var customerId ;
$(document).ready(function () {
    CreateDatatable();
    EditAndChange();
})
function EditAndChange() {
    
    $(".blockUser").on("click", function () {
       
        var formData = new FormData();
        var CustomerId = $(".blockUser").attr("data-CustomerIdBlock")
        var NoteBlock = $(".noteBlock").val();
        formData.append('CustomerId', CustomerId)
        formData.append('NoteBlock', NoteBlock)
        if (confirm('İstifadəcinin blok etmek istiyrsiz?')) {
            $.ajax({
                type: 'POST',
                url: '/Customer/AddBlock',
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
function CreateDatatable() {
    var table = $('#table').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'excelHtml5'
        ],
        "language": {
            "search": "",
            "emptyTable": "Heç bir məlumat yoxdur",
            "sSearchPlaceholder": "Axtar",
            "sLengthMenu": "_MENU_ sətir göstər",
            "paginate": {
                "previous": "Əvvəlki",
                "next": "Sonrakı",
                "first": "Birinci",
                "last": "Axırıncı"
            },
            "info": "_TOTAL_ sətirdən _START_-dən _END_-kimi göstərir",
            "infoEmpty": "0 nəticə",
            "infoFiltered": "( _MAX_ sətirdən filter olunub)",
            "zeroRecords": "Uyğun nətica tapılmadı",
        }
    });
    table.columns().every(function () {
        var that = this;
        $('input', this.header()).on('keyup change', function () {
            if (that.search() !== this.value) {
                that
                    .search(this.value)
                    .draw();
            }
        });
    });
    $('#table th input').on('click', function () {
        return false;
    });
    
    $(document).on("dblclick", ".TableBodyDetails tr", function () {
        window.location.href = $(this).attr('data-href');
    })
    $(document).on("click", ".customerEdit", function () {
        window.location.href = $(this).attr('data-href');
    })
    $(document).on("click", ".modalCustomer", function () {
        customerId = $(this).attr("data-CustomerId")
        $(".blockUser").attr("data-CustomerIdBlock", customerId);
    })
}
 