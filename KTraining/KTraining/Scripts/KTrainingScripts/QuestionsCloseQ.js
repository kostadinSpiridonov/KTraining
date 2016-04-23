$(document).ready(function () {
    function getCookie(key) {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }
    var culture = getCookie("_lang").toString();
    var val1, val2,val3;
    if (culture === 'en') {
        val1 = "Choose";
        val2 = "Remove";
        val3 = "Upload";
    }
    else if (culture === 'bg') {
        val1 = "Избери";
        val2 = "Премахни";
        val3 = "Качи";
    }
    $("#fiele_upload").fileinput({ showCaption: false, browseLabel:val1, removeLabel: val2 ,uploadLabel:val3});
    $("#import-btn").click(function () {
        {
            if ($("#import-panel").hasClass("hide")) {
                $("#import-panel").removeClass("hide");
            }
            else {
                $("#import-panel").addClass("hide");
            }
        }
    });
});