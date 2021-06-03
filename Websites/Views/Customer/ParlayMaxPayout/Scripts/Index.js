$(document).ready(function () {
    var cachedDOM = {
        btnUpdate: $('#btnUpdate'),
        Message: $('#result-message'),
        txtCustId: $('#txtCustId'),
        txtCustName: $('#txtCustName'),
        chkUnlimitedElements: $('input[id^=chkUnlimited_]'),
        txtInputFields: $('.input-field'),
        txtRoleId: $('#txtRoleId')
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
        parent.BlockUI();

        if (!ValidateInput()) {
            parent.jQuery.unblockUI();
            return;
        }

        var params = {
            CustId: cachedDOM.txtCustId.val(),
            ParlayMaxPayoutList: []
        };

        var soccerParlayRow = {
            SportType: 1,
            Bettype: 9,
            SingleMatchParlay: GetValue('1_9_SingleMatchParlay'),
            Combo2: GetValue('1_9_Combo2'),
            Combo35: GetValue('1_9_Combo35'),
            ComboOver6: GetValue('1_9_ComboOver6')
        };

        if (HasValueChanged('1_9_'))
            params.ParlayMaxPayoutList.push(soccerParlayRow);

        var MixSportsParlayRow = {
            SportType: 99,
            Bettype: 9,
            SingleMatchParlay: GetValue('99_9_SingleMatchParlay'),
            Combo2: GetValue('99_9_Combo2'),
            Combo35: GetValue('99_9_Combo35'),
            ComboOver6: GetValue('99_9_ComboOver6')
        };

        if (HasValueChanged('99_9_'))
            params.ParlayMaxPayoutList.push(MixSportsParlayRow);

        if (params.ParlayMaxPayoutList.length === 0) {
            ShowErrorMessage('There is no change to update.');
            parent.jQuery.unblockUI();
            return;
        }

        $.ajax({
            type: "POST",
            url: "Check",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(params),
            success: function (result) {
                if (result.ErrorCode === 0 || confirm(result.ErrorMessage)) {
                    $.ajax({
                        type: "POST",
                        url: "Update",
                        contentType: "application/json; charset=utf-8",
                        data: JSON.stringify(params),
                        success: function (result) {
                            if (result.ErrorCode === 0) {
                                ShowSuccessMessage();
                                setTimeout(function () {
                                    location.reload(true);
                                }, 1000);
                            } else {
                                ShowErrorMessage(result.ErrorMessage);
                            }
                        },
                        complete: function () {
                            parent.jQuery.unblockUI();
                        }
                    });

                }
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    });

    function ValidateInput() {
        var isValid = true;
        var errMsg = '';
        cachedDOM.txtInputFields.each(function () {
            var $this = $(this);
            var $chkUnlimited = $('#' + $this.attr('data-checkbox-unlimited-id'));
            var defaultPayoutValue = parseInt($this.attr('data-default-payout-value').replaceAll(',', ''));
            var rowHeader = $this.attr('data-row-header');
            var columnName = $this.attr('data-column-name');
            var inputValue = GetValue($this.attr('id'));
            var isCheckedUnlimited = $chkUnlimited.prop('checked');
            if (!isCheckedUnlimited && inputValue <= 0) {
                isValid = false;
                errMsg = 'The value must be a positive amount.';
                return false;
            }

            if (!isCheckedUnlimited && defaultPayoutValue > 0 && inputValue > defaultPayoutValue) {
                isValid = false;
                errMsg = rowHeader + ": The setting of " + columnName + " is higher than the " + (cachedDOM.txtRoleId.val() === "4" ? "company setting." : "upline's.");
                return false;
            }
        });

        if (!isValid) {
            ShowErrorMessage(errMsg);
        }

        return isValid;
    }

    function IsValueChanged(id) {
        var val = GetValue(id);
        var origVal = parseInt($('#' + id).attr('data-original-value').replaceAll(',', ''));
        return val !== origVal;
    }

    function HasValueChanged(prefixId) {
        return IsValueChanged(prefixId + 'SingleMatchParlay')
            || IsValueChanged(prefixId + 'Combo2')
            || IsValueChanged(prefixId + 'Combo35')
            || IsValueChanged(prefixId + 'ComboOver6');
    }

    function GetValue(id) {
        return $('#chkUnlimited_' + id).prop('checked') ? 0 : parseInt($('#' + id).val().replaceAll(',', ''));
    }

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

    function HighlightError(element) {
        $(element).addClass('ui-state-error');
    }

    function ClearHighlight(element) {
        $(element).removeClass('ui-state-error');
    }
});