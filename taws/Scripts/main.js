"use strict";

(function ($) {
  $(document).ready(function () {
    // Pagetop
    var pageTop = $('.page-top');
    pageTop.hide();
    $(window).scroll(function () {
      if ($(this).scrollTop() > 256) {
        pageTop.fadeIn();
      } else {
        pageTop.fadeOut();
      }
    });
    pageTop.click(function () {
      $('body, html').animate({
        scrollTop: 0
      }, 480);
      return false;
    });
  });
})(jQuery);