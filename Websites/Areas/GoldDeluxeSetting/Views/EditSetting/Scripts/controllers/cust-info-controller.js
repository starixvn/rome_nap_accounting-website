fanex.goldDeluxeSetting.controller('CustomerInfoController',
  [
      '$scope',
      function ($scope) {
          'use strict';

          $scope.enableGoldDeluxe = !settingViewModel.Setting.DisableGoldDeluxe;

          $scope.updateSettingStatus = function (setting) {
              setting.DisableGoldDeluxe = !$scope.enableGoldDeluxe;
          };

          $scope.updateDownlineStatus = function () {
              if (settingViewModel.Setting.DisableUpline) {
                  $scope.disableCheckBoxGoldDeluxe = true;
                  return;
              }

              $scope.disableCheckBoxGoldDeluxe = false
          }

          $scope.updateDownlineStatus();
      }
  ]);