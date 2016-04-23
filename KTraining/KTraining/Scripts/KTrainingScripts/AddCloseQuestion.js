$(document).ready(function () {
    function getCookie(key) {
        var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
        return keyValue ? keyValue[2] : null;
    }
    var culture = getCookie("_lang").toString();
    var val1, val2;
    if (culture === 'en') {
        val1 = "Choose";
        val2 = "Remove";
    }
    else if (culture === 'bg') {
        val1 = "Избери";
        val2 = "Премахни";
    }
    $("#input-4").fileinput({ showCaption: false, showUpload: false, browseLabel: val1, removeLabel: val2 });
    $(".input-5").fileinput({ showCaption: false, showUpload: false, browseLabel: val1, removeLabel: val2 });

    $(document).ready(function () {
        tinymce.init({
            language: "bg_BG",
            selector: "textarea",
            menubar: false,
            statusbar: false,
            plugins: [
                "advlist autolink lists link image charmap print preview anchor",
                "searchreplace visualblocks code fullscreen",
                "insertdatetime media table contextmenu paste"
            ],
            toolbar: "insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent"
        });
    });

    function initMCEexact(e) {
        tinyMCE.init({
            mode: "exact",
            elements: e
        });
    }
    $("#addAnswer").click(function () {
        var count = $(".answer").length;
        var newHtml = $(".answer").first()[0].outerHTML;
        var res = replaceAll('[0]', '[' + count + ']', newHtml);
        res = replaceAll('_0_', '_' + count + '_', res);
        $("#answers").append(res);
        initMCEexact($("textarea").last().attr("id"));
        tinyMCE.triggerSave();
        $(".input-5").last().parent().parent().remove();
        var html = '<input class="input-5" formenctype="multipart/form-data" id="Answers_' + count + '__Images" multiple="multiple" name="Answers[' + count + '].Images" type="file" value="System.Web.HttpPostedFileBase[]">';
        $(".answer").children(".form-group").last().children('.col-md-10').first().append(html);
        var culture = getCookie("_lang").toString();
        var val11, val12;
        if (culture === 'en') {
            val11 = "Choose";
            val12 = "Remove";
        }
        else if (culture === 'bg') {
            val11 = "Избери";
            val12 = "Премахни";
        }
        $(".input-5").last().fileinput({ showCaption: false, showUpload: false, browseLabel: val11, removeLabel: val12 });

    });

    function replaceAll(find, replace, str) {
        while (str.indexOf(find) > -1) {
            str = str.replace(find, replace);
        }
        return str;
    }
});