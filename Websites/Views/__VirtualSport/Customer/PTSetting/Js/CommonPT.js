
function clearMessage(containerid) {

    $("#" + containerid).html('');
}

function showAlertMessage(containerid, msg, type) {
    // Success=0, Error=1, Warning=2
    if (document.getElementById(containerid) != null) {
        switch (type) {
            case 0:
                $("#" + containerid).html("<div class='alertboxscc'><div>" + msg + "</div></div>");
                var t = setTimeout("clearMessage('" + containerid + "')", 6000);
                break;
            case 1:
                $("#" + containerid).html("<div class='alertboxerr'><div>" + msg + "</div></div>");
                break;
            case 2:
                $("#" + containerid).html("<div class='alertboxwrn'><div>" + msg + "</div></div>");
                break;
            default:
                break;
        }
        window.scroll(0, 0);
        parent.scroll(0, 0);

    }
}


function formatAlertMessage(msg, type) {
    // Success=0, Error=1, Warning=2
    switch (type) {
        case 0:
            return "<div class='msgcorrect'>" + msg + "</div>";
            break;
        case 1:
            return "<div class='msgerror'>" + msg + "</div>";
            break;
        case 2:
            return "<div class='msgwarning'>" + msg + "</div>";
            break;
        default:
            return "";
            break;
    }
}

function showEnrichMessage(containerid, ErrorHandler) {
    var isError500 = true;
    var isSuccess = true;
    var result = '';
    if (ErrorHandler != null) {
        if (ErrorHandler.length > 0) {
            $.each(ErrorHandler, function (index, Result) {
                if (Result.ErrorCode == 100 || Result.ErrorCode == 99 || Result.ErrorCode == 98) {
                    if (isError500) {
                        result += formatAlertMessage(Result.ErrorMessage, 2);
                    }
                    isSuccess = false;
                }
                else if (Result.ErrorCode > 0) {
                    if (isError500) {
                        result += formatAlertMessage(Result.ErrorMessage, 1);
                    }
                    isSuccess = false;
                }
                else {
                    result += formatAlertMessage(Result.ErrorMessage, 0);
                }
                if (Result.ErrorCode == 500) isError500 = false;
            });
        }
        else {
            if (ErrorHandler.ErrorCode <= 0) {
                result = '<div class="boxscc">' + result + '</div>';
                t1 = setTimeout("clearMessage('" + containerid + "')", 8000);
            }
            else {
                result = '<div class="boxerr">' + ErrorHandler.ErrorMessage + '</div>';
                t2 = setTimeout("clearSuccessMessages()", 8000);
            }
        }
    }


    $("#".concat(containerid)).html(result);

    window.scroll(0, 0);
    parent.scroll(0, 0);
    return isSuccess;
}

function clearSuccessMessages() {

    $("#err .msgcorrect").remove();
    clearTimeout(t2);
}

var customerInfoBinding = {
    createDependentObservable: function (setting) {
        setting.isChecked = ko.dependentObservable(function () {
            var uplineStatus = setting.custSetting.statusSetting.disableUplineVirtualSports;
            var customerStatus = setting.custSetting.statusSetting.disableVirtualSports;

            if (uplineStatus | customerStatus)
                return false;
            else
                return true;
        }, setting);
        setting.isCheckedDownlines = ko.dependentObservable(function () {
            var uplineStatus = setting.custSetting.statusSetting.disableUplineVirtualSports;
            var customerStatus = setting.custSetting.statusSetting.disableVirtualSports;
            var downlinesStatus = setting.custSetting.statusSetting.disableDownlinesVirtualSports;

            if (uplineStatus | customerStatus | downlinesStatus)
                return false;
            else
                return true;
        }, setting);
        setting.isEnable = ko.dependentObservable(function () {
            var uplineStatus = setting.custSetting.statusSetting.disableUplineVirtualSports;
            var customerStatus = setting.custSetting.statusSetting.disableVirtualSports;

            if (uplineStatus)
                return false;
            else
                return true;
        }, setting);
        setting.isEnableDownline = ko.dependentObservable(function () {

            return false;
        }, setting);
    },
    enableCheckboxClick: function (roleId) {
        if (roleId > 1) {
            var checked = $('#chkEnabled').prop("checked");
            var checkedDownline = $('#chkDownline').prop("checked");
            var oldvalue = $('#chkEnabled').attr("value") == "true" ? true : false;
            var oldvaluedownline = $("#chkDownline").attr("value") == "true" ? true : false;

            if (checked != oldvalue) {
                if (checked) {
                    $("#chkDownline").prop("disabled", checkedDownline);
                }
                else {
                    $("#chkDownline").prop("disabled", true);
                    $("#chkDownline").prop("checked", false);
                }
            }
            else {
                $("#chkDownline").prop("disabled", !oldvalue || oldvaluedownline);
                $("#chkDownline").prop("checked", oldvaluedownline);
            }
        }
    },
    save: function (roleId, oldStatus) {
        var statusSetting = Fanex.copy(oldStatus);
        statusSetting.disableVirtualSports = !($('#chkEnabled').prop('checked'));
        if (roleId > 1)
            statusSetting.disableDownlinesVirtualSports = !($('#chkDownline').prop('checked'));
        return statusSetting;
    }
};