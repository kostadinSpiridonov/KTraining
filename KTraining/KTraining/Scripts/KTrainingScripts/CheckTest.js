$(document).ready(function () {
    function getCookie(key) {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }

    function Mark() {
        var points = 0;
        var maxP = parseFloat($(".maxPoints").val());
        var rate = parseFloat($(".rate").val());
        $(".test-point").each(function (index) {
            if (!IsNumeric($(this).val()) )
            {
                $(this).val(0);
            }
            points += parseFloat($(this).val());
        });
        htm = ((parseFloat(points) - parseFloat(rate) * parseFloat(maxP) / 100) * 3) / (parseFloat(maxP) - parseFloat(rate) * parseFloat(maxP) / 100) + parseFloat(3);
        if (htm > 6) {
            htm = 6;
        }
        if (htm < 2) {
            htm = 2;
        }
        var culture = getCookie("_lang").toString();
        var val1;
        if (culture === 'en') {
            val1 = "Mark: ";
        }
        else if (culture === 'bg') {
            val1 = "Оценка: ";
        }
        $(".markS").html(val1 + parseFloat(htm).toFixed(2));
        $("#mark-hidden").val(parseFloat(htm).toFixed(2));
    }
    function IsNumeric(input) {
        var RE = /^-{0,1}\d*\.{0,1}\d+$/;
        return (RE.test(input));
    }
    Mark();
    $(".test-point").focusout(function () {
        if (isNaN(t))
        {
            $(this).val(0);
        }
    });
    $(".test-point").keyup(function () {
        var t = parseFloat($(this).val());
        var m = parseFloat($(this).attr("max"));
        if (isNaN(t))
        {
            return;
        }
        if (t > m) {
            $(this).val($(this).attr("max"));
        }
        if ($(this).val() < 0) {
            $(this).val(0);
        }
        Mark();
    });
});