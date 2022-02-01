var isDenied = false;
//$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    $('#SenedinTarixi').val($('#ST').val());
    $('#SenedinDaxilOlmaTarixi').val($('#SDT').val());
    $('#deny').on('click', function () {
        isDenied = true;
        $('#qmFrm').submit();
    });
    $('#final').on('click', function () {
        isDenied = false;
        $('#qmFrm').submit();
    });
    $('.SifarisinNovu').val($('#SifarisinNovu').val());
    OnSubmit();
    OnSelectChangeValidation();
    Selectize();
    DatePickers();
});
function DatePickers() {
    $('#SenedinTarixi').datetimepicker({
        format: 'DD/MM/YYYY'
    });
    $('#SenedinDaxilOlmaTarixi').datetimepicker({
        format: 'DD/MM/YYYY'
    });
}
function Selectize() {
    $('select').selectize({
        placeholder: 'Axtar'
    });
}

function OnSubmit() {
    counter = 0;
    $('#qmFrm').on('submit', function () {
        var form = $(this);
        var isValid = true;
        $.validator.unobtrusive.parse(form);
        form.validate();
        $('select').each(function (i) {
            if (!$(this).val()) {
                $(this).parents('div').first().find('span').html('*Daxil edilməlidir');
                form.valid(false);
                isValid = false;
                return;
            }
        });
        if (form.valid() && isValid) {
            if (isDenied) {
                $('<input />').attr('type', 'hidden')
                    .attr('name', "Status")
                    .attr('value', 6)
                    .appendTo('#qmFrm');
            } else {
                $('<input />').attr('type', 'hidden')
                    .attr('name', "Status")
                    .attr('value', 2)
                    .appendTo('#qmFrm');
            }
            return true;
        } else {
            return false;
        }
    });
}

function OnSelectChangeValidation() {
    $('select').on('change', function () {
        selected = $(this).val();
        $(this).parents('div').first().find('input').attr('value', selected);
        if (selected) {
            $(this).parents('div').first().find('span').html('');
        } else {
            $(this).parents('div').first().find('span').html('*Daxil edilməlidir');
        }
    });
}