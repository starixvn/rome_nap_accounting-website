"use strict";

KenoModule.directive('superOnMemberPositionTakingElement', ['commonService', function (commonService) {
    return {
        restrict: 'A',
        scope: {
            positionTaking: "=ngModel",
            index: "=index"
        },
        templateUrl: domain + 'Areas/CustomerSetting/Views/Keno/Setting/Views/Partial/PositionTaking/SuperOnMemberPositionTaking.htm',
        link: function ($scope, elem, attr, ctrl) {
            //            console.log($scope.positionTaking);

            $scope.originalPositionTaking = angular.copy($scope.positionTaking);

            $scope.isAutoPTCount = 0;
            $scope.ptCount = 0;

            $scope.s1Range = commonService.getRangePTValue($scope.positionTaking.s1Max, $scope.positionTaking.s1US1Min);
            $scope.uS1Range = commonService.getRangePTValue($scope.positionTaking.s4, $scope.positionTaking.s1US1Min);

            $scope.setRangePTWhenInit = function () {
                if ($scope.positionTaking.s1Max < $scope.positionTaking.s1) {
                    $scope.s1Range.unshift($scope.positionTaking.s1);
                }

                if ($scope.positionTaking.s1US1Min > $scope.positionTaking.s1) {
                    $scope.s1Range.push($scope.positionTaking.s1);
                }

                if ($scope.positionTaking.s4 < $scope.positionTaking.uS1) {
                    $scope.uS1Range.unshift($scope.positionTaking.uS1);
                }

                if ($scope.positionTaking.s1US1Min > $scope.positionTaking.uS1) {
                    $scope.uS1Range.push($scope.positionTaking.uS1);
                }
            }

            $scope.setRangePTWhenInit();

            $scope.setRangePTAfterChosen = function () {
                if ($scope.isAutoPTCount == 1) {
                    if ($scope.positionTaking.s4 >= $scope.positionTaking.uS1 || $scope.positionTaking.s1US1Min <= $scope.positionTaking.uS1) {
                        $scope.uS1Range = commonService.getRangePTValue($scope.positionTaking.s4, $scope.positionTaking.s1US1Min);
                    }
                }

                if ($scope.ptCount == 1) {
                    if ($scope.positionTaking.s1Max >= $scope.positionTaking.s1 || $scope.positionTaking.s1US1Min <= $scope.positionTaking.s1) {
                        $scope.s1Range = commonService.getRangePTValue($scope.positionTaking.s1Max, $scope.positionTaking.s1US1Min);
                    }
                }
            };

            $scope.monitorSaveButton = function () {
                if ($scope.positionTaking.isUpdateS1
                    || $scope.positionTaking.isUpdateUS1) {
                    $scope.positionTaking.isUpdatePT = true;
                }
                else {
                    $scope.positionTaking.isUpdatePT = false;
                }

                $scope.$emit('totallyChecking');
            };


            $scope.$watch('[positionTaking.s1, positionTaking.uS1,  positionTaking.iS1]', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    if ($scope.originalPositionTaking.iS1) { // Load in case within isAuto
                        if ($scope.positionTaking.iS1) {
                            $scope.positionTaking.isUpdateS1 = false;
                            $scope.positionTaking.isUpdateUS1 = (($scope.originalPositionTaking.uS1 != $scope.positionTaking.uS1)
                                                    || ($scope.originalPositionTaking.iS1 != $scope.positionTaking.iS1)) == true ? true : false;
                        }
                        else {
                            $scope.positionTaking.isUpdateS1 = true;
                            $scope.positionTaking.isUpdateUS1 = true;
                        }
                    }
                    else {
                        if ($scope.positionTaking.iS1) {
                            $scope.positionTaking.isUpdateS1 = true;
                            $scope.positionTaking.isUpdateUS1 = true;
                        }
                        else {
                            $scope.positionTaking.isUpdateS1 = $scope.originalPositionTaking.s1 != $scope.positionTaking.s1 ? true : false;
                            $scope.positionTaking.isUpdateUS1 = false;
                        }

                    }

                    if ($scope.originalPositionTaking.uS1 != $scope.positionTaking.uS1) {
                        $scope.isAutoPTCount++;
                    }

                    if ($scope.originalPositionTaking.s1 != $scope.positionTaking.s1) {
                        $scope.ptCount++;
                    }

                    $scope.setRangePTAfterChosen();
                    $scope.monitorSaveButton();
                }
            }, true);
        }
    };
} ]);
