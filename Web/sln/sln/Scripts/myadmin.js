﻿
$('.btn-minuse').on('click', function () {
    $(this).parent().siblings('input').val(parseInt($(this).parent().siblings('input').val()) - 1)
})

$('.btn-pluss').on('click', function () {
    alert(2);
    $(this).parent().siblings('input').val(parseInt($(this).parent().siblings('input').val()) + 1)
})
