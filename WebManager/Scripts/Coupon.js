/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var Status = $("#ddlSearchStatus").val();
    var Type = $("#ddlSearchType").val();
    var Name = $("#txtSearchName").val();
    window.location.href = "/Coupon/CouponList?s=" + Status + "&t=" + Type + "&cn=" + Name;

}



function operateCoupon(ID) {
    var Name = $.trim($("#txtName").val());
    if (Name == "") {
        alert("请输入名称");
        return;
    }
    var ImageURL = $("#img").attr("f");
    if (ImageURL == "") {
        alert("请上传图片");
        return;
    }

    var Detail = $.trim($("#txtDetail").val());
    var Type = $.trim($("#ddlType").val());
    if (Type == 0) {
        alert("请选择类型");
        return;
    }

    var Rule = $.trim($("#txtRule").val());
    if (Rule == "") {
        alert("请输入规则");
        return;
    }

    var IsDuplicate = $("input[name='IsDuplicate']:checked").val();
    if (typeof (IsDuplicate) == "undefined") {
        alert("请选择可否重复领取");
        return;
    }

    var IsReuse = $("input[name='IsReuse']:checked").val();
    if (typeof (IsReuse) == "undefined") {
        alert("请选择可否多次使用");
        return;
    }


    var ExchangeType = $("input[name='ExchangeType']:checked").val();
    if (typeof (ExchangeType) == "undefined") {
        alert("请选择兑换方式");
        return;
    }


    var ExchangeAmount = $.trim($("#txtExchangeAmount").val());
    if (ExchangeAmount == "") {
        alert("请输入兑换金额");
        return;
    }
    var MaxQty = $.trim($("#txtMaxQty").val());
    if (MaxQty == "") {
        alert("请输入最大数量");
        return;
    }

    var ValidType = $("input[name='ValidType']:checked").val();
    if (typeof (ValidType) == "undefined") {
        alert("请选择有效期类型");
        return;
    }
    var ValidRUle = $.trim($("#txtValidRUle").val());
    if (ValidRUle == "") {
        alert("请输入有效期规则");
        return;
    }

    var Weights = $.trim($("#txtWeights").val());
    if (Weights == "") {
        alert("请输入权重");
        return;
    }

    var model = {
        Name: Name,
        ImageURL: ImageURL,
        Detail: Detail,
        Type: Type,
        Rule: Rule,
        IsDuplicate: IsDuplicate,
        IsReuse: IsReuse,
        ExchangeType: ExchangeType,
        ExchangeAmount: ExchangeAmount,
        MaxQty: MaxQty,
        ValidType: ValidType,
        ValidRUle: ValidRUle,
        Weights: Weights,
        ID: ID
    }

    $.ajax({
        type: "POST",
        url: "/Coupon/OperateCoupon",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(model),
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("网络繁忙,请稍后再试！");
        },
        success: function (data) {
            if (data.Code == "1") {
                alert(data.Message);
                window.location.href = "/Coupon/CouponList"
            }
            else {
                alert(data.Message);
            }
        }
    });
}




function deleteCoupon(ID, Status) {
    var model = {
        ID: ID,
        Status: Status
    }

    if (confirm("确定要提交该操作么?")) {
        $.ajax({
            type: "POST",
            url: "/Coupon/DeleteCoupon",
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