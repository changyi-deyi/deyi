
function MemberOrderSearch(CustomerCode) {
    var CustomerName = $("#txtSearchCustomerName").val();
    var OrderStatus = $("#ddlSearchOrderStatus").val();
    var PaymentStatus = $("#ddlSearchPaymentStatus").val();
    var StartDate = $("#txtSearchSt").val();
    var EndDate = $("#txtSearchEt").val();
    window.location.href = "/Order/MemberOrderList?cd=" + CustomerCode+"&cn=" + CustomerName + "&st=" + StartDate + "&et=" + EndDate + "&os=" + OrderStatus + "&ps=" + PaymentStatus;

}


function CancelMemberOrder(OrderCode) {
    var model = {
        OrderCode: OrderCode
    }

    if (confirm("确定要取消该订单么?")) {
        $.ajax({
            type: "POST",
            url: "/Order/CancelMemberOrder",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(model),
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("网络繁忙,请稍后再试！");
            },
            success: function (data) {
                if (data.Code == "1") {
                    alert(data.Message);
                    window.location.href = "/Order/MemberOrderList";
                }
                else {
                    alert(data.Message);
                }
            }
        });
    }
}

function QueryPayResult(OrderCode) {
    var model = {
        OrderCode: OrderCode
    }

    if (confirm("确定要刷新支付信息么?")) {
        $.ajax({
            type: "POST",
            url: "/Order/QueryPayResult",
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



function ServiceOrderSearch(CustomerCode) {
    var CustomerName = $("#txtSearchCustomerName").val();
    var ServiceName = $("#txtSearchServiceName").val();
    var DoctorName = $("#txtSearchDoctorName").val();
    var OrderStatus = $("#ddlSearchOrderStatus").val();
    var PaymentStatus = $("#ddlSearchPaymentStatus").val();
    var ServiceStatus = $("#ddlSearchServiceStatus").val();
    var StartDate = $("#txtSearchSt").val();
    var EndDate = $("#txtSearchEt").val();
    window.location.href = "/Order/ServiceOrderList?cn=" + CustomerName + "&sn=" + ServiceName + "&dn=" + DoctorName + "&st=" + StartDate + "&et=" + EndDate + "&os=" + OrderStatus + "&ps=" + PaymentStatus + "&ss=" + ServiceStatus + "&cd" + CustomerCode;

}

function closeInput() {
    $("#mymodal").hide();
    $("#txtArrangedTime").val("");
    $("#ddlDoctor").val("");
    $("#txtAddress").val("");
    $("#txtCustomerInfo").val("");
    $("#hTitle").attr("cn", "");
    $("#hTitle").attr("m", "");
    $("#ddlStaff").val("");

}

function ShowInput(OrderCode, ArrangedTime, DoctorCode, Address,Name,Mobile,StaffCode) {
    $("#mymodal").show();
    $("#txtArrangedTime").val(ArrangedTime);
    $("#ddlDoctor").val(DoctorCode);
    $("#txtAddress").val(Address);
    $("#txtCustomerInfo").val(Name + " (" + Mobile + ")");
    $("#hTitle").attr("cn", Name);
    $("#hTitle").attr("m", Mobile);
    $("#ddlStaff").val(StaffCode);
}


function operateServiceOrder(OrderCode) {
    var OrderCode = OrderCode;
    var ArrangedTime = $("#txtArrangedTime").val();
    var DoctorCode = $("#ddlDoctor").val();
    var Address = $("#txtAddress").val();
    var Name = $("#hTitle").attr("cn");
    var Mobile = $("#hTitle").attr("m");
    var ReceptionistCode = $("#ddlStaff").val();
    if (ArrangedTime == "") {
        alert("请选择就诊时间");
        return;
    }


    if (DoctorCode == "") {
        alert("请选择医生");
        return;
    }
    if (ReceptionistCode == "") {
        alert("请选择接待人");
        return;
    }
    var HospitalName = $("#ddlDoctor").find("option:selected").attr("h");
    var DepartmentName = $("#ddlDoctor").find("option:selected").attr("d");
    var DoctorName = $("#ddlDoctor").find("option:selected").attr("dn");
    var DoctorMobile = $("#ddlDoctor").find("option:selected").attr("dm");
    var ReceptionistPhone = $("#ddlStaff").find("option:selected").attr("m");
    var model = {
        OrderCode: OrderCode,
        ArrangedTime: ArrangedTime,
        DoctorCode: DoctorCode,
        Address: Address,
        Name: Name,
        Mobile: Mobile,
        HospitalName: HospitalName,
        DepartmentName: DepartmentName,
        DoctorName: DoctorName,
        DoctorMobile: DoctorMobile,
        ReceptionistCode: ReceptionistCode,
        ReceptionistPhone: ReceptionistPhone

    }

    $.ajax({
        type: "POST",
        url: "/Order/ReplyServiceOrder",
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


function operateOrderAmount(OrderCode) {
    var OrderAmount = $("#txtOrderAmount").val();
    if (OrderAmount == "") {
        alert("请输入订单金额");
        return;
    } else if (OrderAmount < 0) {
        alert("订单金额必须大于零");
        return;
    }
    if (confirm("确认修改订单金额为" + OrderAmount+"?")) {
        var model = {
            OrderCode: OrderCode,
            OrderAmount: OrderAmount
        }
        $.ajax({
            type: "POST",
            url: "/Order/UpdateServiceOrderAmount",
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




function UpdateServiceOrderStatus(OrderCode, ServiceStatus) {
    if (confirm("确认修改服务状态吗?")) {
        var model = {
            OrderCode: OrderCode,
            ServiceStatus: ServiceStatus
        }
        $.ajax({
            type: "POST",
            url: "/Order/UpdateServiceOrderStatus",
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




function QueryServicePayResult(OrderCode) {
    var model = {
        OrderCode: OrderCode
    }

    if (confirm("确定要刷新支付信息么?")) {
        $.ajax({
            type: "POST",
            url: "/Order/QueryServicePayResult",
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