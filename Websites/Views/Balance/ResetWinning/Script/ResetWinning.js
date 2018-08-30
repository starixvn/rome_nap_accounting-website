ResetWinning = function () {
    var self = this;

    var _tables = {
        txtDate: null,
        reportContainer: null
    };

    var reportParams = {
        resetWinningBeforeMovebetDayParams: {
            WinLossDate: null
        },
        resetWinningAfterMovebetDayParams: {
            FromDate: null,
            ToDate: null,
            ViewOrResetStatus: null
        }
    };

    var resetWinningParams = {
        resetWinningDate: null
    };

    var cachedData = {
        resetWinningDate: null,
        getResetWinningByDate: null
    };

    var enums = {
        timePeriodEnums: {
            sinceLastMoveBetDay: 0,
            specificDay: 1
        },

        statusResetWinningEnums: {
            resetSuccessful: 0,
            notReset: 2,
            notPermission: 1008
        }
    };

    self.cachedDomElements = function () {
        _tables.txtDate = $('#txtfromdate');
        _tables.reportContainer = $("#reportContainer");
    };

    self.getResetWinningParamsBeforeMoveBetDay = function () {
        reportParams.resetWinningBeforeMovebetDayParams.WinLossDate = _tables.txtDate.val();
        return reportParams.resetWinningBeforeMovebetDayParams;
    };

    self.getResetWinningParamsForSpecificDayAfterMoveBetDay = function () {
        reportParams.resetWinningAfterMovebetDayParams.FromDate = _tables.txtDate.val();
        reportParams.resetWinningAfterMovebetDayParams.ToDate = _tables.txtDate.val();
        reportParams.resetWinningAfterMovebetDayParams.ViewOrResetStatus = 0;

        return reportParams.resetWinningAfterMovebetDayParams;
    };

    self.getResetWinningParamsForSinceLastMoveBetDay = function () {
        reportParams.resetWinningAfterMovebetDayParams.FromDate = lastMoveDay;
        reportParams.resetWinningAfterMovebetDayParams.ToDate = chosenDate;
        reportParams.resetWinningAfterMovebetDayParams.ViewOrResetStatus = 0;

        return reportParams.resetWinningAfterMovebetDayParams;
    };

    self.InitPage = function () {
        self.cachedDomElements();
        self.setupCalendar("txtfromdate", minDate, currdate, chosenDate);
        self.bindingEvents();
    };

    self.bindingEvents = function () {
        $("#btnViewReport").click(function () {
            self.getResetWinningReport();
        });

        $("#btnResetWinning").click(function () {
            resetWinningByDay(cachedData.resetWinningDate);
        });

        $("#chkTimePeriod").change(function () {
            if ($("#chkTimePeriod").val() == enums.timePeriodEnums.specificDay) {
                $("#txtfromdate").removeAttr("disabled");
            }
            else {
                $("#txtfromdate").attr("disabled", "disabled");
            }
        });
    };

    self.renderCalendar = function (ids, opts, chosenDate) {
        var defaults = {
            changeMonth: true,
            changeYear: true,
            showOtherMonths: true,
            selectOtherMonths: true
        };
        var new_opts = $.extend(true, defaults, opts);
        var dates = $("#" + ids).datepicker(new_opts);

        $("#" + ids).datepicker("setDate", chosenDate);
    };

    self.setupCalendar = function (id, fromDate, toDate, chosenDate) {

        var limitDate = {
            minDate: fromDate,
            maxDate: toDate
        };

        self.renderCalendar(id, limitDate, chosenDate);
        $('#' + id).click(function () {
            $('#' + id).datepicker("show");
        });
    };

    self.getResetWinningBeforeMovebetDay = function () {
        var params = self.getResetWinningParamsBeforeMoveBetDay();
        $.ajax({
            type: "POST",
            url: "GetResetWinningBeforeMoveBetDayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                self.renderResetWinningBeforeMovebetDay(rs);
                parent.jQuery.unblockUI();
            },
            error: function (rs) {
                console.log(rs);
            }
        });
    };

    self.renderResetWinningBeforeMovebetDay = function (rs) {
        self.resetHtml();
        var bodyHtml = [];
        if (rs && rs.ReportRows.length > 0) {
            for (var i = 0; i < rs.ReportRows.length; i++) {
                var currentRow = rs.ReportRows[i];
                var htmlRow = self.renderResetWinningForSpecificDayRow(currentRow);
                bodyHtml.push(htmlRow);
            }

            var totalHtmlRow = '<tr class="totalRow bold"><td><div class="col140 align-left">Total</div></td>\
                    <td><div class="col120 align-right">' + rs.WinLoss + '</div></td>\
                    <td><div class="col140 align-right">' + rs.WinLossExtra + '</div></td>\
                    <td><div class="col120 align-right">' + rs.Reset + '</div></td>\
                    <td><div class="col120 align-right">' + rs.Variance + '</div></td>\
                </tr>'
            bodyHtml.push(totalHtmlRow);

            $("#tblResetWinningForSpecificDay").html(bodyHtml.join(''));
            $("#resetWinningForSpecificDay").removeClass("hidden").reRenderScroller(660, 'auto', true);
        } else {
            var emptyDataHtml = '<tr><td colspan="5"><div class="align-left">There is no data.</div></td></tr>'
            $("#tblResetWinningForSpecificDay").addClass("col660");
            $("#tblResetWinningForSpecificDay").html(emptyDataHtml);
            $("#resetWinningForSpecificDay").removeClass("hidden").reRenderScroller(660, 'auto', true);
        }
    };

    self.renderResetWinningForSpecificDayRow = function (currentRow) {
        var htmlRow = '';
        var cssClass = '';

        if (currentRow.Variance != "N/A" && parseFloat(currentRow.Variance) != 0) {
            cssClass = 'highlightRow';
        }

        if (currentRow.Product == "OTHERS") {
            cssClass = 'bgOthers';

            if (currentRow.Variance != "N/A" && parseFloat(currentRow.Variance) != 0) {
                cssClass += ' otherVariance';
            }
        }

        if (currentRow.Product && currentRow.Product.toLowerCase().indexOf("due to") != -1) {
            cssClass = 'dueToPartnerRow';

            if (currentRow.Variance != 0) {
                cssClass += ' dueToPartnerVariance';
            }
        }

        htmlRow += '<tr class="' + cssClass + '">\
            <td class="align-left col140 productTitle bold">' + currentRow.Product + '</td>\
            <td class="align-right col120">' + currentRow.WinLoss + '</td>\
            <td class="align-right col140">' + currentRow.WinLossExtra + '</td>\
            <td class="align-right col120">' + currentRow.Reset + '</td>\
            <td class="align-right col120 colVariance">' + currentRow.Variance + '</td>\
            </tr>';

        return htmlRow;
    };

    self.getResetWinningForSpecificDayAfterMovebetDay = function () {
        var params = self.getResetWinningParamsForSpecificDayAfterMoveBetDay();
        cachedData.resetWinningDate = params.FromDate;
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "GetResetWinningAfterMoveBetDayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                self.renderResetWinningForSpecificDayAfterMovebetDay(rs);
                parent.jQuery.unblockUI();
            },
            error: function () {
            }
        });
    };

    self.renderResetWinningForSpecificDayAfterMovebetDay = function (rs) {
        self.resetHtml();
        var bodyHtml = [];
        var cssClass = '';

        if (hasPermissionToReset) {
            if (self.compareDateTime(new Date(cachedData.resetWinningDate), new Date(today))) {
                $("#btnResetWinning").addClass("hidden");
                $("#lblReset").removeClass("hidden");
            }
            else {
                $("#lblReset").addClass("hidden");
                $("#btnResetWinning").removeClass("hidden");
            }
        }
        else {
            $("#btnResetWinning").addClass("hidden");
            $("#lblReset").removeClass("hidden");
        }

        if (rs && rs.ReportRows.length > 0) {
            for (var i = 0; i < rs.ReportRows.length; i++) {
                var currentRow = rs.ReportRows[i];
                for (j = 0; j < currentRow.ReportRows.length; j++) {
                    var resetWinningByDay = currentRow.ReportRows[j];
                    if (j % 2 == 0) {
                        cssClass = '';
                    }
                    else {
                        cssClass = "oddRow";
                    }

                    if (resetWinningByDay.Variance != 0) {
                        cssClass = 'highlightRow';
                    }

                    if (resetWinningByDay.Product && resetWinningByDay.Product.toLowerCase().indexOf("due to") != -1) {
                        cssClass = 'dueToPartnerRow';

                        if (resetWinningByDay.Variance != 0) {
                            cssClass += ' dueToPartnerVariance';
                        }
                    }

                    if (resetWinningByDay.Product == "OTHERS") {
                        cssClass = 'bgOthers';

                        if (resetWinningByDay.Variance != "N/A" && parseFloat(resetWinningByDay.Variance) != 0) {
                            cssClass += ' otherVariance';
                        }
                    }

                    var htmlRow = self.renderResetWinningForSpecificDayAfterMovebetDayRow(resetWinningByDay, cssClass);
                    bodyHtml.push(htmlRow);
                }

                var totalHtmlRow = '<tr class="totalRow bold"><td><div class="col140 align-left">Total</div></td>\
                        <td><div class="col120 align-right">' + currentRow.WinLoss_Str + '</div></td>\
                        <td><div class="col140 align-right">' + currentRow.WinLossExtra_Str + '</div></td>\
                        <td><div class="col120 align-right">' + currentRow.Reset_Str + '</div></td>\
                        <td><div class="col120 align-right">' + currentRow.Variance_Str + '</div></td>\
                    </tr>'
            }

            bodyHtml.push(totalHtmlRow);

            var resetDate = cachedData.resetWinningDate.replace(/\//g, "");
            $("#tblResetWinningForSpecificDayAfterMoveBetDay").html(bodyHtml.join(''));
            $("#resetWinningForSpecificDayAfterMoveBetDay").removeClass("hidden").reRenderScroller(660, 'auto', true);
        } else {
            var emptyDataHtml = '<tr><td colspan="5"><div class="align-left">There is no data.</div></td></tr>'
            $("#tblResetWinningForSpecificDay").addClass("col660");
            $("#tblResetWinningForSpecificDay").html(emptyDataHtml);
            $("#resetWinningForSpecificDay").removeClass("hidden").reRenderScroller(660, 'auto', true);
        }
    };

    self.renderResetWinningForSpecificDayAfterMovebetDayRow = function (currentRow, cssClass) {
        var htmlRow = '';

        htmlRow += '<tr class="' + cssClass + '">\
            <td class="align-left col140 productTitle bold">' + currentRow.Product + '</td>\
            <td class="align-right col120">' + currentRow.WinLoss_Str + '</td>\
            <td class="align-right col140">' + currentRow.WinLossExtra_Str + '</td>\
            <td class="align-right col120">' + currentRow.Reset_Str + '</td>\
            <td class="align-right col120 colVariance">' + currentRow.Variance_Str + '</td>\
            </tr>';

        return htmlRow;
    };

    self.resetWinningForSpecificDayAfterMoveBetDay = function (resetWinningDate) {
        $("#errMsg").addClass("hidden");
        resetWinningParams.resetWinningDate = resetWinningDate;
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "ResetWinningByDay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(resetWinningParams),
            async: false,
            success: function (rs) {
                $("#errMsg").removeClass("hidden");
                if (rs.ErrorCode == enums.statusResetWinningEnums.notPermission) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 2);
                }
                else if (rs.ErrorCode == enums.statusResetWinningEnums.resetSuccessful) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 0);
                    setTimeout(self.getResetWinningForSpecificDayAfterMovebetDay(), 10000);
                }
                else if (rs.ErrorCode == enums.statusResetWinningEnums.notReset) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 2);
                    //setTimeout(self.getResetWinningForSpecificDayAfterMovebetDay(), 10000);
                }
                else {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                }

                parent.jQuery.unblockUI();
            },
            error: function (rs) {
                console.log(rs);
            }
        });
    };

    self.getResetWinningSinceLastMovebetDay = function () {
        var params = self.getResetWinningParamsForSinceLastMoveBetDay();
        $.ajax({
            type: "POST",
            url: "GetResetWinningAfterMoveBetDayReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                self.renderResetWinningSinceLastMovebetDay(rs);
                parent.jQuery.unblockUI();
            },
            error: function () {
            }
        });
    };

    self.renderResetWinningSinceLastMovebetDay = function (rs) {
        self.resetHtml();
        var resetWinningHtml = [];
        if (rs.ReportRows.length > 0) {
            for (var i = 0; i < rs.ReportRows.length - 1; i++) {
                var resetWinningByDay = rs.ReportRows[i];
                resetWinningHtml.push(self.renderResetWinningByDay(resetWinningByDay, ''));
            }

            var lastDayResetWinning = rs.ReportRows[rs.ReportRows.length - 1];
            resetWinningHtml.push(self.renderResetWinningByDay(lastDayResetWinning, 'lastDayResetWinning'));

            $("#tblresetWinningSinceLastMoveBetDay tbody").empty().append(resetWinningHtml.join(''));
            $("#resetWinningSinceLastMoveBetDay").removeClass("hidden").reRenderScroller(660, 800, true);
        } else {
            var emptyDataHtml = '<tr><td colspan="5"><div class="align-left">There is no data.</div></td></tr>'
            $("#tblResetWinningForSpecificDay").addClass("col660");
            $("#tblResetWinningForSpecificDay").html(emptyDataHtml);
            $("#resetWinningForSpecificDay").removeClass("hidden").reRenderScroller(660, 'auto', true);
        }
    };

    self.renderResetWinningByDay = function (resetWinningByDay, tableClass) {
        var contentHtml = [];
        var cssClass = '';
        contentHtml.push(self.renderResetWinningByDayHeader(resetWinningByDay));
        for (var i = 0; i < resetWinningByDay.ReportRows.length; i++) {
            var currentRow = resetWinningByDay.ReportRows[i];
            if (i % 2 == 0) {
                cssClass = '';
            }
            else {
                cssClass = 'oddRow';
            }

            if (currentRow.Variance != 0) {
                cssClass = 'highlightRow';
            }

            if (currentRow.Product && currentRow.Product.toLowerCase().indexOf("due to") != -1) {
                cssClass = 'dueToPartnerRow';

                if (currentRow.Variance != 0) {
                    cssClass += ' dueToPartnerVariance';
                }
            }

            if (currentRow.Product == "OTHERS") {
                cssClass = 'bgOthers';

                if (currentRow.Variance != "N/A" && parseFloat(currentRow.Variance) != 0) {
                    cssClass += ' otherVariance';
                }
            }
            contentHtml.push(self.renderResetWinningByDayContent(currentRow, cssClass));
        }

        var resetTableClass = resetWinningByDay.WinLossDate.replace(/\//g, "");
        var totalHtmlRow = '<tr class="totalRow bold"><td><div class="col140 align-left">Total</div></td>\
                <td><div class="col120 align-right">' + resetWinningByDay.WinLoss_Str + '</div></td>\
                <td><div class="col140 align-right">' + resetWinningByDay.WinLossExtra_Str + '</div></td>\
                <td><div class="col120 align-right">' + resetWinningByDay.Reset_Str + '</div></td>\
                <td><div class="col120 align-right">' + resetWinningByDay.Variance_Str + '</div></td>\
            </tr>'
        contentHtml.push(totalHtmlRow);
        var resetWinningByDayContentHtml = contentHtml.join('');

        var resetWinningByDayHtml = '<tr><td class="none-border"><table class="resetWinningByDayTbl resetWinningByDay' + resetTableClass + ' ' + tableClass + '">\
        <caption class="dateTitle align-left">' + resetWinningByDay.WinLossDate + '</caption>' + resetWinningByDayContentHtml + '</table></td></tr>';

        return resetWinningByDayHtml;
    };

    self.renderResetWinningByDayHeader = function (resetWinningByDay) {
        var resetHtml = '';
        if (hasPermissionToReset) {
            resetHtml = '<input type="button" class="btn" value="Reset" name="btnReset" onclick="resetWinningByDay(\'' + resetWinningByDay.WinLossDate + '\')"/></div>';
        }
        else {
            resetHtml = '<div>Reset</div>';
        }

        var headerHtml =
        '<thead>\
            <tr>\
                <th style="border-bottom: none 0px;">\
                    <div class="col140 c">\
                    </div>\
                </th>\
                <th style="border-bottom: none 0px;">\
                    <div class="col120 c">\
                        Win/Loss</div>\
                </th>\
                <th style="border-bottom: none 0px;">\
                    <div class="col140 c">\
                        Win/Loss (After Extra PT)</div>\
                </th>\
                <th style="border-bottom: none 0px;">\
                    <div class="col120 c">' +
                            resetHtml +
                '</th>\
                <th style="border-bottom: none 0px;">\
                    <div class="col120 c">\
                        Variance</div>\
                </th>\
            </tr>\
        </thead>';

        return headerHtml;
    };

    self.renderResetWinningByDayContent = function (resetWinningByDayRow, cssClass) {
        var htmlRow = '';

        htmlRow += '<tr class="' + cssClass + '">\
            <td class="align-left col140 productTitle bold">' + resetWinningByDayRow.Product + '</td>\
            <td class="align-right col120">' + resetWinningByDayRow.WinLoss_Str + '</td>\
            <td class="align-right col140">' + resetWinningByDayRow.WinLossExtra_Str + '</td>\
            <td class="align-right col120">' + resetWinningByDayRow.Reset_Str + '</td>\
            <td class="align-right col120 colVariance">' + resetWinningByDayRow.Variance_Str + '</td>\
            </tr>';

        return htmlRow;
    };

    self.getResetWinningReport = function () {
        var timePeriod = $("#chkTimePeriod").val();
        parent.BlockUI();
        if (timePeriod == enums.timePeriodEnums.specificDay) {
            if (new Date(_tables.txtDate.val()) <= new Date(moveBetDay)) {
                self.getResetWinningBeforeMovebetDay();
            }
            else {
                self.getResetWinningForSpecificDayAfterMovebetDay();
            }
        }
        else {
            self.getResetWinningSinceLastMovebetDay();
        }
    };

    self.resetHtml = function () {
        $("#errMsg").addClass("hidden");
        $("#tblResetWinningForSpecificDay tbody").empty();
        $("#resetWinningForSpecificDay").addClass("hidden");
        $("#tblResetWinningForSpecificDayAfterMoveBetDay tbody").empty();
        $("#resetWinningForSpecificDayAfterMoveBetDay").addClass("hidden");
        $("#resetWinningSinceLastMoveBetDay").addClass("hidden");
        $("#tblResetWinningForSpecificDay").removeClass("col660");
    };

    resetWinningByDay = function (resetWinningDate) {
        $("#errMsg").addClass("hidden");
        resetWinningParams.resetWinningDate = resetWinningDate;
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "ResetWinningByDay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(resetWinningParams),
            success: function (rs) {
                $("#errMsg").removeClass("hidden");
                if (rs.ErrorCode == enums.statusResetWinningEnums.notPermission) {
                    showAlertMessage("errMsg", rs.ErrorMessage, 2);
                }
                else if (rs.ErrorCode == enums.statusResetWinningEnums.resetSuccessful) {
                    showAlertMessage("errMsg", "The process has been successful!", 0);

                    setTimeout(self.getResetWinningReport(), 2000);
                }
                else if (rs.ErrorCode == enums.statusResetWinningEnums.notReset) {
                    showAlertMessage("errMsg", "No products need to reset winning!", 2);
                }
                else {
                    showAlertMessage("errMsg", rs.ErrorMessage, 1);
                }

                parent.jQuery.unblockUI();
            },
            error: function (rs) {
                console.log(rs);
            }
        });
    };

    self.getResetWinningByDay = function (resetWinningDate) {
        var params = { resetWinningDate: resetWinningDate };
        cachedData.getResetWinningByDate = resetWinningDate;
        parent.BlockUI();
        $.ajax({
            type: "POST",
            url: "GetResetWinningByDay",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            success: function (rs) {
                self.renderResetWinningByDayAfterReset(rs);
                parent.jQuery.unblockUI();
            },
            error: function () {
            }
        });
    };

    self.resetWinningByDayHtml = function (rs) {
        var bodyTableHtml = [];
        var cssClass = '';
        for (var i = 0; i < rs.ReportRows.length; i++) {
            var currentRow = rs.ReportRows[i];

            if (i % 2 == 0) {
                cssClass = '';
            }
            else {
                cssClass = "oddRow";
            }

            if (currentRow.Variance != 0) {
                cssClass = 'highlightRow';
            }

            if (currentRow.Product && currentRow.Product.toLowerCase().indexOf("due to") != -1) {
                cssClass = 'dueToPartnerRow';

                if (currentRow.Variance != 0) {
                    cssClass += ' dueToPartnerVariance';
                }
            }

            if (currentRow.Product == "OTHERS") {
                cssClass = 'bgOthers';

                if (currentRow.Variance != "N/A" && parseFloat(currentRow.Variance) != 0) {
                    cssClass += ' otherVariance';
                }
            }

            bodyTableHtml.push(self.renderResetWinningForSpecificDayAfterMovebetDayRow(currentRow, cssClass));
        }

        var totalHtmlRow = '<tr class="totalRow bold"><td><div class="col140 align-left">Total</div></td>\
                <td><div class="col120 align-right">' + rs.WinLoss_Str + '</div></td>\
                <td><div class="col140 align-right">' + rs.WinLossExtra_Str + '</div></td>\
                <td><div class="col120 align-right">' + rs.Reset_Str + '</div></td>\
                <td><div class="col120 align-right">' + rs.Variance_Str + '</div></td>\
            </tr>'
        bodyTableHtml.push(totalHtmlRow);
        return bodyTableHtml.join('');
    };

    self.renderResetWinningByDayAfterReset = function (rs) {
        var bodyHtml = self.resetWinningByDayHtml(rs);

        var resetDate = cachedData.getResetWinningByDate.replace(/\//g, "");
        var tableResetWinningTableClass = ".resetWinningByDay" + resetDate;
        $(tableResetWinningTableClass).find("tbody").empty().html(bodyHtml);
    };

    self.compareDateTime = function (dateCompare, dateToCompare) {
        if (dateCompare.getDate() == dateToCompare.getDate() &&
        dateCompare.getMonth() == dateToCompare.getMonth() &&
        dateCompare.getYear() == dateToCompare.getYear()) {
            return true;
        }
        else {
            return false;
        }
    };
};

$(document).ready(function () {
    var resetWinning = new ResetWinning();
    resetWinning.InitPage();
});
