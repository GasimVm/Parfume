$(document).ready(function () {
    AddNote();
    SendBackOnClick();
    SendBackOkClick();
    Approve();
});
function AddNote() {
    $('#qeydElaveEt').on('click', function (e) {
        e.preventDefault();
        if ($('#Qeyd').val()) {
            $.ajax({
                type: 'POST',
                url: $(this).attr('href'),
                data: { qeyd: $('#Qeyd').val(), telebId: $('#ID').val() },
                success: function (response) {
                    if (response) {
                        alert('Əməliyyat uğurla yerinə yetirildi.');
                        window.location.reload();
                    }
                },
                error: function () {
                    alert('Xəta baş verdi.');
                }
            });
        } else {
            $('#qyd_vld_msg').html('*Daxil edilməlidir');
            $('#Qeyd').on('keyup', function () {
                if ($(this).val().length > 0) {
                    $('#qyd_vld_msg').html('');
                } else {
                    $('#qyd_vld_msg').html('*Daxil edilməlidir');
                }
            });
        }
    });
}
function SendBackOnClick() {
    $('.snd-back').on('click', function () {
        $('#sendBackModal').modal('show');
    });
}
function SendBackOkClick() {
    $('#backOk').on('click', function (e) {
        e.preventDefault();
        if ($('#sndBackTextArea').val()) {
            $.ajax({
                type: 'POST',
                url: $(this).attr('href'),
                data: { telebId: $('#ID').val(), qeyd: $('#sndBackTextArea').val() },
                success: function (response) {
                    if (response) {
                        window.location.hash = '#success';
                        window.location.reload();
                    } else {
                        window.location.hash = '#error';
                        window.location.reload();
                    }
                },
                error: function () {
                    window.location.hash = '#error';
                    window.location.reload();
                }
            });
        } else {
            $('#spnSndBack').html('*Daxil edilməlidir');
            $('#sndBackTextArea').keyup(function () {
                if ($(this).val().length > 0) {
                    $('#spnSndBack').html('');
                } else {
                    $('#spnSndBack').html('*Daxil edilməlidir');
                }
            });
        }
    });
}

function Approve() {
    $('.approve-tir').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: $(this).attr('href'),
            success: function (response) {
                if (response) {
                    window.location.hash = '#success';
                    window.location.reload();
                } else {
                    window.location.hash = '#error';
                    window.location.reload();
                }
            },
            error: function () {
                window.location.hash = '#error';
                window.location.reload();
            }
        });
    });
    $('.approve-sm').on('click', function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: $(this).attr('href'),
            data: { telebId: $('#ID').val(), qarsilayanId: $('#qarsilayan').val() },
            success: function (response) {
                if (response) {
                    window.location.hash = '#success';
                    window.location.reload();
                } else {
                    window.location.hash = '#error';
                    window.location.reload();
                }
            },
            error: function () {
                window.location.hash = '#error';
                window.location.reload();
            }
        });
    });
}