$(function () {

    tabClose();
    tabCloseEven();

    $("[data-href]").on("click tap", function () {

        var href = $(this).attr("data-href");
        var tabTitle = $(this).text();
        if (!$('#tabs').tabs('exists', tabTitle)) {
            $('#tabs').tabs('add', {
                title: tabTitle,
                content: createFrame(href),
                closable: true,
                width: $('#mainPanle').width() - 10,
                height: $('#mainPanle').height() - 26
            });
        } else {
            $('#tabs').tabs('select', tabTitle);
        }

        tabClose();

    });

    //打开到Tab的锚标签
    $("a[target='_tab']").on("click tap", function () {
        var href = $(this).attr("href");
        var title = $(this).attr("title");
        if (href != "" && title != "") {
            openNewTag(href, title);
        }
        return false;
    });

    $.fn.datebox.defaults.formatter = dateformatter;
    $.fn.datebox.defaults.parser = dateparser;

    $.fn.datetimebox.defaults.formatter = datetimeformatter;
    $.fn.datetimebox.defaults.parser = datetimeparser;


});

function createFrame(url) {
    if (url.indexOf("http") == "-1") {
        var s = '<iframe name="mainFrame" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:99%;"></iframe>';
        return s;
    }
    else {
        var s = '<pre class="brush: c-sharp;"> <iframe name="mainFrame" scrolling="auto" frameborder="0"  src="' + url + '" style="position:absolute; left:0; top:0; height:100%; width:100%; border:0px;"></iframe> </pre> <br>';
        return s;
    }

}
function closeTab(title) {
    if ($('#tabs').tabs('exists', title)) {
        $('#tabs').tabs('close', title);
    }
}
function openNewTag(url, title) {

    var tabs = $("#tabs");
    closeTag(title);
    if (tabs.length == 0 && window.parent) {
        window.parent.openNewTag(url, title);
        return;
    }

    if (!tabs.tabs('exists', title)) {
        tabs.tabs('add', {
            title: title,
            content: createFrame(url),
            closable: true,
            width: $('#mainPanle').width() - 10,
            height: $('#mainPanle').height() - 26
        });
    } else {
        tabs.tabs('select', title);
    }

    tabClose();
}

function closeTag(title) {
    var tabs = $("#tabs");
    if (tabs.length == 0 && window.parent) {
        window.parent.closeTag(title);
        return;
    }
    tabs.tabs('close', title);
}

function tabClose() {
    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children("span").text();
        $('#tabs').tabs('close', subtitle);
    })

    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY,
        });

        var subtitle = $(this).children("span").text();
        $('#mm').data("currtab", subtitle);

        return false;
    });
}


//绑定右键菜单事件
function tabCloseEven() {
    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('#tabs').tabs('close', currtab_title);
    })
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            $('#tabs').tabs('close', t);
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t != currtab_title)
                $('#tabs').tabs('close', t);
        });
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });

    //退出
    $("#mm-exit").click(function () {
        $('#mm').menu('hide');
    });
}

//初始化数据列表
function InitialDataGrid(setting) {

    var config = $.extend({}, {
        datatable: "datagrid",
        toolbar: "toolbar",
        showPagination: true,
        total: 1,
        pageSize: 10,
        pageNumber: 1
    }, setting);

    $("#" + config.datatable).datagrid({
        toolbar: '#' + config.toolbar,
        singleSelect: true,
        rownumbers: true,
        pagination: config.showPagination
    });

    if (config.showPagination) {

        $("#" + config.datatable).datagrid("getPager").pagination({
            layout: ["first", "prev", "sep", "links", "sep", "next", "last", "manual"],
            showPageList: false,
            showRefresh: false,
            total: config.total,
            pageSize: config.pageSize,
            pageNumber: config.pageNumber,
            displayMsg: "Displaying {from} to {to} of {total} items",
            onSelectPage: function (pageNumber, pageSize) {
                $("form").append("<input type='hidden' name='__PageNumber' value='" + pageNumber + "' />");
                $("form").attr("method", "get");
                $("form").submit();
            }
        });
    }
}
function openDlg2(r, t, w, h) {
    openDlg({
        url: r,
        title: t,
        width: w,
        height: h
    });
}
function openDlg(setting) {
    var config = $.extend({}, {
        url: "",
        title: "新建",
        width: 500,
        height: 400,
        minimizable: false,
        maximizable: false,
        resizable: false
    }, setting);

    if ($("#__easydlg").length == 0) {
        $("body").append("<div id='__easydlg'></div>");
    }

    $("#__easydlg").dialog({
        href: config.url,
        cache: false,
        title: config.title,
        modal: true,
        closed: false,
        width: config.width,
        height: config.height,
        minimizable: config.minimizable,
        maximizable: config.maximizable,
        resizable: config.resizable,

        iconCls: 'icon-save'
    });

}

function closeDlg(dlg) {

    if (dlg == undefined || dlg == null) {
        dlg = "__easydlg";
    }

    if ($("#" + dlg).length > 0) {
        $("#" + dlg).dialog("close");
    }

}

function submitDlg(form, onsuccess) {
    var item = $("#" + form);

    //第一步，数据验证
    // $("#" + form).validate();
    //第二步，提交数据
    var action = item.attr("action");
    $.post(action, item.serialize(), function (result) {
        if (result.state == "success") {
            closeDlg();
            if (onsuccess == null || onsuccess == undefined) {

                //location.reload();
                try {
                    refush();
                } catch (e) {
                    alert(e.message);
                }
            }
            else {
                onsuccess();
            }
        }
        else {
            $.messager.alert("错误", "提交数据失败！", "error");
        }
    });
}


function submitDlgWithValidate(form, onsuccess) {
    var item = $("#" + form);

    //第一步，数据验证
    if (validate()) {
        //第二步，提交数据
        var action = item.attr("action");
        $.post(action, item.serialize(), function (result) {
            if (result.state == "success") {
                closeDlg();
                if (onsuccess == null || onsuccess == undefined) {

                    //location.reload();
                    try {
                        $.messager.alert("信息", result.message, "success");
                        refresh();
                    } catch (e) {
                        alert(e.message);
                    }
                }
                else {
                    onSubmitSuccess(result.id);
                }
            }
            else {
                if (result.message == "") {
                    $.messager.alert("错误", "提交数据失败！", "error");
                } else {
                    $.messager.alert("错误", result.message, "error");
                }
            }
        });
    }
}

//格式化EasyUI的DataGrid日期类型数据
function getTime(/** timestamp=0 **/) {
    var ts = arguments[0] || 0;
    var t, y, m, d, h, i, s;
    t = ts ? new Date(ts * 1000) : new Date();
    y = t.getFullYear();
    m = t.getMonth() + 1;
    d = t.getDate();
    h = t.getHours();
    i = t.getMinutes();
    s = t.getSeconds();
    // 可根据需要在这里定义时间格式  
    return y + '-' + (m < 10 ? '0' + m : m) + '-' + (d < 10 ? '0' + d : d) + ' ' + (h < 10 ? '0' + h : h) + ':' + (i < 10 ? '0' + i : i) + ':' + (s < 10 ? '0' + s : s);
}

/**
格式化时间显示方式、用法:format="yyyy-MM-dd hh:mm:ss";
*/
formatDate = function (v, format) {
    if (!v) return "";
    var d = v;
    if (typeof v === 'string') {
        if (v.indexOf("/Date(") > -1)
            d = new Date(parseInt(v.replace("/Date(", "").replace(")/", ""), 10));
        else
            d = new Date(Date.parse(v.replace(/-/g, "/").replace("T", " ").split(".")[0]));//.split(".")[0] 用来处理出现毫秒的情况，截取掉.xxx，否则会出错
    }
    var o = {
        "M+": d.getMonth() + 1,  //month
        "d+": d.getDate(),       //day
        "h+": d.getHours(),      //hour
        "m+": d.getMinutes(),    //minute
        "s+": d.getSeconds(),    //second
        "q+": Math.floor((d.getMonth() + 3) / 3),  //quarter
        "S": d.getMilliseconds() //millisecond
    };
    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (d.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
};

//格式化EasyUI的DateBox日期类型数据
function dateformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
}
function dateparser(s) {
    if (!s) return new Date();
    var ss = (s.split('-'));
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
}

//格式化EasyUI的DateTimeBox日期类型数据
function datetimeformatter(date) {
    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    var d = date.getDate();
    var h = date.getHours();
    var min = date.getMinutes();
    var sec = date.getSeconds();
    return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d) + ' ' + (h < 10 ? ('0' + h) : h) + ':' + (min < 10 ? ('0' + min) : min) + ':' + (sec < 10 ? ('0' + sec) : sec);
}

function datetimeparser(s) {
    if (!s) return new Date();

    var items = s.split(/[\/|\-|\:|\s]/);

    var y = items[0];
    var m = items[1];
    var d = items[2];
    var h = items[3];
    var min = items[4];
    var sec = items[5];
    if (!isNaN(y) && !isNaN(m) && !isNaN(d) && !isNaN(h) && !isNaN(min) && !isNaN(sec)) {
        return new Date(y, m - 1, d, h, min, sec);
    } else {
        return new Date();
    }
}

(function (a) { a.fn.mask = function (c, b) { a(this).each(function () { if (b !== undefined && b > 0) { var d = a(this); d.data("_mask_timeout", setTimeout(function () { a.maskElement(d, c) }, b)) } else { a.maskElement(a(this), c) } }) }; a.fn.unmask = function () { a(this).each(function () { a.unmaskElement(a(this)) }) }; a.fn.isMasked = function () { return this.hasClass("masked") }; a.maskElement = function (d, c) { if (d.data("_mask_timeout") !== undefined) { clearTimeout(d.data("_mask_timeout")); d.removeData("_mask_timeout") } if (d.isMasked()) { a.unmaskElement(d) } if (d.css("position") == "static") { d.addClass("masked-relative") } d.addClass("masked"); var e = a('<div class="loadmask"></div>'); if (navigator.userAgent.toLowerCase().indexOf("msie") > -1) { e.height(d.height() + parseInt(d.css("padding-top")) + parseInt(d.css("padding-bottom"))); e.width(d.width() + parseInt(d.css("padding-left")) + parseInt(d.css("padding-right"))) } if (navigator.userAgent.toLowerCase().indexOf("msie 6") > -1) { d.find("select").addClass("masked-hidden") } d.append(e); if (c !== undefined) { var b = a('<div class="loadmask-msg" style="display:none;"></div>'); b.append("<div>" + c + "</div>"); d.append(b); b.css("top", Math.round(d.height() / 2 - (b.height() - parseInt(b.css("padding-top")) - parseInt(b.css("padding-bottom"))) / 2) + "px"); b.css("left", Math.round(d.width() / 2 - (b.width() - parseInt(b.css("padding-left")) - parseInt(b.css("padding-right"))) / 2) + "px"); b.show() } }; a.unmaskElement = function (b) { if (b.data("_mask_timeout") !== undefined) { clearTimeout(b.data("_mask_timeout")); b.removeData("_mask_timeout") } b.find(".loadmask-msg,.loadmask").remove(); b.removeClass("masked"); b.removeClass("masked-relative"); b.find("select").removeClass("masked-hidden") } })(jQuery);

var infotext = "正在玩命加载..";

var myajax = $.ajax;
$.ajax = function (a) {

    $("body").mask(infotext);
    var successback = a.success;
    var completeback = a.complete;
    a.success = function (a1) {

        $("body").unmask();
        if (successback) {
            successback(a1);
        }
        if (a1 && a1.Result && a1.Result == -1) {
            window.location.href = getRootPath() + "/Account/Login";
        }

    }
    a.complete = function (a1) {

        $("body").unmask();
        if (completeback) completeback(a1);


    }
    myajax(a);
}
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    if (postPath == "/Report") {
        postPath = "";
    }
    return (prePath + postPath);
}
//打印网页（例：PrintUrl("https://www.baidu.com/")）
function PrintUrl(Url) {
    $.get(Url, function (data) {
        var newWin = window.open('printer', '', '');
        //newWin.document.write(data);
        newWin.document.location.reload();
        newWin.print();
        newWin.close();
    })
}
//打印LODOP 
var Print = {
    URL: function (strURL) {
        strURL = getRootPath() + "/DownLoad/" + strURL;
        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        //var LODOP = getLodop();
        var intTop = 25;
        var intLeft = 25;
        var intWidth = "100%";
        var intHeight = "99%";
        LODOP.ADD_PRINT_URL(intTop, intLeft, intWidth, intHeight, strURL);
        LODOP.PREVIEW();
        //LODOP.PRINT();
    }
}
//校验显示中文提示
$.extend($.validator.messages, {
    required: "此项必填",
    remote: "请修正该字段",
    email: "请输入正确格式的电子邮件",
    url: "请输入合法的网址",
    date: "请输入合法的日期",
    dateISO: "请输入合法的日期 (ISO).",
    number: "请输入合法的数字",
    digits: "只能输入整数",
    creditcard: "请输入合法的信用卡号",
    equalTo: "请再次输入相同的值",
    accept: "请输入拥有合法后缀名的字符串",
    maxlength: $.validator.format("请输入一个长度最多是 {0} 的字符串"),
    minlength: $.validator.format("请输入一个长度最少是 {0} 的字符串"),
    rangelength: $.validator.format("请输入一个长度介于 {0} 和 {1} 之间的字符串"),
    range: $.validator.format("请输入一个介于 {0} 和 {1} 之间的值"),
    max: $.validator.format("请输入一个最大为 {0} 的值"),
    min: $.validator.format("请输入一个最小为 {0} 的值")
});

