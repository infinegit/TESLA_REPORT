function RequestR(strName) {
    var url = window.location.href;
    var Pos = url.indexOf("?");
    var strRight = url.substr(Pos + 1);
    var arrTemp = url.split("=");
    return arrTemp[1] + "=" + arrTemp[2];

}