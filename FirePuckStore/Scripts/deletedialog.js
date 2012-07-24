$(document).ready(function () {
    $('#dialog').dialog({
        modal: true,
        autoOpen: false,
        resizable: false
    });

    $('#cancelButton').click(function (e) {
        $('#dialog').dialog("close");
        e.preventDefault();
    });

    $('.deleteCard').click(function (e) {
        $('#confirm').attr('action', e.target.href);
        $('#dialog').dialog("open");
        e.preventDefault();
    });

    $("input:submit, #cancelButton").button();
});