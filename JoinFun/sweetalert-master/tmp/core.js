"use strict";
/*
 * SweetAlert
 * 2014-2017 – Tristan Edwards
 * https://github.com/t4t5/sweetalert
 */
Object.defineProperty(exports, "__esModule", { value: true });
var init_1 = require("./modules/init");
var actions_1 = require("./modules/actions");
var state_1 = require("./modules/state");
var options_1 = require("./modules/options");
;
var swal = function () {
    var args = [];
    for (var _i = 0; _i < arguments.length; _i++) {
        args[_i] = arguments[_i];
    }
    // Prevent library to be run in Node env:
    if (typeof window === 'undefined')
        return;
    var opts = options_1.getOpts.apply(void 0, args);
    return new Promise(function (resolve, reject) {
        state_1.default.promise = { resolve: resolve, reject: reject };
        init_1.default(opts);
        // For fade animation to work:
        setTimeout(function () {
            actions_1.openModal();
        });
    });
};
swal.close = actions_1.onAction;
swal.getState = actions_1.getState;
swal.setActionValue = state_1.setActionValue;
swal.stopLoading = actions_1.stopLoading;
swal.setDefaults = options_1.setDefaults;
exports.default = swal;
//# sourceMappingURL=core.js.map