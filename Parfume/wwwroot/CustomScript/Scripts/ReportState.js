 
$(document).ready(function () {
    $('#Lastdate').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    ApplyBtn();
});
 
function ApplyBtn() {
    $("#RepotStart").click(function () {
        var dateRange = $("#Last_date").val();
        var formData = new FormData();
        formData.append('dateRange', dateRange)
        console.log(dateRange)
        $(".SaleMany").val("");
        $(".OrderCount").val("");
        $(".CachOrder").val("");
        $(".CrediteOrder").val("");
        $(".Debt").val("");
        $(".CachMany").val("");
        $(".CrediteMany").val("");
        $(".GeneralMany").val("");
        $(".income").val("");
        $(".NeededMany").val("");
        $(".GeneralBalans").val("");
        $(".FirstBalans").val("");
        $(".expense").val("");
        $(".Bonus").val("");
        $.ajax({
            type: 'POST',
            url: '/Report/ReportState',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log(response.data)
                $(".SaleMany").val(response.data["saleMany"]);
                $(".OrderCount").val(response.data["orderCount"]);
                $(".CachOrder").val(response.data["cachOrder"]);
                $(".CrediteOrder").val(response.data["crediteOrder"]);
                $(".Debt").val(response.data["debt"]);
                $(".CachMany").val(response.data["cachMany"]);
                $(".CrediteMany").val(response.data["crediteMany"]);
                $(".GeneralMany").val(response.data["generalMany"])
                $(".income").val(response.data["income"])
                $(".NeededMany").val(response.data["neededMany"])
                $(".GeneralBalans").val(response.data["generalBalans"])
                $(".FirstBalans").val(response.data["firstBalans"])
                $(".expense").val(response.data["expense"])
                $(".Bonus").val(response.data["bonus"])

            },
            error: function (response) { console.log(response.data) }
        });

    })

}