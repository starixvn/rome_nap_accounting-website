var CURDATE = new Date();

function CheckDecNumber(e) {

    var code = "";
    if (e.which)
        code = e.which;
    else if (e.keyCode)
        code = e.keyCode;


    if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118 || code == 46) //e or E or "-"
        return true;
    if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 9)
        return false;
    return true;
}

function SetupCalendar(strdatetime, txtdate) {

    $(txtdate).datetimepicker({
        showSecond: true,
        timeFormat: 'hh:mm:ss',
        stepHour: 1,
        stepMinute: 1,
        stepSecond: 1,
        hourGrid: 3,
        minuteGrid: 5,
        secondGrid: 5

    });
    if (strdatetime != null && strdatetime != "") {
        $(txtdate).val(strdatetime).datetimepicker('setDate', (new Date(strdatetime)));
    }
}

$(function () {
    ticketsListing.renderLicensee();
    ticketsListing.loadBettype();
    SetupCalendar(CURDATE.format('m/d/Y H:i:s'), '#txtFromdate');
    SetupCalendar(CURDATE.format('m/d/Y H:i:s'), '#txtTodate');
    ticketsListing.eventOnDateTime();
    $("#bodybettype").delegate("input", "click", function () {
        var checkedall = true;
        var uncheckedall = false;
        $(".chkbettype").each(function () {
            if (!$(this).attr("checked")) {
                checkedall = false;
            }
            else {
                uncheckedall = true;
            }
        });
        $("#chkallbettype").attr("checked", checkedall);
        ticketsListing.isValidBetType = uncheckedall;
        ticketsListing.enableShowBtn();
    });
    $('#txtmatchid').each(function () {
        $(this).bind('keypress', function (e) { allowInt(e); })
                .bind('keyup', function (e) {
                    ticketsListing.isValidMatchID = $("#txtmatchid").val().length > 0;
                    ticketsListing.enableShowBtn();
                })
                .bind('focusout', function () {
                    ticketsListing.isValidMatchID = $("#txtmatchid").val().length > 0;
                    ticketsListing.enableShowBtn();
                });
    });
});
var ticketsListing = {
    matchID: '',
    Licensee: '',
    LicenseeId: 0,
    listBettype: '',
    listBettypeLive: '',
    group: 0,
    sportGroup: "SportsBook",
    sportGroupId: 0,
    LiveType: '2',
    SAFilter: 0,
    HomeScore: '',
    AwayScore: '',
    FromTransDate: '',
    ToTransDate: '',
    matchInfo: {
        away: '',
        eventdate: '',
        home: '',
        matchcode: ''
    },
    isEnable: false,
    isValidMatchID: false,
    isValidBetType: true,
    htmlBettypeSB: '',
    htmlBettypeNG: '',
    htmlBettypeVS: '',
    getHTMLBettype: function (arrBettype) {
        var len = arrBettype.length;
        var arr = [];
        for (var i = 0; i < len; i += 5) {
            var row = '<tr>\
                        <td class="l col180">' + (i < len ? ('<input type="checkbox" checked="checked" class="chkbettype" id="chkbettype_' + arrBettype[i].Id + '" />' + arrBettype[i].Name) : '&nbsp;') + '</td>\
                        <td class="l col180">' + (i + 1 < len ? ('<input type="checkbox" checked="checked" class="chkbettype" id="chkbettype_' + arrBettype[i + 1].Id + '" />' + arrBettype[i + 1].Name) : '&nbsp;') + '</td>\
                        <td class="l col180">' + (i + 2 < len ? ('<input type="checkbox" checked="checked" class="chkbettype" id="chkbettype_' + arrBettype[i + 2].Id + '" />' + arrBettype[i + 2].Name) : '&nbsp;') + '</td>\
                        <td class="l col180">' + (i + 3 < len ? ('<input type="checkbox" checked="checked" class="chkbettype" id="chkbettype_' + arrBettype[i + 3].Id + '" />' + arrBettype[i + 3].Name) : '&nbsp;') + '</td>\
                        <td class="l col180">' + (i + 4 < len ? ('<input type="checkbox" checked="checked" class="chkbettype" id="chkbettype_' + arrBettype[i + 4].Id + '" />' + arrBettype[i + 4].Name) : '&nbsp;') + '</td>\
                     </tr>';
            arr.push(row);
        }
        return arr.join('');
    },
    renderRowCol: function (isLive, rows, col) {
        var arr = [];
        var len = rows.length;
        for (var i = 1; i <= len; i++) {
            var html = '';
            var modulo = i % col;
            if (i == 1 || modulo == 1) {
                html += '<tr>';
            }
            html += '<td class="l col180"><input type="checkbox" checked="checked" class="chkbettype ' + (isLive ? 'chklivebettype' : 'chknonelivebettype') + '" id="chkbettype_' + rows[i - 1].Id + '" />' + rows[i - 1].Name + '</td>';

            if (i == len && modulo > 0) {
                for (var j = 0; j < (col - modulo); j++) {
                    html += '<td class="col180">&nbsp;</td>';
                }
            }

            if (i > 1 && (i == col || i == len)) {
                html += '</tr>';
            }
            arr.push(html);
        }
        return arr.join('');
    },
    getHTMLBettypeNB: function (arrBettypeLive, arrBettypeNoneLive) {
        var htmlLive = '<tr class="title"><td colspan="5" class="l b">Live ball</td></tr>';
        var htmlNoneLive = '<tr class="title"><td colspan="5" class="l b">Dead ball</td></tr>';
        htmlLive = htmlLive + ticketsListing.renderRowCol(true, arrBettypeLive, 5);
        htmlNoneLive = htmlNoneLive + ticketsListing.renderRowCol(false, arrBettypeNoneLive, 5);
        return htmlNoneLive + htmlLive;
    },
    renderBettype: function () {
        $("#tblbettype").removeClass("hidden");
        switch (ticketsListing.sportGroupId) {
            case 0:
                $("#bodybettype").empty().html(ticketsListing.htmlBettypeSB);
                $('.liveindicator').show();
                $("#typeFilterRow").show();
                if ($('#ddlLiveIndicator').val() == '1') {
                    $('.type').show();                   
                }
                break;
            case 1:
                $("#bodybettype").empty().html(ticketsListing.htmlBettypeNG);
                $('.liveindicator').hide();
                $('.type').hide();
                $("#typeFilterRow").hide();
                break;
            case 2:
                $("#bodybettype").empty().html(ticketsListing.htmlBettypeVS);
                $('.liveindicator').hide();
                $('.type').hide();
                $("#typeFilterRow").hide();
                break;
        }
        ticketsListing.enableShowBtn();
    },
    loadBettype: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Services/TicketsListingService.svc/SelBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                ticketsListing.htmlBettypeSB = ticketsListing.getHTMLBettype(rs.ItemsSB);
                ticketsListing.htmlBettypeNG = ticketsListing.getHTMLBettypeNB(rs.ItemsNGLive, rs.ItemsNGNoneLive);
                ticketsListing.htmlBettypeVS = ticketsListing.getHTMLBettype(rs.ItemsVS);
                ticketsListing.renderBettype();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderLicensee: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Services/TicketsListingService.svc/SelAllLicensee",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                ticketsListing.Licensee = rs.Items;
                var arr = [];
                var len = ticketsListing.Licensee.length;
                arr.push('<option value="0">All</option>');
                arr.push('<option value="-1">IBC Group</option>');
                arr.push('<optgroup  label="Licensee">');
                for (var i = 0; i < len; i++) {
                    var row = '<option value="' + ticketsListing.Licensee[i].Id + '">' + ticketsListing.Licensee[i].Name + '</option>';
                    arr.push(row);
                }
                arr.push('</optgroup>');
                $("#ddlsite").empty().html(arr.join(''));
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    changeSport: function (sportGroupName) {
        var lastSportGroupId = ticketsListing.sportGroupId;
        switch (sportGroupName) {
            case 'SportsBook':
                ticketsListing.sportGroup = 'SportsBook';
                ticketsListing.sportGroupId = 0;
                break;
            case 'NumberGame':
                ticketsListing.sportGroup = 'NumberGame';
                ticketsListing.sportGroupId = 1;
                break;
            case 'VirtualSports':
                ticketsListing.sportGroup = 'VirtualSports';
                ticketsListing.sportGroupId = 2;
                break;
        }

        if (ticketsListing.sportGroupId != lastSportGroupId) {
            ticketsListing.renderBettype();
            $("#chkallbettype").attr("checked", true);
            isValidBetType: true;
        }
    },
    checkAllBettype: function () {
        var checked = $("#chkallbettype").attr("checked");
        $(".chklivebettype, .chknonelivebettype, .chkbettype ").attr("checked", checked);
        ticketsListing.isValidBetType = checked;
        ticketsListing.enableShowBtn();
    },
    autoGetBettype: function (e) {
        var code;
        if (e.which)
            code = e.which;
        else if (e.keyCode)
            code = e.keyCode;
        if (code == 13) {
            ticketsListing.renderBettype();
        }
        return true
    },
    enableShowBtn: function () {
        if (ticketsListing.isValidBetType && ticketsListing.isValidMatchID) {
            ticketsListing.isEnable = true;
            $("#btnReport").removeAttr("disabled");
        }
        else {
            ticketsListing.isEnable = false;
            $("#btnReport").attr("disabled", true);
        }
    },
    getListBettype: function (live) {
        var listBettype = '';
        $("#bodybettype " + live).each(function () {
            listBettype += $(this).attr("checked") ? ("," + $(this).attr("id").replace("chkbettype_", "")) : "";
        });

        return listBettype.length > 0 ? listBettype.substring(1, listBettype.length) : listBettype;
    },
    renderData: function () {
        if (!ticketsListing.isEnable) return;
        $("#matchInfo").addClass("hidden");
        $("#tickestListing").addClass("hidden");
        $("#gridbetlist").addClass("hidden");

        $("#err").empty();

        if (!isInt($("#txtmatchid").val())) {
            showAlertMessage("err", "Invalid Match ID.", 1);
            return;
        }

        ticketsListing.matchID = $("#txtmatchid").val();
        ticketsListing.LicenseeId = $("#ddlsite").val();
        ticketsListing.group = parseInt($("#ddlgroup").val());
        switch (ticketsListing.sportGroupId) {
            case 0:
                ticketsListing.listBettype = ticketsListing.getListBettype(".chkbettype");
                ticketsListing.LiveType = $('#ddlLiveIndicator').val();
                if ($('#ddlLiveIndicator').val() == '1') {
                    if ($('#txtHomeScore').val() == '' && $('#txtAwayScore').val().length > 0) {
                        showAlertMessage("err", "Please enter a Live Home Score !", 1);
                        $('#txtHomeScore').focus();
                        return;
                    }
                    if ($('#txtHomeScore').val().length > 0 && $('#txtAwayScore').val() == '') {
                        showAlertMessage("err", "Please enter a Live Away Score !", 1);
                        $('#txtAwayScore').focus();
                        return;
                    }
                }
                break;
            case 1:
                ticketsListing.listBettype = ticketsListing.getListBettype(".chknonelivebettype");
                ticketsListing.listBettypeLive = ticketsListing.getListBettype(".chklivebettype");
                ticketsListing.LiveType = '0';
                break;
            case 2:
                ticketsListing.listBettype = ticketsListing.getListBettype(".chkbettype");
                break;
        }

        try { parent.BlockUI(); } catch (e) { }

        //        var HomeScore = $('#txtHomeScore').val();
        //        var AwayScore = $('#txtAwayScore').val();
        //        var TransDate = $('#txtdate').val();
        ticketsListing.HomeScore = $('#txtHomeScore').val();
        ticketsListing.AwayScore = $('#txtAwayScore').val();
        ticketsListing.FromTransDate = $('#txtFromdate').val();
        ticketsListing.ToTransDate = $('#txtTodate').val();
        ticketsListing.SAFilter = $("#ddlSAFilter").val();
        if (ticketsListing.LiveType != '1') {
            HomeScore = '';
            AwayScore = '';
            FromTransDate = '';
            ToTransDate = '';
        }

        var data = {
            matchID: ticketsListing.matchID,
            licenseeID: ticketsListing.LicenseeId,
            betTypes: ticketsListing.listBettype,
            betTypesLive: ticketsListing.listBettypeLive,
            Type: ticketsListing.sportGroupId,
            GroupBy: ticketsListing.group,
            LiveType: ticketsListing.LiveType,
            HomeScore: ticketsListing.HomeScore,
            AwayScore: ticketsListing.AwayScore,
            FromTransDate: ticketsListing.FromTransDate,
            ToTransDate: ticketsListing.ToTransDate,
            SAFilter: ticketsListing.SAFilter
        };

        $.ajax({
            type: "POST",
            url: "Services/TicketsListingService.svc/SelTicketsListing",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs.ErrCode > 0) {
                    showAlertMessage("err", rs.ErrMsg, 1);
                    return;
                }
                if (rs.MatchInfo) {
                    ticketsListing.matchInfo = rs.MatchInfo;
                    ticketsListing.renderMatchInfo();
                }
                $('#gridbetlist').addClass("hidden");

                var arr = [];
                var len = rs.Items.length;
                if (len > 0) {
                    for (var i = 0; i < len; i++) {
                        var item = rs.Items[i];
                        arr.push(ticketsListing.renderMainRow(item));
                        var totalitems = item.Items.length;
                        for (j = 0; j < totalitems; j++) {
                            arr.push(ticketsListing.renderDetailRow(item.Items[j], item.Id, (j % 2 == 0 ? "bgeven" : "")));
                        }
                    }
                    $("#tblscroll").empty().html(arr.join(''));
                }
                else {
                    $("#tblscroll").empty().html('<tr><td><div class="c b" style="width:426px;">There is no data.</div></td></tr>');
                }


                $("#tickestListing").removeClass("hidden").reRenderScroller(429, 352, true);

            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    renderMatchInfo: function () {
        $("#matchInfo").removeClass("hidden");
        var html = '<tr class="b">\
                        <td>' + ticketsListing.matchInfo.matchcode + '</td>\
                        <td>' + ticketsListing.matchInfo.home + ' -vs- ' + ticketsListing.matchInfo.away + '</td>\
                        <td>' + ticketsListing.matchInfo.eventdate + '</td>';

        $("#matchInfoContent").html(html);
    },
    renderMainRow: function (row) {

        var isLive = (ticketsListing.sportGroupId == 0 ? ticketsListing.LiveType : row.LiveIndicator);
        var html = '<tr class="total">\
                    <td>\
                        <div class="col260 l b"><span class="tree-expanded" onclick="ticketsListing.toggleDetail(this,\'' + row.Id + '\')"></span><span id="group_' + row.Id + '_' + isLive + '">' + row.Name + '</span></div>\
                    </td>\
                    <td>\
                        <div class="col100 r b"><span class="canchange" onclick="betList.mappingData(\'' + row.Id + '\',\'\',\'' + row.Name + '\',' + isLive + ',true);">' + row.TotalTrans + '</span></div>\
                    </td>\
                    <td>\
                        <div class="col60 c"><input type="button" onclick="javascript:betList.exportExcelDirect(\'' + row.Id + '\',\'\',\'' + row.Name + '\',' + isLive + ',true);" value="  " id="btnExport' + row.Name + '" class="btnExcel" title="Export to Excel" /></div>\
                    </td>\
                </tr>';
        return html;
    },
    renderDetailRow: function (row, groupName, cssClass) {
        var isLive = (ticketsListing.sportGroupId == 0 ? ticketsListing.LiveType : row.LiveIndicator);
        var html = '<tr class="group_' + groupName + ' ' + cssClass + '">\
                        <td>\
                            <div class="col260 l"><span class="indent" style="margin-right:6px;"></span>' + row.Name + '</div>\
                        </td>\
                        <td>\
                            <div class="col100 r"><span class="canchange" onclick="betList.mappingData(\'' + row.Id + '\',\'' + groupName + '\',\'' + row.Name + '\',' + isLive + ',false)">' + row.TotalTrans + '</span></div>\
                        </td>\
                        <td>\
                            <div class="col60 c"><input type="button" onclick="javascript:betList.exportExcelDirect(\'' + row.Id + '\',\'' + groupName + '\',\'' + row.Name + '\',' + isLive + ',false);" value="  " id="btnExport' + row.Name + '" class="btnExcel" title="Export to Excel" /></div>\
                        </td>\
                </tr>';
        return html;
    },
    toggleDetail: function (obj, groupname) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");
            $("#tblscroll tr.group_" + groupname).removeClass("hidden");
        }
        else {
            $("#tblscroll tr.group_" + groupname).addClass("hidden");
            $(obj).addClass("tree-expand");
        }
        $("#tickestListing").removeClass("hidden").reRenderScroller(429, 341, true);
    },
    changeGroup: function () {
        //ticketsListing.group = parseInt($("#ddlgroup").val());
    },
    LiveindicatorChange: function () {
        var type = $('#ddlLiveIndicator').val();
        ticketsListing.LiveType = parseInt(type);
        switch (type) {
            case '1':
                $('#txtHomeScore').val('');
                $('#txtAwayScore').val('');
                $('#chkDateTime').attr('checked', false);
                $('#txtFromdate').val('');
                $('#txtTodate').val('');
                $('.type').show();
                break;
            default:
                $('.type').hide();
                break;
        }
    },
    eventOnDateTime: function () {
        var IsChecked = $('#chkDateTime').is(':checked');
        if (!IsChecked) {
            $('#txtFromdate').val('');
            $('#txtFromdate').attr('disabled', true);
            $('#txtTodate').val('');
            $('#txtTodate').attr('disabled', true);
        }
        else {
            CURDATE = new Date();
            $('#txtFromdate').val(CURDATE.format('m/d/Y H:i:s'));
            $('#txtFromdate').attr('disabled', false);
            $('#txtTodate').val(CURDATE.format('m/d/Y H:i:s'));
            $('#txtTodate').attr('disabled', false);
        }
    }
};

var betList = {
    params: {
        fromCache: false,
        pageSize: 500,
        pageIndex: 1,
        isMain: true,
        Type: 0,
        matchID: '',
        Licensees: '',
        bettypes: '',
        bettypesLive: '',
        groupBy: 0,
        LiveType: '0',
        HomeScore: '',
        AwayScore: '',
        FromTransDate: '',
        ToTransDate: '',
        SAFilter: 0
    },
    exportParams: {
        id: '',
        groupname: '',
        name: '',
        isLive: true,
        isMain: true
    },
    pageIndex: 1,
    pageSize: 500,
    totalItem: 1000,
    reportTitle: '',
    assignMainBetlistParams: function (id, Name, isLive) {
        betList.params.pageSize = betList.pageSize;
        betList.params.pageIndex = betList.pageIndex;
        betList.params.isMain = true;
        betList.params.Type = ticketsListing.sportGroupId;
        betList.params.matchID = ticketsListing.matchID;
        //betList.params.Licensees = ticketsListing.group == 0 ? Name : ($("#ddlsite option:selected").text() == "IBC Group" ? "IBC" : $("#ddlsite option:selected").text());
        betList.params.groupBy = ticketsListing.group;
        betList.params.HomeScore = ticketsListing.HomeScore;
        betList.params.AwayScore = ticketsListing.AwayScore;
        betList.params.FromTransDate = ticketsListing.FromTransDate;
        betList.params.ToTransDate = ticketsListing.ToTransDate;
        betList.params.SAFilter = ticketsListing.SAFilter;
        betList.params.LiveType = isLive;
        /*if (ticketsListing.group == 0) {
        betList.params.bettypes = ticketsListing.listBettype;
        betList.params.bettypesLive = ticketsListing.listBettypeLive;
        }
        else {
        if (isLive == 1) {
        betList.params.bettypes = '';
        betList.params.bettypesLive = id;
        }
        else {
        betList.params.bettypes = id;
        betList.params.bettypesLive = '';
        }
        }*/
    },
    assignDetailBetlistParams: function (id, name, isLive) {
        betList.params.pageSize = betList.pageSize;
        betList.params.pageIndex = betList.pageIndex;
        betList.params.isMain = false;

        betList.params.Type = ticketsListing.sportGroupId;
        betList.params.matchID = ticketsListing.matchID;

        betList.params.HomeScore = ticketsListing.HomeScore;
        betList.params.AwayScore = ticketsListing.AwayScore;
        betList.params.FromTransDate = ticketsListing.FromTransDate;
        betList.params.ToTransDate = ticketsListing.ToTransDate;

        betList.params.LiveType = isLive;
        betList.params.SAFilter = ticketsListing.SAFilter;

        /* if (ticketsListing.group == 0) {
        betList.params.Licensees = name;

        if (ticketsListing.sportGroupId == 0) {
        betList.params.bettypes = id;
        betList.params.bettypesLive = '';
        }
        else {
        if (isLive == 1) {
        betList.params.bettypes = '';
        betList.params.bettypesLive = id;
        }
        else {
        betList.params.bettypes = id;
        betList.params.bettypesLive = '';
        }
        }

        }
        else {

        //betList.params.Licensees = id;
        if (ticketsListing.sportGroupId == 0) {
        betList.params.bettypes = name;
        betList.params.bettypesLive = '';
        }
        else {
        if (isLive == 1) {
        betList.params.bettypes = '';
        betList.params.bettypesLive = name;
        }
        else {
        betList.params.bettypes = name;
        betList.params.bettypesLive = '';
        }
        }
        }*/


    },
    mappingData: function (id, groupname, name, isLive, isMain) {
        //$("#tickestListing").addClass("hidden");
        betList.pageIndex = 1;
        betList.pageSize = 500;
        betList.exportParams.id = id;
        betList.exportParams.groupname = groupname;
        betList.exportParams.name = name;
        betList.exportParams.isLive = isLive;
        betList.exportParams.isMain = isMain;
        if (isMain) {
            betList.assignMainBetlistParams(id, name, isLive);
            betList.params.Licensees = ticketsListing.group == 0 ? name : ($("#ddlsite option:selected").text() == "IBC Group" ? "IBC" : $("#ddlsite option:selected").text());
            if (ticketsListing.group == 0) {
                betList.params.bettypes = ticketsListing.listBettype;
                betList.params.bettypesLive = ticketsListing.listBettypeLive;

            }
            else {
                if (ticketsListing.sportGroupId == 0) {
                    betList.params.bettypes = id;
                    betList.params.bettypesLive = '';
                }
                else {
                    if (isLive == 1) {
                        betList.params.bettypes = '';
                        betList.params.bettypesLive = id;
                    }
                    else {
                        betList.params.bettypes = id;
                        betList.params.bettypesLive = '';
                    }
                }
            }
        }
        else {
            betList.assignDetailBetlistParams(id, groupname, isLive);
            if (ticketsListing.group == 0) {
                betList.params.Licensees = groupname;
                if (ticketsListing.sportGroupId == 0) {
                    betList.params.bettypes = id;
                    betList.params.bettypesLive = '';
                }
                else {
                    if (isLive == 1) {
                        betList.params.bettypes = '';
                        betList.params.bettypesLive = id;
                    }
                    else {
                        betList.params.bettypes = id;
                        betList.params.bettypesLive = '';
                    }
                }
            }
            else {
                betList.params.Licensees = name;
                if (ticketsListing.sportGroupId == 0) {
                    betList.params.bettypes = groupname;
                    betList.params.bettypesLive = '';
                }
                else {
                    if (isLive == 1) {
                        betList.params.bettypes = '';
                        betList.params.bettypesLive = groupname;
                    }
                    else {
                        betList.params.bettypes = groupname;
                        betList.params.bettypesLive = '';
                    }
                }
            }
        }
        try { parent.BlockUI(); } catch (e) { }
        betList.reportTitle = betList.renderTitle(groupname, name, isLive);
        betList.renderBetlist(id, groupname, name, isLive, isMain, false);
    },
    renderBetlist: function (id, groupname, name, isLive, isMain, fromCache) {
        betList.params.fromCache = fromCache;
        //betList.params.LiveType = $('#ddlLiveIndicator').val();

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Services/TicketsListingService.svc/GetTicketsListingBetlist",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(betList.params),
            beforeSend: function (xhr) {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function (rs) {
                if (ticketsListing.sportGroupId == 2) {
                    $("#S.AColumn").addClass("hidden");
                }
                else {
                    $("#S.AColumn").removeClass("hidden");
                }

                if (rs.Items != null) {
                    betList.totalItem = parseInt(rs.totalItem);
                    if (rs.Items.length > 0) {
                        $("#tblbodybetlist").html(betList.renderListRows(rs.Items));
                        //Render Pager
                        betList.initPager(betList.totalItem, betList.params.pageSize, betList.params.pageIndex - 1);
                    }
                    else {
                        $("#pagination").html('&nbsp;');
                        if (ticketsListing.sportGroupId == 2) {
                            $("#tblbodybetlist").html('<tr><td colspan="8"><div class="c" style="width:911px;">There is no data.</div></td></tr>');
                        }
                        else {
                            $("#tblbodybetlist").html('<tr><td colspan="9"><div class="c" style="width:964px;">There is no data.</div></td></tr>');
                        }
                    }
                    //$("#tickestListing").addClass("hidden");

                    $("#gridbetlist #gridtitle").html(betList.reportTitle);
                }
                else {
                    $("#pagination").html('&nbsp;');
                    if (ticketsListing.sportGroupId == 2) {
                        $("#tblbodybetlist").html('<tr><td colspan="8"><div class="c" style="width:911px;">There is no data.</div></td></tr>');
                    }
                    else {
                        $("#tblbodybetlist").html('<tr><td colspan="9"><div class="c" style="width:964px;">There is no data.</div></td></tr>');
                    }
                }

                if (ticketsListing.sportGroupId == 2) {
                    $('#gridbetlist').removeClass("hidden").reRenderScroller(1090, 580, true);
                }
                else {
                    $('#gridbetlist').removeClass("hidden").reRenderScroller(1143, 580, true);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_item, items_per_page, current_page) {
        var opt = {
            max_items_per_page: 500,
            item_per_page_arr: [50, 100, 200, 300, 400, 500, 1000],
            callback: betList.pageSelectCallback
        };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(betList.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        betList.params.pageIndex = page_index + 1;
        betList.params.pageSize = _items_per_page;
        betList.renderBetlist(true);
        return false;
    },
    goBack: function () {
        $('#gridbetlist').addClass("hidden");
        $("#tickestListing").removeClass("hidden");
    },
    exportExcel: function () {

        betList.exportExcelDirect(betList.exportParams.id, betList.exportParams.groupname, betList.exportParams.name, betList.exportParams.isLive, betList.exportParams.isMain);
    },
    exportExcelDirect: function (id, groupname, name, isLive, isMain) {
        var Licensee = "";
        var bettype = "", bettypeLive = "";
        if (isMain) {
            betList.assignMainBetlistParams(id, name, isLive);
            if (ticketsListing.group == 0) {
                Licensee = name;
                bettype = ticketsListing.listBettype;
                bettypeLive = ticketsListing.listBettypeLive;
            }
            else {
                Licensee = $("#ddlsite option:selected").text() == "IBC Group" ? "IBC" : $("#ddlsite option:selected").text();
                if (ticketsListing.sportGroupId == 0) {
                    bettype = id;
                    bettypeLive = '';
                }
                else {
                    if (isLive == 1) {
                        bettype = '';
                        bettypeLive = id;
                    }
                    else {
                        bettype = id;
                        bettypeLive = '';
                    }
                }
            }
        }
        else {
            betList.assignDetailBetlistParams(id, groupname, isLive);
            if (ticketsListing.group == 0) {
                Licensee = groupname;
                if (ticketsListing.sportGroupId == 0) {
                    bettype = id;
                    bettypeLive = '';
                }
                else {
                    if (isLive == 1) {
                        bettype = '';
                        bettypeLive = id;
                    }
                    else {
                        bettype = id;
                        bettypesLive = '';
                    }
                }
            }
            else {
                Licensee = id;
                if (ticketsListing.sportGroupId == 0) {
                    bettype = groupname;
                    bettypeLive = '';
                }
                else {
                    if (isLive == 1) {
                        bettype = '';
                        bettypeLive = groupname;
                    }
                    else {
                        bettype = groupname;
                        bettypeLive = '';
                    }
                }
            }
        }

        betList.params.LiveType = isLive;


        var url = "TicketsListing.aspx?isexportexcel=true&";
        url += "matchID=" + betList.params.matchID;
        url += "&Licensees=" + Licensee;
        url += "&bettypes=" + bettype;
        url += "&bettypesLive=" + bettypeLive;
        url += "&Type=" + betList.params.Type;
        url += "&groupBy=" + betList.params.Type;
        url += "&Language=0";
        url += "&isMain=" + isMain;
        url += "&away=" + ticketsListing.matchInfo.away;
        url += "&home=" + ticketsListing.matchInfo.home;
        url += "&eventdate=" + ticketsListing.matchInfo.eventdate;
        url += "&matchcode=" + ticketsListing.matchInfo.matchcode;
        url += "&HomeScore=" + betList.params.HomeScore;
        url += "&AwayScore=" + betList.params.AwayScore;
        url += "&FromTransDate=" + betList.params.FromTransDate;
        url += "&ToTransDate=" + betList.params.ToTransDate;
        url += "&LiveType=" + betList.params.LiveType;
        url += "&title=" + betList.renderTitle(groupname, name, isLive);
        url += "&SAFilter=" + betList.params.SAFilter;

        $("#frm").attr("src", url);
    },
    renderListRows: function (rows) {

        var arr = [];
        var len = rows.length;
        var html = "";
        for (var i = 0; i < len; i++) {
            var row = rows[i];
            var odds = row.Odds;
            var isMixparlay = row.BettranDetail.Bettype == 9 || row.BettranDetail.Bettype == 29;

            //var transDt=row.TransDate.indexOf('&nbsp;')>=0 ? (row.TransDate.split('&nbsp;')[0] + "<br>" + row.TransDate.split('&nbsp;')[1]):row.TransDate;
            if (parseInt(row.BettranDetail.SATicket) == 0) {
                html = '<tr class="' + (row.BettranDetail.Liveindicator ? 'event-live' : '') + '" >\
                          <td><div class="col100 c">' + row.Transid + '</div></td>\
                          <td><div class="col120 c">' + row.TransDate + '</div></td>\
                          <td><div class="col100 c">' + row.Username + '</div></td>\
                          <td>\
                            <div class="col260 r">\
                                <span class="bl-choice b">' + (isMixparlay ? row.MSportEvent.Choice : row.SportEvent.Choice) + '</span>\
                                <span class="b">' + (isMixparlay ? row.MSportEvent.Hdp : row.SportEvent.Hdp) + '</span>\
								<span class="bl-choice b">' + (row.BettranDetail.Liveindicator ? (isMixparlay ? row.MSportEvent.LiveScore : row.SportEvent.LiveScore) : '') + '</span>\
								<span class="b">' + (isMixparlay ? ('@' + row.oddsM) : '') + '</span>' + (isMixparlay ? ('<br><span class="bl_bettypem">' + row.MSportEvent.BetType + '</span>') : '') + '\
                              </div>\
                          </td>\
                          <td><div class="col80 r ' + (parseFloat(row.Odds) < 0 ? 'red' : '') + '"><b>' + row.Odds + '</b>' + (row.OddName != '' ? ('<span class="bl_oddtype">&nbsp;(' + row.OddName + ')</span>') : '') + '</div></td>\
                          <td><div class="col80 r">' + row.Stake + '</div></td>\
                          <td><div class="col80 r ' + (parseFloat(row.AgentWinloss) < 0 ? 'red' : '') + '">' + row.AgentWinloss + '</div></td>\
                          <td><div class="col80 c">' + row.BettranDetail.CurrencyName + '</div></td>\
						  <td><div class="col80 c">' + row.SportEvent.BetType + '</div></td>\
                          <td><div class="col80 c">' + row.BettranDetail.Status + '</div></td>' +
                          (ticketsListing.sportGroupId != 2 ? '<td><div class="col50 c"></div></td>' : '') +
                       '</tr>';
            }
            else {
                html = '<tr style="background-color:#FFD0FF;">\
                          <td><div class="col100 c">' + row.Transid + '</div></td>\
                          <td><div class="col120 c">' + row.TransDate + '</div></td>\
                          <td><div class="col100 c">' + row.Username + '</div></td>\
                          <td>\
                            <div class="col260 r">\
                                <span class="bl-choice b">' + (isMixparlay ? row.MSportEvent.Choice : row.SportEvent.Choice) + '</span>\
                                <span class="b">' + (isMixparlay ? row.MSportEvent.Hdp : row.SportEvent.Hdp) + '</span>\
								<span class="bl-choice b">' + (row.BettranDetail.Liveindicator ? (isMixparlay ? row.MSportEvent.LiveScore : row.SportEvent.LiveScore) : '') + '</span>\
								<span class="b">' + (isMixparlay ? ('@' + row.oddsM) : '') + '</span>' + (isMixparlay ? ('<br><span class="bl_bettypem">' + row.MSportEvent.BetType + '</span>') : '') + '\
                              </div>\
                          </td>\
                          <td><div class="col80 r ' + (parseFloat(row.Odds) < 0 ? 'red' : '') + '"><b>' + row.Odds + '</b>' + (row.OddName != '' ? ('<span class="bl_oddtype">&nbsp;(' + row.OddName + ')</span>') : '') + '</div></td>\
                          <td><div class="col80 r">' + row.Stake + '</div></td>\
                          <td><div class="col80 r ' + (parseFloat(row.AgentWinloss) < 0 ? 'red' : '') + '">' + row.AgentWinloss + '</div></td>\
                          <td><div class="col80 c">' + row.BettranDetail.CurrencyName + '</div></td>\
						  <td><div class="col80 c">' + row.SportEvent.BetType + '</div></td>\
                          <td><div class="col80 c">' + row.BettranDetail.Status + '</div></td>' +
                          (ticketsListing.sportGroupId != 2 ? '<td><div style="width:27px"  class="c checkSA"></div></td>' : '') +
                '</tr>';
            }
            arr.push(html);
        }
        return arr.join('');
    },
    renderTitle: function (main, detail, isLive) {
        if (ticketsListing.group == 0) {
            return "Tickets Listing for - Match ID: " + ticketsListing.matchID + " - Licensee: " + (main == "" ? detail : main) + (main != "" ? (" - Bet type: " + detail) : "");
        }
        else {
            return "Tickets Listing for - Match ID: " + ticketsListing.matchID + " - Bet type: " + (main == "" ? detail : $("#group_" + main + "_" + isLive).html()) + (main != "" ? (" - Licensee: " + detail) : "");
        }
    }


};

function allowInt(event) {
    var code = parseInt(event.which);
    //Allow Ctrl+C/V
    var isCtrl = false;
    if (event.ctrlKey) isCtrl = true;
    if ((code == 99 || code == 118) && isCtrl) {
        return true;
    }

    if (code == 0 || code == 37 || code == 39 || (code >= 48 && code <= 57) || code == 9 || code == 8)
    { }
    else {
        event.preventDefault();
    }
}

function isInt(value) {
    if ((parseFloat(value) == parseInt(value)) && !isNaN(value) && parseFloat(value) < 2147483647) {
        return true;
    } else {
        return false;
    }
}



