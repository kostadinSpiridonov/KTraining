$(document).ready(function () {
    $('.fancybox').fancybox();
    $('.add-asnwer-btn').click(function () {
        $('.add-answer-panel').removeClass('hide');
        $(this).addClass('hide');
    });
    $('.edit-question-btn').click(function () {
        $('.edit-question-panel').removeClass('hide');
        $(this).addClass('hide');
    });
    $('.edit-answer-btn').click(function () {

        $(this).parent().children(".edit-answer-panel").removeClass("hide");
        $(this).addClass('hide');
    });

    $('.upload-image-question-btn').click(function () {
        $('.upload-image-question-panel').removeClass('hide');
        $(this).addClass('hide');
    });

    $('.upload-image-answer-btn').click(function () {

        $(this).parent().children(".upload-image-answer-panel").removeClass("hide");
        $(this).addClass('hide');
    });
  
});