"use strict";

KenoModule.directive('memberPositionTakingElement', ['commonService', function (commonService) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        templateUrl: domain + 'Areas/CustomerSetting/Views/Keno/Setting/Views/Partial/PositionTaking/MemberPositionTaking.htm',
        link: function ($scope, elem, attr, ctrl) {
            console.log($scope.positionTaking);

            $scope.originalPositionTaking = angular.copy($scope.positionTaking);
            $scope.fixIECount = 0;

            $scope.a1Range = commonService.getRangePTValue($scope.positionTaking.a2, 0);
            if ($scope.positionTaking.a2 < $scope.positionTaking.a1) {
                $scope.a1Range.unshift($scope.positionTaking.a1);
            }

            $scope.$watch('positionTaking.a1', function () {
                if ($scope.positionTaking.a2 > $scope.positionTaking.a1) {
                    $scope.a1Range = commonService.getRangePTValue($scope.positionTaking.a2, 0);
                }

                $scope.positionTaking.isUpdateA1 = $scope.originalPositionTaking.a1 != $scope.positionTaking.a1 ? true : false;
                $scope.positionTaking.isUpdatePT = $scope.positionTaking.isUpdateA1;

                $scope.fixIECount++;
                $scope.$emit('totallyChecking');
            }, true);
        }
    };
} ]);
