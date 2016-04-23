$(document).ready(function () {
    GetUsers(1);
    GetCourses(1);
    $('.page-link-course:contains("' + parseInt(1) + '")').parent().addClass("active");
    $('.page-link-user:contains("' + parseInt(1) + '")').parent().addClass("active");

    $('.page-link-user').click(function () {
        var page = $(this).html()
        $(".page-link-user").each(function () {
            $(this).parent().removeClass("active");
        });
        $(this).parent().addClass("active");
        GetUsers(page);
    });

    $('.page-link-course').click(function () {
        var page = $(this).html()
        $(".page-link-course").each(function () {
            $(this).parent().removeClass("active");
        });
        $(this).parent().addClass("active");
        GetCourses(page);
    });

    $('#next-course').click(function () {
        var page = parseInt($('.active').children('.page-link-course').text());
        if (page + 1 <= $('#courses-page-count').val()) {

            $(".page-link-course").each(function () {
                $(this).parent().removeClass("active");
            });
            $('.page-link-course:contains("' + parseInt(page + 1) + '")').parent().addClass("active");
            GetCourses(page + 1);
        }
    });

    $('#previous-course').click(function () {
        var page = parseInt($('.active').children('.page-link-course').text());
        if (page - 1 > 0) {

            $(".page-link-course").each(function () {
                $(this).parent().removeClass("active");
            });
            $('.page-link-course:contains("' + parseInt(page - 1) + '")').parent().addClass("active");
            GetCourses(page - 1);
        }
    });

    $('#next-user').click(function () {
        var page = parseInt($('.active').children('.page-link-user').text());
        if (page + 1 <= $('#users-page-count').val()) {

            $(".page-link-user").each(function () {
                $(this).parent().removeClass("active");
            });
            $('.page-link-user:contains("' + parseInt(page + 1) + '")').parent().addClass("active");
            GetUsers(page + 1);
        }
    });

    $('#previous-user').click(function () {
        var page = parseInt($('.active').children('.page-link-user').text());
        if (page - 1 > 0) {

            $(".page-link-user").each(function () {
                $(this).parent().removeClass("active");
            });
            $('.page-link-user:contains("' + parseInt(page - 1) + '")').parent().addClass("active");
            GetUsers(page - 1);
        }
    });

    function GetCourses(page) {
        var word = $('#search-word').val();
        $.ajax({
            type: "GET",
            url: "/Search/SearchCourse",
            data: { q: word, page: page },
            traditional: true,
            success: function (data) {
                $('.courses').html(data);
            }
        });
    }

    function GetUsers(page) {
        var word = $('#search-word').val();
        $.ajax({
            type: "GET",
            url: "/Search/SearchUser",
            data: { q: word, page: page },
            traditional: true,
            success: function (data) {
                $('.users').html(data);
            }
        });
    }
});