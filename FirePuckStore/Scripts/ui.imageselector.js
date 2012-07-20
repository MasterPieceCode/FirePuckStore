$(document).ready(function () {

    var image = $('#logo');
    // if no image set, then setup default width
    if (image.attr('src') == null || image.attr('src') == '') {
        $('.editor-image').css('width', '300px');
    }
    else {
        hidePredefinedUploadDiv();
    }

    $('.editor-image').mousedown(function (e) {
        if (e.which == '1') {
            $('#FileInput').click();            
        }
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
            $('#logo').attr('src', e.target.result);
            hidePredefinedUploadDiv();
        };
    })(file);

    fileReader.readAsDataURL(file);
}

function hidePredefinedUploadDiv() {
    $('#uploadLabel').remove();
    $('.editor-image').css('width', '');
    $('#logo').css({ 'visibility': 'visible', 'height': '400px' });
}
