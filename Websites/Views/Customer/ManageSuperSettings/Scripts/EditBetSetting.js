var editBetSetting = {
    prevParam: {
        sourceSuper: "",
        superList: ""
    },
    enableDisableInput: function (flag) {
        $("#tab-ebs-superlist").prop('disabled', flag);
        $("#tab-ebs-btnCheckSupers").prop('disabled', flag);
    },
    resetSelect: function () {
        editBetSetting.enableDisableInput(false);

        $("#tab-ebs-superlist").val('');
        $("#tab-ebs-superSetting").addClass("hidden");
        $('#tab-ebs-err-msg').html('');
        $('#tab-ebs-err-supers').html('');
    },
    checkSuperList: function () {
        $('#tab-ebs-err-msg').html('');
        $('#tab-ebs-err-supers').html('');

        var superList = $("#tab-ebs-superlist").val();
        if (superList === "") {
            showAlertMessage('tab-ebs-err-msg', "Please enter at least 1 Super Account username.", 1);
            return;
        }

        if (superList.split(',').length > 100) {
            showAlertMessage('tab-ebs-err-msg', "There're more than 100 Usernames.", 1);
            parent.jQuery.unblockUI();
            return;
        }

        var request = {
            SourceSuper: '',
            SuperList: superList,
            IsCopy: false,
            Mode: 'BetSetting'
        };

        $("#tab-ebs-superSetting").addClass("hidden");
        $("#tab-ebs-iframe").attr("src", "../_loading.htm");

        $.ajax({
            type: "POST",
            url: "CheckSuperList",
            contentType: "application/json",
            data: JSON.stringify(request),
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (data) {
                if (data.ErrorCode === 999) {
                    showAlertMessage('tab-ebs-err-msg', data.ErrorMessage, 1);
                    return;
                }

                if (data.Result.ErrorCode !== 0) {
                    showAlertMessage('tab-ebs-err-msg', data.Result.ErrorMessage, 1);
                    return;
                }

                editBetSetting.changeProduct(1);
                editBetSetting.enableDisableInput(true);
                var url = "EditBetSettings?sourceSuper="
                    + "&superList=" + superList
                    + "&productGroupId=1";

                editBetSetting.prevParam = {
                    sourceSuper: '',
                    superList: superList
                };

                $("#tab-ebs-iframe").attr("src", url);
                setTimeout(() => {
                    $("#tab-ebs-superSetting").removeClass("hidden");
                }, 1000);
            },
            error: function (xhr) {
                if (xhr.status === 403) {
                    showAlertMessage("tab-ebs-err-msg", $.parseJSON(xhr.responseText).ErrorMessage, 1);
                }
                else {
                    showAlertMessage('tab-ebs-err-msg', 'Internal server error.', 1);
                }
                parent.jQuery.unblockUI();
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        });
    },
    changeProduct: function (productId) {
        $('#tab-ebs-superSetting .tab-product').removeClass('current');
        $('#tab-ebs-superSetting .tab-product-' + productId).addClass('current');

        var url = "EditBetSettings?sourceSuper="
            + "&superList=" + editBetSetting.prevParam.superList
            + "&productGroupId=" + productId;
        $("#tab-ebs-iframe").attr("src", url);
    }
};