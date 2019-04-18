import jquery from "jquery";
import { render } from 'timeago.js';

export default (function ($) {
    var my = {};
    my.init = function () {
        render($('.relativeTime'), );
    };
    return my;
}(jquery));
