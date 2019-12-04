"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var utils_1 = require("../utils");
;
;
exports.CONFIRM_KEY = 'confirm';
exports.CANCEL_KEY = 'cancel';
var defaultButton = {
    visible: true,
    text: null,
    value: null,
    className: '',
    closeModal: true,
};
var defaultCancelButton = Object.assign({}, defaultButton, {
    visible: false,
    text: "Cancel",
    value: null,
});
var defaultConfirmButton = Object.assign({}, defaultButton, {
    text: "OK",
    value: true,
});
exports.defaultButtonList = {
    cancel: defaultCancelButton,
    confirm: defaultConfirmButton,
};
var getDefaultButton = function (key) {
    switch (key) {
        case exports.CONFIRM_KEY:
            return defaultConfirmButton;
        case exports.CANCEL_KEY:
            return defaultCancelButton;
        default:
            // Capitalize:
            var text = key.charAt(0).toUpperCase() + key.slice(1);
            return Object.assign({}, defaultButton, {
                text: text,
                value: key,
            });
    }
};
var normalizeButton = function (key, param) {
    var button = getDefaultButton(key);
    /*
     * Use the default button + make it visible
     */
    if (param === true) {
        return Object.assign({}, button, {
            visible: true,
        });
    }
    /* Set the text of the button: */
    if (typeof param === "string") {
        return Object.assign({}, button, {
            visible: true,
            text: param,
        });
    }
    /* A specified button should always be visible,
     * unless "visible" is explicitly set to "false"
     */
    if (utils_1.isPlainObject(param)) {
        return Object.assign({
            visible: true,
        }, button, param);
    }
    return Object.assign({}, button, {
        visible: false,
    });
};
var normalizeButtonListObj = function (obj) {
    var buttons = {};
    for (var _i = 0, _a = Object.keys(obj); _i < _a.length; _i++) {
        var key = _a[_i];
        var opts = obj[key];
        var button = normalizeButton(key, opts);
        buttons[key] = button;
    }
    /*
     * We always need a cancel action,
     * even if the button isn't visible
     */
    if (!buttons.cancel) {
        buttons.cancel = defaultCancelButton;
    }
    return buttons;
};
var normalizeButtonArray = function (arr) {
    var buttonListObj = {};
    switch (arr.length) {
        /* input: ["Accept"]
         * result: only set the confirm button text to "Accept"
         */
        case 1:
            buttonListObj[exports.CANCEL_KEY] = Object.assign({}, defaultCancelButton, {
                visible: false,
            });
            break;
        /* input: ["No", "Ok!"]
         * result: Set cancel button to "No", and confirm to "Ok!"
         */
        case 2:
            buttonListObj[exports.CANCEL_KEY] = normalizeButton(exports.CANCEL_KEY, arr[0]);
            buttonListObj[exports.CONFIRM_KEY] = normalizeButton(exports.CONFIRM_KEY, arr[1]);
            break;
        default:
            utils_1.throwErr("Invalid number of 'buttons' in array (" + arr.length + ").\n      If you want more than 2 buttons, you need to use an object!");
    }
    return buttonListObj;
};
exports.getButtonListOpts = function (opts) {
    var buttonListObj = exports.defaultButtonList;
    if (typeof opts === "string") {
        buttonListObj[exports.CONFIRM_KEY] = normalizeButton(exports.CONFIRM_KEY, opts);
    }
    else if (Array.isArray(opts)) {
        buttonListObj = normalizeButtonArray(opts);
    }
    else if (utils_1.isPlainObject(opts)) {
        buttonListObj = normalizeButtonListObj(opts);
    }
    else if (opts === true) {
        buttonListObj = normalizeButtonArray([true, true]);
    }
    else if (opts === false) {
        buttonListObj = normalizeButtonArray([false, false]);
    }
    else if (opts === undefined) {
        buttonListObj = exports.defaultButtonList;
    }
    return buttonListObj;
};
//# sourceMappingURL=buttons.js.map