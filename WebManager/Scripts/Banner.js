/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var Status = $("#ddlSearchStatus").val();
    window.location.href = "/Setting/BannerList?s=" + Status;

}



function operateBanner(ID) {
    var ImageURL = $("#img").attr("f");
    if (ImageURL == "") {
        alert("请上传图片");
        return;
    }
    var Type = $.trim($("#ddlType").val());
    var Value = $.trim($("#txtValue").val());
    
    var model = {
        ID: ID,
        ImageURL: ImageURL,
        Type: Type,
        Value: Value
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateBanner",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/BannerList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteBanner(ID, Status) {
    var model = {
        ID: ID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteBanner",
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


function UpdateSort(ID) {

    var Sort = $("#txtSort" + ID).val();
    if (Sort == "") {
        alert("排序不能位空");
        return;
    } else if (isNaN(Sort)) {
        alert("排序必须为数字");
        return;
    }

    var model = {
        ID: ID,
        Sort: Sort
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/UpdateSort",
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