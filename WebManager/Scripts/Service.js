/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var Status = $("#ddlSearchStatus").val();
    window.location.href = "/Service/ServiceList?s=" + Status;

}



function operateService(ServiceCode) {
    var Name = $.trim($("#txtName").val());
    if (Name == "") {
        alert("请输入名称");
        return;
    }
    var Summary = $.trim($("#txtSummary").val());
    if (Summary == "") {
        alert("请输入服务简介");
        return;
    }

    var Introduct = $.trim(txtIntroduct.getContent());


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

    var ExchangePrice = $.trim($("#txtExchangePrice").val());
    if (ExchangePrice == "") {
        alert("请输入兑换价");
        return;
    }

    var ListImageURL = $("#img").attr("f");
    if (ListImageURL == "") {
        alert("请上传图片");
        return;
    }
    var IsVisible = $("input[name='IsVisible']:checked").val();
    if (typeof (IsVisible) == "undefined") {
        alert("请选择客户可见");
        return;
    }


    var model = {
        Name: Name,
        Summary: Summary,
        Introduct: Introduct,
        OriginPrice: OriginPrice,
        PromPrice: PromPrice,
        ExchangePrice: ExchangePrice,
        ListImageURL: ListImageURL,
        ServiceCode: ServiceCode,
        IsVisible: IsVisible
    }

    $.ajax({
        type: "POST",
        url: "/Service/OperateService",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Service/ServiceList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteService(ServiceCode, Status) {
    var model = {
        ServiceCode: ServiceCode,
        Status: Status
    }

    if (confirm("确认删除此服务？")) {
        $.ajax({
            type: "POST",
            url: "/Service/DeleteService",
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

function changeBargain() {
    var checked = $("#IsBargain").prop("checked");
    if (checked) {
        $("#tbBody [name='Bargain']").show();
        $("#tbBody [name='price']").hide();
    } else {

        $("#tbBody [name='Bargain']").hide();
        $("#tbBody [name='price']").show();
    }
}



function operateServiceDoctor(ServiceCode) {
    var IsBargain = 2;
    var checked = $("#IsBargain").prop("checked");
    if (checked) {
        IsBargain = 1;
    }

    var doctorList = new Array();

    $("#tbBody input[name='doctor']:checked").each(function () {
        var doctorCode = $(this).val();
        var price = 0;
        if (IsBargain == 2) {
            price = $(this).parent().parent().find("[name='price']").val();
        }
        var doctor = {
            doctorCode: doctorCode,
            IsBargain: IsBargain,
            ServiceCode: ServiceCode,
            price: price
        }
        doctorList.push(doctor);
    });

    var model = {
        ServiceCode: ServiceCode,
        Data: doctorList
    }

    
    $.ajax({
        type: "POST",
        url: "/Service/OperateServiceDoctor",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Service/ServiceList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function OperateMemberService(e, ID, ServiceCode, LevelID) {
    var Discount = $(e).prev().val();

    if (Discount == "") {
        alert("请输入折扣");
        return;
    }
    var model = {
        ID: ID,
        ServiceCode: ServiceCode,
        LevelID: LevelID,
        Discount: Discount
    }


    $.ajax({
        type: "POST",
        url: "/Service/OperateMemberService",
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



function addServiceImg(ServiceCode, FileName) {
    var model = {
        ServiceCode: ServiceCode,
        FileName: FileName
    }

    $.ajax({
        type: "POST",
        url: "/Service/addServiceImg",
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


function deleteServiceImage(ID,Status) {
    if (confirm("确定要提交该操作么?")) {
        var model = {
            ID: ID,
            Status: Status
        }
        $.ajax({
            type: "POST",
            url: "/Service/deleteServiceImg",
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



function UpdateSort(ServiceCode) {

    var Sort = $("#txtSort" + ServiceCode).val();
    if (Sort == "") {
        alert("排序不能位空");
        return;
    } else if (isNaN(Sort)) {
        alert("排序必须为数字");
        return;
    }

    var model = {
        ServiceCode: ServiceCode,
        Sort: Sort
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Service/UpdateSort",
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