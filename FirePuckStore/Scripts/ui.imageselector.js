$(document).ready(function () {

    var image = $('#logo');
    if (image.attr('src') == null || image.attr('src') == '') {
        $('.editor-image').css('width', '300px');
    }
    else {
        $('#uploadLabel').remove();
        $('.editor-image').css('width', '');
        $('#logo').css({ 'visibility': 'visible', 'height': '400px' });
    }

    $('.editor-image').mousedown(function (e) {
        $('#fileInput').click();
    });
});

function handleFiles(files) {
    if (files.length == 0) {
        return;
    }
    var file = files[0];        
    if (!file.type.match('image.*')) {
        return;
    }
    var fileReader = new FileReader();

    fileReader.onload = (function (theFile) {
        return function (e) {
            $('#uploadLabel').remove();
            $('.editor-image').css('width', '');
            $('#logo').css({ 'visibility': 'visible', 'height': '400px' });
            $('#logo').attr('src', e.target.result);
            $('#filePath').text(theFile.name);
        };
    })(file);

    fileReader.readAsDataURL(file);
}
