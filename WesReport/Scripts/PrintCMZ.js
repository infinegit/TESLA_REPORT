function createPrintPage(cmzMstr, cmzDets) {
    if (cmzMstr == null || cmzDets == null) {
        $.messager.alert("错误", "单据异常！", "Error");
        return;
    }
    else {
        var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        LODOP.PRINT_INIT(cmzMstr.CMZCode + "打印");
        LODOP.ADD_PRINT_SETUP_BKIMG("../../Images/1172_001jpg_Page1.jpg");
        LODOP.SET_SHOW_MODE("BKIMG_PRINT", false);
        LODOP.SET_SHOW_MODE("BKIMG_LEFT", 2);
        LODOP.SET_SHOW_MODE("BKIMG_WIDTH", "275mm");
        LODOP.SET_SHOW_MODE("BKIMG_HEIGHT", "220mm");

        LODOP.ADD_PRINT_TEXT(50, 20, 100, 20, "第#页/共&页");
        LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);
        //头部信息

        printHeaderData(LODOP, cmzMstr);
        //LODOP.ADD_PRINT_TEXT(295, 750, 100, 25, "共" + cmzMstr.PkgCount + "箱");//箱数
        //明细
        var j = 0;
        var pageDataCount = 6;//每页的行数
        for (var i = 0; i < cmzDets.length; i++) {

            LODOP.ADD_PRINT_TEXT(295 + j * 25, 40, 85, 25, cmzDets[i].PartNO);//供应商零件编号号
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 120, 80, 25, cmzDets[i].CustPartNo);//客户零件编号
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 190, 350, 25, cmzDets[i].PartDesc);//零件号
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 420, 30, 25, cmzDets[i].PackageType);//包装类型
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 545, 30, 25, cmzDets[i].PartUnit);//单位
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 585, 50, 25, cmzDets[i].Qty);//数量
            LODOP.ADD_PRINT_TEXT(295 + j * 25, 750, 50, 25, cmzDets[i].PackageCount);//包装数量
            j++;
            if ((i + 1) % pageDataCount == 0 && (i + 1) != cmzDets.length) {
                j = 0;
                if (cmzDets[i].PartNO != "") {
                    LODOP.NewPageA();
                    printHeaderData(LODOP, cmzMstr);
                }
            }

        }
        LODOP.PREVIEW();//打印预览
        //LODOP.PRINT();//直接打印

    }
}
//打印头部
function printHeaderData(LODOP, cmzMstr) {

    //LODOP.SET_PRINT_STYLEA(0, "ItemType", 2);

    LODOP.SET_PRINT_STYLE("FontSize", 13);

    LODOP.ADD_PRINT_TEXT(140, 172, 200, 26, cmzMstr.DestPlace);
    LODOP.ADD_PRINT_TEXT(180, 352, 105, 25, "卡车");
    LODOP.ADD_PRINT_TEXT(180, 533, 150, 25, cmzMstr.CarPlateNo);
    LODOP.ADD_PRINT_TEXT(180, 714, 200, 25, cmzMstr.LetterNo);
    LODOP.ADD_PRINT_TEXT(180, 879, 100, 25, "销售");
    // LODOP.ADD_PRINT_BARCODE(60, 700, 300, 50, "128A", cmzMstr.CMZCode);
    LODOP.ADD_PRINT_TEXT(40, 780, 300, 20, cmzMstr.CMZCode);

    var date = new Date();

    LODOP.ADD_PRINT_TEXT(450, 585, 400, 25, "打印时间:" + date.toLocaleString());//打印时间
    LODOP.ADD_PRINT_TEXT(450, 250, 400, 25, "需求时间:" + fomatTime(cmzMstr.ExpectedArrivalTime));//需求时间
    LODOP.ADD_PRINT_TEXT(480, 585, 400, 25, cmzMstr.Memo);
    LODOP.ADD_PRINT_TEXT(700, 150, 200, 25, cmzMstr.CreateUser + " " + cmzMstr.UserName);

    LODOP.SHOW_CHART();

}
function formatterDate(date) {
    var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
    var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0"
    + (date.getMonth() + 1);
    return date.getFullYear() + '-' + month + '-' + day + ' ' + date.toLocaleTimeString();
}
function fomatTime(data) {
    if (data == null)
        return "";
    var datetime = new Date(parseInt(data.replace("/Date(", "").replace(")/", ""), 10));
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1 < 10 ? "0" + (datetime.getMonth() + 1) : datetime.getMonth() + 1;
    var currentDate = datetime.getDate() < 10 ? "0" + datetime.getDate() : datetime.getDate();
    var hour = datetime.getHours() < 10 ? "0" + datetime.getHours() : datetime.getHours();
    var minute = datetime.getMinutes() < 10 ? "0" + datetime.getMinutes() : datetime.getMinutes();
    var second = datetime.getSeconds() < 10 ? "0" + datetime.getSeconds() : datetime.getSeconds();
    return year + "年" + month + "月" + currentDate + "日" + hour + ":" + minute + ":" + second;
}
