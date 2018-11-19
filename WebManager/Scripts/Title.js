/// <reference path="../assets/js/jquery-2.0.3.min.js" />



function operateTitle(TitleID) {
    var TitleName = $.trim($("#txtTitleName").val());
    if (TitleName == "") {
        alert("请输入职称名称");
        return;
    }

    
    var model = {
        TitleID: TitleID,
        TitleName: TitleName
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateTitle",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/TitleList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteTitle(TitleID, Status) {
    var model = {
        TitleID: TitleID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteTitle",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(model),
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("网络繁忙,请稍后再试！");
            },
            success: function (data) {
                if (data.Code == "1") {
                    alert(data.Message);
                    window.location.reload();
                }
                else {
                    alert(data.Message);
                }
            }
        });
    }
}