/// <reference path="../assets/js/jquery-2.0.3.min.js" />



function operateTag(TagID) {
    var TagName = $.trim($("#txtTagName").val());
    if (TagName == "") {
        alert("请输入标签名称");
        return;
    }

    
    var model = {
        TagID: TagID,
        TagName: TagName
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateTag",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/TagList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteTag(TagID, Status) {
    var model = {
        TagID: TagID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteTag",
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