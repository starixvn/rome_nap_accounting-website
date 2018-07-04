"use strict";

KenoModule.controller('mainController', function ($scope, $timeout, kenoRepositories, ngDialog, commonService) {
    kenoRepositories.getData().then(
        function (kenoSetting) {
            $scope.kenoSetting = kenoSetting;
            $scope.customerInformation = $scope.kenoSetting.customerInformation;
            $scope.betSettingList = $scope.kenoSetting.betSettings;
            $scope.betSettingCopyAll = $scope.kenoSetting.betSettingCopyAll;
            $scope.positionTakingList = $scope.kenoSetting.positionTakings;
            $scope.positionTakingCopyAll = $scope.kenoSetting.positionTakingCopyAll;
            $scope.statusCopyAll = $scope.kenoSetting.statusCopyAll;

            $scope.isUpdatedKeno = false;
            $scope.isPositionTakingPartial = false;
            $scope.isBetSettingPartial = false;
            $scope.isBetSettingValid = true;
            $scope.confirmIsAutoArray = [];
            $scope.errors = {};

            $scope.$watch('[isBetSettingPartial, isPositionTakingPartial, isBetSettingValid]', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.isUpdatedKeno = ($scope.isPositionTakingPartial || $scope.isBetSettingPartial) && $scope.isBetSettingValid;
                }
            }, true);

            $scope.updated = function (isUpdatePartial, partialName) {
                if ($scope.positionTakingCopyAll.isCopiedAll) {
                    $scope.$broadcast('resetPositionTaking', { "betTypeId": $scope.positionTakingCopyAll.betTypeId });
                }

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
                } else {
                    $scope.doUpdated($scope.getUrlUpdated());
                }
            };

            $scope.getUrlUpdated = function () {
                var url = "";
                switch ($scope.kenoSetting.customerInformation.actionType) {
                    case 4:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateSuper';
                        break;
                    case 3:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateMaster';
                        break;
                    case 2:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateAgent';
                        break;
                    case 21:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateAgentOnMember';
                        break;
                    case 41:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateSuperOnMember';
                        break;
                    case 31:
                        url = 'customersetting/KenoSetting/kenosetting/UpdateMasterOnMember';
                        break;
                    default:
                        break;
                }

                return domain + url;
            }

            $scope.showMesseges = function (html, isSuccess) {
                if (isSuccess) {
                    $timeout(function () {
                        $("#divShowInfo").append(html);
                        commonService.unblockUI();
                    }, 4000);
                }
                else {
                    $("#divShowInfo").append(html);
                    commonService.unblockUI();
                }
            }

            $scope.doUpdated = function (url) {
                commonService.blockUI();
                kenoRepositories.save(url, $scope.kenoSetting).then(
                    function (results) {
                        $("#divShowInfo").empty().show();

                        var html = "";
                        if (typeof results != 'undefined' && typeof results != 'string') {
                            var length = results.length;
                            var isError = false;
                            for (var i = 0; i < length; i++) {
                                if (results[i].ErrorCode > 0) {
                                    isError = true;
                                    html += '<div class="alerterror">' + results[i].ErrorMessage + '</div>';
                                }
                            }

                            if (!isError) {
                                $scope.showMesseges('<div class="alertsuccess">' + resources.successful + '</div>', true);
                                $timeout(function () {
                                    location.reload();
                                    (parent.fContent || parent.centerFr).location.reload();
                                }, 5000);
                            } else {
                                $scope.showMesseges(html, false);
                            }
                        } else {
                            $scope.showMesseges(results, false);
                        }
                    },
                    function (data) {
                        $scope.showMesseges('<div class="alerterror">' + (data && data.ErrorCode === 99999 ? data.ErrorMessage : resources.failed) + '</div>', false);
                    });
            };
        }, function (reason) { }, function (update) { });
});