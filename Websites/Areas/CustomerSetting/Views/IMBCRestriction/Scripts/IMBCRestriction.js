IMBC = {};
IMBC.UpdateRestriction = function () {
    Fanex.CustomerSetting.Core.BlockUI();
    $.ajax({
        type: 'POST',
        url: resolveClientUrl('IMBCAdmin/Update'),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({ isAllowed: $('#chkAllowedPTChargeComm').prop('checked') }),
        beforeSend: function (jqXHR, settings) { },
        success: function (data, textStatus, jqXHR) {
            var code = data.ErrorCode === 0 ? 0 : 1;
            var message = code == 0 ? 'Updated successfully' : data.ErrorMessage;
            showAlertMessage('errMsg', message, code);
            if (code === 0) {
                window.setTimeout(function () {
                    document.location.reload(true);
                }, 3000);
            }
        },
        complete: function (jqXHR, textStatus) { Fanex.CustomerSetting.Core.UnBlockUI(); }
    });
};