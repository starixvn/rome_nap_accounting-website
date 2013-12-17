$(function () {
    autoFollowBet.initPage();
});

var autoFollowBet = {
    width: 400,
    height: 500,
    status: 0,

    followTypeEnums: {
        LiveStatus: 0,
        LiveStatus_Both: 2,
        LiveStatus_Live: 1,
        LiveStatus_NonLive: 0,
        LiveStatus_All: 3,
        LiveStatus_None: -2,

        HDP: 1,
        HDP_Follow: 1,
        HDP_NotFollow: 0,
        HDP_All: 2,
        HDP_None: -2,

        _1stHDP: 2,
        _1stHDP_Follow: 1,
        _1stHDP_NotFollow: 0,
        _1stHDP_All: 2,
        _1stHDP_None: -2,

        OverUnder: 3,
        OverUnder_Follow: 1,
        OverUnder_NotFollow: 0,
        OverUnder_All: 2,
        OverUnder_None: -2,

        _1stOverUnder: 4,
        _1stOverUnder_Follow: 1,
        _1stOverUnder_NotFollow: 0,
        _1stOverUnder_All: 2,
        _1stOverUnder_None: -2
    },
    Tables: {
        dropdownFollowType: null,
        TextboxUsername: null,
        dropdownSource: null,
        dropdownBAEnabled: null,
        errorMessage: null,
        reportGrid: null,
        reportGridBody: null,
        addNewFollowedCustomerDialog: null,
        buttonAddNewFollowedCustomer: null,
        reportGridPagination: null
    },
    CachedData: {
        FollowTypes: null,
        FollowedCustomerList: null,
        editIcon: '../App_Themes/Images/icn_edit.png',
        saveIcon: '../App_Themes/Images/Save.png',
        undoIcon: '../App_Themes/Images/undo.png',
        deleteIcon: '../App_Themes/Images/icn_del.png',
        getListInfoParams: {
            Username: null,
            BAStatus: null,
            Source: null,
            LiveStatus: null,

            HDP: null,
            _1stHDP: null,
            OverUnder: null,
            _1stOverUnder: null
        },
        deleteInfoParams: {
            Username: null,
            RowId: null
        },
        updateInfoParams: {
            RowId: null,
            Username: null,
            SourceId: null,
            Source: null,
            Rate: null,
            LiveStatus: null,
            FollowedHDP: null,
            Followed1stHDP: null,
            FollowedOverUnder: null,
            Followed1stOverUnder: null
        },
        gridHeight: $(window).height() - 150,
        gridWidth: 913,
        pageIndex: 0,
        totalItems: 0,
        pageSize: 50
    },

    GetAutoFollowedBet: function () {
        autoFollowBet.blockUI();
        var postParams = autoFollowBet.getListInfoParams();
        autoFollowBet.CachedData.getListInfoParams = postParams;
        autoFollowBet.Tables.errorMessage.empty().addClass("display-none");
        if (!autoFollowBet.validateSelectInfoParams(postParams)) {
            autoFollowBet.unblockUI();
            return;
        }

        autoFollowBet.CachedData.pageIndex = 0;
        autoFollowBet.CachedData.pageSize = 50;
        autoFollowBet.callAjax(this,
            "../AutoFollowBet/GetAutoFollowedBetCustomer",
            postParams,
            autoFollowBet.renderFollowBetResult,
            autoFollowBet.renderFollowBetResult,
            autoFollowBet.unblockUI,
            false);
    },
    validateSelectInfoParams: function (postParams) {
        autoFollowBet.Tables.errorMessage.empty().addClass("display-none");
        if (postParams.LiveStatus.length == 0) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_LiveStatus_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return false;
        }

        if (postParams.HDP.length == 0) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_HDPStatus_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return false;
        }

        if (postParams._1stHDP.length == 0) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_1stHDPStatus_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return false;
        }

        if (postParams.OverUnder.length == 0) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_OverUnderStatus_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return false;
        }

        if (postParams._1stOverUnder.length == 0) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_1stOverUnderStatus_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return false;
        }

        return true;
    },
    getListInfoParams: function () {
        return {
            Username: autoFollowBet.Tables.TextboxUsername.val(),
            BAStatus: autoFollowBet.Tables.dropdownBAEnabled.val(),
            Source: autoFollowBet.Tables.dropdownSource.val(),
            LiveStatus: autoFollowBet.GetLiveStatusParam(),

            HDP: autoFollowBet.GetHDPParam(),
            _1stHDP: autoFollowBet.Get1stHDPParam(),
            OverUnder: autoFollowBet.GetOverUnderParam(),
            _1stOverUnder: autoFollowBet.Get1stOverUnderParam()
        };
    },
    GetLiveStatusParam: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var groupId = "#dropdown-followType" + followTypeEnums.LiveStatus;
        if ($(groupId).attr("checked")) {
            return followTypeEnums.LiveStatus_All.toString();
        }

        var paramList = [];
        if ($(groupId + "_" + followTypeEnums.LiveStatus_Both).attr("checked")) {
            paramList.push(followTypeEnums.LiveStatus_Both);
        }

        if ($(groupId + "_" + followTypeEnums.LiveStatus_Live).attr("checked")) {
            paramList.push(followTypeEnums.LiveStatus_Live);
        }

        if ($(groupId + "_" + followTypeEnums.LiveStatus_NonLive).attr("checked")) {
            paramList.push(followTypeEnums.LiveStatus_NonLive);
        }

        return paramList.join(",");
    },
    GetHDPParam: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var groupId = "#dropdown-followType" + followTypeEnums.HDP;
        if ($(groupId).attr("checked")) {
            return followTypeEnums.HDP_All.toString();
        }

        var paramList = [];
        if ($(groupId + "_" + followTypeEnums.HDP_Follow).attr("checked")) {
            paramList.push(followTypeEnums.HDP_Follow);
        }

        if ($(groupId + "_" + followTypeEnums.HDP_NotFollow).attr("checked")) {
            paramList.push(followTypeEnums.HDP_NotFollow);
        }

        return paramList.join(",");
    },
    Get1stHDPParam: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var groupId = "#dropdown-followType" + followTypeEnums._1stHDP;
        if ($(groupId).attr("checked")) {
            return followTypeEnums._1stHDP_All.toString();
        }

        var paramList = [];
        if ($(groupId + "_" + followTypeEnums._1stHDP_Follow).attr("checked")) {
            paramList.push(followTypeEnums._1stHDP_Follow);
        }

        if ($(groupId + "_" + followTypeEnums._1stHDP_NotFollow).attr("checked")) {
            paramList.push(followTypeEnums._1stHDP_NotFollow);
        }

        return paramList.join(",");
    },
    GetOverUnderParam: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var groupId = "#dropdown-followType" + followTypeEnums.OverUnder;
        if ($(groupId).attr("checked")) {
            return followTypeEnums.OverUnder_All.toString();
        }

        var paramList = [];
        if ($(groupId + "_" + followTypeEnums.OverUnder_Follow).attr("checked")) {
            paramList.push(followTypeEnums.OverUnder_Follow);
        }

        if ($(groupId + "_" + followTypeEnums.OverUnder_NotFollow).attr("checked")) {
            paramList.push(followTypeEnums.OverUnder_NotFollow);
        }

        return paramList.join(",");
    },
    Get1stOverUnderParam: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var groupId = "#dropdown-followType" + followTypeEnums._1stOverUnder;
        if ($(groupId).attr("checked")) {
            return followTypeEnums._1stOverUnder_All.toString();
        }

        var paramList = [];
        if ($(groupId + "_" + followTypeEnums._1stOverUnder_Follow).attr("checked")) {
            paramList.push(followTypeEnums._1stOverUnder_Follow);
        }

        if ($(groupId + "_" + followTypeEnums._1stOverUnder_NotFollow).attr("checked")) {
            paramList.push(followTypeEnums._1stOverUnder_NotFollow);
        }

        return paramList.join(",");
    },
    renderFollowBetResult: function (result) {
        autoFollowBet.CachedData.FollowedCustomerList = result;
        var arr = [];
        var rowHTML;
        autoFollowBet.CachedData.totalItems = (result && result.length > 0 ? result.length : 0);
        if (autoFollowBet.CachedData.totalItems) {
            var resultLength = result.length;
            var indexRange = autoFollowBet.getIndexRange(autoFollowBet.CachedData.pageIndex);
            var rowIndex = 0;
            var isEvenRow = true;
            for (var i = 0; i < resultLength; i++) {
                var followInfo = result[i];
                if (followInfo.IsDeleted) {
                    continue;
                }

                if (indexRange.startIndex <= rowIndex && rowIndex <= indexRange.endIndex) {
                    rowHTML = autoFollowBet.renderFollowBetRow(rowIndex, isEvenRow, followInfo);
                    arr.push(rowHTML);
                    isEvenRow = !isEvenRow;
                }

                rowIndex++;
            }

            autoFollowBet.Tables.reportGridBody.html(arr.join(''));
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
            autoFollowBet.initPager(autoFollowBet.CachedData.totalItems, autoFollowBet.CachedData.pageSize, autoFollowBet.CachedData.pageIndex);
        }
        else {
            var rowHTML = '<tr>\
                                <td colspan="11" style="border-right: 0px none">\
                                    <div class="c b" style="width: 911px">' + viewResources.No_Info + '</div>\
                                </td>\
                            </tr>';
            autoFollowBet.Tables.reportGridBody.html(rowHTML);
            autoFollowBet.Tables.reportGridPagination.html('&nbsp;');
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
        }
    },
    initPager: function (totalItem, itemsPerPage, currentPage) {
        var pagingOption = {
            start_per_page: 50,
            step_per_page: 50,
            max_items_per_page: 500,
            item_per_page_arr: [50, 100, 200, 500],
            callback: autoFollowBet.pageSelectCallback,
            items_per_page: itemsPerPage,
            current_page: currentPage
        };

        autoFollowBet.Tables.reportGridPagination.pagination(totalItem, pagingOption);
    },
    pageSelectCallback: function (pageIndex, itemsPerPage, jq) {
        var maxPage = (itemsPerPage == 0 ? 0 : Math.ceil(autoFollowBet.CachedData.totalItems / itemsPerPage));
        pageIndex = pageIndex < maxPage ? pageIndex : maxPage - 1;
        autoFollowBet.CachedData.pageIndex = pageIndex;
        autoFollowBet.CachedData.pageSize = itemsPerPage;

        autoFollowBet.pagingFollowBetResult();
        return false;
    },
    pagingFollowBetResult: function () {
        var arr = [];
        var rowHTML;

        var report = autoFollowBet.CachedData.FollowedCustomerList;
        if (autoFollowBet.CachedData.totalItems) {
            var resultLength = report.length;
            var indexRange = autoFollowBet.getIndexRange(autoFollowBet.CachedData.pageIndex);
            var rowIndex = 0;
            var isEvenRow = true;
            for (var i = 0; i < resultLength; i++) {
                var followInfo = report[i];
                if (followInfo.IsDeleted) {
                    continue;
                }

                if (indexRange.startIndex <= rowIndex && rowIndex < indexRange.endIndex) {
                    rowHTML = autoFollowBet.renderFollowBetRow(rowIndex, isEvenRow, followInfo);
                    arr.push(rowHTML);
                    isEvenRow = !isEvenRow;
                }

                rowIndex++;
            }

            autoFollowBet.Tables.reportGridBody.html(arr.join(''));
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
            autoFollowBet.initPager(autoFollowBet.CachedData.totalItems, autoFollowBet.CachedData.pageSize, autoFollowBet.CachedData.pageIndex);
        }
        else {
            var rowHTML = '<tr>\
                                <td colspan="11" style="border-right: 0px none">\
                                    <div class="c b" style="width: 911px">' + viewResources.No_Info + '</div>\
                                </td>\
                            </tr>';
            autoFollowBet.Tables.reportGridBody.html(rowHTML);
            autoFollowBet.Tables.reportGridPagination.html('&nbsp;');
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
        }
    },
    getIndexRange: function (currentPage) {
        var pageSize = parseInt(autoFollowBet.CachedData.pageSize);
        var startIndex = (currentPage * pageSize);
        var endIndex = startIndex + pageSize;
        if (endIndex > autoFollowBet.CachedData.totalItems) {
            endIndex = autoFollowBet.CachedData.totalItems;
        }

        return {
            startIndex: startIndex,
            endIndex: endIndex
        };
    },

    renderFollowBetRow: function (rowId, isEvenRow, followInfo) {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var cssClass = isEvenRow ? 'bgeven' : '';
        var strRow = '';
        strRow += '<tr class="' + cssClass + ' followInfo_' + rowId + '">';
        strRow += '<td><div class="col120 c">' + followInfo.Username + '</div></td>';
        strRow += '<td><div class="col80 c">' + followInfo.Source + '</div></td>';
        strRow += '<td class="td-labelrate col80">\
                        <div class="c labelrate">' + followInfo.Rate + '</div>\
                   </td>';
        strRow += '<td class="c col140">\
                        <select class="dropdown-livestatus editablefield" disabled="disabled">' +
                            autoFollowBet.generateDropdownLiveStatusOptions(followInfo.LiveStatus) +
                        '</select>\
                   </td>';
        strRow += '<td class="c col60">\
                        <input class="hdpstatus editablefield" type="checkbox" disabled="disabled"' +
                            (followInfo.FollowedHDP == followTypeEnums.HDP_Follow ? ' checked = "checked"' : '') + '/>\
                   </td>';
        strRow += '<td class="c col80">\
                        <input class="_1sthdpstatus editablefield" type="checkbox" disabled="disabled"' +
                            (followInfo.Followed1stHDP == followTypeEnums._1stHDP_Follow ? checked = "checked" : '') + '/>\
                   </td>';
        strRow += '<td class="c col60">\
                        <input class="overunderstatus editablefield" type="checkbox" disabled="disabled"' +
                            (followInfo.FollowedOverUnder == followTypeEnums.OverUnder_Follow ? ' checked = "checked"' : '') + '/>\
                   </td>';
        strRow += '<td class="c col80">\
                        <input class="_1stoverunderstatus editablefield" type="checkbox" disabled="disabled"' +
                            (followInfo.Followed1stOverUnder == followTypeEnums._1stOverUnder_Follow ? checked = "checked" : '') + '/>\
                   </td>';
        strRow += '<td class="col60 c"><div class="col35 ' + (followInfo.BAStatus ? 'checkBA' : '') + '"></div></td>';

        strRow += '<td>';
        strRow += '<div class="col60 c">';

        strRow += '<img class="editcontrol editicon"' +
            ' src="' + autoFollowBet.CachedData.editIcon + '"' +
            ' onclick="autoFollowBet.beginEditFollowInfo(event, \'' + rowId + '\')"' +
            ' title="' + viewResources.Edit + '"/>';
        strRow += '<img class="saveicon editcontrol display-none"' +
            ' src="' + autoFollowBet.CachedData.saveIcon + '"' +
            ' onclick="autoFollowBet.updateFollowInfo(event, \'' + rowId + '\')"' +
            ' title="' + viewResources.Save + '" />';
        strRow += '<img class="undoicon editcontrol display-none"' +
            ' src="' + autoFollowBet.CachedData.undoIcon + '"' +
            ' onclick="autoFollowBet.undoEditFollowInfo(event, \'' + rowId + '\')"' +
            ' title="' + viewResources.Cancel + '" />';
        strRow += '<td>';
        strRow += '<div class="col60 c">';
        strRow += '<img class="deleteicon editcontrol"' +
            ' src="' + autoFollowBet.CachedData.deleteIcon + '"' +
            ' onclick="autoFollowBet.deleteFollowInfo(event, \'' + rowId + '\')"' +
            ' title="' + viewResources.Delete + '" /></div></td>';
        return strRow;
    },
    beginEditFollowInfo: function (event, rowId) {
        var followInfo = autoFollowBet.CachedData.FollowedCustomerList[rowId];
        var rowInfo = $(".followInfo_" + rowId);
        rowInfo.find(".editablefield").attr("disabled", null);
        var iconEdit = rowInfo.find('.editicon');
        var iconSave = rowInfo.find('.saveicon');
        var iconUndo = rowInfo.find('.undoicon');
        iconEdit.hide();
        iconSave.show()
        iconUndo.show();
        var rate = rowInfo.find('.labelrate').html();
        var tdLabelRate = rowInfo.find('.td-labelrate');

        tdLabelRate.html('<input class="textboxrate txt" maxlength="4" type="text"' +
            'onkeyup="autoFollowBet.onEditFollowInfo(event ,\'' + rowId + '\');" editor="true"/>');
        rowInfo.find('.textboxrate').val(rate);
    },
    generateDropdownLiveStatusOptions: function (liveStatus) {
        var option = '';
        var followTypeEnums = autoFollowBet.followTypeEnums;
        option += '<option value="' + followTypeEnums.LiveStatus_Both + '"' +
                        (liveStatus == followTypeEnums.LiveStatus_Both ? ' selected="selected"' : '') +
                   '>' + viewResources.Both + '</option>';
        option += '<option value="' + followTypeEnums.LiveStatus_Live + '"' +
                        (liveStatus == followTypeEnums.LiveStatus_Live ? ' selected="selected"' : '') +
                   '>' + viewResources.Live + '</option>';
        option += '<option value="' + followTypeEnums.LiveStatus_NonLive + '"' +
                        (liveStatus == followTypeEnums.LiveStatus_NonLive ? ' selected="selected"' : '') +
                   '>' + viewResources.Non_Live + '</option>';
        return option;
    },
    undoEditFollowInfo: function (event, rowId) {
        var followInfo = autoFollowBet.CachedData.FollowedCustomerList[rowId];
        var rowInfo = $(".followInfo_" + rowId);
        rowInfo.find(".editablefield").attr("disabled", "disabled");
        var iconEdit = rowInfo.find('.editicon');
        var iconSave = rowInfo.find('.saveicon');
        var iconUndo = rowInfo.find('.undoicon');
        iconSave.hide()
        iconUndo.hide();
        iconEdit.show();
        var rate = rowInfo.find('.labelrate').html();
        var tdLabelRate = rowInfo.find('.td-labelrate');
        tdLabelRate.html('<div class="col80 c labelrate">' + followInfo.Rate + '</div>');

        var dropdownLiveStatus = rowInfo.find('.dropdown-livestatus');
        dropdownLiveStatus.find('option[value=' + followInfo.LiveStatus + ']').attr('selected', 'selected');
        var hdpStatus = rowInfo.find('.hdpstatus');
        hdpStatus.attr("checked", followInfo.FollowedHDP ? "checked" : null);
        var _1stHDPStatus = rowInfo.find('._1sthdpstatus');
        _1stHDPStatus.attr("checked", followInfo.Followed1stHDP ? "checked" : null);
        var overUnderStatus = rowInfo.find('.overunderstatus');
        overUnderStatus.attr("checked", followInfo.FollowedOverUnder ? "checked" : null);
        var _1stOverUnderStatus = rowInfo.find('._1stoverunderstatus');
        _1stOverUnderStatus.attr("checked", followInfo.Followed1stOverUnder ? "checked" : null);
    },
    deleteFollowInfo: function (event, rowId) {
        autoFollowBet.blockUI();
        var followInfo = autoFollowBet.CachedData.FollowedCustomerList[rowId];
        var isDelete = confirm(viewResources.Warning_Admin_AutoFollowBetManagement_DeleteFollowInfo);
        if (!isDelete) {
            autoFollowBet.unblockUI();
        }

        var postParams = {
            Username: followInfo.Username,
            RowId: rowId
        };

        autoFollowBet.CachedData.deleteInfoParams = postParams;
        autoFollowBet.callAjax(this,
            "../AutoFollowBet/DeleteAutoFollowedBetCustomer",
            postParams,
            autoFollowBet.renderDeleteFollowInfoResult,
            autoFollowBet.renderDeleteFollowInfoResult,
            autoFollowBet.unblockUI,
            true);
    },
    renderDeleteFollowInfoResult: function (rs) {
        var rowId = autoFollowBet.CachedData.deleteInfoParams.RowId;
        if (parseInt(rs.ErrorCode) != 0) {
            showAlertMessage("errormessage", rs.ErrorMessage, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(390);
            return;
        }

        showAlertMessage("errormessage", viewResources.Success_Admin_AutoFollowBetManagement_DeleteFollowInfo, 0);
        autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(390);
        var rowInfo = $(".followInfo_" + rowId);
        rowInfo.remove();
        autoFollowBet.CachedData.FollowedCustomerList[rowId].IsDeleted = true;
        autoFollowBet.CachedData.totalItems = autoFollowBet.CachedData.totalItems - 1;
        if (autoFollowBet.CachedData.totalItems <= autoFollowBet.CachedData.pageIndex * autoFollowBet.CachedData.pageSize) {
            autoFollowBet.CachedData.pageIndex = autoFollowBet.CachedData.pageIndex - 1;
        }

        autoFollowBet.initPager(autoFollowBet.CachedData.totalItems, autoFollowBet.CachedData.pageSize, autoFollowBet.CachedData.pageIndex);
        autoFollowBet.pagingFollowBetResult();
        var t = setTimeout(function () {
            autoFollowBet.Tables.errorMessage.addClass("display-none").empty();
        }, 3000);
    },
    updateFollowInfo: function (event, rowId) {
        autoFollowBet.blockUI();
        var postParams = autoFollowBet.getUpdateInfoParams(rowId);

        autoFollowBet.CachedData.updateInfoParams = postParams;
        if (!autoFollowBet.validateUpdateInfoParams(postParams)) {
            autoFollowBet.unblockUI();
            return;
        }

        autoFollowBet.callAjax(this,
            "../AutoFollowBet/UpdateAutoFollowedBetCustomer",
            postParams,
            autoFollowBet.renderUpdateFollowInfoResult,
            autoFollowBet.renderUpdateFollowInfoResult,
            autoFollowBet.unblockUI,
            true);
    },
    getUpdateInfoParams: function (rowId) {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        var followInfo = autoFollowBet.CachedData.FollowedCustomerList[rowId];

        var rowInfo = $(".followInfo_" + rowId);
        var iconEdit = rowInfo.find('.editicon');
        var iconSave = rowInfo.find('.saveicon');
        var iconUndo = rowInfo.find('.undoicon');
        var rate = rowInfo.find('.textboxrate').val();
        var dropdownLiveStatus = rowInfo.find('.dropdown-livestatus');
        var hdpStatus = rowInfo.find('.hdpstatus');
        var _1stHDPStatus = rowInfo.find('._1sthdpstatus');
        var overUnderStatus = rowInfo.find('.overunderstatus');
        var _1stOverUnderStatus = rowInfo.find('._1stoverunderstatus');

        return {
            RowId: rowId,
            Username: followInfo.Username,
            SourceId: followInfo.SourceId,
            Source: followInfo.Source,
            Rate: rate,
            LiveStatus: dropdownLiveStatus.find("option:selected").val(),
            FollowedHDP: hdpStatus.attr("checked") ? followTypeEnums.HDP_Follow : followTypeEnums.HDP_NotFollow,
            Followed1stHDP: _1stHDPStatus.attr("checked") ? followTypeEnums._1stHDP_Follow : followTypeEnums._1stHDP_NotFollow,
            FollowedOverUnder: overUnderStatus.attr("checked") ? followTypeEnums.OverUnder_Follow : followTypeEnums.OverUnder_NotFollow,
            Followed1stOverUnder: _1stOverUnderStatus.attr("checked") ? followTypeEnums._1stOverUnder_Follow : followTypeEnums._1stOverUnder_NotFollow
        };
    },
    validateUpdateInfoParams: function (postParams) {
        var rowInfo = $(".followInfo_" + postParams.RowId);
        var maxMessageWidth = 450;
        if (postParams.Rate == '') {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(maxMessageWidth);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        if (!$.isNumeric(postParams.Rate)) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Number, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(maxMessageWidth);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        var rate = parseFloat(postParams.Rate);
        if (rate < 0.3 || 3 < rate) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Range, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(maxMessageWidth);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        if (parseFloat(rate.toFixed(2)) != rate) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_DecimalDigit, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(maxMessageWidth);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        autoFollowBet.Tables.errorMessage.empty().addClass("display-none");
        return true;
    },
    renderUpdateFollowInfoResult: function (result) {
        if (parseInt(result.ErrorCode) != 0) {
            showAlertMessage("errormessage", result.ErrorMessage, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            return;
        }

        showAlertMessage("errormessage", viewResources.Success_Admin_AutoFollowBetManagement_UpdateFollowInfo, 0);
        autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
        autoFollowBet.endEditFollowInfo();
        var t = setTimeout(function () {
            autoFollowBet.Tables.errorMessage.addClass("display-none").empty();
        }, 3000);
    },
    endEditFollowInfo: function () {
        var rowId = autoFollowBet.CachedData.updateInfoParams.RowId;
        var rowInfo = $(".followInfo_" + rowId);
        rowInfo.find(".editablefield").attr("disabled", "disabled");
        var iconEdit = rowInfo.find('.editicon');
        var iconSave = rowInfo.find('.saveicon');
        var iconUndo = rowInfo.find('.undoicon');
        iconSave.hide()
        iconUndo.hide();
        iconEdit.show();

        var tdLabelRate = rowInfo.find('.td-labelrate');
        tdLabelRate.html('<div class="col80 c labelrate">' + autoFollowBet.CachedData.updateInfoParams.Rate + '</div>');

        autoFollowBet.CachedData.FollowedCustomerList[rowId].Rate = autoFollowBet.CachedData.updateInfoParams.Rate;
        autoFollowBet.CachedData.FollowedCustomerList[rowId].LiveStatus = autoFollowBet.CachedData.updateInfoParams.LiveStatus;
        autoFollowBet.CachedData.FollowedCustomerList[rowId].FollowedHDP = autoFollowBet.CachedData.updateInfoParams.FollowedHDP;
        autoFollowBet.CachedData.FollowedCustomerList[rowId].Followed1stHDP = autoFollowBet.CachedData.updateInfoParams.Followed1stHDP;
        autoFollowBet.CachedData.FollowedCustomerList[rowId].FollowedOverUnder = autoFollowBet.CachedData.updateInfoParams.FollowedOverUnder;
        autoFollowBet.CachedData.FollowedCustomerList[rowId].Followed1stOverUnder = autoFollowBet.CachedData.updateInfoParams.Followed1stOverUnder;
    },

    initPage: function () {
        autoFollowBet.cacheDOMElement();
        autoFollowBet.getFollowType();

        autoFollowBet.Tables.addNewFollowedCustomerDialog =
            autoFollowBet.Tables.addNewFollowedCustomerDialog.dialog({
                title: viewResources.Add_New_Followed_Customer,
                autoOpen: false,
                height: 200,
                width: 420,
                modal: true
            });

        autoFollowBet.bindingEvent();
    },
    bindingEvent: function () {
        autoFollowBet.Tables.buttonAddNewFollowedCustomer.unbind("click");
        autoFollowBet.Tables.buttonAddNewFollowedCustomer.click(function () {
            autoFollowBet.ShowAddNewFollowedCustomerDialog();
        });

        autoFollowBet.Tables.buttonSubmit_AddDialog.unbind("click");
        autoFollowBet.Tables.buttonSubmit_AddDialog.click(function () {
            autoFollowBet.addNewFollowedCustomer();
        });
    },
    cacheDOMElement: function () {
        autoFollowBet.Tables.TextboxUsername = $("#textbox-username");
        autoFollowBet.Tables.dropdownSource = $("#dropdown-source");
        autoFollowBet.Tables.dropdownBAEnabled = $("#dlStatus");
        autoFollowBet.Tables.errorMessage = $("#errormessage");
        autoFollowBet.Tables.reportGrid = $("#autofollowbet-grid");
        autoFollowBet.Tables.reportGridBody = $("#autofollowbet-gridbody");
        autoFollowBet.Tables.reportGridPagination = autoFollowBet.Tables.reportGrid.find(".pagination");

        autoFollowBet.Tables.addNewFollowedCustomerDialog = $('#dialog-addnewfollowedcustomer');
        autoFollowBet.Tables.buttonAddNewFollowedCustomer = $('#button-addnewfollowedcustomer');

        autoFollowBet.Tables.resultMessage_AddDialog = autoFollowBet.Tables.addNewFollowedCustomerDialog.find('#resultmessage-adddialog');
        autoFollowBet.Tables.textboxUsername_AddDialog = $('#textbox-username-addnewdialog');
        autoFollowBet.Tables.textboxRate_AddDialog = $('#textbox-rate-addnewdialog');
        autoFollowBet.Tables.buttonSubmit_AddDialog = $('#buttonsubmit-addnewdialog');

        autoFollowBet.Tables.dropdownLiveStatus_AddDialog = $('#dropdown-livestatus-adddialog');
        autoFollowBet.Tables.hdpStatus_AddDialog = autoFollowBet.Tables.addNewFollowedCustomerDialog.find('.hdpstatus');
        autoFollowBet.Tables._1stHDPStatus_AddDialog = autoFollowBet.Tables.addNewFollowedCustomerDialog.find('._1sthdpstatus');
        autoFollowBet.Tables.overUnderStatus_AddDialog = autoFollowBet.Tables.addNewFollowedCustomerDialog.find('.overunderstatus');
        autoFollowBet.Tables._1stOverUnderStatus_AddDialog = autoFollowBet.Tables.addNewFollowedCustomerDialog.find('._1stoverunderstatus');
    },
    getFollowType: function () {
        autoFollowBet.blockUI();
        autoFollowBet.callAjax(this,
            "../AutoFollowBet/GetFollowType",
            null,
            autoFollowBet.renderFollowType,
            autoFollowBet.renderFollowType,
            autoFollowBet.unblockUI,
            true);
    },
    renderFollowType: function (result) {
        autoFollowBet.CachedData.FollowTypes = result;
        var followTypeEnums = autoFollowBet.followTypeEnums;
        autoFollowBet.Tables.dropdownFollowType = $("#dropdown-followtype");
        var ddwnFollowTypes = [];
        if (result && result.length > 0) {
            var optionLength = result.length;
            var prefixId = 'dropdown-followType';
            for (var i = 0; i < optionLength; i++) {
                var followType = result[i];
                var subTypesLength = followType.ChildsInfo.length;
                var subType = [];
                for (var j = 0; j < subTypesLength; j++) {
                    var info = followType.ChildsInfo[j];
                    subType.push({
                        Id: prefixId + followType.Id + "_" + info.Id,
                        Value: info.Id,
                        Text: info.Name,
                        Checked: true,
                        Enable: true,
                        CssClass: "",
                        Items: []
                    });
                }

                ddwnFollowTypes.push({
                    Id: prefixId + followType.Id,
                    Value: followType.Id,
                    Text: followType.Name,
                    Checked: true,
                    Enable: true,
                    CssClass: "",
                    Items: subType
                });
            }
        }

        autoFollowBet.Tables.dropdownFollowType.multiSelect(
        {
            minWidth: 160,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: true,
            showFooter: false,
            showTooltip: false,
            checkAllText: viewResources.All,
            isUsedCustomCheckAllText: true,
            customCheckAllText: viewResources.All,
            noItemText: viewResources.No_type_found,
            selectedList: 3,
            listData: ddwnFollowTypes
        });

        autoFollowBet.Tables.dropdownFollowType = $("#dropdown-followtype");
    },
    blockUI: function () {
        try {
            parent.BlockUI();
        }
        catch (e) {
        }
    },
    unblockUI: function () {
        try {
            parent.jQuery.unblockUI();
        }
        catch (e) {
        }
    },
    callAjax: function (reportHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction,
        isKeepingMessage) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: requestParams ? JSON.stringify(requestParams) : null,
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) == 'function') {
                    successFunction.call(reportHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) == 'function') {
                    errorFunction.call(reportHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) == 'function') {
                    completeFunction();
                }
            }
        });
    },
    onEditFollowInfo: function (event, rowId) {
        //        var rowInfo = $(".followInfo_" + rowId);
        //        var rate = rowInfo.find(".textboxrate").val();
    },
    ShowAddNewFollowedCustomerDialog: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        autoFollowBet.Tables.resultMessage_AddDialog.addClass("display-none");
        autoFollowBet.Tables.textboxUsername_AddDialog.val('');
        autoFollowBet.Tables.textboxRate_AddDialog.val('');

        autoFollowBet.Tables.dropdownLiveStatus_AddDialog.find('option[value=' + followTypeEnums.LiveStatus_Both + ']').attr('selected', 'selected');
        autoFollowBet.Tables.hdpStatus_AddDialog.attr("checked", "checked");
        autoFollowBet.Tables._1stHDPStatus_AddDialog.attr("checked", "checked");
        autoFollowBet.Tables.overUnderStatus_AddDialog.attr("checked", "checked");
        autoFollowBet.Tables._1stOverUnderStatus_AddDialog.attr("checked", "checked");

        autoFollowBet.Tables.addNewFollowedCustomerDialog.removeClass("display-none").dialog("open");
    },
    addNewFollowedCustomer: function () {
        autoFollowBet.blockUI();
        var postParams = autoFollowBet.getAddInfoParams();
        if (!autoFollowBet.validateAddInfoParams(postParams)) {
            autoFollowBet.unblockUI();
            return;
        }

        autoFollowBet.callAjax(this,
            "../AutoFollowBet/AddAutoFollowedBetCustomer",
            postParams,
            autoFollowBet.renderAddFollowInfoResult,
            autoFollowBet.renderAddFollowInfoResult,
            autoFollowBet.unblockUI,
            true);
    },
    getAddInfoParams: function () {
        var followTypeEnums = autoFollowBet.followTypeEnums;
        return {
            Username: autoFollowBet.Tables.textboxUsername_AddDialog.val(),
            Rate: autoFollowBet.Tables.textboxRate_AddDialog.val(),
            LiveStatus: autoFollowBet.Tables.dropdownLiveStatus_AddDialog.val(),
            FollowedHDP: autoFollowBet.Tables.hdpStatus_AddDialog.attr("checked") ?
                followTypeEnums.HDP_Follow :
                followTypeEnums.HDP_NotFollow,
            Followed1stHDP: autoFollowBet.Tables._1stHDPStatus_AddDialog.attr("checked") ?
                followTypeEnums._1stHDP_Follow :
                followTypeEnums._1stHDP_NotFollow,
            FollowedOverUnder: autoFollowBet.Tables.overUnderStatus_AddDialog.attr("checked") ?
                followTypeEnums.OverUnder_Follow :
                followTypeEnums.OverUnder_NotFollow,
            Followed1stOverUnder: autoFollowBet.Tables._1stOverUnderStatus_AddDialog.attr("checked") ?
                followTypeEnums._1stOverUnder_Follow :
                followTypeEnums._1stOverUnder_NotFollow
        };
    },
    validateAddInfoParams: function (postParams) {
        var maxMessageWidth = 367;
        if (postParams.Username == '') {
            showAlertMessage("resultmessage-adddialog", viewResources.Error_Admin_AutoFollowBetManagement_Username_Empty, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            autoFollowBet.Tables.textboxUsername_AddDialog.focus();
            return false;
        }

        if (postParams.Rate == '') {
            showAlertMessage("resultmessage-adddialog", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Empty, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            autoFollowBet.Tables.textboxRate_AddDialog.focus();
            return false;
        }

        if (!$.isNumeric(postParams.Rate)) {
            showAlertMessage("resultmessage-adddialog", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Number, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            autoFollowBet.Tables.textboxRate_AddDialog.focus();
            return false;
        }

        var rate = parseFloat(postParams.Rate);
        if (rate < 0.3 || 3 < rate) {
            showAlertMessage("resultmessage-adddialog", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Range, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            autoFollowBet.Tables.textboxRate_AddDialog.focus();
            return false;
        }

        if (parseFloat(rate.toFixed(2)) != rate) {
            showAlertMessage("resultmessage-adddialog", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_DecimalDigit, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            autoFollowBet.Tables.textboxRate_AddDialog.focus();
            return false;
        }

        autoFollowBet.Tables.errorMessage.empty().addClass("display-none");
        return true;
    },
    renderAddFollowInfoResult: function (rs) {
        var maxMessageWidth = 367;
        if (parseInt(rs.ErrorCode) != 0) {
            showAlertMessage("resultmessage-adddialog", rs.ErrorMessage, 1);
            autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
            return;
        }

        showAlertMessage("resultmessage-adddialog", viewResources.Success_Admin_AutoFollowBetManagement_AddFollowInfo, 0);
        autoFollowBet.Tables.resultMessage_AddDialog.removeClass("display-none").find("div:first").width(maxMessageWidth);
    }
};

//$(document).ready(function () {
//    autoFollowBet.buildGrid(autoFollowBet.model);
//});