/// <reference path="../assets/js/jquery-2.0.3.min.js" />



function operateHospital(HospitalID) {
    var HospitalName = $.trim($("#txtHospitalName").val());
    if (HospitalName == "") {
        alert("请输入医院名称");
        return;
    }


    var Introduction = $.trim(txtIntroduction.getContent());
    if (Introduction == "") {
        alert("请输入介绍");
        return;
    }

    var model = {
        HospitalID: HospitalID,
        HospitalName: HospitalName,
        Introduction: Introduction
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateHospital",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/HospitalList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteHospital(HospitalID, Status) {
    var model = {
        HospitalID: HospitalID,
        Status: Status
    }

    if (confirm("确定要删除该医院么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteHospital",
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