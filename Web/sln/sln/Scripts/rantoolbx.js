$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('#whatsup').click(function () {
        var url = $(this).attr("data-url");
        window.location.href = "whatsapp://send?text=" + url;

    });
    $('#waze').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#print').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#calc').click(function () {
        var url = $(this).attr("data-url");
        // alert(url);
        // changeUrl(url);
    });
    $('#end').click(function () {
        var url = $(this).attr("data-url");
        //  alert(url);
        changeUrl(url);
    });
});
