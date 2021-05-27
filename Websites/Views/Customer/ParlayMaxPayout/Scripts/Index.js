$(document).ready(function () {
    var cachedDOM = {
        btnUpdate: $('#btnUpdate'),
        Message: $('#result-message'),
        txtCustId: $('#txtCustId'),
        txtCustName: $('#txtCustName'),
        chkUnlimitedElements: $('input[id^=chkUnlimited_]')
    };

    cachedDOM.chkUnlimitedElements.unbind();
    cachedDOM.chkUnlimitedElements.change(function () {
        var $this = $(this);
        var alternativeFieldId = $this.attr('id').replace('chkUnlimited_', '');
        var $field = $('#' + alternativeFieldId);
        var $fieldUnlimited = $('#' + alternativeFieldId + '_Unlimited');
        if ($this.prop('checked')) {
            $field.hide();
            $fieldUnlimited.show();
        }
        else {
            $field.show();
            $fieldUnlimited.hide();
        }
    });

    cachedDOM.btnUpdate.click(function () {
        try { parent.BlockUI(); } catch (e) { }
        var params = {
            CustId: cachedDOM.txtCustId.val(),
            ParlayMaxPayoutList: []
        };

        params.ParlayMaxPayoutList.push({
            SportType: 1,
            Bettype: 9,
            SingleMatchParlay: $('#chkUnlimited_1_9_SingleMatchParlay').prop('checked') ? 0 : parseInt($('#1_9_SingleMatchParlay').val().replaceAll(',', '')),
            Combo2: $('#chkUnlimited_1_9_Combo2').prop('checked') ? 0 : parseInt($('#1_9_Combo2').val().replaceAll(',', '')),
            Combo35: $('#chkUnlimited_1_9_Combo35').prop('checked') ? 0 : parseInt($('#1_9_Combo35').val().replaceAll(',', '')),
            ComboOver6: $('#chkUnlimited_1_9_ComboOver6').prop('checked') ? 0 : parseInt($('#1_9_ComboOver6').val().replaceAll(',', ''))
        });

        params.ParlayMaxPayoutList.push({
            SportType: 99,
            Bettype: 9,
            SingleMatchParlay: $('#chkUnlimited_99_9_SingleMatchParlay').prop('checked') ? 0 : parseInt($('#99_9_SingleMatchParlay').val().replaceAll(',', '')),
            Combo2: $('#chkUnlimited_99_9_Combo2').prop('checked') ? 0 : parseInt($('#99_9_Combo2').val().replaceAll(',', '')),
            Combo35: $('#chkUnlimited_99_9_Combo35').prop('checked') ? 0 : parseInt($('#99_9_Combo35').val().replaceAll(',', '')),
            ComboOver6: $('#chkUnlimited_99_9_ComboOver6').prop('checked') ? 0 : parseInt($('#99_9_ComboOver6').val().replaceAll(',', ''))
        });

        $.ajax({
            type: "POST",
            url: "Update",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode === 0) {
                    ShowSuccessMessage();
                    setTimeout(function () {
                        parent.$('#dialog-modalParlayMaxPayout').dialog('close');
                    }, 3000);
                } else {
                    ShowErrorMessage(result.ErrorMessage);
                }
            },
            error: function () {
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });

    function ShowErrorMessage(errMsg) {
        cachedDOM.Message.removeClass();
        cachedDOM.Message.addClass('errorMsg');
        cachedDOM.Message.html(errMsg);
        cachedDOM.Message.show();
    };

    function ShowSuccessMessage() {
        cachedDOM.Message.removeClass();
        cachedDOM.Message.addClass('successMsg');
        cachedDOM.Message.html('Updated Successfully.');
        cachedDOM.Message.show();
    }
});