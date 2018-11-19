/// <reference path="../assets/js/jquery-2.0.3.min.js" />

function operateMemberAct(ID) {
    var ImageURL = $("#img").attr("f");
    if (ImageURL == "") {
        alert("请上传图片");
        return;
    }
    var NAME = $.trim($("#txtName").val());
    if (NAME == "") {
        alert("请输入名称");
        return;
    }
    var LinkURL = $.trim($("#txtLinkURL").val());
    if (LinkURL == "") {
        alert("请输入链接值");
        return;
    }
    var LevelID = $.trim($("#ddlLevel").val());

    var Remark = $.trim(txtRemark.getContent());
    var model = {
        ID: ID,
        ImageURL: ImageURL,
        NAME: NAME,
        LinkURL: LinkURL,
        LevelID: LevelID,
        Remark: Remark
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateMemberAct",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/MemberActList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteMemberAct(ID, Status) {
    var model = {
        ID: ID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteMemberAct",
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

function SearchInfo(id) {
    var HandleSts = $("#ddlSearchHandleSts").val();
    window.location.href = "/Setting/MemberActInfo?id=" + id + "&s=" + HandleSts;

}


function updateHandleSts(ID) {
    var model = {
        ID: ID
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/updateHandleSts",
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


function DownloadMemberActInfo(ID) {
    var HandleSts = $("#ddlSearchHandleSts").val();
    var model = {
        ID: ID,
        HandleSts: HandleSts
    }

    $.ajax({
        type: "POST",
        url: "/Setting/DownloadMemberActInfo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = data.Data;
            }
            else {
                alert(data.Message);
            }
        }
    });
}
