"use strict";
var MAX_VALUE = Math.pow(2, 53);

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
            $scope.confirmIsAutoArray = [];
            $scope.errors = {};

            switch ($scope.kenoSetting.customerInformation.actionType) {
                case 41:
                    $scope.isBetSettingPartial = true;
                    break;
                case 31:
                    $scope.isBetSettingPartial = true;
                    break;
                default:
                    break;
            }

            $scope.$watch('[isBetSettingPartial, isPositionTakingPartial]', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    $scope.isUpdatedKeno = $scope.isPositionTakingPartial && $scope.isBetSettingPartial;
                }
            }, true);

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

            $scope.showMesseges = function (html) {
                $("#divShowInfo").append(html).show();
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
                                    html += '<div class="alerterror">' + results[i].ErrorMessage + '</div>';
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
                        } else {
                            $scope.showMesseges('<div class="alerterror">' + resources.failed + '</div>');
                        }
                    },
                    function () {
                        $scope.showMesseges('<div class="alerterror">' + resources.failed + '</div>');
                    });
            };
        }, function (reason) { }, function (update) { });
});