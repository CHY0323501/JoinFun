"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var utils_1 = require("./utils");
var buttons_1 = require("./options/buttons");
var class_list_1 = require("./class-list");
var OVERLAY = class_list_1.default.OVERLAY, SHOW_MODAL = class_list_1.default.SHOW_MODAL, BUTTON = class_list_1.default.BUTTON, BUTTON_LOADING = class_list_1.default.BUTTON_LOADING;
var state_1 = require("./state");
exports.openModal = function () {
    var overlay = utils_1.getNode(OVERLAY);
    overlay.classList.add(SHOW_MODAL);
    state_1.default.isOpen = true;
};
var hideModal = function () {
    var overlay = utils_1.getNode(OVERLAY);
    overlay.classList.remove(SHOW_MODAL);
    state_1.default.isOpen = false;
};
/*
 * Triggers when the user presses any button, or
 * hits Enter inside the input:
 */
exports.onAction = function (namespace) {
    if (namespace === void 0) { namespace = buttons_1.CANCEL_KEY; }
    var _a = state_1.default.actions[namespace], value = _a.value, closeModal = _a.closeModal;
    if (closeModal === false) {
        var buttonClass = BUTTON + "--" + namespace;
        var button = utils_1.getNode(buttonClass);
        button.classList.add(BUTTON_LOADING);
    }
    else {
        hideModal();
    }
    state_1.default.promise.resolve(value);
};
/*
 * Filter the state object. Remove the stuff
 * that's only for internal use
 */
exports.getState = function () {
    var publicState = Object.assign({}, state_1.default);
    delete publicState.promise;
    delete publicState.timer;
    return publicState;
};
/*
 * Stop showing loading animation on button
 * (to display error message in input for example)
 */
exports.stopLoading = function () {
    var buttons = document.querySelectorAll("." + BUTTON);
    for (var i = 0; i < buttons.length; i++) {
        var button = buttons[i];
        button.classList.remove(BUTTON_LOADING);
    }
};
//# sourceMappingURL=actions.js.map