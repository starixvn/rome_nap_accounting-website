"use strict";

KenoModule.directive('superPositionTakingElement', ['commonService', function (commonService) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        templateUrl: domain + 'Areas/CustomerSetting/Views/Keno/Setting/Views/Partial/PositionTaking/SuperPositionTaking.htm',
        link: function ($scope, elem, attr, ctrl) {
            $scope.originalPositionTaking = angular.copy($scope.positionTaking);

            $scope.maxPTRange = commonService.getRangePTValue($scope.positionTaking.l4, 0);
            $scope.fixIECount = 0;

            $scope.monitorSaveButton = function () {
                if ($scope.positionTaking.isUpdateS4
                    || $scope.positionTaking.isUpdateN4) {
                    $scope.positionTaking.isUpdatePT = true;
                }
                else {
                    $scope.positionTaking.isUpdatePT = false;
                }

                $scope.fixIECount++;
                $scope.$emit('totallyChecking');
            };

            $scope.$watch('positionTaking.s4', function () {
                $scope.positionTaking.isUpdateS4 = $scope.originalPositionTaking.s4 != $scope.positionTaking.s4 ? true : false;

                if ($scope.positionTaking.s4 <= $scope.positionTaking.n4) {
                    $scope.positionTaking.n4 = $scope.positionTaking.s4;
                }

                $scope.minPTRange = commonService.getRangePTValue($scope.positionTaking.s4, 0);
                $scope.monitorSaveButton();
            }, true);

            $scope.$watch('positionTaking.n4', function () {
                $scope.positionTaking.isUpdateN4 = $scope.originalPositionTaking.n4 != $scope.positionTaking.n4 ? true : false;
                $scope.monitorSaveButton();
            }, true);

        }
    };
} ]);
