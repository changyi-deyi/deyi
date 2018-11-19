/// <reference path="../assets/js/jquery-2.0.3.min.js" />

function UserLogin() {
    var LoginUserName = $.trim($("#txtUserAccount").val());
    var password = $.trim($("#txtPassWord").val());

    if (LoginUserName == "") {
        alert("请输入账号");
        return;
    }

    if (password == "") {
        alert("请输入密码");
        return;
    }

    var param = '{"LoginUserName":"' + LoginUserName + '","password":"' + password + '"}';

    $.ajax({
        type: "POST",
        url: "/Login/UserLogin",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: param,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                window.location.href = "/Home/Index";
            }
            else {
                alert(data.Message);
            }
        }
    });
}



