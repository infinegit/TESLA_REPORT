(function ($) {
    $.extend($.fn, {
        filemanager: function (options) {
            var uploader = new $.bl_fileUploader(this, options);
            $.bl_fileUploader.insertInstance("key" + $.bl_fileUploader.instanceCnt, uploader);
            uploader.addFileFrame();
        }
    });

    $.bl_fileUploader = function (obj, options) {

        var real = $(obj);
        if (!$(obj).is("div")) {
            var id;
            for (i = 0; i < 100; i++) {
                if ($("#newDivUploader" + i).length > 0) {
                    continue;
                }
                else {
                    id = "newDivUploader" + i;
                    break;
                }
            }

            $(obj).parent().css("position", "relative");
            var div = "<div id='" + id + "' style='position: absolute;top:" + $(obj).position().top + "px;left:" + $(obj).position().left + "px;width:" + $(obj).outerWidth() + "px;height:" + $(obj).outerHeight() + "px; filter: alpha(opacity=0);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;'></div>";
            $(obj).parent().append($(div));
            real = $("#" + id);
        }
        else {
            $(obj).css("position", "relative");
        }

        this.reference = real;
        this.setting = $.extend(true, {}, this.defaults, options);
        this.fileCount = 0;
        this.files = new Array();
    }

    $.extend($.bl_fileUploader, {
        uploaders: {},
        getInstance: function (key) {
            return $.bl_fileUploader.uploaders[key];
        },
        insertInstance: function (key, instance) {
            $.bl_fileUploader.uploaders[key] = instance;
            instance.key = key;
            $.bl_fileUploader.instanceCnt++;
        },
        instanceCnt: 0
    });

    $.extend($.bl_fileUploader.prototype, {
        defaults: {
            //提交的地址，用于接收地址
            remote: "",
            //正在上传的图标位置
            loadingImg: "",
            //上传文件类型
            fileType: "*.*",
            onFileTypeInvalid: function (fileName) {
                alert("The file '" + fileName + "' is invalid!");
            },
            //上传开始时要执行的方法
            onUploadStart: function (id, fileName) { },
            //上传结束时要执行的方法
            onUploadEnd: function (id, result) { },
            //存放文件列表的ID
            listID: "",
            //存放文件列表的模板
            listTemplate: "<div id='{id}'><span>{fileName}</span>{loadimg}</div>",
            //是否可以重复上传文件
            repeatable: true,
            onRepeat: function (fileName) { },
            //上传参数
            params: {},
            //自动移除失败记录的时间，-1为不消失
            removefailure: -1
        },
        key: "",
        reference: null,
        setting: {},
        filecount: 0,
        files: new Array(),
        addFileFrame: function () {
            var id = "Uploader" + this.key + this.filecount;
            this.filecount++;
            this.reference.append("<iframe id='iframe" + id + "' style='width: " + this.reference.outerWidth() + "px; height: " + this.reference.outerHeight() + "px; position: absolute; top: 0px; left: 0px; filter: alpha(opacity=0);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;' src='' allowTransparency='true'  scrolling='no' onreadystatechange='$.bl_fileUploader.getInstance(\"" + this.key + "\").endUpload(this,\"div" + id + "\")' onload='$.bl_fileUploader.getInstance(\"" + this.key + "\").endUpload(this,\"div" + id + "\")'></iframe>");
            var doc = $("#iframe" + id).contents().find("body");
            var hiddenElements = "";
            for (var p in this.setting.params) {
                hiddenElements += "<input type='hidden' id='" + p + "' name='" + p + "' value='" + this.setting.params[p] + "' />";
            }
            if (doc.length == 0) {
                doc = window.frames["iframe" + id].document;
                doc.write("<body style='margin:0px;'>");
                doc.write("<style>*{margin:0px;}</style>");
                doc.write("<form action='" + this.setting.remote + "' method='post' enctype='multipart/form-data' id='fileForm'style=' filter: alpha(opacity=100);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;'>" + hiddenElements +
                           "   <input id='div" + id + "' name='div" + id + "' type='file' style='width:" + (this.reference.outerWidth()) + "px;font-size: 100px; margin-left: 0px;cursor: pointer; filter: alpha(opacity=0);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;' onchange='if(parent.$.bl_fileUploader.getInstance(\"" + this.key + "\").beginUpload(this))document.forms[0].submit();'/>" +
                           "</form>");
                doc.write("</body>");
                //doc.css("margin", "0px");
                //doc.append("<style>*{margin:0px;}</style>");
                //doc.append("<form action='" + this.setting.remote + "' method='post' enctype='multipart/form-data' id='fileForm'>" + hiddenElements +
                //           "   <input id='div" + id + "' name='div" + id + "' type='file' style='font-size: 100px; margin-left: -10px;cursor: pointer;' onchange='if(parent.$.bl_fileUploader.getInstance(\"" + this.key + "\").beginUpload(this))document.forms[0].submit();'/>" +
                //           "</form>");
            }
            else {
                doc.css("margin", "0px");
                doc.append("<style>*{margin:0px;}</style>");
                doc.append("<form action='" + this.setting.remote + "' method='post' enctype='multipart/form-data' id='fileForm' style=' filter: alpha(opacity=0);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;'>" + hiddenElements +
                           "   <input id='div" + id + "' name='div" + id + "' type='file' style='width:" + this.reference.outerWidth() + "px;font-size: 100px; margin-left: 0px;cursor: pointer; filter: alpha(opacity=0);-moz-opacity: 1;-khtml-opacity: 1;opacity: 0;' onchange='if(parent.$.bl_fileUploader.getInstance(\"" + this.key + "\").beginUpload(this))document.forms[0].submit();'/>" +
                           "</form>");
            }
        },
        endUpload: function (obj, id) {
            var result = $(obj).contents().find("body").text();
            if ($.trim(result) == "" || result.indexOf("*{margin:0px;}") >= 0) {
                return;
            }
            var re;
            try {
                re = eval("(" + result + ")");
            }
            catch (exception) {
                re = { result: "error", message: $(obj).contents().find("title").text() };
                if (this.setting.removefailure >= 0) {
                    $("#" + id).fadeOut(this.setting.removefailure);
                }
            }
            if (re.result) {

                $("#" + id).find("img[name='imgFileLoading']").remove();
                //if (result) {
                //    $("#" + id).css("color", "green");
                //}
                //else {
                //    $("#" + id).css("color", "red");
                //}
                if (re.result != "success") {
                    if (this.setting.removefailure >= 0) {
                        $("#" + id).fadeOut(this.setting.removefailure);
                    }
                }

                //执行客户定义方法
                this.setting.onUploadEnd(id, re);
            }
        },
        beginUpload: function (obj) {
            var l = this.setting.listTemplate;
            var id = $(obj).attr("id");
            var fileName = $(obj).val();

            if (this.setting.fileType.indexOf(fileName.substring(fileName.lastIndexOf("."))) < 0) {

            }

            if (!this.setting.repeatable) {
                if (this.files.indexOf(fileName) >= 0) {
                    this.setting.onRepeat(fileName);
                    return false;
                }
                else {
                    this.files.push(fileName);
                }
            }

            l = l.replace("{fileName}", fileName).replace("{id}", id).replace("{loadimg}", "<img name='imgFileLoading' src='" + this.setting.loadingImg + "' />");
            $("#" + this.setting.listID).append(l);

            //执行客户定义方法
            this.setting.onUploadStart(id, fileName);
            this.addFileFrame();
            return true;
        }
    })

})
(jQuery)