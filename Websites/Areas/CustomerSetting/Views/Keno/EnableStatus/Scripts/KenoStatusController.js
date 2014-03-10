angular.module('kenoStatusApp', []);

angular.element($('#ng-app')).ready(function () {
    $('#ng-app').show();
});

function KenoStatusController($scope) {
    
    $scope.model = model;
    $scope.betTypes = $scope.model.betTypeStatusList;
    $scope.isDisabledAll = true;
    $scope.isCheckeddAll = true;
    var length = $scope.model.betTypeStatusList.length;
    for (var i = 0; i < length; i++) {
        if (!$scope.model.betTypeStatusList[i].isDisabledUpline) {
            $scope.isDisabledAll = false;
            break;
        }
    }

    $scope.CheckStatus = function (preVal, scope) {
        var betTypesCheckboxes = document.getElementsByName('betTypes0');
        var allChecked = true;

        if (preVal) {
            if (scope.item.enable) {
                scope.item.enableDownline = true;
                scope.item.disableDownline = true;

                scope.item.isDisabled = false;
                scope.item.isDisabledUpline = false;
                scope.item.isDisabledDownlines = false;

                for (var i = 0; i < betTypesCheckboxes.length; i++) {
                    if (!betTypesCheckboxes[i].checked) {
                        allChecked = false;
                        break;
                    }
                }

                if (allChecked)
                    document.getElementById('chkAll').checked = true;
            }
            else {
                document.getElementById('chkAll').checked = false;

                scope.item.enableDownline = false;
                scope.item.disableDownline = true;

                scope.item.isDisabled = true;
                scope.item.isDisabledUpline = false;
                scope.item.isDisabledDownlines = true;
            }
        }
        else {
            if (scope.item.enable) {
                scope.item.enableDownline = false;
                scope.item.disableDownline = false;

                scope.item.isDisabled = false;
                scope.item.isDisabledUpline = false;
                scope.item.isDisabledDownlines = false;

                for (var i = 0; i < betTypesCheckboxes.length; i++) {
                    if (!betTypesCheckboxes[i].checked) {
                        allChecked = false;
                        break;
                    }
                }

                if (allChecked)
                    document.getElementById('chkAll').checked = true;
            }
            else {
                document.getElementById('chkAll').checked = false;

                scope.item.enableDownline = false;
                scope.item.disableDownline = true;

                scope.item.isDisabled = true;
                scope.item.isDisabledUpline = false;
                scope.item.isDisabledDownlines = true;
            }
        }
    };
    $scope.isDisabledAll = true;
    $scope.isCheckeddAll = true;
    var length = $scope.model.betTypeStatusList.length;
    for (var i = 0; i < length; i++) {
        if (!$scope.model.betTypeStatusList[i].isDisabledUpline)
            $scope.isDisabledAll = false;
        break;
    }

    $scope.Update = function () {

        if (parent.BlockUI) {
            try { parent.BlockUI(); } catch (e) { }
        }
        for (var i = 0; i < model.betTypeStatusList.length; i++) {
            model.betTypeStatusList[i].isDisabledDownlines = model.betTypeStatusList[i].enableDownline;
        }
        $.ajax({
            dataType: 'json',
            type: 'post',
            url: resolveClientUrl('CustomerSetting/Keno/EnableStatus/Update'),
            data: {
                id: custId,
                uplineId: uplineId,
                model: JSON.stringify(model),
                roleId: roleId,
                groupName: ''
            }
        })
        .success(function (data) {
            var errorCodes = data.errorCode.toString().indexOf('\u0007') != -1 ? data.errorCode.split('\u0007') : data.errorCode.toString();
            var errorMessages = data.errorMessage.toString().indexOf('\u0007') != -1 ? data.errorMessage.split('\u0007') : data.errorMessage.toString();
            var success = true;
            var message = '';

            $(errorCodes).each(function (i, error) {
                if (error != '' && parseInt(error) > 0) {
                    success = false;
                    message += (parseInt(error) == 600 ? 'Account was disabled' : errorMessages[i]) + '<br />';
                }
            });

            if (data.errorCode == "0" || data.errorCode == "" || success) {
                $('#errMsg').attr('class', 'successMsg');
                $('#errMsg').html('<font color="Blue">' + successMsg + '</font>');
                $('#errMsg').show();

                var timeout = setTimeout(function () {
                    clearTimeout(timeout);
                    try { parent.ClosePopup(); } catch (e) { }
                    // document.location.reload(true);
                }, 3000);
            }
            else {
                $('#errMsg').attr('class', 'errMsg');
                $('#errMsg').html(message.formatErrorMessage());
                $('#errMsg').show();
            }
            if (parent && parent.jQuery && parent.jQuery.unblockUI) {
                parent.jQuery.unblockUI();
            }

            //parent.Fanex.ExtAcc.UnBlockUI();
        }).error(function (data) {
            if (typeof console != 'undefined')
                console.log(data);
            if (parent && parent.jQuery && parent.jQuery.unblockUI) {
                parent.jQuery.unblockUI();
            }
            //parent.Fanex.ExtAcc.UnBlockUI();
        });
    };

    $scope.CheckAll = function () {
        var checkbox = document.getElementById('chkAll');
        var betTypeChexkboxes = document.getElementsByName('betTypes0');
        var betTypeChexkboxesDownlines = document.getElementsByName('betTypes1');

        if (checkbox.checked) {
            for (var i = 0; i < betTypeChexkboxes.length; i++) {
                if (betTypeChexkboxes[i].disabled) continue;

                betTypeChexkboxes[i].checked = true;

                if ($scope.betTypes[i].preVal) {
                    betTypeChexkboxesDownlines[i].checked = true;
                    $scope.betTypes[i].isDisabledDownlines = false;
                }
                else {
                    betTypeChexkboxesDownlines[i].disabled = false;
                    $scope.betTypes[i].isDisabledDownlines = true;
                }

                $scope.betTypes[i].isDisabled = false;
            }
        }
        else {
            for (var i = 0; i < betTypeChexkboxes.length; i++) {
                if (betTypeChexkboxes[i].disabled) continue;

                betTypeChexkboxes[i].checked = false;
                betTypeChexkboxesDownlines[i].checked = false;

                if ($scope.betTypes[i].preVal) {
                    betTypeChexkboxesDownlines[i].disabled = true;
                }
                else {
                    betTypeChexkboxesDownlines[i].disabled = true;
                }

                $scope.betTypes[i].isDisabled = true;
                $scope.betTypes[i].isDisabledDownlines = true;
            }
        }
    };

    $scope.$watch('model', function () {
        var length = $scope.model.betTypeStatusList.length;
        $scope.isCheckeddAll = true;
        for (var i = 0; i < length; i++) {
            var bettypeTmp = $scope.model.betTypeStatusList[i];
            if (bettypeTmp.isDisabled) {
                $scope.isCheckeddAll = false;
                return;
            }
        }
    }, true);
};
