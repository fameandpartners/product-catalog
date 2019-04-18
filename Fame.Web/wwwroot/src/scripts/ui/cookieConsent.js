import jquery from "jquery";

export default (function ($) {
    var my = {};
    my.init = function () {

        $("#cookieConsent button[data-cookie-string]").click(function () {
            var button = $(this)[0];
            var cookieString = button.attributes["data-cookie-string"].value;
            document.cookie = cookieString;
            $("#cookieConsent").remove();
        });

    };
    return my;
}(jquery));
