import jquery from "jquery";

export default (function ($) {
    var my = {};
    my.init = function () {

        $(".notification img[close]").click(function () {
            $(this)[0].parentElement.remove();
        });

    };
    return my;
}(jquery));