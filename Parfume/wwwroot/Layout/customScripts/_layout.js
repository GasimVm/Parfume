$(document).ready(function () {
    if (sessionStorage.getItem('isOpen')) {
        if (parseInt(sessionStorage.getItem('isOpen')) === 0) {
            $('#sidebar, #content').addClass('active');
        }
    }
    $("#sidebar").mCustomScrollbar({
        theme: "minimal"
    });
    $('#sidebar li a').each(function (k) {
        $(this).attr('id', k);
    });
    $('#sidebar li a').on('click', function () {
        sessionStorage.setItem('activeId', $(this).attr('id'));
    });
    CollapseSidebar();
    CollapseOthers();
    ActiveMenu();
    ShowOperationResult();
});

function ShowOperationResult() {
    result = window.location.hash;
    if (result === '#success') {
        alert('Əməliyyat uğurla yerinə yetirildi.');
    } else if (result === '#error') {
        alert('Xəta baş verdi.');
    }
    window.location.hash = '';
}

function CollapseSidebar() {
    $('#sidebarCollapse').on('click', function () {
        isOpen = sessionStorage.getItem('isOpen');
        $('#sidebar, #content').toggleClass('active');
        if (isOpen) {
            if (parseInt(isOpen) === 1) {
                sessionStorage.setItem('isOpen', 0);
            } else if (parseInt(isOpen) === 0) {
                sessionStorage.setItem('isOpen', 1);
            }
        } else {
            sessionStorage.setItem('isOpen', 0);
        }
        $('.collapse.in').toggleClass('in');
        $('a[aria-expanded=true]').attr('aria-expanded', 'false');
    });

}

function ActiveMenu() {
    if (sessionStorage.getItem('activeId')) {
        var activeId = sessionStorage.getItem('activeId');
        $('#' + activeId).css("background-color", "black");
        $('#' + activeId).parents('ul').closest('a').attr('aria-expanded', 'true');
        $('#' + activeId).parents('ul').addClass('show');
    }
}

function CollapseOthers() {
    $('#sidebar a.menu').on('click', function () {
        var currentId = parseInt($(this).attr('id'));
        $('li a.menu').each(function () {
            if (currentId !== parseInt($(this).attr('id')) && !$(this).hasClass('collapsed')) {
                var ul = $(this).parent().find('ul');
                $(this).attr('aria-expanded', 'false');
                $(this).addClass('collapsed');
                $(this).parent().find('ul').hide(300);
                setTimeout(function () {
                    ul.removeClass('show');
                    ul.attr('style', '');
                }, 350);
            }
        });
    });
}
