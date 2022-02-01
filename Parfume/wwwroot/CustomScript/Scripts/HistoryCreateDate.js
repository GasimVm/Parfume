var isAllSelected = false;
var documentUrlIndex = 0;
var start = moment().subtract(1, 'month');
var end = moment();
var excelData;
var dateRange;
$(document).ready(function () {
    DateRang();
    ApplyBtn();
});

function DateRang() {
    function cb(start, end) {
        if (!isAllSelected) {
            $('#reportrange span').html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY')).change();
        } else {
            $('#reportrange span').html('Hamısı').change();
        }
    }

    if (!start._isValid || !end._isValid) {
        start = moment().subtract(1, 'month');
        end = moment();
    }



    $('#reportrange').daterangepicker({
        startDate: start,
        endDate: end,
        //"alwaysShowCalendars": true,
        ranges: {
            'Hamısı': [null, null],
            'Bu gün': [moment(), moment()],
            'Dünən': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Son 7 gün': [moment().subtract(6, 'days'), moment()],
            'Son 30 gün': [moment().subtract(29, 'days'), moment()],
            'Bu ay': [moment().startOf('month'), moment().endOf('month')],
            'Keçən ay': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Tətbiq et",
            "cancelLabel": "Bağla",
            "fromLabel": "From",
            "toLabel": "To",
            "customRangeLabel": "Aralıq seç"
        }
    }, cb);
    cb(start, end);
}

function ApplyBtn() {
    $("#reportrange").change(function () {
        dateRange = $('#reportrange span').html();
        var formData = new FormData();
        formData.append('dateRange', dateRange)
        $.ajax({
            type: 'POST',
            url: '/Home/HistoryPaymentDate',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                 

            }
        });

    })

}