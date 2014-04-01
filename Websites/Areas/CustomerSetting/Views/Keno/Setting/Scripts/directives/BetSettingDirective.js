"use strict";

KenoModule.directive('betSettingElement', function () {
    return {
        restrict: 'EA',
        priority: 1,
        scope: {
            betSetting: "=ngModel",
            index: "=index"
        },
        templateUrl: domain +  'Areas/CustomerSetting/Views/Keno/Setting/Views/Partial/BetSetting/BetSetting.htm',
        controller: ['$scope',
            function ($scope) {
                $scope.originalBetSetting = angular.copy($scope.betSetting);

                $scope.isStatuschkDisabled = false;
                $scope.isDownlinechkDisabled = true;

                $scope.isCopyAll = false;

                $scope.valids = {
                    "minBet": true,
                    "maxBet": true,
                    "maxPerMatch": true
                };

                if (!$scope.betSetting.upline.isEnabled || !$scope.betSetting.upline.isDownlinesEnabled) {
                    $scope.isStatuschkDisabled = true;
                }

                $scope.$on('betSetting.minBet', function (event, isValid) {
                    $scope.valids.minBet = isValid;
                });

                $scope.$on('betSetting.maxBet', function (event, isValid) {
                    $scope.valids.maxBet = isValid;
                });

                $scope.$on('betSetting.maxPerMatch', function (event, isValid) {
                    $scope.valids.maxPerMatch = isValid;
                });

                $scope.$on('inputDisabledChecking', function (event, isCopiedAll) {
                    $scope.isCopyAll = isCopiedAll;
                });

                $scope.$watch('[betSetting.minBet,betSetting.maxBet,betSetting.maxPerMatch, valids]', function (newValue, oldValue) {
                    if ($scope.valids.minBet && $scope.valids.maxBet && $scope.valids.maxPerMatch) {
                        $scope.betSetting.isValid = true;
                    } else {
                        $scope.betSetting.isValid = false;
                    }

                    if (($scope.betSetting.minBet != $scope.originalBetSetting.minBet) || ($scope.betSetting.maxBet != $scope.originalBetSetting.maxBet) || ($scope.betSetting.maxPerMatch != $scope.originalBetSetting.maxPerMatch)) {
                        $scope.betSetting.isBetSettingUpdated = true;
                    } else {
                        $scope.betSetting.isBetSettingUpdated = false;
                    }

                    $scope.$emit('totallyChecking', $scope.isCopyAll);

                }, true);

                $scope.$watch('betSetting.isEnabled', function (newValue, oldValue) {
                    if (newValue !== oldValue) {
                        if ($scope.originalBetSetting.isEnabled) {
                            $scope.isDownlinechkDisabled = true;
                            if ($scope.betSetting.isEnabled != $scope.originalBetSetting.isEnabled) {
                                if ($scope.originalBetSetting.isDownlinesEnabled) {
                                    $scope.betSetting.isDownlinesEnabled = !$scope.originalBetSetting.isDownlinesEnabled;
                                }
                                $scope.betSetting.isStatusUpdated = true;
                            } else {
                                $scope.betSetting.isDownlinesEnabled = $scope.originalBetSetting.isDownlinesEnabled;
                                $scope.betSetting.isStatusUpdated = false;
                            }
                        } else {
                            $scope.betSetting.isStatusUpdated = $scope.betSetting.isEnabled != $scope.originalBetSetting.isEnabled;
                            $scope.isDownlinechkDisabled = $scope.betSetting.isEnabled == false;
                        }

                        $scope.$emit('totallyChecking', $scope.isCopyAll);
                    }
                }, true);
            }
        ]
    };
});


KenoModule.directive('betSettingInput', ['$filter', '$parse',
    function ($filter, $parse) {
        return {
            require: '^ngModel',
            link: function (scope, element, attrs, ctrl) {
                if (!ctrl) return;

                scope.formatNumber = function (value) {
                    return $filter("number")(value);
                };

                var initValueModel = eval("scope." + attrs.ngModel);
                var initValueViewModel = scope.formatNumber(initValueModel);

                ctrl.$formatters.unshift(function () {
                    return scope.formatNumber(ctrl.$modelValue);
                });

                ctrl.$parsers.unshift(function (viewValue) {
                    var plainNumber = viewValue.replace(/[^\d|\-+|\.+]/g, '');
                    ctrl.$modelValue = eval(plainNumber);
                    element.val(scope.formatNumber(plainNumber));
                    return plainNumber;
                });

                scope.$on('inputDisabledChecking', function (event, isCopiedAll) {
                    if (isCopiedAll && scope.index != 0) {
                        element.addClass('inputDisabled');
                        $(element).attr('disabled', true);
                    } else {
                        element.removeClass('inputDisabled');
                        $(element).removeAttr('disabled');
                    }
                });

                scope.setConstrain = function () {
                    scope.createTooltip = function (titleTootip) {
                        element.attr('title', titleTootip);
                        element.attr('original-title', titleTootip);
                        element.tipsy({
                            delayIn: 0,
                            delayOut: 0,
                            gravity: "s",
                            opacity: 1,
                            autoHide: {
                                isAuto: true,
                                timeOut: 7
                            }
                        });
                    };

                    scope.emitToParent = function (isValid) {
                        if (isValid) {
                            element.removeClass('invalidValue');
                            scope.$emit(attrs.ngModel, true);
                        } else {
                            element.addClass('invalidValue');
                            scope.$emit(attrs.ngModel, false);
                        }
                    };

                    var valueRange = scope.$eval(attrs.valueRange);
                    var minValue = eval(valueRange.min);
                    var maxValue = eval(valueRange.max);

                    if (ctrl.$modelValue < minValue && minValue > -1) {
                        scope.createTooltip("The " + valueRange.model + " should be more than or equal to " + valueRange.minName);
                        scope.emitToParent(false);
                        return false;
                    }

                    if (ctrl.$modelValue > maxValue && maxValue > -1) {
                        scope.createTooltip("The " + valueRange.model + " should be less than or equal to " + valueRange.maxName);
                        scope.emitToParent(false);
                        return false;
                    }

                    scope.createTooltip("");
                    scope.emitToParent(true);
                }

                scope.$watch(attrs.ngModel, function (newValue, oldValue) {
                    scope.setConstrain();
                }, true);

                attrs.$observe(
                    "valueRange",
                    function (value) {
                        scope.setConstrain();
                    });

            }
        };
    }
]);

KenoModule.directive('betSettingConstrain', ['$filter',
    function ($filter) {
        return {
            require: '?ngModel',
            link: function (scope, element, attrs, ctrl) {
                if (!ctrl) return;

                var valueModel = eval("scope." + attrs.ngModel);

                if (valueModel > 0) {
                    var symbol = attrs.betSettingConstrain == 1 ? " >= " : " <= ";

                    element.text(symbol + scope.formatNumber(valueModel));
                }
            }
        };
    }
]);