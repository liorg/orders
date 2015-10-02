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
    $("#btnEdit").click(function (e) {
        //debugger;
        e.preventDefault();
        if ($("#frm").valid()) {
            //submit
            $("#frm").submit();
        }
        else {
            alert("יש למלא שדות חובה");
            return false;
        }
    });
    $('#btnBack').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#btnCancel').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#btnNext2').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#btnNext3').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });
    $('#btnAddItem').click(function () {
        var url = $(this).attr("data-url");
        changeUrl(url);
    });

});
