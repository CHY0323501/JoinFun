"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var markup_1 = require("../markup");
var modal_1 = require("./modal");
/*
 * Fixes a weird bug that doesn't wrap long text in modal
 * This is visible in the Safari browser for example.
 * https://stackoverflow.com/a/3485654/2679245
 */
var webkitRerender = function (el) {
    if (navigator.userAgent.includes('AppleWebKit')) {
        el.style.display = 'none';
        el.offsetHeight;
        el.style.display = '';
    }
};
exports.initTitle = function (title) {
    if (title) {
        var titleEl = modal_1.injectElIntoModal(markup_1.titleMarkup);
        titleEl.textContent = title;
        webkitRerender(titleEl);
    }
};
exports.initText = function (text) {
    if (text) {
        var textNode_1 = document.createDocumentFragment();
        text.split('\n').forEach(function (textFragment, index, array) {
            textNode_1.appendChild(document.createTextNode(textFragment));
            // unless we are on the last element, append a <br>
            if (index < array.length - 1) {
                textNode_1.appendChild(document.createElement('br'));
            }
        });
        var textEl = modal_1.injectElIntoModal(markup_1.textMarkup);
        textEl.appendChild(textNode_1);
        webkitRerender(textEl);
    }
};
//# sourceMappingURL=text.js.map