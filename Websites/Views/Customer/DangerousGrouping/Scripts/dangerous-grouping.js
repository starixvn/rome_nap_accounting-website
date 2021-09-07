var editMode;
var singleLevelSellected;
var multipleLevelSellected;
var btnUpdateMultiple;
var btn1UpdateMultiple;
var btnUpdateSingle;
var btn1UpdateSingle;
$(document).ready(function () {
    btnUpdateSingle = $("#btnUpdateSingle");
    btn1UpdateSingle = $("#btn1UpdateSingle");
    btnUpdateMultiple = $("#btnUpdateMultiple");
    btn1UpdateMultiple = $("#btn1UpdateMultiple");
    editMode = 'Multiple';
    $('#selectModeOption').change(function () {
        RemoveSelectedLevel();
        HideErrMsg();
        var dropdown = $('#selectModeOption').val();
        editMode = dropdown;
        $('div[id^="tab_content_"]').hide();
        $('#tab_content_' + dropdown).show();
    });

    $('#getUserDanger').submit(function (event) {
        event.preventDefault();
        HideErrMsg();
        GetCustomerDanger();
    });

    $('#multipleDangerousType').change(function () {
        $("#multipleLevelSelectContent").removeClass("hide");
    });

    $('#singleDangerousType').change(function () {
        $("#singleLevelSelectContent").removeClass("hide");
    });

    if (customerDanger !== null) {
        InitCustomerDanger();
        $('#selectModeOption').val("Single");
        $('#selectModeOption').change();
    }

    $("#singleNoteMessageInfo").click(function () {
        OpenNoteMessage();
    });

    $("#multipleNoteMessageInfo").click(function () {
        OpenNoteMessage();
    });

    $("#usernames").off("change").on("change", function () {
        var usernames = $("#usernames");
        var usernamesValue = $.trim(usernames.val());
        if (usernamesValue.length > 0) {
            if (usernamesValue.indexOf("\n") >= 0) {
                usernamesValue = usernamesValue.replace(/\n/g, " ");
            }

            if (usernamesValue.indexOf("\r") >= 0) {
                usernamesValue = usernamesValue.replace(/\r/g, " ");
            }

            if (usernamesValue.indexOf("\t") >= 0) {
                usernamesValue = usernamesValue.replace(/\t/g, " ");
            }

            usernamesValue = usernamesValue.replace(/\s/g, ",");
            var usernamesArray = usernamesValue.split(",").filter(item => item.length > 0);
            usernames.val(usernamesArray.join(","));
        }
    });
});

function OpenNoteMessage() {
    $('#noteMessage').dialog({
        title: 'Info',
        modal: true,
        width: 400,
        height: 100,
    });
}

function InitCustomerDanger() {
    $('#isSingleApplyAllDownlines').prop('checked', false);
    $("#username").val(customerDanger.Username);
    if (customerDanger.IsMember) {
        $("#div-hasdownline").addClass('hide');
    } else {
        $("#div-hasdownline").removeClass('hide');
    }
    DrawDangerCell($("#customerOriginDanger"), customerDanger.OriginDanger.Color, customerDanger.OriginDanger.BackgroundColor, customerDanger.OriginDanger.Name);
    DrawDangerCell($("#customerABIDanger"), customerDanger.ABIDanger.Color, customerDanger.ABIDanger.BackgroundColor, customerDanger.ABIDanger.Name);
    DrawDangerCell($("#customerBasketballDanger"), customerDanger.BasketballDanger.Color, customerDanger.BasketballDanger.BackgroundColor, customerDanger.BasketballDanger.Name);
    DrawDangerCell($("#customerESportsDanger"), customerDanger.ESportsDanger.Color, customerDanger.ESportsDanger.BackgroundColor, customerDanger.ESportsDanger.Name);
    DrawDangerCell($("#customerOtherDanger"), customerDanger.OtherSportDanger.Color, customerDanger.OtherSportDanger.BackgroundColor, customerDanger.OtherSportDanger.Name);
}

function DrawDangerCell(cell, levelColor, levelBackgroundColor, level) {
    cell.css('color', levelColor);
    cell.css('background-color', levelBackgroundColor);
    cell.html('<p style="margin: auto;">' + level + '</p>');
    cell.css('display', 'flex');
}

function RemoveSelectedLevel() {
    btnUpdateMultiple.attr("disabled", "disabled");
    btn1UpdateMultiple.attr("disabled", "disabled");
    btnUpdateSingle.attr("disabled", "disabled");
    btn1UpdateSingle.attr("disabled", "disabled");
    singleLevelSellected = null;
    multipleLevelSellected = null;
    $("#selectedMultipleLevelContent").hide();
    $("#selectedSingleLevelContent").hide();
}

function selectDangerLevel(level, levelValue, levelColor, levelBackgroundColor) {
    if (editMode === 'Multiple') {
        selectMultipleDangerLevel(level, levelValue, levelColor, levelBackgroundColor);
        btn1UpdateMultiple.attr("disabled", null);
        btnUpdateMultiple.attr("disabled", null);
    } else {
        selectSingleDangerLevel(level, levelValue, levelColor, levelBackgroundColor);
        btn1UpdateSingle.attr("disabled", null);
        btnUpdateSingle.attr("disabled", null);
    }
}

function selectMultipleDangerLevel(level, levelValue, levelColor, levelBackgroundColor) {
    multipleLevelSellected = levelValue;
    $("#selectedMultipleDangerLevel").css('color', levelColor);
    $("#selectedMultipleDangerLevel").css('background-color', levelBackgroundColor);
    $("#selectedMultipleDangerLevel").html('<p style="margin: auto;">' + level + '</p>');
    $("#selectedMultipleLevelContent").css('display', 'flex');
}

function selectSingleDangerLevel(level, levelValue, levelColor, levelBackgroundColor) {
    singleLevelSellected = levelValue;
    $("#selectedSingleDangerLevel").css('color', levelColor);
    $("#selectedSingleDangerLevel").css('background-color', levelBackgroundColor);
    $("#selectedSingleDangerLevel").html('<p style="margin: auto;">' + level + '</p>');
    $("#selectedSingleLevelContent").css('display', 'flex');
}

function GetCustomerDanger() {
    var username = $("#username").val();
    $.ajax({
        type: "GET",
        url: "GetCustomerDanger?username=" + username,
        contentType: "application/json",
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            if (data.Result.ErrorCode === 0) {
                customerDanger = data.CustomerDanger;
                InitCustomerDanger();
            } else {
                showAlertMessage('errMsg', data.Result.ErrorMessage, 1);
            }
        },
        error: function (err) {
            showAlertMessage('errMsg', 'Internal server error.', 1);
        },
        complete: function () {
            parent.jQuery.unblockUI();
        }
    });
}

function GetValidateResponse(usernames, dangerType, dangerLevel) {
    return new Promise(resolve => {
        var validateUsernamesRequest = {
            usernames: usernames,
            dangerType: dangerType,
            dangerLevel: dangerLevel
        };
        $.ajax({
            type: "POST",
            url: "ValidateUsernames",
            contentType: "application/json",
            data: JSON.stringify(validateUsernamesRequest),
            success: function (data) {
                if (data.ErrorCode === 0) {
                    resolve('');
                } else {
                    resolve(data.ErrorMessage);
                }
            },
            error: function (err) {
                resolve('Internal server error.');
            }
        });
    });
}

async function ValidateUsernames(event) {
    event.preventDefault();
    parent.BlockUI();
    var usernames = $("#usernames").val();
    var dangerType = GetDangerType($('#multipleDangerousType').val());
    GetValidateResponse(usernames, dangerType, dangerLevel).then(validateResponse => {
        if (validateResponse !== '') {
            showAlertMessage('errMsg', validateResponse, 1);
        } else {
            showAlertMessage('errMsg', 'Validate Successfully.', 0);
        }
        parent.jQuery.unblockUI();
    });
}

function UpdateSingleAccountDanger() {
    HideErrMsg();
    if (singleLevelSellected === undefined || singleLevelSellected === null) {
        showAlertMessage('errMsg', 'Please select danger level.', 1);

        return;
    }

    var username = $("#username").val();
    if (username === undefined || username === '') {
        showAlertMessage('errMsg', 'Username is required.', 1);

        return;
    }

    var dangerType = GetDangerType($('#singleDangerousType').val());
    var dangerLevel = singleLevelSellected;
    try { parent.BlockUI(); } catch (e) { }
    GetValidateResponse(username, dangerType, dangerLevel).then(function (validateResponse) {
        if (validateResponse !== '') {
            showAlertMessage('errMsg', validateResponse, 1);
            parent.jQuery.unblockUI();
        } else {
            var customerDangerUpdateRequest = {
                Customer: username,
                UpdateDownline: $('#isSingleApplyAllDownlines').is(":checked"),
                DangerType: dangerType,
                DangerLevel: singleLevelSellected,
            }

            $.ajax({
                type: "POST",
                url: "UpdateCustomerDanger",
                contentType: "application/json",
                data: JSON.stringify(customerDangerUpdateRequest),
                dataType: "json",
                success: function (data) {
                    if (data.ErrorCode === 0) {
                        showAlertMessage('errMsg', 'Update successfully.', 0);
                        setTimeout(function () {
                            HideErrMsg();
                            GetCustomerDanger();
                        }, 3000);
                    } else {
                        showAlertMessage('errMsg', data.ErrorMessage, 1);
                    }
                },
                error: function (err) {
                    showAlertMessage('errMsg', 'Internal server error.', 1);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    });
}

function UpdateMultipleAccountDanger() {
    HideErrMsg();
    if (multipleLevelSellected === undefined || multipleLevelSellected === null) {
        showAlertMessage('errMsg', 'Please select danger level.', 1);

        return;
    }

    var usernames = $("#usernames").val();
    if (usernames === undefined || usernames === '') {
        showAlertMessage('errMsg', 'Username(s) is required.', 1);

        return;
    }

    var dangerType = GetDangerType($('#multipleDangerousType').val());
    var dangerLevel = multipleLevelSellected;
    try { parent.BlockUI(); } catch (e) { }
    GetValidateResponse(usernames, dangerType, dangerLevel).then(function (validateResponse) {
        if (validateResponse !== '') {
            showAlertMessage('errMsg', validateResponse, 1);
            parent.jQuery.unblockUI();
        } else {
            var multipleCustomersDangerUpdateRequest = {
                CustomerNames: usernames,
                UpdateDownline: $('#isMultipleApplyAllDownlines').is(":checked"),
                DangerType: dangerType,
                DangerLevel: multipleLevelSellected
            };

            $.ajax({
                type: "POST",
                url: "UpdateMultipleCustomersDanger",
                contentType: "application/json",
                data: JSON.stringify(multipleCustomersDangerUpdateRequest),
                dataType: "json",
                success: function (data) {
                    if (data.ErrorCode === 0) {
                        showAlertMessage('errMsg', 'Update successfully.', 0);
                        setTimeout(function () {
                            HideErrMsg();
                        }, 3000);
                    } else {
                        showAlertMessage('errMsg', data.ErrorMessage, 1);
                    }
                },
                error: function (err) {
                    showAlertMessage('errMsg', 'Internal server error.', 1);
                },
                complete: function () {
                    parent.jQuery.unblockUI();
                }
            });
        }
    });
}

function GetDangerType(dangerTypeName) {
    switch (dangerTypeName) {
        case "Origin":
            return 1;
        case "ABI":
            return 2;
        case "OtherSports":
            return 3;
        case "Basketball":
            return 4;
        case "ESports":
            return 5;
    }
}

function HideErrMsg() {
    $('#errMsg').html('');
}