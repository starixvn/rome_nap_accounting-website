function EnableThirdPartyProductStatus() {
    var self = this;

    self.checkAll_OnChange = function () {
        var checkEnableAll_Element = top.content.$('#checkAll');
        var checkEnableAll_Value = checkEnableAll_Element.is(':checked');
        var checkEnable_Collection = top.content.$('input[id^="checkEnable_"]:enabled', '#updateStatusDialog')
        checkEnable_Collection.each(function () {
            var $this = $(this);
            if (checkEnableAll_Value) {
                if (!$this.attr('checked')) {
                    $this.attr('checked', 'checked');
                    $this.trigger('change');
                }
            } else {
                if ($this.attr('checked')) {
                    $this.removeAttr('checked');
                    $this.trigger('change');
                }
            }
        });
    }

    self.checkEnable_OnChange = function (productStatusDataRow) {
        var checkEnable_Element = top.content.$('#checkEnable_' + productStatusDataRow.SportType + '_' + productStatusDataRow.BetType);
        var checkEnableDownlines_Element = top.content.$('#checkEnableDownlines_' + productStatusDataRow.SportType + '_' + productStatusDataRow.BetType);
        var checkEnable_Value = checkEnable_Element.is(':checked');
        var checkEnableDownlines_Value = checkEnableDownlines_Element.is(':checked');
        var checkEnable_OriginalValue = !productStatusDataRow.IsDisabled;
        var checkEnableDownlines_OriginalValue = !productStatusDataRow.IsDisabledDownline;
        var submitBtn = top.content.$('#thirdPartyProductStatusSubmitBtn');
        if (checkEnable_Value) {
            if (checkEnableDownlines_OriginalValue) {
                checkEnableDownlines_Element.attr('checked', 'checked');
                checkEnableDownlines_Element.attr('disabled', 'disabled');
            }
            else {
                checkEnableDownlines_Element.removeAttr('checked');
                checkEnableDownlines_Element.removeAttr('disabled');
            }
        }
        else {
            checkEnableDownlines_Element.attr('disabled', 'disabled');
            checkEnableDownlines_Element.removeAttr('checked');
        }

        if (checkEnable_Value == checkEnable_OriginalValue) {
            checkEnable_Element.removeClass('is-data-change');
        }
        else {
            checkEnable_Element.addClass('is-data-change');
        }

        ShowHideSubmitButton();
    }

    self.checkEnableDownlines_OnChange = function (productStatusDataRow) {
        var checkEnableDownlines_Element = top.content.$('#checkEnableDownlines_' + productStatusDataRow.SportType + '_' + productStatusDataRow.BetType);
        var checkEnableDownlines_Value = checkEnableDownlines_Element.is(':checked');
        var checkEnableDownlines_OriginalValue = !productStatusDataRow.IsDisabledDownline;
        var submitBtn = top.content.$('#thirdPartyProductStatusSubmitBtn');
        if (checkEnableDownlines_Value == checkEnableDownlines_OriginalValue) {
            checkEnableDownlines_Element.removeClass('is-data-change');
        } else {
            checkEnableDownlines_Element.addClass('is-data-change');
        }

        ShowHideSubmitButton();
    }

    self.submit = function (model) {
        parent.BlockUI();
        ClearMessages();
        for (var i = 0 ; i < model.ProductStatusList.length; i++) {
            var productStatusDataRow = model.ProductStatusList[i];
            productStatusDataRow.IsDisabled = !top.content.$('#checkEnable_' + productStatusDataRow.SportType + '_' + productStatusDataRow.BetType).is(':checked');
            if (productStatusDataRow.HasDownline) {
                productStatusDataRow.IsDisabledDownline = !top.content.$('#checkEnableDownlines_' + productStatusDataRow.SportType + '_' + productStatusDataRow.BetType).is(':checked');
            }
        }

        RemoveUnchangedDataRow(model, originalModel);
        if (model.ProductStatusList.length > 0) {
            var param = {
                custId: model.CustId,
                productId: model.ProductStatusList[0].ProductId,
                productStatus: JSON.stringify(model.ProductStatusList),
                isDeposit: model.IsDeposit
            };

            $.post("ThirdPartyProductStatus/Update", param, function (result) { UpdateSuccess(result); });
        } else {
            top.content.$('#updateStatusDialog>#wrnMsg').text(errorMsg.ThereIsNoChange).removeClass('hidden');
            parent.jQuery.unblockUI();
        }
    }

    function RemoveUnchangedDataRow(model, originalModel) {
        model.ProductStatusList = model.ProductStatusList.filter(function (item, index) {
            var originalItem = originalModel.ProductStatusList[index];
            return !(originalItem && originalItem.IsDisabled == item.IsDisabled && originalItem.IsDisabledDownline == item.IsDisabledDownline);
        });
    }

    function UpdateSuccess(result) {
        var isSuccess = true;
        var errorMsg = '';
        for (var i = 0 ; i < result.length; i++) {
            var errorCode = result[i].ErrorCode;
            var errorMessage = result[i].ErrorMessage;
            if (errorCode > 0) {
                isSuccess = false;
                errorMsg += errorMessage + '\n';
            }
        }

        if (result.ErrorCode == "999") {
            isSuccess = false;
            errorMsg = result.ErrorMessage;
        }

        if (isSuccess) {
            var $successDiv = top.content.$('#updateStatusDialog>#successMsg');
            $successDiv.text('Status updated successfully');
            $successDiv.removeClass('hidden');
            top.content.$('#thirdPartyProductStatusSubmitBtn').attr('disabled', 'disabled');
            top.content.$('#thirdPartyProductStatusSubmitBtn').unbind();
            setTimeout(function () {
                top.content.$('#dialog-modal').dialog('close');
                parent.BlockUI();
                var iframe = top.content.document.getElementById("leftPanel");
                iframe.src = iframe.src;
            }, 5000);
        } else {
            var $errorDiv = top.content.$('#updateStatusDialog>#errMsg');
            $errorDiv.text(errorMsg);
            $errorDiv.removeClass('hidden');
        }

        parent.jQuery.unblockUI();
    }

    function ClearMessages() {
        var $successDiv = top.content.$('#updateStatusDialog>#successMsg');
        var $errorDiv = top.content.$('#updateStatusDialog>#errMsg');
        var $warningDiv = top.content.$('#updateStatusDialog>#wrnMsg');
        $successDiv.text('');
        $successDiv.addClass('hidden');
        $errorDiv.text('');
        $errorDiv.addClass('hidden');
        $warningDiv.text('');
        $warningDiv.addClass('hidden');
    }

    function ShowHideSubmitButton() {
        var submitBtn = top.content.$('#thirdPartyProductStatusSubmitBtn');
        if (top.content.$('input[id^="checkEnable_"],input[id^="checkEnableDownlines_"]').hasClass('is-data-change')) {
            submitBtn.removeAttr('disabled'); 
        }
        else {
            submitBtn.attr('disabled', 'disabled');
        }
    }
};

var enableThirdPartyProductStatus = new EnableThirdPartyProductStatus();