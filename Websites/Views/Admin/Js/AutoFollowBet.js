$(function () {
    $('#addNewFollowCustomer').dialog({
        title: "Add New Follow Customer",
        autoOpen: false,
        height: 140,
        width: 370,
        modal: true
    });

    autoFollowBet.initPage();
});

function showPopup() {
    $('#errMsgDig').hide();
    $('#txtUserName').val('');
    $('#txtRate').val('');
    $('#addNewFollowCustomer').removeClass("hidden").dialog("open");
}

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
        dropdownSource: null
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
        gridWidth: 913
    },

    GetAutoFollowedBet: function () {
        autoFollowBet.blockUI();
        var postParams = autoFollowBet.getListInfoParams();
        autoFollowBet.CachedData.getListInfoParams = postParams;
        autoFollowBet.callAjax(this,
            "../AutoFollowBet/GetAutoFollowedBetCustomer",
            postParams,
            autoFollowBet.renderFollowBetResult,
            autoFollowBet.renderFollowBetResult,
            autoFollowBet.unblockUI,
            false);
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
        if (result && result.length > 0) {
            var resultLength = result.length;
            for (var i = 0; i < resultLength; i++) {
                var followInfo = result[i];
                var rowHTML = autoFollowBet.renderFollowBetRow(i, i % 2 != 0, followInfo);
                arr.push(rowHTML);
            }

            autoFollowBet.Tables.reportGridBody.html(arr.join(''));
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
        }
        else {

            var rowHTML = '<tr>\
                                <td colspan="11" style="border-right: 0px none">\
                                    <div class="c b" style="width: 911px">' + viewResources.No_Info + '</div>\
                                </td>\
                            </tr>';
            autoFollowBet.Tables.reportGridBody.html(rowHTML);
            autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);
        }
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
        autoFollowBet.Tables.reportGrid.removeClass("hidden")
                                           .reRenderScroller(autoFollowBet.CachedData.gridWidth, autoFollowBet.CachedData.gridHeight, true);

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
        var rate = parseFloat(postParams.Rate);
        var rowInfo = $(".followInfo_" + postParams.RowId);
        if (postParams.Rate == '') {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Empty, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        if (!$.isNumeric(postParams.Rate)) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Number, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        if (rate < 0 || 3 < rate) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_Range, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
            rowInfo.find('.textboxrate').focus();
            return false;
        }

        if (parseFloat(rate.toFixed(2)) != rate) {
            showAlertMessage("errormessage", viewResources.Error_Admin_AutoFollowBetManagement_InvalidRate_DecimalDigit, 1);
            autoFollowBet.Tables.errorMessage.removeClass("display-none").find("div:first").width(450);
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
    addNewFollewCustomer: function () {
        var username = $("#txtUserName").val();
        var rate = $("#txtRate").val();
        if (username == '') {
            showAlertMessage("errMsgDig", "Please input Username!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
        }
        else if (rate == '') {
            showAlertMessage("errMsgDig", "Please input Rate!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
        }
        else if (!isPositiveDouble(rate) || parseFloat(rate) <= 0) {
            showAlertMessage("errMsgDig", "Invalid Rate . Please input positive double value!", 1);
            $("#errMsgDig").show().find("div:first").width(350);
            return;
        }
        else {

            var data = '{"username":"' + username + '","rate":"' + rate + '"}';
            $.ajax({
                type: "POST",
                url: "InsAutoFollowedBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                data: data,
                success: function (rs) {
                    if (parseInt(rs.ErrorCode) != 0) {
                        showAlertMessage("errMsgDig", rs.ErrorMessage, 1);
                        $("#errMsgDig").show().find("div:first").width(350);
                    }
                    else {

                        showAlertMessage("errMsgDig", "Inserted Successfully!", 0);
                        $("#errMsgDig").show().find("div:first").width(350);
                        var data = { Status: autoFollowBet.status };
                        $.ajax({
                            type: "POST",
                            url: "GetAutoFollowedBet",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify(data),
                            success: function (rs) {
                                autoFollowBet.buildGrid(rs);
                            }
                        });

                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }

                }
            });
        }
        var t = setTimeout(function () {
            $("#errMsgDig").empty();
        }, 4000);


    },
    onKeyEnterAdd: function (e) {
        if (e.keyCode == 13) {
            autoFollowBet.addNewFollewCustomer();
        }
    },

    initPage: function () {
        autoFollowBet.cacheDOMElement();
        autoFollowBet.getFollowType();
    },
    cacheDOMElement: function () {
        autoFollowBet.Tables.TextboxUsername = $("#textbox-username");
        autoFollowBet.Tables.dropdownSource = $("#dropdown-source");
        autoFollowBet.Tables.dropdownBAEnabled = $("#dlStatus");
        autoFollowBet.Tables.errorMessage = $("#errormessage");
        autoFollowBet.Tables.reportGrid = $("#autofollowbet-grid");
        autoFollowBet.Tables.reportGridBody = $("#autofollowbet-gridbody");
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
            showTooltip: true,
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
    }
};

//$(document).ready(function () {
//    autoFollowBet.buildGrid(autoFollowBet.model);
//});