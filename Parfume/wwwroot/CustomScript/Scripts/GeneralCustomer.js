var customerId;
$(document).ready(function () {
    CreateDatatable();
    EditAndChange();

})
function EditAndChange() {
    $(".modalCustomer").click(function () {
        customerId = $(this).attr("data-CustomerId")

    });
     

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
}