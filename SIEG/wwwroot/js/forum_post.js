/*標題、內文自動調適高度*/
document.addEventListener('DOMContentLoaded', function () {
    autosize(document.querySelectorAll('#story'));
}, false);

$('document').ready(function () {

    /*送出按鈕*/
    $("#submit").click(function () {
        var target = $(this);
        if (target.hasClass("done")) {
            // do nothing
        } else {
            target.addClass("processing");
            setTimeout(function () {
                target.removeClass("processing");
                target.addClass("done");
            }, 2200);
        }
    });

    /*發文頁照片*/
    $('#imageInput').on('change', function () {
        $input = $(this);
        if ($input.val().length > 0) {
            fileReader = new FileReader();
            fileReader.onload = function (data) {
                $('.image-preview').attr('src', data.target.result);
            }
            fileReader.readAsDataURL($input.prop('files')[0]);
            $('.image-button').css('display', 'none');
            $('.change-image').css('display', 'inline');
            $('.image-preview').css('display', 'inline');
        }
    });

    $('.change-image').on('click', function () {
        $control = $(this);
        $('#ImgUpload').val('');
        $preview = $('.image-preview');
        $preview.attr('src', '');
        $preview.css('display', 'none');
        $control.css('display', 'none');
        $('.image-button').css('display', 'inline');
    });

    /*我要回覆鈕*/
    let reply = document.getElementById('Reply_btn');
    reply.addEventListener('click', function () {
        $(this).next(".Reply_block").slideToggle();
    });

});



