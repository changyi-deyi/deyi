/// <reference path="../assets/js/jquery-2.0.3.min.js" />

function changeRowCount(url) {
    var rc = $("#ddlRowsCount").val();
    window.location.href = url + "&rc=" + rc + "&pc=1";
}


function datCompare(date1, date2) {
    var oDate1 = new Date(date1);
    var oDate2 = new Date(date2);
    if (oDate1.getTime() > oDate2.getTime()) {
        return 1;
    } else if (oDate1.getTime() < oDate2.getTime()) {
        return 2;
    } else {
        return 3;
    }
}



function SelectAll(Name) {
    var allName = Name + "All";
    var checked = $("#" + allName).prop("checked");
    if (checked) {
        $("#tbBody [name='" + Name + "']").prop("checked","checked");
    } else {
        $("#tbBody [name='" + Name + "']").prop("checked", "");
    }
}