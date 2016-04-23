$(document).ready(function () {
    $('.select-topic').on("change", function () {
        if (this.checked) {
            $(this).parent().parent().children().last().children(".question-count-topic").val(10);
        }
        if (!this.checked) {
            $(this).parent().parent().children().last().children(".question-count-topic").val(0);
        }
    });
});