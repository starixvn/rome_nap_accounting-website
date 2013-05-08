$(document).ready(function () {
    smaManagement.init();
});

var smaManagement = {
    sites: '',
    status: '',
    colNoWidth: 30,
    colGroupHeadlineWidth: 150,
    colUsernameWidth: 120,
    colStatusWidth: 120,
    colSiteWidth: 100,
    colCurrencyWidth: 60,
    colPositionWidth: 80,
    pageIndex: 1,
    pageSize: 100,
    totalItems: 0,
    currentSMAList: [],
    sortType: '',
    sortClass: '',
    sortCol: '',
    pagerOpts: {
        callback: null,
        start_per_page: 100,
        step_per_page: 100,
        max_items_per_page: 1000,
        items_per_page: 100,
        current_page: 1
    },
    init: function () {
        //#region assign currencies, sites
        smaManagement.selectFilterInfo();
        //#endregion assign currencies, sites

        //#region assign status
        smaManagement.assignStatus();
        //#endregion assign status

        //#region init grid
        var gridOpts = {
            tblContentId: 'tblContent',
            isShowPager: true
        };
        $("#SMAMangementGrid").initGrid(gridOpts);
        //#endregion init grid

        //#region assign event for buttons
        $('#btnSubmit').click(function () {
            smaManagement.getSMAListByGroupHeadline();
        });
        $('#divSortingByGroupHeadline').on('click', function () { smaManagement.sorting(this, 'GroupName'); }); //sorting - GroupHeadline
        $('#divSortingByUsername').on('click', function () { smaManagement.sorting(this, 'Username'); }); //sorting - Username
        $('#btnExportExcel').click(function () {
            smaManagement.exportExcel();
        });
        //#endregion event for buttons
    },
    selectFilterInfo: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SMAManagement/GetAllFilterInfo",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: null,
            success: function (rs) {
                if (rs) {
                    smaManagement.assignCurrencies(rs);
                    smaManagement.assignSites(rs);
                }
            }
        });
    },
    assignCurrencies: function (rs) {
        $('#ddlCurrency').empty();
        $('#ddlCurrency').append('<option value="0">All</option>');
        if (rs.Currencies && rs.Currencies.length > 0) {
            var totalCurrencies = rs.Currencies.length;
            for (var i = 0; i < totalCurrencies; i++) {
                var currency = rs.Currencies[i];
                //not assign test currencies: 20-uus; 27-h; 28-c
                if (currency.Id == 20 | currency.Id == 27 | currency.Id == 28)
                    continue;
                $('#ddlCurrency').append('<option value="' + currency.Id + '">' + currency.Name + '</option>');
            }
        }
    },
    assignSites: function (rs) {
        if (rs.Sites && rs.Sites.length > 0) {
            var totalSites = rs.Sites.length;
            var tempData = [];
            var id = "selectSite";
            for (var i = 0; i < totalSites; i++) {
                var detail = rs.Sites[i];
                tempData.push({
                    Id: id + detail.Id.toString(),
                    Value: detail.Id,
                    Text: detail.Name,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: []
                });
            }
            $("#ddlSites").multiSelect(
                {
                    minWidth: 110,
                    maxHeight: 450,
                    shadow: true,
                    showHeader: true,
                    isCheckAll: true,
                    showFooter: false,
                    checkAllText: 'All',
                    listData: tempData,
                    onCheck: function () {
                        if ($('#ddlSites').multiSelect('getCheckedItems').length <= 0) {
                            showAlertMessage("err", "Please choose Site!", 1);
                        }
                        else {
                            $('#err').empty();
                        }
                    },
                    noItemText: 'No site found.'
                });
        }
        else {
            $("#ddlSites").multiSelect({
                minWidth: 110,
                maxHeight: 450,
                shadow: true,
                showHeader: true,
                isCheckAll: true,
                showFooter: false,
                listData: [],
                noItemText: 'No site found.'
            });
        }
    },
    assignStatus: function () {
        var status = [
            { Id: 'isOpen', Name: 'Open' },
            { Id: 'isSusupended', Name: 'Suspended' },
            { Id: 'isClosed', Name: 'Closed' },
            { Id: 'isDisabled', Name: 'Disabled' }
        ];

        var totalStatus = status.length;
        var tempData = [];
        var id = "ddlStatus";
        for (var i = 0; i < totalStatus; i++) {
            var detail = status[i];
            tempData.push({
                Id: id + detail.Id,
                Value: detail.Id,
                Text: detail.Name,
                Checked: true,
                Enable: true,
                CssClass: "",
                Items: []
            });
        }
        $("#ddlStatus").multiSelect(
        {
            minWidth: 100,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            checkAllText: 'All',
            listData: tempData,
            onCheck: function () {
                if ($('#ddlStatus').multiSelect('getCheckedItems').length <= 0) {
                    showAlertMessage("err", "Please choose Status!", 1);
                }
                else {
                    $('#err').empty();
                }
            },
            noItemText: 'No status found.'
        });
    },
    getParams: function () {
        var username = $('#txtUsername').val().toLowerCase() == 'all' ? '' : $('#txtUsername').val();
        var groupHeadline = $('#txtGroupHeadline').val().toLowerCase() == 'none' ? '' : $('#txtGroupHeadline').val();

        var p = {
            siteId: smaManagement.getSitesSelected(),
            currency: $('#ddlCurrency').val(),
            isOpen: $('#ddlStatusisOpen').prop('checked'),
            isSusupended: $('#ddlStatusisSusupended').prop('checked'),
            isClosed: $('#ddlStatusisClosed').prop('checked'),
            isDisabled: $('#ddlStatusisDisabled').prop('checked'),
            roleId: $('#ddlPosition').val(),
            username: username,
            groupHeadline: groupHeadline,
            pageIndex: smaManagement.pageIndex,
            pageSize: smaManagement.pageSize
        };
        return p;
    },
    getSMAListByGroupHeadline: function () {
        if ($('#ddlSites').multiSelect('getCheckedItems').length <= 0) {
            showAlertMessage("err", "Please choose Site!", 1);
            return false;
        }
        if ($('#ddlStatus').multiSelect('getCheckedItems').length <= 0) {
            showAlertMessage("err", "Please choose Status!", 1);
            return false;
        }

        $('#err').empty();
        smaManagement.pageIndex = 1;
        var p = smaManagement.getParams();
        smaManagement.resetSorting();
        try { parent.BlockUI() } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SMAManagement/GetSMAListByGroupHeadline",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs && rs.SMAGroupsInfo && rs.SMAGroupsInfo.length > 0) {
                    smaManagement.currentSMAList = rs.SMAGroupsInfo;
                    smaManagement.totalItems = rs.TotalGroups;
                    var membersNotAssignedToGroup = [];

                    var arr = [];
                    var arrFrozen = [];
                    var memberNotAssignedToGroupHtml = '';
                    var frozenMemberNotAssignedToGroupHtml = '';
                    var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);
                    var notAssignedGroupIndex = index;
                    var totalSMAInCurrentPage = rs.SMAGroupsInfo.length;
                    for (var i = 0; i < totalSMAInCurrentPage; i++) {
                        var groupInfo = rs.SMAGroupsInfo[i];
                        if (groupInfo.GroupId == 0) {
                            membersNotAssignedToGroup.push(groupInfo);
                        }
                        else {
                            arr.push(smaManagement.renderScrollRows(groupInfo));
                            arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index));
                        }
                        index++
                    }

                    memberNotAssignedToGroupHtml = smaManagement.renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
                    frozenMemberNotAssignedToGroupHtml = smaManagement.renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, (notAssignedGroupIndex));
                    smaManagement.assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
                    smaManagement.initPager(smaManagement.totalItems, smaManagement.pageSize, smaManagement.pageIndex - 1);

                    $('#btnExportExcel').removeClass('hidden');
                }
                else {
                    smaManagement.assignNoInfo();
                }
            },
            error: function () {
                smaManagement.assignNoInfo();
            },
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });
    },
    getSMAListByGroupHeadlinePaging: function () {
        var p = smaManagement.getParams();
        smaManagement.resetSorting();
        $.ajax({
            type: "POST",
            url: window.rootUrl + "SMAManagement/GetSMAListByGroupHeadlineFromCahe",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs && rs.SMAGroupsInfo && rs.SMAGroupsInfo.length > 0) {
                    smaManagement.currentSMAList = rs.SMAGroupsInfo;
                    smaManagement.totalItems = rs.TotalGroups;
                    var membersNotAssignedToGroup = [];
                    var arr = [];
                    var arrFrozen = [];
                    var memberNotAssignedToGroupHtml = '';
                    var frozenMemberNotAssignedToGroupHtml = '';
                    var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);
                    var notAssignedGroupIndex = index;
                    var totalSMAInCurrentPage = rs.SMAGroupsInfo.length;
                    for (var i = 0; i < totalSMAInCurrentPage; i++) {
                        var groupInfo = rs.SMAGroupsInfo[i];
                        if (groupInfo.GroupId == 0) {
                            membersNotAssignedToGroup.push(groupInfo);
                        }
                        else {
                            arr.push(smaManagement.renderScrollRows(groupInfo));
                            arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index));
                        }
                        index++
                    }

                    memberNotAssignedToGroupHtml = smaManagement.renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
                    frozenMemberNotAssignedToGroupHtml = smaManagement.renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, (notAssignedGroupIndex));

                    smaManagement.assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
                    smaManagement.initPager(smaManagement.totalItems, smaManagement.pageSize, smaManagement.pageIndex - 1);
                    $('#btnExportExcel').removeClass('hidden');
                }
                else {
                    smaManagement.assignNoInfo();
                }
            },
            error: function () {
                smaManagement.assignNoInfo();
            },
            complete: function () { try { parent.jQuery.unblockUI() } catch (e) { }; }
        });
    },
    initPager: function (total_items, items_per_page, current_page) {
        smaManagement.pagerOpts.callback = smaManagement.pageSelectCallback;
        smaManagement.pagerOpts.items_per_page = items_per_page;
        smaManagement.pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, smaManagement.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(smaManagement.totalItems / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        smaManagement.pageIndex = page_index + 1;
        smaManagement.pageSize = _items_per_page;
        smaManagement.getSMAListByGroupHeadlinePaging();

        return false;
    },
    getSitesSelected: function () {
        if ($('#ddlSitesHeaderCheckBox').attr('checked'))
            return '';
        var listChecked = $('#ddlSites').next('div.ui-multiselect-options').find('label:not(.ui-multiselect-state-disabled,.ui-multiselect-group-mark)').find('input:not(#' + $('#ddlSites').attr('id') + "HeaderCheckBox)").filter('[checked]');
        var tempArray = [];
        for (var i = 0; i < listChecked.length; i++) {
            tempArray.push(listChecked[i].value);
        }
        return tempArray.join(',');
    },
    assignNoInfo: function () {
        showAlertMessage("err", "No information is available.", 1);
        $('#btnExportExcel').addClass('hidden');
        $('#SMAMangementGrid').addClass('hidden');
    },
    renderFrozenRows: function (smaGroup, groupIndex) {
        var html = [];
        if (smaGroup && smaGroup.SMAList.length > 0) {
            var totalItems = smaGroup.SMAList.length;
            for (var i = 0; i < totalItems; i++) {
                var row = smaGroup.SMAList[i];
                var css = smaManagement.getCssFormatForEachRow(row.Status);
                var rowsSpan = '';
                if (i == 0) {
                    rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 17px">' + groupIndex + '</div>\
                                </td>\
                                <td rowspan="' + totalItems + '" class="group">\
                                    <div class="c " style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 17px" des="Group Headline: ' + smaGroup.GroupName + '">' + smaGroup.GroupName + '</div>\
                                </td>';
                }

                var rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 17px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

                html.push(rowHtml);
            }
        }
        return html.join('');
    },
    renderScrollRows: function (smaGroup) {
        var html = [];
        if (smaGroup && smaGroup.SMAList.length > 0) {
            var totalItems = smaGroup.SMAList.length;
            for (var i = 0; i < totalItems; i++) {
                var row = smaGroup.SMAList[i];
                var rowsSpan = '';
                var css = smaManagement.getCssFormatForEachRow(row.Status);
                if (i == 0) {
                    var height = totalItems * 17;
                    rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: ' + height + 'px">' + smaGroup.GroupName + '</div>\
                                </td>';
                }

                var rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 17px">' + row.Username + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colStatusWidth + 'px; height: 17px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colSiteWidth + 'px; height: 17px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colCurrencyWidth + 'px; height: 17px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colPositionWidth + 'px; height: 17px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

                html.push(rowHtml);
            }
        }
        return html.join('');
    },
    renderFrozenForMemberNotAssignedToGroup: function (smaGroup, groupIndex) {
        var html = [];
        if (smaGroup && smaGroup.length > 0) {
            var i = 0;
            var totalItems = smaGroup.length;
            $.each(smaGroup, function () {
                if (this.SMAList != null && this.SMAList.length > 0) {
                    var row = this.SMAList[0];
                    var rowsSpan = '';
                    var css = smaManagement.getCssFormatForEachRow(row.Status);
                    if (i == 0) {
                        rowsSpan = '<td class="group" rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 17px" des="Not assigned to group">Not assigned to group</div>\
                                </td>';
                    }

                    var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 17px">' + groupIndex + '</div>\
                                </td>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 17px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

                    html.push(rowHtml);
                    groupIndex++;
                    i++;
                }
            });
        }
        return html.join('');
    },
    renderScrollRowsForMemberNotAssignedToGroup: function (smaGroup) {
        var html = [];
        if (smaGroup && smaGroup.length > 0) {
            var i = 0;
            var totalItems = smaGroup.length;
            $.each(smaGroup, function () {
                if (this.SMAList != null && this.SMAList.length > 0) {
                    var row = this.SMAList[0];
                    var rowsSpan = '';
                    var css = smaManagement.getCssFormatForEachRow(row.Status);
                    if (i == 0) {
                        rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 17px">&nbsp;</div>\
                                </td>';
                    }

                    var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 17px">&nbsp;</div>\
                                </td>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 17px">&nbsp;</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colStatusWidth + 'px; height: 17px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colSiteWidth + 'px; height: 17px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colCurrencyWidth + 'px; height: 17px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colPositionWidth + 'px; height: 17px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

                    html.push(rowHtml);
                    i++;
                }
            });
        }
        return html.join('');
    },
    getCssFormatForEachRow: function (status) {
        //The top priority will be Disabled - Closed - Suspended - Open.
        var css = 'open';
        status = status.toLowerCase();
        if (status.indexOf('disabled') != -1) {
            css = 'disabled';
        }
        else if (status.indexOf('closed') != -1) {
            css = 'closed';
        }
        else if (status.indexOf('suspended') != -1) {
            css = 'suspended';
        }

        return css;
    },
    sorting: function (obj, sortCol) {
        if (smaManagement.sortClass == '') {
            smaManagement.sortClass = 'asc';
            smaManagement.sortType = '';
        }
        if (smaManagement.sortClass == 'desc') {
            smaManagement.sortClass = 'asc';
            smaManagement.sortType = '';
        }
        else {
            smaManagement.sortClass = 'desc';
            smaManagement.sortType = '-';
        }
        smaManagement.sortCol = sortCol;

        //#region sorting and re-render grid
        var smaListSorted = [];
        if (sortCol == 'GroupName') {
            var allMembersNotAssignedToGroup = jlinq.from(smaManagement.currentSMAList).equals('GroupId', 0).select();
            var allMembersHasAssigned = jlinq.from(smaManagement.currentSMAList).greater('GroupId', 0).select();
            allMembersHasAssigned = jlinq.from(allMembersHasAssigned).sort(smaManagement.sortType + "GroupName").select();

            if (allMembersNotAssignedToGroup && allMembersNotAssignedToGroup.length > 0) {
                smaListSorted = smaListSorted.concat(allMembersNotAssignedToGroup);
            }
            if (allMembersHasAssigned && allMembersHasAssigned.length > 0) {
                smaListSorted = smaListSorted.concat(allMembersHasAssigned);
            }

        }
        else if (sortCol == 'Username') {
            var newGroup = [];
            var memberNotAssignedToGroup = [];

            $.each(smaManagement.currentSMAList, function () {
                if (this.GroupId == 0 && this.SMAList && this.SMAList.length > 0) {
                    memberNotAssignedToGroup.push(this.SMAList[0]);
                }
                else {

                    var smaList = jlinq.from(this.SMAList).sort(smaManagement.sortType + 'Username').select();
                    var smaListInfo = {
                        GroupId: this.GroupId,
                        GroupName: this.GroupName,
                        SMAList: smaList
                    }
                    newGroup.push(smaListInfo);
                }
            });

            if (memberNotAssignedToGroup && memberNotAssignedToGroup.length > 0) {
                var sortingMembers = jlinq.from(memberNotAssignedToGroup).sort(smaManagement.sortType + 'Username').select();
                $.each(sortingMembers, function () {
                    var smaListInfo = {
                        GroupId: 0,
                        GroupName: 'Not assigned to group',
                        SMAList: [this]
                    }
                    smaListSorted = smaListSorted.concat(smaListInfo);
                });
            }
            if (newGroup && newGroup.length > 0) {
                smaListSorted = smaListSorted.concat(newGroup);
            }
        }

        smaManagement.currentSMAList = smaListSorted;
        var membersNotAssignedToGroup = [];
        var arr = [];
        var arrFrozen = [];
        var memberNotAssignedToGroupHtml = '';
        var frozenMemberNotAssignedToGroupHtml = '';
        var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);
        var notAssignedGroupIndex = index;
        var totalSMAInCurrentPage = smaListSorted.length;
        for (var i = 0; i < totalSMAInCurrentPage; i++) {
            var groupInfo = smaListSorted[i];
            if (groupInfo.GroupId == 0) {
                membersNotAssignedToGroup.push(groupInfo);
            }
            else {
                arr.push(smaManagement.renderScrollRows(groupInfo));
                arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index));
            }
            index++;
        }

        memberNotAssignedToGroupHtml = smaManagement.renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
        frozenMemberNotAssignedToGroupHtml = smaManagement.renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, (notAssignedGroupIndex));

        smaManagement.assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
        smaManagement.addSortingIconAfterRender(obj);
        //#endregion sorting and re-render grid
    },
    addSortingIconAfterRender: function (obj) {
        smaManagement.resetSorting();
        if (smaManagement.sortClass == 'asc') {
            $(obj).addClass("asc");
        }
        else {
            $(obj).addClass("desc");
        }
    },
    exportExcel: function () {
        var p = smaManagement.getParams();
        var url = linkExportExcel + '?siteId=' + p.siteId + '&currency=' + p.currency +
        '&isOpen=' + p.isOpen + '&isSusupended=' + p.isSusupended + '&isClosed=' + p.isClosed + '&isDisabled=' + p.isDisabled +
        '&roleId=' + p.roleId + '&username=' + p.username + '&groupHeadline=' + p.groupHeadline;
        $('#frameExportExcel').attr('src', url);
    },
    resetSorting: function () {
        $('.sorter').removeClass('asc').removeClass('desc');
        smaManagement.sortType = '';
    },
    replaceSpecialCharactersOnIE9: function (text) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        var html = text.replace(expr, '><');
        return html;
    },
    showDescription: function (obj, content) {
        obj.tipsy({ fade: false, gravity: "sw", html: false, title: function () { return content }, opacity: 1 });
    },
    assignGridContent: function (memberNotAssgnedToGroupFrozenHtml, memberNotAssgnedToGroupScrollHtml, arrFrozen, arr) {
        $(".tipsy").remove();
        var htmlScrollRows = arr.join('');
        var htmlFrozenlRows = arrFrozen.join('');
        if ($.browser.msie && parseInt($.browser.version) == 9) {
            memberNotAssgnedToGroupFrozenHtml = smaManagement.replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupFrozenHtml);
            memberNotAssgnedToGroupScrollHtml = smaManagement.replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupScrollHtml);
            htmlScrollRows = smaManagement.replaceSpecialCharactersOnIE9(htmlScrollRows);
            htmlFrozenlRows = smaManagement.replaceSpecialCharactersOnIE9(htmlFrozenlRows);
        }

        $("#tblContentHeader").find('tbody').empty().append(memberNotAssgnedToGroupFrozenHtml).append(htmlFrozenlRows);
        $("#tblContent").find('tbody').empty().append(memberNotAssgnedToGroupScrollHtml).append(htmlScrollRows);

        var frozenWidth = ((smaManagement.colNoWidth + smaManagement.colGroupHeadlineWidth + smaManagement.colUsernameWidth) + (3 * 4 - 2));
        var gridOpts = {
            tblContentId: 'tblContent',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: 110,
            freezeWidth: frozenWidth,
            numberFreezeCol: 3
        };
        $("#SMAMangementGrid").removeClass("hidden").gridScroller(gridOpts);
        //#region assign tooltip
        $('#tblContentHeader tbody tr').each(function () {
            var obj = $(this).find("td.group");
            var des = $(obj).find("div").attr('des');
            smaManagement.showDescription(obj, des);
            $(obj).hover(function (e) {
                if (parseInt($('div.tipsy-sw').css('top')) < 90)
                    $('div.tipsy-sw').css({ top: 100 });
            });
        });
        //#endregion assign tooltip
    }
};