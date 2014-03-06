angular.module('kenoStatusApp', []);
angular.element($('#ng-app')).ready(function () {
    $('#ng-app').show();
});

function KenoStatusController($scope) {
    $scope.model = model;

    $scope.betTypes = $scope.model.betTypeStatusList;

    $scope.CheckStatus = function (preVal, scope) {
        if (preVal) {
            if (scope.item.enable) {
                scope.item.enableDownline = true;
                scope.item.disableDownline = true;
            }
            else {
                scope.item.enableDownline = false;
                scope.item.disableDownline = true;
            }
        }
        else {
            if (scope.item.enable) {
                scope.item.enableDownline = false;
                scope.item.disableDownline = false;
            }
            else {
                scope.item.enableDownline = false;
                scope.item.disableDownline = true;
            }
        }
    };

    $scope.Update = function () {
        //parent.Fanex.ExtAcc.BlockUI();
       
        if (parent.BlockUI) {
            try { parent.BlockUI(); } catch (e) { }
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
            var success = true;

            $(errorCodes).each(function (i, error) {
                if (error != '' && parseInt(error) >= 0) {
                    success = false;
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
                $('#errMsg').html(data.errorMessage.formatErrorMessage());
                $('#errMsg').show();
            }
            if (parent && parent.jQuery && parent.jQuery.unblockUI) {
                parent.jQuery.unblockUI();
            }
            
        }).error(function (data) {
            if (typeof console != 'undefined')
                console.log(data);
            if (parent && parent.jQuery && parent.jQuery.unblockUI) {
                parent.jQuery.unblockUI();
            }
            
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
                betTypeChexkboxesDownlines[i].checked = true;
            }
        }
        else {
            for (var i = 0; i < betTypeChexkboxes.length; i++) {
                if (betTypeChexkboxes[i].disabled) continue;

                betTypeChexkboxes[i].checked = false;
                betTypeChexkboxesDownlines[i].checked = false;
            }
        }
    };
};
    
///*
// test data
//*/
//var model = {
//    custID: 1176070,
//    userName: "SuperTest1",
//    roleID: 4,
//    uplineID: 122451,
//    userID: 51154,
//    betTypeStatusList: [{
//        betTypeID: 1,
//        betTypeName: "Beijing",
//        isDisabled: true,
//        isDisabledUpline: true,
//        isDisabledDownlines: true
//    },
//    {
//        betTypeID: 2,
//        betTypeName: "Canada",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: true
//    },
//    {
//        betTypeID: 3,
//        betTypeName: "Australia",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }
//    , {
//        betTypeID: 4,
//        betTypeName: "Slovakia",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }, {
//        betTypeID: 5,
//        betTypeName: "Ohio",
//        isDisabled: true,
//        isDisabledUpline: false,
//        isDisabledDownlines: true
//    }, {
//        betTypeID: 6,
//        betTypeName: "West Canada",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }, {
//        betTypeID: 7,
//        betTypeName: "Malta",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }, {
//        betTypeID: 8,
//        betTypeName: "Taiwan",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }, {
//        betTypeID: 8,
//        betTypeName: "Massac",
//        isDisabled: false,
//        isDisabledUpline: false,
//        isDisabledDownlines: false
//    }]
//};