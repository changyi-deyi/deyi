/// <reference path="../assets/js/jquery-2.0.3.min.js" />


function operateGetPoint(key) {
    var value = "";
    switch (key) {
        case "IntroText":
            value = $.trim(txtIntroText.getContent());
            break;
        case "SingleReward":
            value = $.trim($("#txtSingleReward").val());
            break;
        case "Cycle":
            value = $.trim($("#txtCycle").val());
            break;
        case "CycleReward":
            value = $.trim($("#txtCycleReward").val());
            break;
        case "UploadReward":
            value = $.trim($("#txtUploadReward").val());
            break;
        case "JoinReward":
            value = $.trim($("#txtJoinReward").val());
            break;
    }

    if (value == "") {
        alert("该属性不能为空");
    }


    var model = {
        key: key,
        value:value
    }


    $.ajax({
        type: "POST",
        url: "/Setting/OperateGetPoint",
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