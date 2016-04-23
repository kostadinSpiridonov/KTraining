$(document).ready(function () {
    function getCookie(key) {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }

    $.ajax({
        type: "GET",
        url: "/Course/LatestCourses",
        traditional: true,
        success: function (data) {
            $("#latest-courses").html(unescape(data));

        }
    });

    $.ajax({
        type: "GET",
        url: "/User/FirstUserName",
        traditional: true,
        success: function (data) {
            var culture = getCookie("_lang").toString();
            var val;
                   if (culture === 'en') {
                       val = "Hello, "
            }
                   else if (culture === 'bg') {
                       val = "Здравейте, "
            }
            $(".hello").html(val +unescape(data));

        }
    });
    $.ajax({
        type: "GET",
        url: "/Course/MostFamous",
        traditional: true,
        success: function (data) {
            $("#most-famous-courses").html(unescape(data));

        }
    });
    $.ajax({
        type: "GET",
        url: "/Notification/CountNotifications",
        traditional: true,
        success: function (data) {
            if (parseInt(data) > 0) {
                var culture = getCookie("_lang").toString();
                var val1,val2;
                       if (culture === 'en') {
                           val1 = "You have ";
                    val2=" new notifications"
                }
                       else if (culture === 'bg') {
                           val1 = "Имате ";
                    val2 = " нови известия"
                }
                var content = val1 + unescape(data) + val2;
                $(".nottf").html('<a href=/Notification>' + content + '<\a>');
            }

        }
    });
});