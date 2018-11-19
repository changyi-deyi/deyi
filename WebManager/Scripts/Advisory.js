/// <reference path="../assets/js/jquery-2.0.3.min.js" />
function Search(CustomerCode) {
    var isDone = $("#ddlSearchIsDone").val();
    var CustomerName = $("#txtSearchCustomerName").val();
    window.location.href = "/Advisory/AdvisoryList?cd=" + CustomerCode + "d=" + isDone + "&cn=" + CustomerName;

}


function AnswerAdvisory(GroupID) {
    var Content = $("#txtContent").val();
    var model = {
        GroupID: GroupID,
        Content: Content
    }

    if (confirm("确定要提交回答么?")) {
        $.ajax({
            type: "POST",
            url: "/Advisory/AnswerAdvisory",
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