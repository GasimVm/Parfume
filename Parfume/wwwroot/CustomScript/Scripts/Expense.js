$(document).ready(function () {
    $('#datetimeExpense').datetimepicker({
        format: 'DD/MM/YYYY',
        sideBySide: false
    });
    $(".sendExpense").click(function () {
        var formData = new FormData();

        var name = $(".name").val();
        var money = $(".money").val();
        var date = $("#Create_Date").val();
        formData.append("name", name)
        formData.append("money", money)
        formData.append("date", date)
        $.ajax({
            type: 'POST',
            url: '/Expense/Create',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.success === "success") {
                    alert(response.message)

                } else {
                    alert(response.message)
                }
            }

        })
        refresh()
    })
    CreateDataTable();
    DeleteCard();
})
function refresh() {
    setTimeout(function () {
        location.reload()
        return false
    }, 1000);
}

function CreateDataTable() {

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
}

function DeleteCard() {
        $(document).on("click", ".expenseDelete", function () {
        var formData = new FormData();

        var ExpenseId = $(this).attr("data-expenseId");

        formData.append('ExpenseId', ExpenseId)
        $.ajax({
            type: 'POST',
            url: '/Expense/ExpenseDelete',
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