"use strict";

KenoModule.directive('memberPositionTakingElement', ['commonService', '$templateCache', function (commonService, $templateCache) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        templateUrl: $templateCache.get('memberPositionTakingTemplate'),
        link: function ($scope, elem, attr, ctrl) {
            $scope.originalPositionTaking = angular.copy($scope.positionTaking);
            $scope.fixIECount = 0;
            $scope.ptCount = 0;

            $scope.a1Range = commonService.getRangePTValue($scope.positionTaking.a2, 0);
            if ($scope.positionTaking.a2 < $scope.positionTaking.a1) {
                $scope.a1Range.unshift($scope.positionTaking.a1);
            }

            $scope.$watch('positionTaking.a1', function () {
                $scope.positionTaking.isUpdateA1 = $scope.originalPositionTaking.a1 != $scope.positionTaking.a1;
                $scope.positionTaking.isUpdatePT = $scope.positionTaking.isUpdateA1;

                if ($scope.originalPositionTaking.a1 != $scope.positionTaking.a1) {
                    $scope.ptCount++;
                    if ($scope.ptCount == 1) {
                        $scope.a1Range = commonService.getRangePTValue($scope.positionTaking.a2, 0);
                    }
                }

                $scope.fixIECount++;
                $scope.$emit('totallyChecking');
            }, true);
        }
    };
} ]);
