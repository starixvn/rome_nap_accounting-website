"use strict";

KenoModule.directive('masterPositionTakingElement', ['commonService', '$templateCache', function (commonService, $templateCache) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        transclude: true,
        templateUrl: $templateCache.get('masterPositionTakingTemplate'),
        link: function ($scope, elem, attr, ctrl) {

            $scope.originalPositionTaking = angular.copy($scope.positionTaking);

            $scope.s3Range = $scope.uS3Range = commonService.getRangePTValue($scope.positionTaking.s4, 0);
            $scope.maxPTRange = {};
            $scope.minPTRange = {};
            $scope.positionTaking.memberNewValueRange = {};

            $scope.fixIECount = 0;

            $scope.monitorSaveButton = function () {
                if ($scope.positionTaking.isUpdateUS3
                    || $scope.positionTaking.isUpdateS3
                    || $scope.positionTaking.isUpdateN3
                    || $scope.positionTaking.isUpdateM3) {
                    $scope.positionTaking.isUpdatePT = true;
                }
                else {
                    $scope.positionTaking.isUpdatePT = false;
                }

                $scope.fixIECount++;
                $scope.$emit('totallyChecking');
            };

            $scope.$watch('[positionTaking.s3, positionTaking.uS3, positionTaking.iS3]', function () {
                if ($scope.originalPositionTaking.iS3) { // Load in case within isAuto
                    if ($scope.positionTaking.iS3) {
                        $scope.positionTaking.l3 = $scope.positionTaking.s4;
                        $scope.positionTaking.isUpdateS3 = false;
                        $scope.positionTaking.isUpdateUS3 = (($scope.originalPositionTaking.uS3 != $scope.positionTaking.uS3)
                                                    || ($scope.originalPositionTaking.iS3 != $scope.positionTaking.iS3)) == true ? true : false;
                        $scope.positionTaking.isConfirmIsAuto = false;
                        $scope.$emit('confirmIsAuto', { "betTypeId": $scope.positionTaking.betTypeId, "isAdd": false });
                    }
                    else {
                        $scope.positionTaking.l3 = Math.round(($scope.positionTaking.s4 - $scope.positionTaking.s3) * 100) / 100;
                        $scope.positionTaking.isUpdateS3 = true;
                        $scope.positionTaking.isUpdateUS3 = true;
                        $scope.positionTaking.isConfirmIsAuto = true;
                        if ($scope.positionTaking.l3 <= 0) {
                            $scope.positionTaking.memberNewValueRange = [0];
                        }
                        else {
                            $scope.positionTaking.memberNewValueRange = commonService.getRangePTValue($scope.positionTaking.l3, 0);
                        }
                        $scope.$emit('confirmIsAuto', { "betTypeId": $scope.positionTaking.betTypeId, "isAdd": true });
                    }
                }
                else {
                    if ($scope.positionTaking.iS3) {
                        $scope.positionTaking.l3 = $scope.positionTaking.s4;
                        $scope.positionTaking.isUpdateS3 = true;
                        $scope.positionTaking.isUpdateUS3 = true;
                    }
                    else {
                        $scope.positionTaking.l3 = Math.round(($scope.positionTaking.s4 - $scope.positionTaking.s3) * 100) / 100;
                        $scope.positionTaking.isUpdateS3 = $scope.originalPositionTaking.s3 != $scope.positionTaking.s3 ? true : false;
                        $scope.positionTaking.isUpdateUS3 = false;
                    }
                }

                $scope.monitorSaveButton();
            }, true);

            $scope.$watch('positionTaking.l3', function (newValue, oldValue) {
                if ($scope.positionTaking.l3 < $scope.positionTaking.m3) {
                    $scope.positionTaking.m3 = $scope.positionTaking.l3;
                }

                var min = Math.round(($scope.positionTaking.n4 - ($scope.positionTaking.iS3 ? $scope.positionTaking.uS3 : $scope.positionTaking.s3)) * 100) / 100;
                $scope.maxPTRange = commonService.getRangePTValue($scope.positionTaking.l3, Math.max($scope.positionTaking.n3, min));

                if ($scope.positionTaking.m3 < min) {
                        $scope.maxPTRange.push($scope.positionTaking.m3);
                    }

                $scope.monitorSaveButton();
            }, true);

            $scope.$watch('positionTaking.m3', function () {
                $scope.positionTaking.isUpdateM3 = $scope.originalPositionTaking.m3 != $scope.positionTaking.m3 ? true : false;

                if ($scope.positionTaking.m3 <= $scope.positionTaking.n3) {
                    $scope.positionTaking.n3 = $scope.positionTaking.m3;
                }

                $scope.minPTRange = commonService.getRangePTValue($scope.positionTaking.m3, 0);
                $scope.monitorSaveButton();
            }, true);

            $scope.$watch('positionTaking.n3', function () {
                $scope.positionTaking.isUpdateN3 = $scope.originalPositionTaking.n3 != $scope.positionTaking.n3 ? true : false;
                $scope.monitorSaveButton();
            }, true)
        }
    };
} ]);

