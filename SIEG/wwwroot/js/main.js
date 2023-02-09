(function ($) {
    "use strict";

    // load
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();

    // wow

    var wow = new WOW(
        {
            mobile: true,
        }
    );
    new WOW().init();


    // Sticky Navbar
    $(window).scroll(function () {
        if ($(this).scrollTop() > 0) {
            $('.sticky-top').addClass('shadow-sm').css('top', '0px');
        } else {
            $('.sticky-top').removeClass('shadow-sm').css('top', '-100px');
        }
    });


    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500, 'easeInOutExpo');
        return false;
    });


    //Facts counter
    $('[data-toggle="counter-up"]').counterUp({
        delay: 10,
        time: 2000
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        dots: false,
        loop: true,
        nav: true,
        navText: [
            '<i class="bi bi-chevron-left"></i>',
            '<i class="bi bi-chevron-right"></i>'
        ]
    });


    // search toggle
    $(function () {
        $(".header_search").click(function () {
            $(".header_search_form").slideToggle();
        })
    });


    // index
    $('.banner-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        dots: true,
        nav: false,
    })

    $('.index_brand-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        smartSpeed: 1000,
        items: 3,
        margin: 24,
        dots: false,
        nav: false,
        responsive: {
            0: {
                items: 2,
                margin: 12
            },
            576: {
                items: 2,
                margin: 24
            },
            768: {
                items: 3
            },
        }
    })

    $('.index_ad-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        dots: false,
        nav: false,
    })

    $('.index_news-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        smartSpeed: 1000,
        items: 4,
        margin: 24,
        dots: false,
        nav: false,
        responsive: {
            0: {
                items: 1
            },
            768: {
                items: 2
            },
            992: {
                items: 3
            },
            1200: {
                items: 4
            }
        }
    })


    // 側欄分類-下拉
    $(".sub_child").hide();
    $(".has_child_btn").click(function () {
        $(this).parent().next().slideToggle();
        $(this).parent().parent().toggleClass("active");
    })


    //about-member
    $('.about_member-carousel').owlCarousel({
        loop: true,
        autoplay: true,
        smartSpeed: 1000,
        items: 1,
        margin: 0,
        dots: false,
        nav: false,
    })




})(jQuery);

