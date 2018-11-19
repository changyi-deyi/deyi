var app = angular.module('LoginApp', []);
app.controller('LoginController', function ($scope, $http, $interval) {
    $scope.canClick = false;
    $scope.description = "获取验证码";
    $scope.Agent = "";
    var second = 59;
    var timerHandler;
    var userAgentInfo = navigator.userAgent;

    $scope.getAuthCode = function () {
        if (angular.isUndefined($scope.mobile) || $scope.mobile== "" ) {
            swal("请输入手机");
            return;
        }

        $scope.canClick = true;
               
        $http({
            method: 'post',
            url: '/Login/getAuthCode',
            params: {
                mobile: $scope.mobile
            }
        }).success(function (req) {
            if (req.Code == "1") {
                $scope.Agent = req.Data;
                timerHandler = $interval(function () {
                    if (second <= 0) {
                        $interval.cancel(timerHandler);
                        second = 59;
                        $scope.description = "获取验证码";
                        $scope.canClick = false;
                    } else {
                        $scope.description = second + "s后重发";
                        second--;
                        $scope.canClick = true;
                    }
                }, 1000);
            } else {
                swal(req.Message);
                $scope.canClick = false;
            }
        }).error(function () {
            swal("网络繁忙,请稍后再试！");
            $scope.canClick = true;
        });
    }

    $scope.Login = function () {
        if (angular.isUndefined($scope.mobile) || $scope.mobile == "") {
            swal("请输入手机");
            return;
        }

        if (angular.isUndefined($scope.Auth) || $scope.Auth == "") {
            swal("请输入验证码");
            return;
        }
        
        $http({
            method: 'post',
            url: '/Login/LoginRegister',
            params: {
                mobile: $scope.mobile,
                userAgent: userAgentInfo,
                Auth: $scope.Auth
            }
        }).success(function (req) {
            if (req.Code == "1") {
                if (req.Data == "1") {
                    window.location.href = "/Home/Home";
                } else {
                    swal({
                        text: "您是初次登陆的用户，为了更好的体验本产品，请进行身份认证",
                        buttons: ["取消", "确认"],
                    }).then((confirm) =>{
                        if (confirm) {
                            window.location.href = "/MyHome/ConfirmRealName";
                        } else {
                            window.location.href = "/Home/Home";
                        }
                    });
                }

            } else {
                swal(req.Message);
            }
        }).error(function () {
            swal("网络繁忙,请稍后再试！");
        });

    }

});