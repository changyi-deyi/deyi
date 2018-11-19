/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var Status = $("#ddlSearchStatus").val();
    window.location.href = "/Level/LevelList?s=" + Status;

}



function operateLevel(LevelID) {
    var Name = $.trim($("#txtName").val());
    if (Name == "") {
        alert("请输入名称");
        return;
    }
    var IconURL = $("#img").attr("f");
    var Summary = $.trim(txtSummary.getContent());

    var TermYears = $.trim($("#txtTermYears").val());
    if (TermYears == "") {
        alert("请输入期限");
        return;
    }
    var OriginPrice = $.trim($("#txtOriginPrice").val());
    if (OriginPrice == "") {
        alert("请输入原价");
        return;
    }
    var PromPrice = $.trim($("#txtPromPrice").val());
    if (PromPrice == "") {
        alert("请输入优惠价");
        return;
    }

    var model = {
        Name: Name,
        IconURL: IconURL,
        Summary: Summary,
        TermYears: TermYears,
        OriginPrice: OriginPrice,
        PromPrice: PromPrice,
        LevelID: LevelID
    }

    $.ajax({
        type: "POST",
        url: "/Level/OperateLevel",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Level/LevelList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteLevel(LevelID, Status) {
    var model = {
        LevelID: LevelID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Level/DeleteLevel",
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