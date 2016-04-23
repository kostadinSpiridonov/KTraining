$(document).ready(function () {
    $('.select-close-q').on("change", function () {
        if (this.checked) {
            $(this).parent().parent().children().last().children(".question-count-close").val(10);
        }
        if (!this.checked) {
            $(this).parent().parent().children().last().children(".question-count-close").val(0);
        }
    });
    $('.select-open-q').on("change", function () {
        if (this.checked) {
            $(this).parent().parent().children().last().children(".question-count-open").val(5);
        }
        if (!this.checked) {
            $(this).parent().parent().children().last().children(".question-count-open").val(0);
        }
    });
})