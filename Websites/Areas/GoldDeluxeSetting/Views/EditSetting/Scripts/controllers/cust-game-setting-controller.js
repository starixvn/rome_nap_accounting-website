if (fanex.goldDeluxeSetting) {
    fanex.goldDeluxeSetting.controller('CustGameSettingController',
    [
        '$scope',
        '$http',
        'CustomerGameSettingService',
        function ($scope, $http, customerGameSettingService) {
            'use strict';
            $scope.gameSetting = fanex.cloneObject(settingViewModel.Setting.GameSetting);
            $scope.isCrossLevelOrNotMember = settingViewModel.Level !== fanex.goldDeluxeSetting.customerLevel.Member
                || settingViewModel.UpLineLevel - settingViewModel.Level > 1;

            $scope.getGameSetting = function () {
                return $scope.gameSetting;
            };

            $scope.updateGameSetting = function () {
                customerGameSettingService.updateGameSetting($scope.getGameSetting(), $scope.$parent);
            };

            $scope.checkShowWarningUncheckedAll = function () {
                var isShowWarning = false;
                angular.forEach($scope.gameSetting, function (gameType) {
                    var isUncheckedAll = true;
                    angular.forEach(gameType.BettingLimit, function (stakeLevel) {
                        if (stakeLevel.IsChecked) {
                            isUncheckedAll = false;
                            return;
                        }
                    });

                    if (isUncheckedAll) {
                        isShowWarning = true;
                        return;
                    }
                });

                var message = language.getString('WarningUncheckAllStakeLevelMsg');
                $scope.$parent.mainResult.Warning = {
                    IsShow: isShowWarning,
                    Message: message
                };
            };

            $scope.checkToDisable = function (gameIndex) {
                if ($scope.isCrossLevelOrNotMember) {
                    return;
                }
                
                var gameTypes = document.getElementsByClassName('game' + gameIndex);
                var checkedCount = 0;
                for (var i = 0; i < gameTypes.length; i++) {
                    if (gameTypes[i].checked) {
                        ++checkedCount;
                    }
                }

                if (checkedCount >= MemberMaxStakeLevels) {
                    for (var i = 0; i < gameTypes.length; i++) {
                        if (!gameTypes[i].checked) {
                            gameTypes[i].setAttribute('disabled', true);
                        }
                    }
                }
                else {
                    for (var i = 0; i < gameTypes.length; i++) {
                        if (!gameTypes[i].checked) {
                            gameTypes[i].removeAttribute('disabled');
                        }
                    }
                }
            };
        }
    ]);
}
else {
    alert('Missing fanex.goldDeluxeSetting!');
}
$(document).ready(function () {
    var gameSettingScope = angular.element('[data-ng-controller="CustGameSettingController"]').scope();
    angular.forEach(gameSettingScope.gameSetting, function (gameType) {
        gameSettingScope.checkToDisable(gameType.Id);
    });
});