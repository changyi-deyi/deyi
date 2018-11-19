/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var StaffName = $("#txtSearchStaffName").val();
    var Role = $("#ddlSearchRole").val();
    window.location.href = "/Staff/StaffList?sn=" + StaffName + "&r=" + Role;

}




function operateStaff(StaffCode, UserID) {
    var LoginUserName = "";
    if (StaffCode == "") {
        LoginUserName = $.trim($("#txtLoginUserName").val());
        if (LoginUserName == "") {
            alert("请输入登陆账号");
            return;
        }
    }




    var Name = $.trim($("#txtName").val());
    if (Name == "") {
        alert("请输入姓名");
        return;
    }
    var Mobile = $.trim($("#txtMobile").val());
    if (Mobile == "") {
        alert("请输入手机");
        return;
    }
    var Gender = $("input[name='Gender']:checked").val();
    if (typeof (Gender) == "undefined") {
        alert("请选择性别");
        return;
    }
    var Role = $("input[name='Role']:checked").val();
    if (typeof (Role) == "undefined") {
        alert("请选择角色权限");
        return;
    }

    var Status = $("input[name='Status']:checked").val();
    if (typeof (Status) == "undefined") {
        alert("请选择状态");
        return;
    }
    var User = {
        LoginUserName: LoginUserName,
        Type: 3,
        Status: Status
    }

    var Staff = {
        UserID: UserID,
        StaffCode: StaffCode,
        Name: Name,
        Gender: Gender,
        Role: Role,
        Mobile: Mobile,
        Status: Status

    }

    var model = {
        UserID: UserID,
        UserCode: StaffCode,
        User: User,
        Staff: Staff
    }

    $.ajax({
        type: "POST",
        url: "/Staff/OperateStaff",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Staff/StaffList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}


function deleteStaff(UserID,StaffCode, Status) {
    var model = {
        UserID: UserID,
        UserCode: StaffCode,
        Type: 3,
        Status: Status
    }

    if (confirm("确认删除此运营人员？")) {
        $.ajax({
            type: "POST",
            url: "/Staff/DeleteStaff",
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