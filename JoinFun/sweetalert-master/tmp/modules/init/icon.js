"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//import { stringToNode } from '../utils';
var modal_1 = require("./modal");
var markup_1 = require("../markup");
var class_list_1 = require("../class-list");
var ICON = class_list_1.default.ICON, ICON_CUSTOM = class_list_1.default.ICON_CUSTOM;
var PREDEFINED_ICONS = ["error", "warning", "success", "info"];
var ICON_CONTENTS = {
    error: markup_1.errorIconMarkup(),
    warning: markup_1.warningIconMarkup(),
    success: markup_1.successIconMarkup(),
};
/*
 * Set the warning, error, success or info icons:
 */
var initPredefinedIcon = function (type, iconEl) {
    var iconTypeClass = ICON + "--" + type;
    iconEl.classList.add(iconTypeClass);
    var iconContent = ICON_CONTENTS[type];
    if (iconContent) {
        iconEl.innerHTML = iconContent;
    }
};
var initImageURL = function (url, iconEl) {
    iconEl.classList.add(ICON_CUSTOM);
    var img = document.createElement('img');
    img.src = url;
    iconEl.appendChild(img);
};
var initIcon = function (str) {
    if (!str)
        return;
    var iconEl = modal_1.injectElIntoModal(markup_1.iconMarkup);
    if (PREDEFINED_ICONS.includes(str)) {
        initPredefinedIcon(str, iconEl);
    }
    else {
        initImageURL(str, iconEl);
    }
};
exports.default = initIcon;
//# sourceMappingURL=icon.js.map