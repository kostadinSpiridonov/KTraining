$(document).ready(function () {

    function getCookie(key) {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }

    $(".participate").click(function () {
        var culture = getCookie("_lang").toString();
        var val;
        if (culture === 'en') {
            val = "The request is sent"
        }
        else if (culture === 'bg') {
            val = "Изпратена е заявка"
        }
        var id = $(this).attr("id");
        var url = "/Request/SendRequestToJoin/" + id;
        $.post(url, function () {
            var htm = ' <input disabled value="' + val + '" class="btn btn-default" />';
            $("input[id=" + id + "]").parent().html(htm);
        });
    });
});