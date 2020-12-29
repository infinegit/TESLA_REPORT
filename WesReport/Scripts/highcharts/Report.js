//#region   图表
//图表类型
var ChartType = {
    Line: "line",     //线图
    Pie: "pie",       //饼图
    Column: "column", //柱形图
    Area: "area"      //区域图
};
//生成图表
var oChart = {
    chartDiv: "",       //图表所在DIV
    chartType: "line",  //图表类型
    chartTitle: "",     //图表标题
    chartSubTitle: "",  //图表子标题
    chartXAxis: [],     //图表X轴列名
    chartXTitle: "",     //图表X轴标题
    chartYTitle: "",     //图表Y轴标题
    chartSeries: [],     //图表数据
    chartValueSuffix: '',
    chartBind: function (json) {
        var csArray = [];

        if (json.length >= 1) {

            //#region  数据处理  
            jQuery.each(json, function (i, o) {
                var ocs = {
                    name: "",
                    data: []
                };

                ocs.name = o.Name;

                if (oChart.chartXAxis.length >= 1) {

                    for (var i = 0; i < oChart.chartXAxis.length; i++) {

                        //ocs.data.push(o[oChart.chartXAxis[i]]);
                        ocs.data.push(parseFloat(o.Count));
                    }

                    csArray.push(ocs)
                }

            });
            //#endregion


            oChart.chartSeries = csArray;

        }

        var optionChart = {
            chart: {
                type: oChart.chartType,
                renderTo: oChart.chartDiv,
                zoomType: 'xy'
            },

            title: {
                text: oChart.chartTitle
            },

            subtitle: {
                text: oChart.chartSubTitle
            },

            xAxis: {
                title: { text: oChart.chartXTitle },
                categories: oChart.chartXAxis
            },
            yAxis: {

                title: {
                    text: oChart.chartYTitle
                },
                labels: {
                    format: '{value}' + oChart.chartValueSuffix
                }
            },

            tooltip: {
                valueSuffix: oChart.chartValueSuffix
            },

            legend: {
                enabled: true
            },
            plotOptions: {
                column: {
                    stacking: 'normal',

                    dataLabels: {
                        enabled: true
                        //,color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || '#606060'

                    }
                },
                line: {
                    dataLabels: {
                        enabled: true,
                        format: '{y} ' + oChart.chartValueSuffix  //在所有显示的数据值后面附加%

                    }
                }
            },
            series: oChart.chartSeries
        };

        var chart = new Highcharts.Chart(optionChart);
    }
};
//#endregion

//#region   图表数据
var ReportData = {
    Show: function (id, json) {
        var sHtml = "";
        jQuery.each(json, function (i, o) {
            sHtml += "<tr>";
            sHtml += "<td>" + o.Name + "</td>";
            //for (var i = 0; i < oChart.chartXAxis.length; i++) {
            sHtml += "<td>" + o.Count + "</td>";
            //}
            sHtml += "</tr>";
        });

        jQuery("#" + id).html(sHtml);
        jQuery("#" + id).closest("table").addClass("easyui-datagrid");

    }
}
//#endregion
//#region   列表
var ZYQDatagrid = {
    CreateNew: function () {
        var Grid = {
            ProcName: "",
            Type: 'grid',
            Pagination: true,
            //用于展示表格的TABLE 注意：ID前需加#
            Div: "#datagrid",
            DivSearch: "#divSearch",
            //如果为空 ，则取数据的列名
            Columns: [[]],
            //追加列，场景:动态生成列后，再添加操作列
            ColumnsAfter: [[]],
            //追加查询条件
            ParameterAfter: [],
            //冻结列
            FrozenColumns: [[]],
            //默认页大小，如果修改需注意后台的 @ConstInfo.PAGE_SIZE
            PageSize: 50,
            //查询参数
            PostData: [],
            //用于行格式化
            RowStyler: function () { },
            Styler: function (value, row, index) { },
            Formatter: function (value, row, index) { },
            //查询数据后台方法的路径
            URL: "../Report/GetReportData",
            ViewName: "",
            SortColumn: "",
            View: null,
            DetailFormatter: null,
            onExpandRow: null,
            striped: true,
            fitColumns: true,
            singleSelect: true,
            rownumbers: true,
            pagination: false,
            nowrap: false,
            showFooter: false,
            Nowrap: false,
            IdField: null,
            TreeField: null,
            Sort: '',
            Order: '',
            onSuccess:null,
            IsImportAllColumn:true,
            //明细
            DetailViewName: "",
            //+- 明细表的列
            DetailColumns: [[]],
            DetailURL: "",
            DetailParameter: [],
            BindConfig: function () {
                var p = this;
                var config = {
                    method: 'POST',
                    fit: false,
                    striped: true,
                    fitColumns: true,
                    singleSelect: Grid.SingleSelect,
                    rownumbers: true,
                    pagination: Grid.Pagination,
                    nowrap: Grid.Nowrap,
                    styler: Grid.Styler,
                    formatter: Grid.Formatter,
                    toolbar: "toolbar",
                    showFooter: false,
                    rowStyler: Grid.RowStyler,
                    sortable:true,
                    columns: Grid.Columns,
                    onSortColumn: function (sort, order) {
                        p.Sort = sort;
                        p.Order = order;
                        p.Search(p.DivSearch);
                    },
                    frozenColumns: Grid.FrozenColumns
                };
                if (Grid.DetailViewName != "") {
                    config.view = detailview;

                    config.detailFormatter = function (index, row) {
                        return '<div style="padding:2px"><table class="ddv"></table></div>';
                    };
                    config.onExpandRow = function (index, row) {
                        var ddv = $(this).datagrid('getRowDetail', index).find('table.ddv');
                        var strPar = "ViewName=" + Grid.DetailViewName;
                        if (Grid.DetailParameter.length >= 1) {
                            jQuery(Grid.DetailParameter).each(function (i, o) {
                                if (JSON.stringify(Grid.DetailParameter[i]).split(':').length > 1) {
                                    var str2 = JSON.stringify(Grid.DetailParameter[i]).split(':');
                                    var str = str2[1];
                                    var valuestr = str2[2];
                                    var key = str.substr(1, str.indexOf(',', 0)-2);
                                    //var value = str.substr(str.indexOf(',', 0) + 1);
                                    var value = valuestr.substr(1, valuestr.indexOf('}')-2);
                                    strPar += "&" + key + "=" + row[value];
                                } else {
                                    strPar += "&" + o + "=" + row[o];
                                }
                            });
                        }

                        ddv.datagrid({
                            url: Grid.DetailURL + "?" + strPar,
                            fitColumns: true,
                            singleSelect: true,
                            rownumbers: true,
                            loadMsg: '',
                            height: 'auto',
                            columns: Grid.DetailColumns,
                            onResize: function () {
                                $(Grid.Div).datagrid('fixDetailRowHeight', index);
                            },
                            onLoadSuccess: function () {
                                setTimeout(function () {
                                    $(Grid.Div).datagrid('fixRowHeight', index);
                                    $(Grid.Div).datagrid('fixDetailRowHeight', index);
                                }, 0);
                            }
                        });
                        $(Grid.Div).datagrid('fixDetailRowHeight', index);
                    };

                }
                if (Grid.View)
                    config.view = Grid.View;
                if (Grid.DetailFormatter)
                    config.detailFormatter = Grid.DetailFormatter;
                if (Grid.onExpandRow)
                    config.onExpandRow = Grid.onExpandRow;
                if (Grid.IdField)
                    config.idField = Grid.IdField;
                if (Grid.TreeField)
                    config.treeField = Grid.TreeField;
                if (Grid.Type == "grid")
                    $(Grid.Div).datagrid(config);
                else
                    $(Grid.Div).treegrid(config);
                Grid.BindPage(1);
            },
            //pd :传参
            Init: function (div, pd) {

                if (Grid.IsCustomTable) {

                } else {

                    Grid.BindConfig();
                    $(Grid.Div).datagrid('loadData', { total: 0, rows: [] });

                }
            },
            DataBind: function (pageNumber) {
                if (Grid.IsCustomTable) {
                    var u = Grid.URL + "?page=" + pageNumber;

                    BindEasyTable(Grid.Div, u, Grid.PostData, Grid.FrozenColumns[0]);
                }
                else {
                    $.ajax({
                        url: Grid.URL + "?page=" + pageNumber,
                        data: JSON.stringify(Grid.PostData),
                        type: 'POST',
                        async: true,
                        contentType: 'application/json',
                        success: function (res) {
                 
                            if (res.Result == "1") {

                                var data = JSON.parse(res.Data);
                                if (Grid.onSuccess) Grid.onSuccess(res);
                                if (Grid.Columns[0].length == 0) {

                                    var strJson = "[[";

                                    var length = 0;
                                    for (var key in data[0]) {
                                        length++;
                                    }

                                    for (var key in data[0]) {

                                        if (key !== "rowIndex") {
                                            if (Grid.FrozenColumns[0].length == 0
                                                || IsContainColumn(Grid.FrozenColumns[0], key) == false) {
                                                strJson += "{field:'" + key + "',title:'" + key + "'" + (Grid.Sort != "" ? ",sortable:true" : "") + ",align:'center'" + ((length < 15) ? ",width:80" : ",width:'100px'") + "},";
                                            }
                                        }
                                    }
                                    if (strJson != "[[") {

                                        strJson = strJson.substring(0, strJson.length - 1);
                                    }
                                    strJson += "]]";

                                    Grid.Columns = eval(strJson);

                                    Grid.BindConfig();
                                }
                                if (Grid.ColumnsAfter[0].legend >= 0) {
                                    jQuery(Grid.ColumnsAfter[0]).each(function (i, o) {
                                        Grid.Columns[0].push(o);
                                    });
                                    Grid.ColumnsAfter = [[]];
                                    Grid.BindConfig();
                                }
                                if (Grid.Type == "grid")
                                    $(Grid.Div).datagrid('loadData', { total: res.Message, rows: data });
                                else
                                    $(Grid.Div).treegrid('loadData', {
                                        total: res.Message, rows: data
                                    });


                            } else {
                                $.messager.alert("错误", "操作失败！" + res.Message, "error");
                            }
                        }
                    });
                }
            },
            DataBindAfter: function (DataBindAfterFun) {
                DataBindAfterFun();
                Grid.BindPage();
            },
            BindPage: function (CurrentNumber) {
                var pg = $(Grid.Div).datagrid('getPager');

                if (pg) {
                    $(pg).pagination({
                        pageNumber: CurrentNumber, //默认显示第几页   
                        pageSize: Grid.PageSize,
                        showPageList: false,
                        beforePageText: '第',
                        afterPageText: '页     共{pages}页',
                        displayMsg: 'Displaying {from} to {to} of {total} items',
                        onBeforeRefresh: function () {

                        },
                        onRefresh: function (pageNumber, pageSize) {

                        },
                        onChangePageSize: function () {

                        },
                        onSelectPage: function (pageNumber, pageSize) {

                            Grid.DataBind(pageNumber);

                        }
                    });
                }

            },

            //div:查询控件所在的DIV，pd:查询条件
            Search: function (div, pd) {
                var postData = [];
                if (pd) {
                    postData = pd;
                }
                else {
                    if (div && div != "") {
                        Grid.DivSearch = div;
                    }
                    postData = GetWebControls(Grid.DivSearch);
                }
                if (Grid.ProcName != "") {
                    Grid.ViewName = Grid.ProcName;
                }

                if (Grid.ViewName != "") {
                    postData.push({ Key: "ViewName", Value: Grid.ViewName, "Operator": "", "ColumnName": "ViewName" });
                }
                if (Grid.SortColumn != "") {
                    postData.push({ Key: "SortColumn", Value: Grid.SortColumn, "Operator": "", "ColumnName": "SortColumn" });
                }
                if (Grid.Sort != "") {
                    postData.push({ Key: "sort", Value: Grid.Sort, "Operator": "=", "ColumnName": "sort" });
                }
                if (Grid.Order != "") {
                    postData.push({ Key: "order", Value: Grid.Order, "Operator": "=", "ColumnName": "order" });
                }
                if (Grid.ParameterAfter.length >= 1) {
                    jQuery(Grid.ParameterAfter).each(function (i, o) {
                        postData.push(o);
                    });
                }
                Grid.URL = Grid.URL;
                Grid.PostData = postData;
                Grid.Init();
                Grid.DataBind(1);

            },
            Export: function (ExportUrl, pd) {
                var postData = [];
                if (pd) {
                    postData = pd;
                }
                else {
                    postData = GetWebControls(Grid.DivSearch);
                }

                if (Grid.ViewName != "") {
                    postData.push({ Key: "ViewName", Value: Grid.ViewName, "Operator": "", "ColumnName": "ViewName" });
                }
                if (Grid.SortColumn != "") {
                    postData.push({ Key: "SortColumn", Value: Grid.SortColumn, "Operator": "", "ColumnName": "SortColumn" });
                }
                if (Grid.ParameterAfter.length >= 1) {
                    jQuery(Grid.ParameterAfter).each(function (i, o) {
                        postData.push(o);
                    });
                }

                Grid.PostData = postData;

                var parList = Grid.PostData;
                if (!Grid.IsImportAllColumn) {
                    $(Grid.Columns[0]).each(function (i, o) {
                        parList.push({ Key: o.field, Value: o.title, "Operator": "export", "ImportColumnName": o.name, "ColumnName": o.field });

                    });
                } else {
                    $(Grid.Columns[0]).each(function (i, o) {
                        parList.push({ Key: o.field, Value: o.title, "Operator": "export", "ColumnName": o.field });

                    });
                }

                var strJson = JSON.stringify(parList);
                strJson = encodeURIComponent(strJson);

                var frmExport = document.createElement("form");
                frmExport.id = "frmExport";
                frmExport.name = "frmExport";
                document.body.appendChild(frmExport);
                var input = document.createElement("input");
                input.type = "hidden";
                input.name = "w";
                input.value = strJson;
                frmExport.appendChild(input);
                //frmExport.method = "POST";
                //frmExport.action = ExportUrl;
                $(frmExport).ajaxSubmit({
                    type: 'post',
                    url: ExportUrl,
                    success: function (data) {
                        if (data != "") {
                            window.location.href = data;
                        }
                    },
                    error: function (err) {
                        alert(err);
                    }
                });
                //frmExport.submit();
                document.body.removeChild(frmExport);

            }
        }
        return Grid;
    }
}
var Datagrid = ZYQDatagrid.CreateNew();
//#endregion

function IsContainColumn(arr, str) {
    var IsContain = false;
    jQuery(arr).each(function (i, o) {
        if (o.field == str) {
            IsContain = true;
            return true;
        }
    });
    return IsContain;
}

function BindTable(TableId, url, postData) {

    $.ajax({
        url: url,
        data: JSON.stringify(postData),
        type: 'POST',
        async: true,
        contentType: 'application/json',
        success: function (res) {
            var strTable = "";
            var strHead = '<thead><tr>';
            var strbody = '<tbody>';
            if (res.Result == "1") {

                var data = jQuery.parseJSON(res.Data);

                for (var key in data.Table[0]) {
                    if (key !== "rowIndex") {
                        strHead += '<th>' + key + '</th>';
                    }
                }
                jQuery(data.Table).each(function (i, o) {
                    strbody += '<tr>';
                    for (var key in data.Table[0]) {
                        if (key !== "rowIndex") {
                            strbody += '<td>' + o.key + '</td>';
                        }
                    }
                    strbody += '</tr>';
                });

                strbody += '</tbody>';
                strHead += '</tr></thead>';

                strTable = strHead + strbody;
                $(TableId).html(strTable);
                //$(Grid.Div).datagrid('loadData', { total: res.Message, rows: data });


            } else {
                $.messager.alert("错误", "操作失败！" + res.Message, "error");
            }
        }
    });
}

function BindEasyTable(TableId, url, postData, FrozenColumns) {
    $.ajax({
        url: url,
        data: JSON.stringify(postData),
        type: 'POST',
        async: true,
        contentType: 'application/json',
        success: function (res) {
            var arrColumns = [];
            var strTable = "";
            var strHead = '<thead class="datagrid-header" ><tr class="datagrid-header-row">';
            var strbody = '<tbody class="datagrid-btable" >';
            if (res.Result == "1") {

                var data = jQuery.parseJSON(res.Data);

                if (FrozenColumns && FrozenColumns.length >= 1) {
                    for (var key in FrozenColumns) {
                        arrColumns.push(FrozenColumns[key].field);
                    }
                }
                for (var key in data[0]) {
                    if (key !== "rowIndex") {
                        if (arrColumns.indexOf(key) <= -1) {
                            arrColumns.push(key);
                        }
                    }
                }
                for (var key in arrColumns) {
                    strHead += '<td class="datagrid-cell">' + arrColumns[key] + '</td>';
                }
                jQuery(data).each(function (i, o) {
                    strbody += '<tr class="datagrid-row" onclick="OnSelect(this)">';
                    var strValue = "";
                    var strColumn = "";
                    for (var key in arrColumns) {
                        strColumn = arrColumns[key];
                        if (o[strColumn] == "undefined"  ) {
                            strValue = "";
                        }
                        else {
                            strValue = o[strColumn];
                        }
                        strbody += '<td class="datagrid-cell">' + strValue + '</td>';

                    }
                    strbody += '</tr>';
                });

                strbody += '</tbody>';
                strHead += '</tr></thead>';

                strTable = strHead + strbody;
                $(TableId).html(strTable);
                //$(Grid.Div).datagrid('loadData', { total: res.Message, rows: data });

                $(TableId).height($(window).height() - $("#divSearch").find("tr").length * 25 - 65);

            } else {
                $.messager.alert("错误", "操作失败！" + res.Message, "error");
            }
        }
    });
}


$(document).ready(function () {
    $("#datagrid").height($(window).height() - $("#divSearch").find("tr").length * 25 - 65);
});

function OnSelect(o) {
    $(".datagrid-row-selected").removeClass("datagrid-row-selected");
    $(o).addClass("datagrid-row-selected");
}

//#region   获取查询条件
/*
自动获取页面控件值
*/
function GetWebControls(element) {
    var reVal = "";
    var operator = "";
    var ColumnName = "";
    var OrColumnName = "";
    var ParameterList = [];
    $(element).find('input,select,textarea').each(function (r) {
        var id = $(this).attr('id');
        var value = $(this).val();
        var type = $(this).attr('type');
        operator = $(this).attr('operator');
        if ($(this).hasClass("no-post")) {
            return;
        }
        if ($(this).is(":hidden") && id != "ViewName" && id != "SortColumn") {
            return;
        }
        if (operator) {
            operator = $.trim(operator);
        } else {
            operator = "=";
        }
        if ($(this).attr('ColumnName')) {
            ColumnName = $(this).attr('ColumnName');
        }
        else {
            ColumnName = id;
        }
        if ($(this).attr('OrColumnName')) {
            OrColumnName = $(this).attr('OrColumnName');
        }
        else {
            OrColumnName = "";
        }
        switch (type) {
            case "radio":
            case "checkbox":
                if (id && id != "") {

                    if ($(this).prop("checked")) {
                        ParameterList.push({ Key: id, Value: "1", "Operator": operator, "ColumnName": ColumnName, "OrColumnName": OrColumnName });
                    } else {
                        ParameterList.push({ Key: id, Value: "0", "Operator": operator, "ColumnName": ColumnName, "OrColumnName": OrColumnName });
                    }

                }
                break;
            case "text":
            case "textarea":
            case "hidden":
                if (id && id != "") {
                    ParameterList.push({ Key: id, Value: $.trim(value), "Operator": operator, "ColumnName": ColumnName, "OrColumnName": OrColumnName });
                }

                break;
            default:
                if (id && id != "") {

                    ParameterList.push({ Key: id, Value: $.trim(value), "Operator": operator, "ColumnName": ColumnName, "OrColumnName": OrColumnName });

                }
                break;
        }

    });

    $(element).find('.easyui-datebox,.easyui-datetimebox,.easyui-combobox,.easyui-textbox,.easyui-numberbox ').each(function (r) {
        var id = $(this).attr('id');
        var value = "";
        if ($(this).hasClass("no-post")) {
            return;
        }
        if ($(this).hasClass("easyui-combobox")) {
            value = $(this).combobox('getValues');
        }
        else if ($(this).hasClass("easyui-datebox") || $(this).hasClass("easyui-datetimebox")) {
            value = $(this).datetimebox("getValue");
        }
        else {

            value = $(this).textbox('getValue');

        }

        operator = $(this).attr('operator');

        if (operator) {
            operator = $.trim(operator);
        } else {
            operator = "=";
        }
        if ($(this).attr('ColumnName')) {
            ColumnName = $(this).attr('ColumnName');
        }
        else {
            ColumnName = id;
        }
        if ($(this).attr('OrColumnName')) {
            OrColumnName = $(this).attr('OrColumnName');
        }
        else {
            OrColumnName = "";
        }

        if (ColumnName && id)
            ParameterList.push({ Key: id, Value: $.trim(value), "Operator": operator, "ColumnName": ColumnName, "OrColumnName": OrColumnName });
    });


    return ParameterList;
}
function appendRow() {
    var tempdata = [];
    for (var i = Datagrid.CurrentIndex; i < Datagrid.Data.length; i++) {
        tempdata.push(Datagrid.Data[i]);
        $(Datagrid.Div).datagrid('appendRow', Datagrid.Data[i]);
        if (Datagrid.CurrentIndex++ % Datagrid.PageSize == 0)
            break;
    }
    //$(Datagrid.Div).datagrid('load', tempdata);
    $("body").unmask();
}



function scroll(viewid, scrollid, size) {
    // 获取滚动条容器  
    var scroll = document.getElementById(scrollid);
    // 将表格拷贝一份  
    var tb2 = document.getElementById(viewid).cloneNode(true);
    // 获取表格的行数  
    var len = tb2.rows.length;
    // 将拷贝得到的表格中非表头行删除  
    for (var i = tb2.rows.length; i > size; i--) {
        // 每次删除数据行的第一行  
        tb2.deleteRow(size);
    }
    // 创建一个div  
    var bak = document.createElement("div");
    // 将div添加到滚动条容器中  
    scroll.appendChild(bak);
    // 将拷贝得到的表格在删除数据行后添加到创建的div中  
    bak.appendChild(tb2);
    // 设置创建的div的position属性为absolute，即绝对定于滚动条容器（滚动条容器的position属性必须为relative）  
    bak.style.position = "absolute";
    // 设置创建的div的背景色与原表头的背景色相同（貌似不是必须）  
    bak.style.backgroundColor = "#cfc";
    // 设置div的display属性为block，即显示div（貌似也不是必须，但如果你不希望总是显示拷贝得来的表头，这个属性还是有用处的）  
    bak.style.display = "block";
    // 设置创建的div的left属性为0，即该div与滚动条容器紧贴  
    bak.style.left = 0;
    // 设置div的top属性为0，初期时滚动条位置为0，此属性与left属性协作达到遮盖原表头  
    bak.style.top = "0px";
    // 给滚动条容器绑定滚动条滚动事件，在滚动条滚动事件发生时，调整拷贝得来的表头的top值，保持其在可视范围内，且在滚动条容器的顶端  
    scroll.onscroll = function () {
        // 设置div的top值为滚动条距离滚动条容器顶部的距离值  
        bak.style.top = this.scrollTop + "px";
    }
}
