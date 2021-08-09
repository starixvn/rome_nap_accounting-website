(function ($) {
    "use strict";
    var drilldownByDateType = "DormantAccount/DrillDownByDateType";
    var getDormantByUsernameUrl = "DormantAccount/GetByUsername";
    var getDormantByDateTypeUrl = "DormantAccount/GetByDateType";
    var exportDrilldownByDateType = "DormantAccount/ExportExcelByDrilldown";
    var exportGetDormantByUsernameUrl = "DormantAccount/ExportExcelByUsername";
    var exportGetDormantByDateTypeUrl = "DormantAccount/ExportExcelByDateType";

    var $fromDate = $('#txtFromDate'),
        $toDate = $('#txtToDate'),
        $notAssign = $('#chkNotAssigned'),
        $radioMode = $('input[type=radio]'),
        $level = $("#ddlLevel"),
        $archiveStage = $("#ddlArchiveStage"),
        $archiveDateType = $("#ddlArchiveDateType"),
        $username = $("#txtUsername"),
        $headlineType = $("#ddlHeadlineType"),
        $headlineGroup = $("#txtHeadlineGroup"),
        $groupHeadline = $("#txtGroupHeadline"),
        $exportExcel = $("#btnExportExcel"),
        $grid = $("#GridByDateType"),
        $tblContentHeader = $("#tblContentHeader"),
        $tblContent = $("#tblContent"),
        $breadScrumb = $("#breadscrumb"),
        $sortByUsernameColumn = $("#divSortingByUsername"),
        $sortByArchiveStageColumn = $("#divSortingByStage"),
        $error = $("#err");

    var colNoWidth = 30,
        colNameWidth = 150,
        colUsernameWidth = 120,
        colMediumWidth = 80,
        colGroupHeadlineWidth = 150,
        colDateWidth = 80,
        colMoneyWidth = 100;

    var pageIndex = 1,
        pageSize = 100,
        totalItems = 0,
        accountList = [];

    var searchUsernameFromFilter = 0;

    var sortType = "-",
        sortClass = "asc";
    var warningMessage = "Login activity found in this line after Archive Close Date. If this line has any new bet, it will not be disabled";

    var gridOptions = {};
    var firstRequest = {};
    var lastRequest = {};
    var lastUrl = "";

    var index = 0;

    function changeModeFilter(value) {
        disableDateFilter(value != "date")
        $error.empty();

        function disableDateFilter(value) {
            if (value == true) {
                $("#ddlLevel").addClass("ui-multiselect-state-disabled");

            } else {
                $("#ddlLevel").removeClass("ui-multiselect-state-disabled");
            }
            $("#ddlLevel").find("input").prop("disabled", value);
            $archiveStage.prop("disabled", value);
            $archiveDateType.prop("disabled", value);
            $fromDate.prop("disabled", value);
            $toDate.prop("disabled", value);

            $username.prop('disabled', !value);
        }
    }

    function disableHeadlineFilter(value) {
        $headlineGroup.prop("disabled", value);
        $headlineType.prop("disabled", value);
    }

    function renderDateRangePicker() {
        $fromDate.datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: minDate,
            maxDate: maxDate,
            defaultDate: fromDate,
            onSelect: function (dateText, inst) {
                var newMaxDate = new Date(dateText);
                newMaxDate.setDate(newMaxDate.getDate() + 90);
                var maxCurrentDate = new Date(maxDate);
                newMaxDate = newMaxDate > maxCurrentDate ? maxCurrentDate : newMaxDate;

                var toDate = new Date($toDate.val());
                if (newMaxDate < toDate) {
                    $toDate.datepicker("option", "setDate", newMaxDate);
                }
                $toDate.datepicker("option", "maxDate", newMaxDate);
            }
        }).datepicker('setDate', fromDate);

        $toDate.datepicker({
            showOtherMonths: true,
            selectOtherMonths: true,
            minDate: minDate,
            maxDate: maxDate,
            defaultDate: toDate,
            onSelect: function (dateText, inst) {
                var newMinDate = new Date(dateText);
                newMinDate.setDate(newMinDate.getDate() - 90);
                var minCurrentDate = new Date(minDate);
                newMinDate = newMinDate < minCurrentDate ? minCurrentDate : newMinDate;

                var fromDate = new Date($fromDate.val());
                if (newMinDate > fromDate) {
                    $fromDate.datepicker("option", "setDate", newMinDate);
                }
                $fromDate.datepicker("option", "minDate", newMinDate);
            }
        }).datepicker('setDate', toDate);
    };

    function assignNoInfo() {
        showAlertMessage("err", "No information is available.", 1);
        clearDataResult();
    }

    function clearDataResult() {
        $exportExcel.addClass("hidden");
        $grid.addClass("hidden");
        $breadScrumb.addClass("hidden");
    }

    function checkValid() {
        var username = $username.val().toLowerCase();
        for (var i = 0; i < $radioMode.length; i++) {
            if ($radioMode[i].checked) {
                var value = $radioMode[i].value;
                if (value == "username") {
                    if (username == "" || username == null) {
                        showAlertMessage("err", "Username is required!.", 1);
                        return false;
                    }
                } else {
                    if ($("#ddlLevel").multiSelect("getCheckedItems").length == 0) {
                        showAlertMessage("err", "Please choose Level!", 1);
                        return false;
                    }
                }
            }
        }

        return true;
    }

    function getParams() {
        var notAssign = $notAssign.is(":checked");
        var headlineType = $headlineType.val();
        var headlineGroup = $headlineGroup.val().toLowerCase().trim();
        if (notAssign == true) {
            headlineType = 0;
            headlineGroup = "";
        }

        var username = $username.val().toLowerCase().trim();
        var level = $("#ddlLevel").multiSelect("getCheckedItems").join(",");
        var archiveStage = $archiveStage.val();
        var archiveDateType = $archiveDateType.val();
        var from = $fromDate.val();
        var to = $toDate.val();
        for (var i = 0; i < $radioMode.length; i++) {
            if ($radioMode[i].checked) {
                var value = $radioMode[i].value;
                if (value == "date") {
                    username = "";
                    searchUsernameFromFilter = 0;
                } else {
                    level = "";
                    archiveStage = 0;
                    archiveDateType = 0;
                    from = fromDate;
                    to = toDate;
                    searchUsernameFromFilter = 1;
                }
            }
        }

        return {
            SearchUsernameFromFilter: searchUsernameFromFilter,
            HeadlineGroup: headlineGroup,
            HeadlineType: headlineType,
            NotAssignGroupOnly: notAssign,
            RoleId: level,
            ArchiveStage: archiveStage,
            DateType: archiveDateType,
            FromDate: from,
            ToDate: to,
            Username: username,
            PageIndex: pageIndex,
            PageSize: pageSize
        };
    }

    function resetSorting() {
        $(".sorter").removeClass("asc desc");
        sortType = "";
    }

    function addSortingIconAfterRender(obj) {
        resetSorting();

        if (sortClass === "asc") {
            $(obj).addClass("asc");
        }
        else {
            $(obj).addClass("desc");
        }
    }

    function assignLevelDropDownList() {
        var level;
        var multiselectData = [];

        for (var i = 0, length = jsonLevel.length; i < length; i++) {
            level = jsonLevel[i];

            multiselectData.push({
                Value: level.Value,
                Text: level.Text,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }

        $level.multiSelect({
            minWidth: 125,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: "All",
            listData: multiselectData,
            onCheck: function () {
                if ($("#ddlLevel").multiSelect("getCheckedItems").length === 0) {
                    showAlertMessage("err", "Please choose Level!", 1);
                }
                else {
                    $error.empty();
                }
            },
            noItemText: "No Level found."
        });
    }

    function getDormantAccountList(formData, url) {
        resetSorting();
        $error.empty();

        $.ajax({
            type: "POST",
            url: window.rootUrl + url,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(formData),
            dataType: "json",
            beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
            success: function (rs) {
                if (!rs.HasData) {
                    assignNoInfo();
                    return;
                }
                lastRequest = rs.Request;
                lastUrl = url;

                searchUsernameFromFilter = rs.SearchUsernameFromFilter;
                var accountGroupBy = [];
                if (SearchByDateType(formData)) {
                    $(".date-type-mode").show();
                    gridOptions = initGridByDateTypeOptions();
                    accountList = rs.DataByDateType;
                    accountGroupBy = jlinq.from(accountList).group("HeadlineGroup");
                    $sortByUsernameColumn.html("Direct Account");
                    $sortByUsernameColumn.removeClass("sorter");
                    $sortByArchiveStageColumn.removeClass("sorter");
                } else {
                    $(".date-type-mode").hide();
                    gridOptions = initGridByUsernameOptions();
                    accountList = rs.DataByUsername;
                    accountGroupBy = jlinq.from(accountList).group("Username");
                    $sortByUsernameColumn.html("Username");
                    $sortByUsernameColumn.addClass("sorter");
                    $sortByArchiveStageColumn.addClass("sorter");
                }

                renderBreadScrumb(rs);
                renderDormantGrid(accountGroupBy);
                totalItems = rs.TotalItem;
                initPager(rs.TotalItem, pageSize, pageIndex - 1);

                $exportExcel.removeClass("hidden");
            },
            error: function () {
                assignNoInfo();
            },
            complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
        });

        function renderBreadScrumb(data) {
            if (data.HasBreadScrumb) {
                $breadScrumb.removeClass("hidden");
                var html = [];
                var length = data.BreadScrumb ? data.BreadScrumb.length : 0;
                for (var i = 0; i < length; i++) {
                    var item = data.BreadScrumb[i];
                    if (item.IsLink) {
                        var link = "<a class='jsdrillDown' href='#' data-username='" + item.Username + "' data-searchusernamefromfilter='" + searchUsernameFromFilter + "'>" + item.Text + "</a>";
                        if (item.Text == "Headlines") {
                            link = "<a class='jsSearchFilter' href='#'>" + item.Text + "</a>";
                        }

                        html.push(link);
                    } else {
                        html.push(item.Text);
                    }
                }

                $breadScrumb.html(html.join(' > '));
            } else {

                $breadScrumb.addClass("hidden");
            }
        }
    }

    function SearchByDateType(formData) {
        return formData.Username == "" || formData.Username == null;
    }

    function renderDormantGrid(accountGroupBy) {
        var arr = [];
        var arrFrozen = [];

        index = (pageSize * (pageIndex - 1) + 1);

        $.each(accountGroupBy, function (i, item) {
            arrFrozen.push(renderFrozenRows(item));
            arr.push(renderScrollRows(item));
        });

        assignGridContent(arrFrozen, arr);

        function renderFrozenRows(group) {
            var html = [];
            var totalItems = group ? group.length : 0;

            for (var i = 0; i < totalItems; i++) {
                var row = group[i];
                var summaryRow = row.RowNumber === 0 ? "closed-byUpline" : "";
                summaryRow = summaryRow == "" && row.IsWarningTicket ? "warning" : summaryRow;
                var tooltip = row.IsWarningTicket ? warningMessage : "";

                var rowHtml = '<tr class="' + summaryRow + '"> '
                if (row.RowNumber === 0) {
                    rowHtml += '<td></td>'
                } else {
                    rowHtml += '<td><div class="c" style="width: ' + colNoWidth + 'px; height: 20px" des="' + tooltip + '">' + index + '</div></td>';
                    index++;
                }

                if (i === 0 && row.HeadlineGroup) {
                    rowHtml += '<td rowspan="' + totalItems + '">\
                                    <div style="width: ' + colNameWidth + 'px; height: 20px" des="' + tooltip + '">' + row.HeadlineGroup + '</div>\
                            </td>\
                            <td rowspan="' + totalItems + '">\
                                <div class="c " style="width: ' + colMediumWidth + 'px; height: 20px" des="' + tooltip + '">' + row.HeadlineTypeText + '</div>\
                            </td>';
                }

                rowHtml += '<td><div style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">';
                if (row.RowNumber === 0 || row.RoleId == 1) {
                    rowHtml += row.Username;
                } else {
                    rowHtml += '<a class="jsdrillDown" href="#" data-username="' + row.Username + '" data-searchusernamefromfilter="' + searchUsernameFromFilter + '">' + row.Username + '</a>';
                }
                rowHtml += '</div></td></tr>';

                html.push(rowHtml);
            }

            return html.join("");
        }

        function renderScrollRows(group) {
            var html = [];
            var totalItems = group ? group.length : 0;

            var row, rowsSpan, rowHtml;

            for (var i = 0; i < totalItems; i++) {
                row = group[i];
                var summaryRow = row.RowNumber === 0 ? "closed-byUpline" : "";
                summaryRow = summaryRow == "" && row.IsWarningTicket ? "warning" : summaryRow;
                var tooltip = row.IsWarningTicket ? warningMessage : "";
                var rowHtml = '<tr class="' + summaryRow + '"><td> <div class="c" style="width: ' + colNoWidth + 'px; height: 20px"';

                rowHtml += '></div> </td>';

                if (row.HeadlineGroup) {
                    rowHtml += '<td> <div style="width: ' + colNameWidth + 'px; height: 20px">' + row.HeadlineGroup + '</div>\</td>' +
                        '<td> <div class="c " style="width: ' + colMediumWidth + 'px; height: 20px">' + row.HeadlineTypeText + '</div>\</td>';
                }

                rowHtml += '<td>\
                                    <div style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.Username + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colMediumWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.RoleText + '</div>\
                                </td>\
                                <td>';
                var status = row.StatusText;
                if (row.IsWarningTicket) {
                    status += '<span class="asterisk">*</span>';
                }

                rowHtml += '<div class="c" style="width: ' + colMediumWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + status + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: 120px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.StageText + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: ' + colMoneyWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.ToBeClearedGivenCreditText + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: ' + colMoneyWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.ClearedGivenCreditText + '</div>\
                                </td>\
                                <td>\
                                    <div class="r" style="width: ' + colMoneyWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.YesterdayBalanceText + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colDateWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.LastLoggedText + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colDateWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.ClosedDateText + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colDateWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.DisabledDateText + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colDateWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.SettingDeletedDateText + '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colDateWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">' + row.AccountDeletedDateText + '</div>\
                                </td>';

                if (row.HeadlineGroup) {
                    rowHtml += '<td>\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">'
                    if (row.ArchivedMaster.Close == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMaster.Close + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="3" data-stageid="1" data-username="' + row.Username + '">' + row.ArchivedMaster.Close + '</a>'
                    }

                    if (row.ArchivedMaster.Disable == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMaster.Disable + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="3" data-stageid="2" data-username="' + row.Username + '">' + row.ArchivedMaster.Disable + '</a>'
                    }
                    if (row.ArchivedMaster.DelSetting == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMaster.DelSetting + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="3" data-stageid="3" data-username="' + row.Username + '">' + row.ArchivedMaster.DelSetting + '</a>'
                    }

                    if (row.ArchivedMaster.DelAccount == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMaster.DelAccount + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="3" data-stageid="4" data-username="' + row.Username + '">' + row.ArchivedMaster.DelAccount + '</a>'
                    }

                    rowHtml += '</div>\
                                </td>\
                                <td>\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">'
                    if (row.ArchivedAgent.Close == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedAgent.Close + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="2" data-stageid="1" data-username="' + row.Username + '">' + row.ArchivedAgent.Close + '</a>'
                    }

                    if (row.ArchivedAgent.Disable == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedAgent.Disable + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="2" data-stageid="2" data-username="' + row.Username + '">' + row.ArchivedAgent.Disable + '</a>'
                    }
                    if (row.ArchivedAgent.DelSetting == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedAgent.DelSetting + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="2" data-stageid="3" data-username="' + row.Username + '">' + row.ArchivedAgent.DelSetting + '</a>'
                    }

                    if (row.ArchivedAgent.DelAccount == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedAgent.DelAccount + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="2" data-stageid="4" data-username="' + row.Username + '">' + row.ArchivedAgent.DelAccount + '</a>'
                    }

                    rowHtml += '</div>\
                                </td>\
                                <td>\
                                     <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px" des="' + tooltip + '">'
                    if (row.ArchivedMember.Close == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMember.Close + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="1" data-stageid="1" data-username="' + row.Username + '">' + row.ArchivedMember.Close + '</a>'
                    }

                    if (row.ArchivedMember.Disable == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMember.Disable + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="1" data-stageid="2" data-username="' + row.Username + '">' + row.ArchivedMember.Disable + '</a>'
                    }
                    if (row.ArchivedMember.DelSetting == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMember.DelSetting + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="1" data-stageid="3" data-username="' + row.Username + '">' + row.ArchivedMember.DelSetting + '</a>'
                    }

                    if (row.ArchivedMember.DelAccount == 0) {
                        rowHtml += '<span class="drilldownStage">' + row.ArchivedMember.DelAccount + '</span>'
                    } else {
                        rowHtml += '<a href="#" class="drilldownStage jsdrilldownStage" data-roleid="1" data-stageid="4" data-username="' + row.Username + '">' + row.ArchivedMember.DelAccount + '</a>'
                    }

                    rowHtml += '</div></td>';
                }

                rowHtml += '</tr>';

                html.push(rowHtml);
            }

            return html.join("");
        }

        function assignGridContent(arrFrozen, arr) {
            var htmlScrollRows = arr.join("");
            var htmlFrozenlRows = arrFrozen.join("");

            if ($.browser.msie && parseInt($.browser.version) == 9) {
                htmlScrollRows = replaceSpecialCharactersOnIE9(htmlScrollRows);
                htmlFrozenlRows = replaceSpecialCharactersOnIE9(htmlFrozenlRows);
            }

            $tblContentHeader.find("tbody").empty().append(htmlFrozenlRows);
            $tblContent.find("tbody").empty().append(htmlScrollRows);
            $grid.removeClass("hidden").gridScroller(gridOptions);

            bindRowEvents();

            $(".tipsy").remove();
            $tblContentHeader.find("tbody tr.warning td").each(function () {
                bindTooltip($(this));
            });

            $tblContent.find("tbody tr.warning td").each(function () {
                bindTooltip($(this));
            });

            function bindTooltip($this) {
                var des = $this.find("div").attr("des");
                $this.tipsy({
                    fade: false, gravity: "sw", html: false, title: function () {
                        return des;
                    }, opacity: 1
                });
            }

            function replaceSpecialCharactersOnIE9(text) {
                var expr = new RegExp(">[ \t\r\n\v\f]*<", "g");
                var html = text.replace(expr, "><");

                return html;
            }
        }
    }

    function drillDownByUsername(username) {
        pageIndex = 1;
        var formData = {
            SearchUsernameFromFilter: searchUsernameFromFilter,
            Username: username,
            HeadlineGroup: firstRequest.HeadlineGroup,
            HeadlineType: firstRequest.HeadlineType,
            NotAssignGroupOnly: firstRequest.NotAssignGroupOnly,
            pageIndex: pageIndex,
            pageSize: pageSize
        };
        var url = getDormantByUsernameUrl;
        if (SearchByDateType(firstRequest)) {
            formData = {
                SearchUsernameFromFilter: searchUsernameFromFilter,
                Username: username,
                HeadlineGroup: firstRequest.HeadlineGroup,
                HeadlineType: firstRequest.HeadlineType,
                NotAssignGroupOnly: firstRequest.NotAssignGroupOnly,
                DateType: firstRequest.DateType,
                FromDate: firstRequest.FromDate,
                ToDate: firstRequest.ToDate,
                pageIndex: pageIndex,
                pageSize: pageSize
            };
            url = drilldownByDateType;
        }

        getDormantAccountList(formData, url);
    }

    function drillDownByStage(username, roleId, stageId) {
        var from = firstRequest.FromDate;
        var to = firstRequest.ToDate;
        var archiveDateType = firstRequest.DateType;
        pageIndex = 1;

        var formData = {
            SearchFromArchiveRoleColumn: true,
            HeadlineGroup: firstRequest.HeadlineGroup,
            HeadlineType: firstRequest.HeadlineType,
            NotAssignGroupOnly: firstRequest.NotAssignGroupOnly,
            Username: username,
            RoleId: roleId,
            ArchiveStage: stageId,
            DateType: archiveDateType,
            FromDate: from,
            ToDate: to,
            pageIndex: pageIndex,
            pageSize: pageSize
        };

        getDormantAccountList(formData, drilldownByDateType);
    }

    function initGridByDateTypeOptions() {
        var frozenWidth = (colNoWidth + colNameWidth + colMediumWidth + colUsernameWidth) + (4 * 4);

        return {
            tblContentId: "tblContent",
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 170,
            freezeWidth: frozenWidth,
            numberFreezeCol: 4,
            pagerOpts: {
                callback: pageSelectCallback,
                start_per_page: 100,
                step_per_page: 100,
                max_items_per_page: 1000,
                items_per_page: 100,
                current_page: 0
            }
        };
    }

    function initGridByUsernameOptions() {
        var frozenWidth = (colNoWidth + colUsernameWidth) + (3 * 2);

        return {
            tblContentId: "tblContent",
            manualCopyHeader: true,
            isShowPager: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 170,
            freezeWidth: frozenWidth,
            numberFreezeCol: 2,
            pagerOpts: {
                callback: pageSelectCallback,
                start_per_page: 100,
                step_per_page: 100,
                max_items_per_page: 1000,
                items_per_page: 100,
                current_page: 0
            }
        };
    }

    function sorting(obj, sortCol) {
        if (sortClass === "" || sortClass === "desc") {
            sortClass = "asc";
            sortType = "";
        }
        else {
            sortClass = "desc";
            sortType = "-";
        }

        var accountGroupBy = [];
        var dormantList = [];
        if (sortCol === "HeadlineGroup") {
            var notAssign = jlinq.from(accountList).equals('CustHeadlineId', 0).select();
            var assign = jlinq.from(accountList).greater("CustHeadlineId", 0).sort(sortType + "HeadlineGroup")
                .select();

            if (assign && assign.length > 0) {
                dormantList = assign;
            }

            if (notAssign && notAssign.length > 0) {
                dormantList = dormantList.concat(notAssign);
            }

            accountGroupBy = jlinq.from(dormantList).group("HeadlineGroup");
        } else if (sortCol == 'Stage') {
            var summaryRow = jlinq.from(accountList).equals("RowNumber", 0).select();
            var normalRow = jlinq.from(accountList).greater("RowNumber", 0)
                .sort(sortType + "StageId")
                .select();
            if (summaryRow && summaryRow.length > 0) {
                dormantList = summaryRow;
            }

            if (normalRow && normalRow.length > 0) {
                dormantList = dormantList.concat(normalRow);
            }

            accountGroupBy = jlinq.from(dormantList).group("Username");
        } else {
            var summaryRow = jlinq.from(accountList).equals("RowNumber", 0).select();
            var normalRow = jlinq.from(accountList).greater("RowNumber", 0)
                .sort(sortType + "Username")
                .select();
            if (summaryRow && summaryRow.length > 0) {
                dormantList = summaryRow;
            }

            if (normalRow && normalRow.length > 0) {
                dormantList = dormantList.concat(normalRow);
            }

            accountGroupBy = jlinq.from(dormantList).group("Username");
        }

        renderDormantGrid(accountGroupBy);
        addSortingIconAfterRender(obj);
    }

    function bindRowEvents() {
        $(".jsdrillDown")
            .unbind("click")
            .bind("click", function () {
                var $this = $(this);
                var userName = $this.attr("data-username");
                drillDownByUsername(userName);
            });
        $(".jsSearchFilter")
            .unbind("click")
            .bind("click", function () {
                pageIndex = 1;
                firstRequest.PageSize = pageSize;
                var url = SearchByDateType(firstRequest) ? getDormantByDateTypeUrl : getDormantByUsernameUrl;
                getDormantAccountList(firstRequest, url);
            });
        $(".jsdrilldownStage")
            .unbind("click")
            .bind("click", function () {
                var $this = $(this);
                var roleid = $this.attr("data-roleid");
                var stageid = $this.attr("data-stageid");
                var username = $this.attr("data-username");
                drillDownByStage(username, roleid, stageid);
            });
    }

    function initPager(total_items, items_per_page, current_page) {
        var pagerOpts = gridOptions.pagerOpts;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, pagerOpts);
    }

    function pageSelectCallback(page_index, _items_per_page, jq) {
        var max_page = Math.ceil(totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        pageIndex = page_index + 1;
        pageSize = _items_per_page;

        lastRequest.PageIndex = pageIndex;
        lastRequest.PageSize = pageSize;
        lastRequest.FromDate = lastRequest.FromDateText;
        lastRequest.ToDate = lastRequest.ToDateText;
        getDormantAccountList(lastRequest, lastUrl);

        return false;
    }

    function bindTooltipHeader($this, html) {
        $this.tipsy({
            fade: false, gravity: "se", html: true, title: function () {
                return html;
            }, opacity: 1
        });
    }

    function initialize() {
        assignLevelDropDownList();
        renderDateRangePicker();
        $radioMode.change(function () { changeModeFilter(this.value); });
        $notAssign.click(function () {
            disableHeadlineFilter(this.checked);
        });

        $(".note-info-message").each(function () {
            var $this = $(this);
            var levelName = $this.attr("data-level");
            var html = $("#NoteLevelDrillDown").html().replace("{level}", levelName);
            bindTooltipHeader($this, html);
        });

        $("#btnSubmit").click(function () {
            clearDataResult();
            pageIndex = 1;
            var isValidData = checkValid();
            if (isValidData) {
                var formData = getParams();
                firstRequest = formData;
                var url = SearchByDateType(formData) ? getDormantByDateTypeUrl : getDormantByUsernameUrl;
                getDormantAccountList(formData, url);
            }
        });
        gridOptions = initGridByDateTypeOptions();
        $grid.initGrid(gridOptions);
        $("#divSortingByGroupHeadline").click(function () {
            sorting(this, "HeadlineGroup");
        });
        $("#divSortingByStage").click(function () {
            var $this = $(this);
            if ($this.hasClass("sorter")) {
                sorting(this, "Stage");
            }
        });
        $sortByUsernameColumn.click(function () {
            sorting(this, "Username");
        });

        $exportExcel.click(function () {
            var request = lastRequest;
            request.FromDate = lastRequest.FromDateText;
            request.ToDate = lastRequest.ToDateText;
            request.PageSize = totalItems;
            request.PageIndex = 1;

            var url = "";
            if (lastUrl == getDormantByDateTypeUrl) {
                url = exportGetDormantByDateTypeUrl;
            } else if (lastUrl == getDormantByUsernameUrl) {
                url = exportGetDormantByUsernameUrl;
            } else {
                url = exportDrilldownByDateType;
            }

            url = window.rootUrl + url + "?" + $.param(lastRequest);

            $("#frameExportExcel").attr("src", url);
        });

    }

    initialize();
})(jQuery);
