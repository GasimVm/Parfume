var isDraft = false;
//$.validator.setDefaults({ ignore: null });
$(document).ready(function () {
    DatePickers();
    $('#draft').on('click', function () {
        isDraft = true;
        $('#qmFrm').submit();
    });
    $('#final').on('click', function () {
        isDraft = false;
        $('#qmFrm').submit();
    });
    OnSubmit();
    OnSelectChangeValidation();
});
function DatePickers() {
    $('.dt').datetimepicker({
        format: 'DD/MM/YYYY'
    });
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
            if (isDraft) {
                $('<input />').attr('type', 'hidden')
                    .attr('name', "Status")
                    .attr('value', 1)
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