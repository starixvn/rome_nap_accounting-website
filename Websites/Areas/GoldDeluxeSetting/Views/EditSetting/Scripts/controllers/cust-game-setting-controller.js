if (fanex.goldDeluxeSetting) {
    fanex.goldDeluxeSetting.controller('CustGameSettingController',
    [
        '$scope',
        '$http',
        'CustomerGameSettingService',
        function ($scope, $http, customerGameSettingService) {
            'use strict';
            $scope.gameSetting = fanex.cloneObject(settingViewModel.Setting.GameSetting);
            var uplineUncheckedAllMsg = '';

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
        }
    ]);
}
else {
    alert('Missing fanex.goldDeluxeSetting!');
}