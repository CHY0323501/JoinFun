"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var buttons_1 = require("./options/buttons");
;
;
var defaultState = {
    isOpen: false,
    promise: null,
    actions: {},
    timer: null,
};
var state = Object.assign({}, defaultState);
exports.resetState = function () {
    state = Object.assign({}, defaultState);
};
/*
 * Change what the promise resolves to when the user clicks the button.
 * This is called internally when using { input: true } for example.
 */
exports.setActionValue = function (opts) {
    if (typeof opts === "string") {
        return setActionValueForButton(buttons_1.CONFIRM_KEY, opts);
    }
    for (var namespace in opts) {
        setActionValueForButton(namespace, opts[namespace]);
    }
};
var setActionValueForButton = function (namespace, value) {
    if (!state.actions[namespace]) {
        state.actions[namespace] = {};
    }
    Object.assign(state.actions[namespace], {
        value: value,
    });
};
/*
 * Sets other button options, e.g.
 * whether the button should close the modal or not
 */
exports.setActionOptionsFor = function (buttonKey, _a) {
    var _b = (_a === void 0 ? {} : _a).closeModal, closeModal = _b === void 0 ? true : _b;
    Object.assign(state.actions[buttonKey], {
        closeModal: closeModal,
    });
};
exports.default = state;
//# sourceMappingURL=state.js.map