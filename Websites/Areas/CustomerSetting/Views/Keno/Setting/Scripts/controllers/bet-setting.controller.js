"use strict";

KenoModule.controller('betSettingController', function ($scope, kenoRepositories) {
    $scope.betSettingList = kenoRepositories.kenoSetting.betSettings;
    $scope.roleId = kenoRepositories.kenoSetting.customerInformation.roleId;

    $scope.betSettingCopyAll = kenoRepositories.kenoSetting.betSettingCopyAll;
    $scope.isBetSettingPartial = false;
    $scope.isCopyAll = false;
    $scope.isValid = false;

//    console.log($scope.betSettingList);

    $scope.copyAll = function (isCopy, betTypeId) {
        $scope.isCopyAll = isCopy;
        $scope.betSettingCopyAll.betTypeId = isCopy == true ? betTypeId : -1;
        $scope.betSettingCopyAll.isCopiedAll = isCopy;

        $scope.$broadcast('totallyChecking', isCopy);
        $scope.$broadcast('inputDisabledChecking', isCopy);
    };

    $scope.$on('totallyChecking', function (event, isCopiedAll) {
        var length = $scope.betSettingList.length;
        var updatedCount = 0;

        $scope.isBetSettingPartial = false;

        if (isCopiedAll) {
            $scope.isBetSettingPartial = $scope.betSettingList[0].isValid;
            return true;
        }
        else {
            for (var i = 0; i < length; i++) {
                var betSettingTmp = $scope.betSettingList[i];
                if (!betSettingTmp.isValid) {
                    $scope.isBetSettingPartial = false;
                    return false;
                }
                else {
                    if (betSettingTmp.isBetSettingUpdated || betSettingTmp.isStatusUpdated) {
                        updatedCount = updatedCount + 1;
                    }
                }
            }

            $scope.isBetSettingPartial = updatedCount > 0 ? true : false;
        }

        $scope.$emit('betSettingPartial', $scope.isBetSettingPartial);
    });

});
