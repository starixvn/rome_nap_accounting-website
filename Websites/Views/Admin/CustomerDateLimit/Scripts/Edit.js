var CustomerDataRangeEdit = {
    txtMinDate: null,
    dataRangeType: {
        none: 1,
        timePeriod: 2,
        minDate: 3
    },
    custIds: [],

    init: function () {
        var txtMinDate = $("#txt-min-date");
        CustomerDataRangeEdit.txtMinDate = txtMinDate;

        CustomerDataRangeEdit.clearForm();

        $("#btn-clear").click(CustomerDataRangeEdit.clearForm);

        txtMinDate.datepicker({
            showOn: 'both',
            buttonImageOnly: true,
            buttonImage: resolveClientUrl('App_Themes/Images/calendar.gif'),
            buttonText: '',
            dateFormat: 'M dd, yy',
            maxDate: new Date(),
        });

        txtMinDate.val($.datepicker.formatDate('M dd, yy', new Date()));

        $(".ui-datepicker").draggable();

        $("#btn-submit").click(CustomerDataRangeEdit.updateDataRange);

        $("#txt-time-period").keydown(function (e) {
            var key = e.charCode || e.keyCode || 0;

            var isAccept =
            (e.ctrlKey && e.keyCode == 67) || //accept ctrl + c(copy)
            (e.ctrlKey && e.keyCode == 86) || //accept ctrl + v(paste)
            key == 16 || //shift
            key == 17 || //ctrl
            key == 8 || //backspace
            key == 36 || //home
            key == 35 || //end
            key == 9 || //tab
            key == 46 || //delete

            (key >= 37 && key <= 40) || //left , right , up , down (arrow)
            (key >= 48 && key <= 57) || //0-9
            (key >= 96 && key <= 105); //numpad 0-9

            return isAccept;
        }).keyup(function () {
            var val = $(this).val();

            if (isNaN(parseFloat(val))) {
                $(this).val("");
            }
        });
        $(".rdb-data-range").click(CustomerDataRangeEdit.clickDataRange);
        CustomerDataRangeEdit.initCustData();
    },
    clickDataRange: function () {
        var type = parseInt($(".rdb-data-range:checked").val(), 0);
        switch (type) {
            case CustomerDataRangeEdit.dataRangeType.none:
                $("#txt-time-period").attr("disabled", "disabled");
                $("#txt-time-period").val("");

                $("#txt-min-date").attr("disabled", "disabled");
                $("#txt-min-date").val($.datepicker.formatDate('M dd, yy', new Date()));
                $("#txt-min-date").datepicker("disable");
                break;
            case CustomerDataRangeEdit.dataRangeType.timePeriod:
                $("#txt-min-date").attr("disabled", "disabled");
                $("#txt-min-date").val($.datepicker.formatDate('M dd, yy', new Date()));
                $("#txt-min-date").datepicker("disable");

                $("#txt-time-period").attr("disabled", null);
                break;
            case CustomerDataRangeEdit.dataRangeType.minDate:
                $("#txt-time-period").attr("disabled", "disabled");
                $("#txt-time-period").val("");

                $("#txt-min-date").attr("disabled", null);
                $("#txt-min-date").datepicker("enable");
                break;
        }
    },
    clearForm: function () {
        $("#rdb-none").attr("checked", "checked");
        CustomerDataRangeEdit.clickDataRange();
    },
    updateDataRange: function () {
        var type = parseInt($(".rdb-data-range:checked").val(), 0);
        CustomerDataRangeEdit.custIds = CustomerDataRangeEdit.getCustIds();

        var params = {
            custIds: CustomerDataRangeEdit.custIds,
            choice: type
        };

        switch (type) {
            case CustomerDataRangeEdit.dataRangeType.none:
                break;
            case CustomerDataRangeEdit.dataRangeType.timePeriod:
                params.LimitDays = $("#txt-time-period").val();

                if (params.LimitDays == "") {
                    showAlertMessage("errMsg", "Time period (day) is required", 1);
                    $("#txt-time-period").focus();
                    return;
                }

                var limitDays = parseInt(params.LimitDays, 0);

                if (limitDays == 0 || limitDays > 32767 || limitDays <= 0 || isNaN(limitDays)) {
                    showAlertMessage("errMsg", "Time period (day) is invalid", 1);
                    $("#txt-time-period").focus();
                    return;
                }

                break;
            case CustomerDataRangeEdit.dataRangeType.minDate:
                params.MinDate = $('#txt-min-date').val();
                break;
        }
        CustomerDataRangeEdit.postUpdateDataRange(params);
    },
    getCustIds: function () {
        return $("#CustIds").val();
    },
    postUpdateDataRange: function (params) {
        $.ajax({
            url: resolveClientUrl("CustomerDateLimit/Update"),
            dataType: 'json',
            type: 'post',
            data: params,
            beforeSend: function () {
                try { BlockUI(); } catch (e) { }
            },
        }).success(function (result) {
            if (result && result.ErrorMessage != "") {
                showAlertMessage("errMsg", result.ErrorMessage, 1);
            } else {
                setTimeout(parent.ClosePopup(), 500);
            }

        }).error(function (xhr, status, err) {
            showAlertMessage("errMsg", err, 1);

        }).complete(function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        });
    },

    initCustData: function () {
        var type = parseInt($("#DataRangeType").val(), 0);
        $(".rdb-data-range[value='" + type + "']").click();
        CustomerDataRangeEdit.clickDataRange();

        switch (type) {
            case CustomerDataRangeEdit.dataRangeType.none:
                break;
            case CustomerDataRangeEdit.dataRangeType.timePeriod:
                $("#txt-time-period").val($("#LimitDays").val());
                break;
            case CustomerDataRangeEdit.dataRangeType.minDate:
                $("#txt-min-date").val($.datepicker.formatDate('M dd, yy', new Date($("#MinDate").val())));
                break;
        }
    }
};

$(document).ready(CustomerDataRangeEdit.init);