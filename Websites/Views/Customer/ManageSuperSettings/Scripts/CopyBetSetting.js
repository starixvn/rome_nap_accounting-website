var copyBetSetting = {
    prevParam: {
        sourceSuper: "",
        superList: ""
    },
    enableDisableInput: function (flag) {
        $("#tab-cbs-sourcesuper").prop('disabled', flag);
        $("#tab-cbs-superlist").prop('disabled', flag);
        $("#tab-cbs-btnCheckSupers").prop('disabled', flag);
    },
    resetSelect: function () {
        copyBetSetting.enableDisableInput(false);

        $("#tab-cbs-superlist").val('');
        $("#tab-cbs-sourcesuper").val('');
        $("#tab-cbs-superSetting").addClass("hidden");
        $('#tab-cbs-errMsg').html('');
        $('#tab-cbs-errSupers').html('');
    },
    checkSuperList: function () {
        $('#tab-cbs-errMsg').html('');
        $('#tab-cbs-errSupers').html('');
        var sourceSuper = $("#tab-cbs-sourcesuper").val();
        if (sourceSuper === "") {
            showAlertMessage('tab-cbs-errMsg', "Please input source super.", 1);
            return;
        }

        var superList = $("#tab-cbs-superlist").val();
        if (superList === "") {
            showAlertMessage('tab-cbs-errMsg', "Please input destination super.", 1);
            return;
        }

        if (superList.split(',').length > 100) {
            showAlertMessage('tab-cbs-errMsg', "There're more than 100 Usernames.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            SourceSuper: sourceSuper,
            SuperList: superList,
            IsCopy: true,
            Mode: 'BetSetting'
        };

        $("#tab-cbs-superSetting").addClass("hidden");
        $("#tab-cbs-iframe").attr("src", "../_loading.htm");

        $.ajax({
            type: "POST",
            url: "CheckSuperList",
            contentType: "application/json",
            data: JSON.stringify(request),
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (data) {
                parent.jQuery.unblockUI();
                if (data.Result.ErrorCode === 0) {
                    copyBetSetting.enableDisableInput(true);
                    var url = "CopyBetSettings?sourceSuper=" + sourceSuper
                        + "&superList=" + superList
                        + "&productGroupId=1";

                    copyBetSetting.prevParam = {
                        sourceSuper: sourceSuper,
                        superList: superList
                    };

                    copyBetSetting.changeProduct(1);
                    $("#tab-cbs-iframe").attr("src", url);

                    setTimeout(() => {
                        $("#tab-cbs-superSetting").removeClass("hidden");
                    }, 1000);
                } else {
                    showAlertMessage('tab-cbs-errMsg', data.Result.ErrorMessage, 1);
                }
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-cbs-errMsg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
                else {
                    showAlertMessage('tab-cbs-errMsg', 'Internal server error.', 1);
                }
                parent.jQuery.unblockUI();
            },
            complete: function () {
            }
        });
    },
    changeProduct: function (productId) {
        $('#tab-cbs-superSetting .tab-product').removeClass('current');
        $('#tab-cbs-superSetting .tab-product-' + productId).addClass('current');

        var url = "CopyBetSettings?sourceSuper=" + copyBetSetting.prevParam.sourceSuper
            + "&superList=" + copyBetSetting.prevParam.superList
            + "&productGroupId=" + productId;
        $("#tab-cbs-iframe").attr("src", url);
    }
};