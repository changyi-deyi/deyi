/// <reference path="../assets/js/jquery-2.0.3.min.js" />



function operateChannel(ChannelID) {
    var ChannelName = $.trim($("#txtChannelName").val());
    if (ChannelName == "") {
        alert("请输入渠道名称");
        return;
    }


    var model = {
        ChannelID: ChannelID,
        ChannelName: ChannelName
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateChannel",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/ChannelList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteChannel(ChannelID, Status) {
    var model = {
        ChannelID: ChannelID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteChannel",
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