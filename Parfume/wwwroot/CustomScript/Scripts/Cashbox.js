
$(document).ready(function () {
    $('#Lastdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    ApplyBtn();
    DataTableCreate();
});

function ApplyBtn() {
    $("#RepotStart").click(function () {
        var dateRange = $("#Last_date").val();
        var userId = $(".userId").children("option:selected").val();
        var formData = new FormData();
        formData.append('dateRange', dateRange)
        formData.append('userId', userId)
        console.log(dateRange)
        $(".CachOrder").remove("");
        $(".message").remove("");
        $("#table").remove("");
        $(".result").remove("");
         
        $.ajax({
            type: 'POST',
            url: '/Order/CashboxState',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
               // console.log(response.data)
                //$(".CachOrder").val(response.data["money"]);
                $(".resultCash").append(response);

                //response.data["CrediteHistories"].each(function (i) {
                //            //$(this).prop('checked', false);
                //            var indexItem = $(".indexItem").last().text() * 1 + 1;
                //            var itemId = $(this).attr("data-itemId");
                //            var itemName = $(this).parent().parent().children(".itemNameAz").text()
                //            var itemCode = $(this).parent().parent().children(".itemCode").text()
                //            var itemMeasure = $(this).parent().parent().children(".itemMeasure").text()
                             
                             
                //                var tr = `<tr class="suplierItem itemRow it">` +
                //                    `<td hidden="" data-Id="` + itemId + `">` +
                //                    `<input name="` + itemName + `" class="itemId" value="` + itemId + `" data-itemId="` + itemId + `"></td>` +
                //                    `<td class="indexItem text-center">` + indexItem + `</td>` +
                //                    `<td >` + itemName + `</td>` +
                //                    `<td>` + itemCode + `</td>` +
                //                    `<td>` + itemMeasure + `</td>` +
                //                    `<td><input type="number" min="1" max="99999" class="itemAmount" name="Amount" size="4" ></td>` +
                //                    `<td><textarea type="text" name="itemNote" class="itemNote" style="height: 25px;"></textarea> </td>` +
                //                    `<td class="delete" data-itemid="` + itemId + `" style="text-align:center;width:50px"><i style="color:red; font-size:16px;" class="fas fa-times"></i></td>` +
                //                    `</tr>`;
                //                $(".TableBodyDetails").append(tr);
                //            });

            },
            error: function (response) { console.log(response.data) },
        });

    })

}
function DataTableCreate() {

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



}