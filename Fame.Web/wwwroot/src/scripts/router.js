import notificationClose from "./ui/notificationClose";
import cookieConsent from "./ui/cookieConsent";
import relativeTime from "./ui/relativeTime";

//http: //viget.com/inspire/extending-paul-irishs-comprehensive-dom-ready-execution
var bc_routes = {

    // Executes on every page
    common: {
        init: function () {
            notificationClose.init();
            cookieConsent.init();
        }
    },

    root: {
        // Executes on every action in every controller in root
        common: {
            init: function () {
            }
        },
        // Home controller
        home: {
            // Executes on every action
            init: function () {
            },

            // Executes on the Index action
            index: function () {
            }
        },
    },

    // Area Routes Go Here e.g.
    admin: {
        common: {
        },
        home: {
            // Executes on every action
            init: function () {
            },

            // Executes on the Index action
            index: function () {
                relativeTime.init();
            }
        },
    }
};

var UTIL = {

    exec: function (areaIn, controllerIn, actionIn) {
        var ns = bc_routes;

        var areaDefault = "root";
        var controllerDefault = "common";
        var actionDefault = "init";

        if (areaIn === undefined) {
            ns[controllerDefault][actionDefault]();
        }
        else {
            var action = (actionIn === undefined) ? actionDefault : actionIn;
            var area = (areaIn === "") ? areaDefault : areaIn;
            var controller = (controllerIn === undefined) ? controllerDefault : controllerIn;

            if (ns[area][controller] !== undefined && typeof ns[area][controller][action] === "function") {
                ns[area][controller][action]();
            }
        }
    },

    init: function () {

        // String Prototype Function for formatting stringss
        String.prototype.format = function () {
            var formatted = this;
            for (var arg in arguments) {
                formatted = formatted.replace("{" + arg + "}", arguments[arg]);
            }
            return formatted;
        };

        var body = document.body;

        var area = body.getAttribute("data-area").toLowerCase();
        var controller = body.getAttribute("data-controller").toLowerCase();
        var action = body.getAttribute("data-action").toLowerCase();

        UTIL.exec();
        UTIL.exec(area);
        UTIL.exec(area, controller);
        UTIL.exec(area, controller, action);
    }

};

$(document).ready(function () {
    UTIL.init();
});