"use strict";

KenoModule.controller('mainController', function ($scope, $timeout, kenoRepositories, ngDialog, commonService) {
    $scope.kenoSetting = kenoRepositories.kenoSetting;
    $scope.isUpdatedKeno = false;
    $scope.isUpdatePT = false;
    $scope.isUpdateBetSetting = false;
    $scope.confirmIsAutoArray = [];
    $scope.errors = {};

    console.log($scope.kenoSetting);

    $scope.$watch(function () {
        if ($scope.isUpdatePT && $scope.isUpdateBetSetting) {
            $scope.isUpdatedKeno = true;
            return;
        }

        $scope.isUpdatedKeno = false;
    });

    $scope.$on('betSettingPartial', function (event, data) {
        $scope.isUpdateBetSetting = data;
    });

    $scope.$on('positionTakingPartial', function (event, data) {
        $scope.isUpdatePT = data;
    });

    $scope.updated = function (isUpdatePartial, partialName) {
        if (isUpdatePartial) {
            switch (partialName) {
                case "PositionTaking":
                    $scope.kenoSetting.betSettings = null;
                    break;
                case "BetSetting":
                    $scope.kenoSetting.positionTakings = null;
                    break;
                default:
                    break;
            }
        }

        if ($scope.confirmIsAutoArray.length > 0) {
            $scope.$broadcast("updateAllMember");
            $scope.$on('confirmUpdateAllMember', function (event) {
                $scope.doUpdated($scope.getUrlUpdated());
            });
        }
        else {
            $scope.doUpdated($scope.getUrlUpdated());
        }
    };

    $scope.getUrlUpdated = function () {
        var url = "";
        switch (kenoRepositories.kenoSetting.customerInformation.actionType) {
            case 4: url = 'customersetting/KenoSetting/kenosetting/UpdateSuper';
                break;
            case 3: url = 'customersetting/KenoSetting/kenosetting/UpdateMaster';
                break;
            case 2: url = 'customersetting/KenoSetting/kenosetting/UpdateAgent';
                break;
            case 21: url = 'customersetting/KenoSetting/kenosetting/UpdateAgentOnMember';
                break;
            case 41: url = 'customersetting/KenoSetting/kenosetting/UpdateSuperOnMember';
                break;
            case 31: url = 'customersetting/KenoSetting/kenosetting/UpdateMasterOnMember';
                break;
            default:
                break;
        }

        return domain + url;
    }

    $scope.showMesseges = function (html) {
        $("#divShowInfo").append(html);
        commonService.unblockUI();
    }

    $scope.doUpdated = function (url) {
        commonService.blockUI();
        kenoRepositories.save(url, $scope.kenoSetting).then(
            function (results) {
                $("#divShowInfo").empty();
                var html = "";
                if (typeof results != 'undefined') {
                    var length = results.length;
                    var isError = false;
                    for (var i = 0; i < length; i++) {
                        if (results[i].ErrorCode > 0) {
                            isError = true;
                            html = '<div class="alerterror">' + results[i].ErrorMessage + '</div>';
                        }
                    }

                    if (!isError) {
                        $timeout(function () {
                            $scope.showMesseges('<div class="alertsuccess">' + resources.successful + '</div>');
                            location.reload();
                            parent.frames["centerFr"].location.reload();
                        }, 5000);
                    } else {
                        $scope.showMesseges(html);
                    }
                }
                else {
                    $scope.showMesseges('<div class="alerterror">' + resources.failed + '<</div>');
                }
            },
            function () {
                $scope.showMesseges('<div class="alerterror">' + resources.failed + '</div>');
            });
    };
});
