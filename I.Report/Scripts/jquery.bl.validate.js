/*!
 * jQuery Validation Plugin v1.0.0
 *
 * Copyright (c) 2016 Xie Bin
 */

(function ($) {
    $.extend($.fn, {
        validate: function (options) {

            //检查是否已经存在
            var validator = $.data(this[0], "validator");
            if (validator) {
                return validator;
            }

            validator = new $.validator(options, this[0]);
            $.data(this[0], "validator", validator);

            //如果是from，如果要求阻止就阻止
            if (this.is("form")) {
                if (validator.settings.onSubmit) {

                    this.on("submit", function (event) {
                        if (validator.validate(true)) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                    this.on("click", ":submit", function (event) {
                        if (validator.validate(true)) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                }
            }

            return validator;

        }
        ,
        validateVal: function () {
            if ($(this).is("select")) {
                return $(this).find(":selected").val();
            }
            if ($(this).is(":radio")) {
                var name = $(this).attr("name");
                return $("[name='" + name + "']").find(":checked").val();
            }
            return $(this).val();

        }
    });

    $.validator = function (options, form) {
        if (!options) {
            options = {};
        }

        this.settings = {};
        this.elements = [];

        $.extend(this.settings, $.validator.defaults, options);
        this.initial(form);
    };


    $.extend($.validator, {

        defaults: {
            errorClass: "error",
            targetClass: "inputerror",
            focusCleanup: true,
            onSubmit: true,
            onFocusOut: true,
            errorDirection: 'after',
            errorElement: 'label',
            errorTemplate: ''
        },
        prototype: {
            settings: {},
            elements: [],
            initial: function (form) {

                var v = this;

                $(form).find("[data-validate]").each(function () {

                    var dv = $(this).attr("data-validate");

                    if (dv != "") {
                        v.elements.push(new validateElement($(this), dv, v.settings));
                    }

                });

            },
            validate: function () {

                //检查是否强制要求Remote返回数据
                var isForce = arguments[0] || false;
                var fr = $.validator.forceRemote;
                $.validator.forceRemote = isForce;
                var result = true;
                for (i in this.elements) {
                    var item = this.elements[i];
                    if (!item.check()) {
                        result = false;
                    }
                }
                $.validator.forceRemote = fr;
                return result;
            },
            addElement: function (selector, checker, options) {

                if (options) {
                    $.extend(options, this.settings);
                }
                else {
                    options = this.settings;
                }

                this.elements.push(new validateElement(selector, checker, options));
            }
        },

        functions: {
            required: {
                message: "此项必填",
                func: function () {
                    return !(this.val() == "");
                }
            },
            remote: {
                message: "{0}已经存在！",
                func: function (url) {

                    var name = $(this.element).attr("name");
                    var v = this.val();

                    var kv = {};

                    if (kv.push) {
                        kv[name].push(v);
                    }
                    else {
                        kv[name] = v;
                    }
                    var elem = this;
                    var force = $.validator.forceRemote;
                    if (!force) {
                        $.get(url, kv, function (data) {
                            if (data == false || data == "false" || data == "False") {
                                if (!elem.isOnError) {
                                    elem.message = elem.format(v);
                                    elem.showError();
                                }
                            }
                        });
                        return "pending";
                    }
                    else {
                        var result = false;
                        $.ajax({
                            url: url,
                            async: false,
                            cache: false,
                            success: function (data) {
                                if (data == true || data == "true" || data == "True") {
                                    result = true;
                                }
                            },
                            error: function () {
                                result = false;
                            }
                        });
                        this.message = this.format(v);
                        return result;
                    }
                }
            },
            email: {
                message: "无效的邮箱地址",
                func: function () {
                    return (!this.val()) || /^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$/.test(this.val());
                }
            },
            url: {
                message: "无效的网络地址",
                func: function () {
                    return (!this.val()) || /^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)(?:\.(?:[a-z\u00a1-\uffff0-9]-*)*[a-z\u00a1-\uffff0-9]+)*(?:\.(?:[a-z\u00a1-\uffff]{2,})).?)(?::\d{2,5})?(?:[/?#]\S*)?$/i.test(this.val());
                }
            },
            date: {
                message: "无效的时间",
                func: function () {
                    return (!this.val()) || !/Invalid|NaN/.test(new Date(this.val()).toString());
                }
            },
            dateISO: {
                message: "无效的日期",
                func: function () {
                    return (!this.val()) || /^\d{4}[\/\-](0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])$/.test(this.val());
                }
            },
            number: {
                message: "此项必须为数字",
                func: function () {
                    return (!this.val()) || /^(?:-?\d+|-?\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(this.val());
                }
            },
            digits: {
                message: "此项必须为整数",
                func: function () {                    
                    return (!this.val()) || /^[1-9]\d*|0$/.test(this.val());
                }
            },
            equalTo: {
                message: "两次输入的密码不一致",
                func: function (compareTo) {
                    var v = $(compareTo).val();
                    if (v == this.val()) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            },
            maxlength: {
                message: "最多只能输入{0}个字符",
                func: function (length) {

                    var v = this.val();
                    if (v.length > length) {
                        this.message = this.format(length);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            },
            minlength: {
                message: "至少输入{0}个字符",
                func: function (length) {

                    var v = this.val();
                    if (v != "" && v.length < length) {
                        this.message = this.format(length);
                        return false;
                    }
                    else {
                        return true;
                    }

                }
            },
            rangelength: {
                message: "输入的字符数必须大于{0}个，少于{1}个",
                func: function (min, max) {
                    var v = this.val();

                    if ((v != "" && v.length < min) || (v.length > max)) {
                        this.message = this.format(min, max);
                        return false;
                    }
                    else {
                        return true;
                    }

                }
            },
            range: {
                message: "输入的值必须介于{0}和{1}之间",
                func: function (min, max) {
                    var v = this.val();

                    if (v == "") {
                        return true;
                    }

                    if (!(isNaN(min) || isNaN(max))) {
                        var nv = parseFloat(v);
                        if (isNaN(nv) || nv > max || nv < min) {
                            this.message = this.format(min, max);
                            return false;
                        }
                        else {
                            return true;
                        }
                    }

                    if (v < min || v > max) {
                        this.message = this.format(min, max);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            },
            max: {
                message: "输入的值不得大于{0}",
                func: function (max) {
                    var v = this.val();

                    if (!isNaN(max)) {
                        var nv = parseFloat(v);
                        if (isNaN(nv) || nv > max) {
                            this.message = this.format(max);
                            return false;
                        }
                        else {
                            return true;
                        }
                    }

                    if (v > max) {
                        this.message = this.format(max);
                        return false;
                    }
                    else {
                        return true;
                    }

                }
            },
            min: {
                message: "输入的值不得小于{0}",
                func: function (min) {
                    var v = this.val();

                    if (!isNaN(min)) {
                        var nv = parseFloat(v);
                        if (isNaN(nv) || nv < min) {
                            this.message = this.format(max);
                            return false;
                        }
                        else {
                            return true;
                        }
                    }

                    if (v != "" && v < min) {
                        this.message = this.format(min);
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
        },
        addFunction: function (func) {

            if (arguments.length === 1) {
                $.extend($.validator.functions, func);
            }
            else if (arguments.length === 3) {
                $.validator.functions[arguments[0]] = { message: arguments[1], func: arguments[2] };
            }
        },
        forceRemote: false

    });

    function validateElement(element, checkers, options) {

        this.initial();

        if (typeof (element) == "string") {
            this.element = $(element);
        }
        else {
            this.element = element;
        }

        if (typeof (checkers) == "string") {

            var nm = checkers.split(";");
            for (var i in nm) {
                var item = nm[i];
                if (item !== "") {
                    if (item[0] == '{') {
                        //转换为配置
                        var o = (new Function("return " + item))();
                        $.extend(this.settings, o);
                    }
                    else {
                        this.checkers.push(new validateChecker(item));
                    }
                }
            }
            if (options) {
                $.extend(this.settings, options, this.settings);
            }

        }
        else {
            for (var item in checkers) {
                var v = checkers[item];
                if (v === null) {
                    this.checkers.push(new validateChecker(item));
                }
                else if (typeof (v) == "string") {
                    this.checkers.push(new validateChecker(item, v));
                }
                else {
                    var m = v.message;
                    var a = v.args;
                    this.checkers.push(new validateChecker(item, m, a));
                }
            }

            $.extend(this.settings, options);

        }

        this.settings.target = this.settings.target || this.element;

        var checkMessage = "";

        var ele = this;

        if (this.settings.focusCleanup) {
            $(this.element).focusin(function () {
                ele.hideError();
            });
        }
        if (this.settings.onFocusOut) {
            $(this.element).focusout(function () {
                ele.check();
            });
        }

    }

    $.extend(validateElement, {

        prototype: {
            settings: {
                target: null,
                tipTarget: null
            },
            element: null,
            errorLabel: null,
            isOnError: false,
            message: "",
            checkers: [],
            initial: function () {

                this.settings = {
                    target: null,
                    tipTarget: null
                };
                this.element = null;
                this.errorLabel = null;
                this.isOnError = false;
                this.message = "";
                this.checkers = [];
            },
            check: function () {

                this.hideError();

                for (index in this.checkers) {

                    var c = this.checkers[index];

                    var args = [];

                    for (item in c.args) {
                        args.push(this.eval2(c.args[item]));
                    }
                    checkMessage = c.message;

                    //除了required，其它数据在为空的情况下默认通过
                    if (this.val() == "" && c.name != "required") {
                        return true;
                    }

                    var item = c.checkfun.apply(this, args);
                    if (item === false) {
                        this.message = this.message || c.message;
                        this.showError();
                        return false;
                    }
                }
                return true;
            },
            showError: function () {

                this.isOnError = true;

                var ele = "";
                if (!this.settings.errorTemplate) {
                    ele = "<" + this.settings.errorElement + " class='" + this.settings.errorClass + "'>" + this.message + "</" + this.settings.errorElement + ">";
                }
                else {
                    ele = this.settings.errorTemplate.replace("{{message}}", this.message);
                }

                if (this.settings.tipTarget) {

                    this.errorLabel = $(ele).appendTo($(this.settings.tipTarget));
                    //$(this.settings.tipTarget).text(this.message);
                }
                else {
                    var item = $(this.settings.target);

                    item.addClass(this.settings.targetClass);

                    if (this.settings.errorDirection == 'before') {
                        this.errorLabel = $(ele).insertBefore(item);
                    }
                    if (this.settings.errorDirection == "after") {
                        this.errorLabel = $(ele).insertAfter(item);
                    }
                    if (this.settings.errorDirection == 'floatTop') {
                        var top = item.offset().top;
                        var left = item.offset().left;
                        this.errorLabel = $(ele).insertBefore(item);
                        this.errorLabel.css("position", "absolute");
                        this.errorLabel.offset({ top: top - this.errorLabel.height(), left: left });
                    }
                    if (this.settings.errorDirection == "FloatBottom") {
                        var top = item.offset().top;
                        var left = item.offset().left;
                        this.errorLabel = $(ele).insertAfter(item);
                        this.errorLabel.css("position", "absolute");
                        this.errorLabel.offset({ top: top + item.height(), left: left });
                    }
                }
            },
            hideError: function () {

                this.isOnError = false;
                this.message = "";
                $(this.settings.target).removeClass(this.settings.targetClass);
                if (this.settings.tipTarget) {
                    $(this.settings.tipTarget).text("");
                }
                else if (this.errorLabel) {
                    this.errorLabel.remove();
                    this.errorLabel = null;
                }

            },
            addChecker: function (checker) {
                if (typeof (checker) === "string") {
                    this.checkers.push(new validateChecker(checker));
                }
                else {
                    this.checkers.push(checker);
                }
            },
            format: function (params) {

                var source = checkMessage;

                if (params === undefined) {
                    return source;
                }
                params = $.makeArray(arguments);
                $.each(params, function (i, n) {
                    source = source.replace(new RegExp("\\{" + i + "\\}", "g"), function () {
                        return n;
                    });
                });
                return source;
            },
            test: function (v, reg) {
                return reg.test(v);
            },
            val: function () {
                return $(this.element).validateVal();
            },
            eval2: function (desc) {
                try {

                    var v = eval(desc);
                    if (v === null || v === undefined) {
                        return $(desc).val();
                    }
                    else {
                        return v;
                    }

                } catch (e) {
                    return $(desc).val();
                }
            }
        },
    });

    function validateChecker(name) {
        this.initial();
        if (arguments.length === 1) {
            if (/^[_|a-z|A-Z][_|a-z|A-Z|0-9]*(\(.+(\,.+)*\))*(\[*.\])*$/.test(name)) {
                var nm = name.split(/\[|\]/);
                if (nm.length > 1) {
                    this.message = nm[1];
                    name = nm[0];
                }

                var items = name.split(/\(|\,|\)/);
                this.name = items[0];
                //不同的浏览器分割出来的数组不一样，IE8会忽略最后一个空，而chrome则会保留空
                if (items[items.length-1] == "") {
                    items.remove(items.length - 1);
                }                
                for (var i = 1; i < items.length; i++) {
                    this.args.push(items[i]);
                }
                var func = $.validator.functions[this.name];
                this.checkfun = func.func;
                this.message = this.message || func.message;
            }
            else {
                throw Error("不正确的格式\"" + name + "\"");
            }
        }
        else if (arguments.length === 2) {
            this.name = arguments[0];
            var func = $.validator.functions[this.name];
            this.checkfun = func.func;
            this.message = arguments[1] || func.message;
        }
        else if (arguments.length === 3) {
            this.name = arguments[0];
            var func = $.validator.functions[this.name];
            this.checkfun = func.func;
            this.args = arguments[1] || [];
            this.message = arguments[2] || func.message;
        }
    }

    $.extend(validateChecker.prototype, {

        message: "",
        checkfun: function () { return true },
        args: [],
        name: null,
        initial: function () {
            this.message = "";
            this.args = [];
            this.name = null;
        }

    });


})(jQuery);