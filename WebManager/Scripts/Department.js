/// <reference path="../assets/js/jquery-2.0.3.min.js" />



function operateDepartment(DepartmentID) {
    var DepartmentName = $.trim($("#txtDepartmentName").val());
    if (DepartmentName == "") {
        alert("请输入科室名称");
        return;
    }


    var UpperID = $.trim($("#ddlUpper").val());
    var model = {
        DepartmentName: DepartmentName,
        UpperID: UpperID,
        DepartmentID: DepartmentID
    }

    $.ajax({
        type: "POST",
        url: "/Setting/OperateDepartment",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Setting/DepartmentList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteDepartment(DepartmentID, Status) {
    var model = {
        DepartmentID: DepartmentID,
        Status: Status
    }

    if (confirm("确定要删除该科室么?")) {
        $.ajax({
            type: "POST",
            url: "/Setting/DeleteDepartment",
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