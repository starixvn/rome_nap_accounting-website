"use strict";

KenoModule.directive('masterOnMemberPositionTakingElement', ['commonService', function (commonService) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        templateUrl: domain +  'Areas/CustomerSetting/Views/Keno/Setting/Views/Partial/PositionTaking/MasterOnMemberPositionTaking.htm',
        link: function ($scope, elem, attr, ctrl) {
            console.log($scope.positionTaking);

            $scope.originalPositionTaking = angular.copy($scope.positionTaking);

            $scope.isAutoPTCount = 0;
            $scope.ptCount = 0;

            $scope.m1Range = commonService.getRangePTValue($scope.positionTaking.m1Max, $scope.positionTaking.m1UM1Min);
            $scope.uM1Range = commonService.getRangePTValue($scope.positionTaking.m3, $scope.positionTaking.m1UM1Min);

            $scope.setRangePTWhenInit = function () {
                if ($scope.positionTaking.m1Max < $scope.positionTaking.m1) {
                    $scope.m1Range.unshift($scope.positionTaking.m1);
                }

                if ($scope.positionTaking.m1UM1Min > $scope.positionTaking.m1) {
                    $scope.m1Range.push($scope.positionTaking.s1);
                }

                if ($scope.positionTaking.m3 < $scope.positionTaking.uM1) {
                    $scope.uM1Range.unshift($scope.positionTaking.uM1);
                }

                if ($scope.positionTaking.m1UM1Min > $scope.positionTaking.uM1) {
                    $scope.uM1Range.push($scope.positionTaking.uM1);
                }
            }

            $scope.setRangePTWhenInit();

            $scope.setRangePTAfterChosen = function () {
                if ($scope.isAutoPTCount == 1) {
                    if ($scope.positionTaking.m3 >= $scope.positionTaking.uM1 || $scope.positionTaking.m1UM1Min <= $scope.positionTaking.uM1) {
                        $scope.uM1Range = commonService.getRangePTValue($scope.positionTaking.m3, $scope.positionTaking.m1UM1Min);
                    }
                }

                if ($scope.ptCount == 1) {
                    if ($scope.positionTaking.m1Max >= $scope.positionTaking.m1 || $scope.positionTaking.m1UM1Min <= $scope.positionTaking.m1) {
                        $scope.m1Range = commonService.getRangePTValue($scope.positionTaking.m1Max, $scope.positionTaking.m1UM1Min);
                    }
                }
            };

            $scope.monitorSaveButton = function () {
                if ($scope.positionTaking.isUpdateM1
                    || $scope.positionTaking.isUpdateUM1) {
                    $scope.positionTaking.isUpdatePT = true;
                }
                else {
                    $scope.positionTaking.isUpdatePT = false;
                }

                $scope.$emit('totallyChecking');
            };

            $scope.$watch('[positionTaking.m1, positionTaking.uM1,  positionTaking.iM1]', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    if ($scope.originalPositionTaking.iM1) { // Load in case within isAuto
                        if ($scope.positionTaking.iM1) {
                            $scope.positionTaking.isUpdateM1 = false;
                            $scope.positionTaking.isUpdateUM1 = (($scope.originalPositionTaking.uM1 != $scope.positionTaking.uM1)
                                                    || ($scope.originalPositionTaking.iM1 != $scope.positionTaking.iM1)) == true ? true : false;
                        }
                        else {
                            $scope.positionTaking.isUpdateM1 = true;
                            $scope.positionTaking.isUpdateUM1 = true;
                        }
                    }
                    else {
                        if ($scope.positionTaking.iM1) {
                            $scope.positionTaking.isUpdateM1 = true;
                            $scope.positionTaking.isUpdateUM1 = true;
                        }
                        else {
                            $scope.positionTaking.isUpdateM1 = $scope.originalPositionTaking.m1 != $scope.positionTaking.m1 ? true : false;
                            $scope.positionTaking.isUpdateUM1 = false;
                        }
                    }

                    if ($scope.originalPositionTaking.uM1 != $scope.positionTaking.uM1) {
                        $scope.isAutoPTCount++;
                    }

                    if ($scope.originalPositionTaking.m1 != $scope.positionTaking.m1) {
                        $scope.ptCount++;
                    }

                    $scope.setRangePTAfterChosen();
                    $scope.monitorSaveButton();
                }
            }, true);
        }
    };
} ]);
