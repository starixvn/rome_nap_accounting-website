"use strict";

KenoModule.controller('betSettingController', function ($scope, kenoRepositories) {
    kenoRepositories.getData().then(function () {
        $scope.roleId = $scope.$parent.customerInformation.roleId;
        $scope.isValid = false;

        if ($scope.roleId == 4) {
            var length = $scope.betSettingList.length;
            for (var i = 0; i < length; i++) {
                var betSetting = $scope.betSettingList[i];
                betSetting.upline.maxBet = betSetting.upline.maxPerMatch = MAX_VALUE;
            }
        }

        $scope.copyAllBetSetting = function (isCopy, betTypeId) {
            $scope.betSettingCopyAll.betTypeId = isCopy == true ? betTypeId : -1;
            $scope.betSettingCopyAll.isCopiedAll = isCopy;

            $scope.$broadcast('totallyChecking');
            $scope.$broadcast('inputDisabledChecking', isCopy);
        };

        $scope.$on('totallyChecking', function (event) {
            var length = $scope.betSettingList.length;
            var updatedCount = 0;

            if ($scope.betSettingCopyAll.isCopiedAll) {
                $scope.$parent.isBetSettingPartial = $scope.betSettingList[0].isValid;
                return true;
            } else {
                for (var i = 0; i < length; i++) {
                    var betSettingTmp = $scope.betSettingList[i];
                    if (!betSettingTmp.isValid) {
                        $scope.$parent.isBetSettingPartial = false;
                        return false;
                    } else {
                        if (betSettingTmp.isBetSettingUpdated || betSettingTmp.isStatusUpdated) {
                            updatedCount = updatedCount + 1;
                        }
                    }
                }

                $scope.$parent.isBetSettingPartial = updatedCount > 0 ? true : false;
            }
        });

    }, function (reason) { }, function (update) { });
});