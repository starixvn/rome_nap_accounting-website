/// <reference path="../../jquery-1.6.2.min.js" />

$(document).ready(function () {
    Core.registerWeekPicker();
    $("form:eq(0)").attr("data-ajax-complete", "Core.onAjaxCompleted")
                    .attr("data-ajax-failure", "Core.onAjaxFailure");
    $('input:button[value="Submit"], input:button[value="Export Excel"]').attr('disabled', false);

    $("input:button").button();
    $("input:submit").button();
    $('.date-area').show();
    $('.month-area, .week-area').hide();
    //var isFirstLoad = ($('#PeriodOption').val() == "1");

    $("#PeriodOption").change(function () {
        setVisibleControl($(this).val(), true);
    });
    Core.setMonthConstraint();
    Core.setFromToDateConstraint();
    Core.setWeekRangeConstraintLimit2Months();
    setVisibleControl($("#PeriodOption").val(), false);

    if ($('#data').length > 0) {
        $('#data').width($(window).width() - 60); //minus pending pixels

        fxheaderfooterInit('data', 600, 1, 0);
        fxheaderfooter();

    }
    
    
    $("#Week").weekpicker("onSelect", function (start, end) {
        $("#DateStart").datepicker("setDate", start);
    });

    $("#ToWeek").weekpicker("onSelect", function (start, end) {
        $("#DateEnd").datepicker("setDate", end);
        
    });

})

function setVisibleControl(currentValue, isDefault) {

    if (currentValue == "Week") {
        $(".date-area, .month-area, .year-area").hide();
        $(".week-area").show();
        if (isDefault) {
            $('#Week').weekpicker('setDate', Core.maxDate);
            $('#ToWeek').weekpicker('setDate', Core.maxDate);
        }        
    }
    else if (currentValue == "Month") {
        $(".date-area, .week-area, .year-area").hide();
        $(".month-area").show();
        if (isDefault) {
            $("#Month").val(Core.maxDate.getMonth() + 1);
            $("#Year").val(Core.maxDate.getFullYear());
        }
    }
    else if (currentValue == "Date") {
        $(".date-area").show();
        $(".week-area, .month-area, .year-area").hide();
        if (isDefault) {
            $("#DateStart").datepicker("setDate", Core.maxDate);
            $("#DateEnd").datepicker("setDate", Core.maxDate);
        }
    }   

}

function submitForm() {
    $('#formDeposit').attr("data-ajax", "false");
    Core.doSubmit(function () {
        $('input:button[value="Submit"], input:button[value="Export Excel"]').attr('disabled', true);
        $('#formDeposit').attr('action', 'Deposit');
        $('#formDeposit').submit();
    });

}

function exportExcel() {
    ajaxRequest = $.ajax({
        url: "DepositExcelExport",
        data: $('#formDeposit').serialize()
    });
/*
    $('#formDeposit').attr("data-ajax", "true");
    Core.doSubmit(function () {
    $('input:button[value="Submit"], input:button[value="Export Excel"]').attr('disabled', true);
    $('#formDeposit').attr('action', 'DepositExcelExport');
    $('#formDeposit').submit();
    });*/
}