"use strict";

KenoModule.controller('positionTakingController', function ($scope, kenoRepositories, ngDialog) {
    $scope.positionTakingList = kenoRepositories.kenoSetting.positionTakings;

    $scope.positionTakingCopyAll = kenoRepositories.kenoSetting.positionTakingCopyAll;
    $scope.isPositionTakingPartial = false;
    $scope.isCopyAll = false;

    $scope.copyAll = function (isCopy, betTypeId) {
        $scope.isCopyAll = isCopy;
        $scope.positionTakingCopyAll.betTypeId = isCopy == true ? betTypeId : -1; ;
        $scope.positionTakingCopyAll.isCopiedAll = isCopy
    };

    $scope.setValueAllMember = function (isUpdateAllMember) {
        var length = $scope.positionTakingList.length;
        if (isUpdateAllMember) {
            for (var i = 0; i < length; i++) {
                $scope.positionTakingList[i].isUpdateAllMember = true;
            }
        } else {
            for (var i = 0; i < length; i++) {
                $scope.positionTakingList[i].isUpdateAllMember = false;
            }
        }
        ngDialog.close();
        $scope.$emit('confirmUpdateAllMember');
    };

    $scope.$on('updateAllMember', function (event) {
        if ($scope.confirmIsAutoArray.length > 0) {
            ngDialog.open({
                template: domain + 'Areas/CustomerSetting/Views/Keno/Setting/Partial/PositionTaking/ConfirmUpdateAllMembers.htm',
                scope: $scope,
                closeByCloseButton: true
            });
        }
    });

    $scope.$on('positionTakingRow', function (event) {
        var length = $scope.positionTakingList.length;
        for (var i = 0; i < length; i++) {
            var positionTakingTmp = $scope.positionTakingList[i];
            if (positionTakingTmp.isUpdatePT) {
                $scope.isPositionTakingPartial = true;
                return true;
            }
        }

        $scope.isPositionTakingPartial = false;
    });

    $scope.$on('confirmIsAuto', function (event, data) {
        var index = $scope.$parent.confirmIsAutoArray.indexOf(data.betTypeId);

        if (data.isAdd && index == -1) {
            $scope.$parent.confirmIsAutoArray.push(data.betTypeId);
        }

        if (!data.isAdd && index != -1) {
            $scope.$parent.confirmIsAutoArray.splice(index, 1);
        }

        return false;
    });

    $scope.$watch('isCopyAll', function (newValue, oldValue) {
        if (newValue !== oldValue) {
            if ($scope.isCopyAll) {
                $scope.isPositionTakingPartial = true;
            }
            else {
                $scope.$broadcast('positionTakingRow');
            }
        }
    }, true);

    $scope.$watch('isPositionTakingPartial', function (newValue, oldValue) {
        if (newValue !== oldValue) {
            $scope.$emit('positionTakingPartial', $scope.isPositionTakingPartial);
        }
    }, true);
});
