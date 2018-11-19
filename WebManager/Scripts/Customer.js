/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search() {
    var CustomerName = $("#txtSearchCustomerName").val();
    var Level = $("#ddlSearchLevel").val();
    var Channel = $("#ddlSearchChannel").val();
    var Status = $("#ddlSearchStatus").val();
    window.location.href = "/Customer/CustomerList?cn=" + CustomerName + "&l=" + Level + "&c=" + Channel + "&s=" + Status;

}

function BalanceSearch(CustomerCode) {
    var StartDate = $("#txtSearchSt").val();
    var EndDate = $("#txtSearchEt").val();
    window.location.href = "/Customer/BalanceList?cd=" + CustomerCode + "&st=" + StartDate + "&et=" +  EndDate;

}


function deleteCustomer(UserID, CustomerCode, Status) {
    var model = {
        UserID: UserID,
        UserCode: CustomerCode,
        Type: 1,
        Status: Status
    }

    if (confirm("确认删除此会员？")) {
        $.ajax({
            type: "POST",
            url: "/Customer/DeleteCustomer",
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


function SearchDocument(CustomerCode) {
    var VerifyStatus = $("#ddlSearchVerifyStatus").val();
    window.location.href = "/Customer/Document?cd=" + CustomerCode + "&s=" + VerifyStatus;

}

function changeVerifyStatus(ID, VerifyStatus) {
    var model = {
        ID: ID,
        VerifyStatus: VerifyStatus
    }

    if (confirm("确认提交审核结果吗？")) {
        $.ajax({
            type: "POST",
            url: "/Customer/changeVerifyStatus",
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


function showBigImg(src) {
    $("#imgBigSrc").attr("src", src);
    $("#mymodal").show();
}


function closeBigImg() {
    $("#mymodal").hide();
    $("#imgBigSrc").attr("src", "");
}