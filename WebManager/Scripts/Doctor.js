/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var DoctorName = $("#txtSearchDoctorName").val();
    var HostpitalID = $("#ddlSearchHospital").val();
    var DepartmentID = $("#ddlSearchDepartment").val();
    window.location.href = "/Doctor/DoctorList?dn=" + DoctorName + "&hId=" + HostpitalID + "&dId=" + DepartmentID;

}


function operateDoctor(DoctorCode, UserID) {
    var LoginUserName = "";
    if (DoctorCode == "") {
        LoginUserName = $.trim($("#txtLoginUserName").val());
        if (LoginUserName == "") {
            alert("请输入登陆账号");
            return;
        }
    }

 


    var Name = $.trim($("#txtName").val());
    if (Name == "") {
        alert("请输入医生名称");
        return;
    }

    var Gender = $("input[name='Gender']:checked").val();
    if (typeof (Gender) == "undefined") {
        alert("请选择性别");
        return;
    }

    var Birthday = $.trim($("#txtBirthday").val());
    if (Birthday == "") {
        alert("请选择生日");
        return;
    }

    var HospitalID = $.trim($("#ddlHospital").val());
    if (HospitalID == 0) {
        alert("请选择医院");
        return;
    }

    var DepartmentID = $.trim($("#ddlDepartment").val());
    if (DepartmentID == 0) {
        alert("请选择科室");
        return;
    }


    var TitleID = $.trim($("#ddlTitle").val());
    if (TitleID == 0) {
        alert("请选择职称");
        return;
    }

    var GoodAt = $.trim($("#txtGoodAt").val());
    var Weights = $.trim($("#txtWeights").val());
    var Introduction = $.trim(txtIntroduction.getContent());


    var Phone = $.trim($("#txtPhone").val());
    if (Phone == "") {
        alert("请输入联系电话");
        return;
    }

    var Type = $("input[name='Type']:checked").val();
    if (typeof (Type) == "undefined") {
        alert("请选择类别");
        return;
    }

    var Status = $("input[name='Status']:checked").val();
    if (typeof (Status) == "undefined") {
        alert("请选择状态");
        return;
    }

    var ImageURL = $("#img").attr("f");
    if (ImageURL == "") {
        alert("请上传图片");
        return;
    }

    var listTag = new Array();

    $("#divTag input[name='Tag']:checked").each(function () {
        listTag.push($(this).val());
    });

    var User = {
        LoginUserName: LoginUserName,
        Type: 2,
        Status: Status
    }

    var Doctor = {
        UserID: UserID,
        DoctorCode: DoctorCode,
        Name: Name,
        Gender: Gender,
        Birthday: Birthday,
        HospitalID: HospitalID,
        DepartmentID: DepartmentID,
        TitleID: TitleID,
        GoodAt: GoodAt,
        Introduction: Introduction,
        Phone: Phone,
        Type: Type,
        Status: Status,
        ImageURL: ImageURL,
        listTag: listTag,
        Weights: Weights

    }

    var model = {
        UserID:UserID,
        UserCode:DoctorCode,
        User: User,
        Doctor: Doctor
    }

    $.ajax({
        type: "POST",
        url: "/Doctor/OperateDoctor",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Doctor/DoctorList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}

function deleteDoctor(UserID, DoctorCode, Status) {
    var model = {
        UserID: UserID,
        UserCode: DoctorCode,
        Type: 2,
        Status: Status
    }

    if (confirm("确定要删除该医生么?")) {
        $.ajax({
            type: "POST",
            url: "/Doctor/DeleteDoctor",
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