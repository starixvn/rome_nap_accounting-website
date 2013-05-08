

var matchWinLoss2aUtils = {
    disableReportButton: function () {
        $('#btnReport').attr('disabled', true);
        $('#btnToday').attr('disabled', true);
        $('#btnYesterday').attr('disabled', true);
        $('#btnExport').attr('disabled', true);
        $('#btnExport').removeClass('hidden');
    },
    enableReportButton: function () {
        $('#btnReport').removeAttr('disabled');
        $('#btnToday').removeAttr('disabled');
        $('#btnYesterday').removeAttr('disabled');
        $('#btnExport').removeAttr('disabled');
    },
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unblockUI: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },
    closePopup: function (popuid) {
        try {
            $("#" + popuid).dialog("close");
        }
        catch (e) {
        }
    },
    nonBorderLastRow: function (tableId) {
        var lastRow = $("#" + tableId + " tbody").find("tr:last");
        var tdLastRow = lastRow.find("td");
        var tdLength = tdLastRow.length;
        for (var i = 0; i < tdLength; i++) {
            $(tdLastRow[i]).addClass("nonBorderBottom");
        }
    }
}