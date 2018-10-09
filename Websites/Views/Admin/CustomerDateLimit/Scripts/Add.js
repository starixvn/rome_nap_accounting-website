var CustomerDataRangeAdd = {
    txtMinDate: null,
    dataRangeType: {
        none: 1,
        timePeriod: 2,
        minDate: 3
    },

    init: function () {
        var txtMinDate = $("#txt-min-date");
         CustomerDataRangeAdd.txtMinDate = txtMinDate;

         CustomerDataRangeAdd.clearForm();

        $("#btn-clear").click( CustomerDataRangeAdd.clearForm);

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

        $("#btn-submit").click( CustomerDataRangeAdd.updateDataRange);

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
        $(".rdb-data-range").click( CustomerDataRangeAdd.clickDataRange);
         CustomerDataRangeAdd.initCustData();
    },
    clickDataRange: function () {
        var type = parseInt($(".rdb-data-range:checked").val(), 0);
        switch (type) {           
            case  CustomerDataRangeAdd.dataRangeType.timePeriod:
                $("#txt-min-date").attr("disabled", "disabled");
                $("#txt-min-date").val($.datepicker.formatDate('M dd, yy', new Date()));
                $("#txt-min-date").datepicker("disable");

                $("#txt-time-period").attr("disabled", null);
                break;
            case  CustomerDataRangeAdd.dataRangeType.minDate:
                $("#txt-time-period").attr("disabled", "disabled");
                $("#txt-time-period").val("");

                $("#txt-min-date").attr("disabled", null);
                $("#txt-min-date").datepicker("enable");
                break;
        }
    },
    clearForm: function () {
        $("#rdb-time-period").attr("checked", "checked");
        $("#userNames").val("");
         CustomerDataRangeAdd.clickDataRange();
    },
    updateDataRange: function () {        
        if (CustomerDataRangeAdd.getUserNames() == "") {
            showAlertMessage("errMsg", "Super username(s) is required", 1);
            return;
        }
        var type = parseInt($(".rdb-data-range:checked").val(), 0);
        var params = {
            userNames: CustomerDataRangeAdd.getUserNames(),
            choice: type
        };

        switch (type) {
            case  CustomerDataRangeAdd.dataRangeType.timePeriod:
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
            case  CustomerDataRangeAdd.dataRangeType.minDate:
                params.MinDate = $('#txt-min-date').val();
                break;
        }
         CustomerDataRangeAdd.postUpdateDataRange(params);
    },
    getUserNames: function () {
        return $("#userNames").val();
    },
    postUpdateDataRange: function (params) {
        $.ajax({
            url: resolveClientUrl("CustomerDateLimit/Update"),
            dataType: 'json',
            type: 'post',
            data: params,
            beforeSend: function () {
                try { parent.BlockUI(); } catch (e) { }
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
         CustomerDataRangeAdd.clickDataRange();
        if (type ==  CustomerDataRangeAdd.dataRangeType.minDate) {
            $("#txt-min-date").val($.datepicker.formatDate('M dd, yy', new Date($("#MinDate").val())));            
        } else {
            $("#txt-time-period").val($("#LimitDays").val());
        }
    }
};

$(document).ready( CustomerDataRangeAdd.init);