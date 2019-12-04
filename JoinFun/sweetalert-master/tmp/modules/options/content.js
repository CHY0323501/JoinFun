"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var utils_1 = require("../utils");
;
var defaultInputOptions = {
    element: 'input',
    attributes: {
        placeholder: "",
    },
};
exports.getContentOpts = function (contentParam) {
    var opts = {};
    if (utils_1.isPlainObject(contentParam)) {
        return Object.assign(opts, contentParam);
    }
    if (contentParam instanceof Element) {
        return {
            element: contentParam,
        };
    }
    if (contentParam === 'input') {
        return defaultInputOptions;
    }
    return null;
};
//# sourceMappingURL=content.js.map