angular.module('syncCustomerApp', []);

angular.element($('#ng-app')).ready(function () {
    $('#ng-app').show();
});

function SyncCustomerController($scope, $compile) {
    try{
        $scope.model = model || {};
        $scope.customers = model.Customers || {};
    }
    catch (e) {
        if (typeof console != 'undefined') {
            console.log(e);
        }
    }
    
    $scope.Check = function () {
        if ($('#usernames').val().trim() == '') {
            showAlertMessage("errMsg", "Please enter username!", 1);
            $("#errMsg").show();
        }
        else if ($('#usernames').val().trim().split(',').length > 500) {
            showAlertMessage("errMsg", "You input more than 500 usernames!", 1);
            $("#errMsg").show();
        }
        else {
            $("#errMsg").hide();

            parent.BlockUI();

            $.ajax({
                type: "POST",
                url: "Check",
                data: { usernames: document.getElementById("usernames").value },
                success: function (rs) {
                    $scope.RegenerateData(rs);
                },
                error: function (rs) {
                    if (rs.IsSuccess == false) {
                        showAlertMessage("errMsg", rs.ErrorMessage, 1);
                        $("#errMsg").show();
                    }
                    else {
                        console.log(rs);
                    }
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    };

    $scope.UnSyncedList = function () {
        parent.BlockUI();

        $.ajax({
            type: "POST",
            url: "UnsyncedList",
            success: function (rs) {
                $scope.RegenerateData(rs);
            },
            error: function (rs) {
                if (rs.IsSuccess == false) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                    $("#errMsg").show();
                }
                else {
                    console.log(rs);
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    $scope.CheckAll = function () {
        var checked = $('#chkAll').is(':checked');
        var checkboxes = $('input[type=checkbox]');
        for (var i = 1; i < checkboxes.length; i++) {
            checkboxes[i].checked = checked;
            $scope.model.Usernames[i - 1].IsSelected = checked;
        }
    };

    $scope.Sync = function () {
        parent.BlockUI();

        var checkboxes = $('input[type=checkbox]');
        for (var i = 0; i < $scope.model.Usernames.length; i++) {
            for (var j = 0; j < checkboxes.length; j++) {
                if (checkboxes[j].id == $scope.model.Usernames[i].CustId) {
                    $scope.model.Usernames[i].IsSelected = checkboxes[j].checked;
                    break;
                }
            }
        }

        $.ajax({
            type: "POST",
            url: "Sync",
            data: { customers: JSON.stringify($scope.model) },
            success: function (rs) {
                $scope.RegenerateData(rs);
            },
            error: function (rs) {
                if (rs.IsSuccess == false) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                    $("#errMsg").show();
                }
                else {
                    console.log(rs);
                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    };

    $scope.RegenerateData = function (rs) {
        $('#chkAll').attr('checked', true);

        if (rs.IsSuccess == false) {
            showAlertMessage("errMsg", rs.ErrorMessage, 1);
            $("#errMsg").show();
        }
        else {
            $scope.$apply(function () {
                $scope.model = rs;
                $scope.customers = $scope.model.Usernames;

                $('.compiled').remove();
                $('#syncCustomerApp').show();

                var notExistUserLen = $scope.model.NotExistUsernames.length;
                if (notExistUserLen > 0) {
                    var message = '';
                    for (var i = 0; i < notExistUserLen; i++) {
                        message += $scope.model.NotExistUsernames[i] + ", ";
                    }

                    showAlertMessage("errMsg", (message.substring(0, message.length - 2) + " cannot be found"), 1);
                    $("#errMsg").show();
                }

                if ($scope.customers.length > 0) {
                    $('#no-info').hide();

                    var hasUnsync = false;
                    for (var i = 0; i < $scope.customers.length; i++) {
                        if ($scope.customers[i].IsSync != $scope.customers[i].IsSync3rd || ($scope.customers[i].IsSync == false && $scope.customers[i].IsSync3rd == false)) {
                            hasUnsync = true;
                            break;
                        }
                    }

                    $('#sync').attr('disabled', !hasUnsync);
                }
                else {
                    $('#no-info').show();
                    $('#sync').attr('disabled', true);
                }

                for (var i = 0; i < $scope.customers.length; i++) {
                    var className = i % 2 == 0 ? '' : 'even-row';
                    var checkbox = ($scope.customers[i].IsSync != $scope.customers[i].IsSync3rd || (!$scope.customers[i].IsSync && !$scope.customers[i].IsSync3rd)) ?
                        '<input onclick="CheckCust()" id="' + $scope.customers[i].CustId + '" type="checkbox" checked />' : '';
                    var rowColor = $scope.customers[i].IsUpdated == true ? 'success-row' : $scope.customers[i].IsUpdated == false ? 'error-row' : '';
                    var title = $scope.customers[i].ErrorMessage || '';
                    var row =
                    '<tr class="' + className + ' ' + rowColor + ' compiled" title="' + title + '">\
                                    <td>' + checkbox + '</td>\
                                    <td>' + (i + 1) + '</td>\
                                    <td>' + $scope.customers[i].Username + '</td>\
                                    <td>' + $scope.customers[i].RoleName + '</td>\
                                    <td>' + $scope.customers[i].UplineUserName + '</td>\
                                    <td>' + $scope.customers[i].CurrencyName + '</td>\
                                    <td>' + ($scope.customers[i].IsSync ? 'Yes' : 'No') + '</td>\
                                    <td>' + ($scope.customers[i].IsSync3rd ? 'Yes' : 'No') + '</td>\
                                </tr>';
                    $('#body-row').append(row);
                }

                $('input[type=checkbox]').click(function () {
                    var checkboxes = $('input[type=checkbox]');
                    var allChecked = false;
                    for (var i = 0; i < checkboxes.length; i++) {
                        if (checkboxes[i].checked) {
                            allChecked = true;
                            break;
                        }
                    }

                    if (!allChecked) {
                        $('#sync').attr('disabled', true);
                    }
                    else {
                        $('#sync').attr('disabled', false);
                    }
                });
            });
        }
    };
}

function CheckCust(checkbox){
    var checkboxes = $('input[type=checkbox]');
    var allChecked = true;
    for (var i = 1; i < checkboxes.length; i++) {
        if (!checkboxes[i].checked) {
            allChecked = false;
            break;
        }        
    }

    $('#chkAll').attr('checked', allChecked);
}