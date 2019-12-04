"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var utils_1 = require("../utils");
var buttons_1 = require("./buttons");
var content_1 = require("./content");
var deprecations_1 = require("./deprecations");
;
var defaultOpts = {
    title: null,
    text: null,
    icon: null,
    buttons: buttons_1.defaultButtonList,
    content: null,
    className: null,
    closeOnClickOutside: true,
    closeOnEsc: true,
    dangerMode: false,
    timer: null,
};
/*
 * Default options customizeable through "setDefaults":
 */
var userDefaults = Object.assign({}, defaultOpts);
exports.setDefaults = function (opts) {
    userDefaults = Object.assign({}, defaultOpts, opts);
};
/*
 * Since the user can set both "button" and "buttons",
 * we need to make sure we pick one of the options
 */
var pickButtonParam = function (opts) {
    var singleButton = opts && opts.button;
    var buttonList = opts && opts.buttons;
    if (singleButton !== undefined && buttonList !== undefined) {
        utils_1.throwErr("Cannot set both 'button' and 'buttons' options!");
    }
    if (singleButton !== undefined) {
        return {
            confirm: singleButton,
        };
    }
    else {
        return buttonList;
    }
};
// Example 0 -> 1st
var indexToOrdinal = function (index) { return utils_1.ordinalSuffixOf(index + 1); };
var invalidParam = function (param, index) {
    utils_1.throwErr(indexToOrdinal(index) + " argument ('" + param + "') is invalid");
};
var expectOptionsOrNothingAfter = function (index, allParams) {
    var nextIndex = (index + 1);
    var nextParam = allParams[nextIndex];
    if (!utils_1.isPlainObject(nextParam) && nextParam !== undefined) {
        utils_1.throwErr("Expected " + indexToOrdinal(nextIndex) + " argument ('" + nextParam + "') to be a plain object");
    }
};
var expectNothingAfter = function (index, allParams) {
    var nextIndex = (index + 1);
    var nextParam = allParams[nextIndex];
    if (nextParam !== undefined) {
        utils_1.throwErr("Unexpected " + indexToOrdinal(nextIndex) + " argument (" + nextParam + ")");
    }
};
/*
 * Based on the number of arguments, their position and their type,
 * we return an object that's merged into the final SwalOptions
 */
var paramToOption = function (opts, param, index, allParams) {
    var paramType = (typeof param);
    var isString = (paramType === "string");
    var isDOMNode = (param instanceof Element);
    if (isString) {
        if (index === 0) {
            // Example: swal("Hi there!");
            return {
                text: param,
            };
        }
        else if (index === 1) {
            // Example: swal("Wait!", "Are you sure you want to do this?");
            // (The text is now the second argument)
            return {
                text: param,
                title: allParams[0],
            };
        }
        else if (index === 2) {
            // Example: swal("Wait!", "Are you sure?", "warning");
            expectOptionsOrNothingAfter(index, allParams);
            return {
                icon: param,
            };
        }
        else {
            invalidParam(param, index);
        }
    }
    else if (isDOMNode && index === 0) {
        // Example: swal(<DOMNode />);
        expectOptionsOrNothingAfter(index, allParams);
        return {
            content: param,
        };
    }
    else if (utils_1.isPlainObject(param)) {
        expectNothingAfter(index, allParams);
        return param;
    }
    else {
        invalidParam(param, index);
    }
};
/*
 * No matter if the user calls swal with
 * - swal("Oops!", "An error occurred!", "error") or
 * - swal({ title: "Oops!", text: "An error occurred!", icon: "error" })
 * ... we always want to transform the params into the second version
 */
exports.getOpts = function () {
    var params = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        params[_i] = arguments[_i];
    }
    var opts = {};
    params.forEach(function (param, index) {
        var changes = paramToOption(opts, param, index, params);
        Object.assign(opts, changes);
    });
    // Since Object.assign doesn't deep clone,
    // we need to do this:
    var buttonListOpts = pickButtonParam(opts);
    opts.buttons = buttons_1.getButtonListOpts(buttonListOpts);
    delete opts.button;
    opts.content = content_1.getContentOpts(opts.content);
    var finalOptions = Object.assign({}, defaultOpts, userDefaults, opts);
    // Check if the users uses any deprecated options:
    Object.keys(finalOptions).forEach(function (optionName) {
        if (deprecations_1.DEPRECATED_OPTS[optionName]) {
            deprecations_1.logDeprecation(optionName);
        }
    });
    return finalOptions;
};
//# sourceMappingURL=index.js.map