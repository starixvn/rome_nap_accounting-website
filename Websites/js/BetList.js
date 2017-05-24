function obj(id) {
    return document.getElementById(id);
}

function showMP(refno, listType) {
    var spEvent = obj('spEvent_' + refno);
    if (!spEvent) return;

    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function (data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    $.unblockUI();
                }
                , error: function () {
                    $.unblockUI();
                }
            });

        }
        else {
            spEvent.className = '';
        }
    }
    else {
        spEvent.className = 'hide';
    }
}

function showCMP(event, refno, listType) {
    var spEvent = obj('spEvent_' + refno);
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("td:first").find("div").each(function (index, element) {
        if ($(element).attr("id").indexOf("spEvent") >= 0) {
            spEvent = element;
        }
    });
    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1',
                dataType: 'html',
                success: function (data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';

                    $(event.target).parents("tr:first").find("td").each(function (index, element) {
                        element.style.verticalAlign = "top";
                    });

                    $.unblockUI();
                }
                , error: function () {
                    $.unblockUI();
                }
            });

        }
        else {
            spEvent.className = '';
            $(event.target).parents("tr:first").find("td").each(function (index, element) {
                element.style.verticalAlign = "top";
            });
        }
    }
    else {
        spEvent.className = 'hide';
        $(event.target).parents("tr:first").find("td").each(function (index, element) {
            element.style.verticalAlign = "middle";
        });
    }
}

function showDetailsCMP(refno, title, transid, betid, custid) {
    //OpenPopup(url, title, width, height, modal, resizable, currWindow, duration, fnRecall, fnOnClose) {
    if (custid == null) {
        parent.OpenNextPopup("systemPartlay", "BetList/CombinationMPDetail.aspx?refno=" + refno + "&transid=" + transid + "&betid=" + betid, title, 720, 570, true, false, window, 0, null, null);
    }
    else {
        parent.OpenNextPopup("systemPartlay", "BetList/CombinationMPDetail.aspx?refno=" + refno + "&transid=" + transid + "&betid=" + betid + "&custid=" + custid, title, 720, 500, true, false, window, 0, null, null);
    }
}

function cmpMouseOutDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("table:first").find("div").get(0).style.backgroundPosition = "0px 0px";
}

function cmpMouseOverDetailButton(event) {
    var target = event.target ? event.target : event.srcElement;
    $(target).parents("table:first").find("div").get(0).style.backgroundPosition = "0px -14px";
}

function confirm_delete(betid, betfrom, matchid, bettype) {
    var url = window.rootUrl + 'Betlist/DeleteBet.aspx?betid=' + betid + '&betfrom=' + betfrom + '&matchid=' + matchid + '&bettype=' + bettype;
    var NewWindow = window.open(url, 'DeleteBet', 'toolbar=no,location=no,status=no,menubar=no,resizable=yes,scrollbars=yes,width=720,height=450,left=400');
    if (NewWindow) NewWindow.focus();
}

function OpenByIP(custid, fdate, ip, type) {
    //window.location= 'BetList.aspx?custid='+custid+'&fdate='+fdate+'&ip='+ip+'&bltype=3&type='+type;
    try { parent.BlockUI(); } catch (e) { }
    var urlLink = '../Handlers/GenBetList.ashx?custid=' + custid + '&requesttype=4' + '&fdate=' + fdate + '&ip=' + ip + '&type=' + type;
    GeneralAjaxCall(urlLink);
}

        width = 750;
        height = 200;
        url = 'Report/ViewLucky3Result.aspx';
        title = "Lucky 3";
    }

    parent.OpenNextPopup(
        "VirtualResult",
        url + '?matchid=' + matchid + (bettype ? '&bettype=' + bettype : '') + '&betid=' + betid + '&sporttype=' + sporttype,
        title,
        width,
        height,
        true,
        false,
        window,
        0,
        null,
        null);
function ViewResultMetricGaming(
    matchid,
    bettype,
    betid,
    sporttype,
    sportName,
    leagueName) {
    var width = 750;
    var params = $.param({
        matchid: matchid,
        bettype: bettype,
        betid: betid,
        sporttype: sporttype
    });

    parent.OpenNextPopup(
        "VirtualResult",
        'Report/ViewMetricGamingResult.aspx?' + params,
        "Result",
        width,
        500,
        true,
        false,
        window,
        0,
        null,
        null);
}

//function ViewResultNumberGame(url) {
//    parent.OpenNextPopup("NumberGamePopup", url, 'Number Game', 750, 200, true, false, window, 0, null, null);
//}

function ViewMicroGamingPopup(custId, siteName, gameId) {
    var width = 750;
    var params = $.param({
        custId: custId,
        siteName: siteName,
        gameId: gameId
    });

    parent.OpenNextPopup(
        "MicrogamingResult",
        'Report/ViewMicroGamingResult.aspx?' + params,
        "Microgaming Result",
        width,
        500,
        true,
        false,
        window,
        0,
        null,
        null);
}

function ViewAGCasinoPopup(transId, betType) {
    var width = 750;
    var params = $.param({
        transId: transId,
        BetType: betType
    });

    parent.OpenNextPopup(
        "AGCasinoResult",
        'Report/ViewAGCasinoResult.aspx?' + params,
        "AG Casino Result",
        width,
        500,
        true,
        false,
        window,
        0,
        null,
        null);
}

function ViewResultLiveCasino(url) {
    parent.OpenNextPopup("LiveCasinoPopup", url, 'Live Casino', 760, 200, true, false, window, 0, null, null);
}

function viewFinanceResult(transid) {
    var width = 420;
    if (parent.ibcWnd)
        parent.ibcWnd.open('Report/ViewFinanceResult.aspx?transid=' + transid, '', 50, 100, width, 200, 'class1', 'class2');
    else
        ibcWnd.open('../Report/ViewFinanceResult.aspx?transid=' + transid, '', 50, 100, width, 200, 'class1', 'class2');
}

function ViewMainReportByBettype(fdate, tdate, bettype) {
    var urlLink = '../Handlers/GenBetList.ashx?fdate=' + fdate + '&tdate=' + tdate + '&bettype=' + bettype + '&requesttype=1';
    GeneralAjaxCall(urlLink);
}

function ViewMainReportByBettype2(fdate, tdate, bettype, page) {
    var urlLink = '../Handlers/GenBetList.ashx?fdate=' + fdate + '&tdate=' + tdate + '&bettype=' + bettype + '&requesttype=1' + '&page=' + page;
    GeneralAjaxCall(urlLink);
}

function ViewMainReportByTransID(transid) {
    var urlLink = '../Handlers/GenBetList.ashx?transid=' + transid + '&requesttype=2';
    GeneralAjaxCall(urlLink);
}

function ViewMainReportByTransIDFromCheckBet(transid, custId) {
    var urlLink = '../Handlers/GenBetList.ashx?transid=' + transid + '&custId=' + custId + '&requesttype=2';
    GeneralAjaxCall(urlLink);
}

function ViewMainReportByTransID(transid, page) {
    var urlLink = '../Handlers/GenBetList.ashx?transid=' + transid + '&requesttype=2' + '&page=' + page;
    GeneralAjaxCall(urlLink);
}

function ViewMainReport() {
    try { parent.BlockUI(); } catch (e) { }
    var requesttype = 1;
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;
    if (rdTransID == true) {
        requesttype = 2;
        var transid = obj('txtTransID').value;
        if (transid != "") {
            if (isNaN(transid) == true) {
                alert('Ref No should be a number!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else if (IsNumeric(transid) == false) {
                alert('Ref No should be a big Integer!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else

                ViewMainReportByTransID(transid);
        }
        else {
            alert('Please input a Ref No!');
            obj('txtTransID').focus();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    }
    else {
        requesttype = 1;
        var bettype = obj('dlBettype').value;
        var fdate = obj('fdate').value;
        var tdate = obj('tdate').value;
        ViewMainReportByBettype(fdate, tdate, bettype);
        obj('txtTransID').value = '';
    }
}

function IsNumeric(sText) {
    var ValidChars = "0123456789";
    var IsNumber = true;
    var Char;


    for (i = 0; i < sText.length && IsNumber == true; i++) {
        Char = sText.charAt(i);
        if (ValidChars.indexOf(Char) == -1) {
            IsNumber = false;
        }
    }
    return IsNumber;

}

function ViewMainReport2(page) {
    try { parent.BlockUI(); } catch (e) { }

    var requesttype = 1;
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;

    if (rdTransID == true) {
        requesttype = 2;
        var transid = obj('txtTransID').value;
        if (transid != "") {
            if (isNaN(transid) == true) {
                alert('Ref No should be a number!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else if (IsNumeric(transid) == false) {
                alert('Ref No should be a big Integer!');
                obj('txtTransID').focus();
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else
                ViewMainReportByTransID(transid, page);
        }
        else {
            alert('Please input a Ref No!');
            obj('txtTransID').focus();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    }
    else {
        requesttype = 1;
        var bettype = obj('dlBettype').value;
        var fdate = obj('fdate').value;
        var tdate = obj('tdate').value;
        ViewMainReportByBettype2(fdate, tdate, bettype, page);
        obj('txtTransID').value = '';
    }
}

function DoReset() {
    obj('rdBetType').checked = true;
    obj('txtTransID').value = '';
    obj('dlBettype').selectedIndex = 0;
    reset_date();
    BL_ChangeOption();
    ChangeDateTime();
}

function PreSubmit(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13)
        obj("btnSubmit").click();
}

String.prototype.trimLeft = function () {
    return this.replace(/^\s*/, "");
}

String.prototype.trimRight = function () {
    return this.replace(/\s*$/, "");
}

String.prototype.trim = function () {
    return this.trimRight().trimLeft();
}

function ViewBLIPReport() {
    try { parent.BlockUI(); } catch (e) { }
    var ip = obj('ip').value;
    if (ip.trim() == "") {
        showAlertMessage('errMsg', 'Please input IP!', 1);
        obj('ip').focus();
        try { parent.jQuery.unblockUI(); } catch (e) { }
    }
    else {
        $('#errMsg').html('');
        var urlLink = '../Handlers/GenBetList.ashx?ip=' + ip + '&requesttype=3' + '&fdate=' + obj('fdate').value;
        GeneralAjaxCall(urlLink);
    }
}

function ViewBLByStatement(fdate) {
    try { parent.BlockUI(); } catch (e) { }
    var userName = gup("custname");
    var transTypeId = gup("transtypeid");

    var urlLink = '../Handlers/GenBetList.ashx?requesttype=5' + '&fdate=' + fdate + "&username=" + userName + "&transtypeid=" + transTypeId;
    GeneralAjaxCall(urlLink);
}

function GeneralAjaxCall(urlLink) {
    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function (data) {
            obj('sContent').innerHTML = data;

            $(".pagenav_ibtn").click(function () {
                if (isNumberNotNull("txtpage")) {
                    var pageIndex = $("#txtpage").val();
                    GotoPage(pageIndex);
                }
            });

            $("#divcontainer").removeClass("allhidden");
            //check heigh of table
            var tbl = $("#btable");
            if (tbl.height() < 560) {
                //$("#btable").width("905px");
                //$("#htable").width("905px");
                //$(".ui-jqgrid-sdiv").css("margin-top", "-1px");

                //                $("#tdSTitle").width("685px");
                //                $("#tdSWL").width("105px");
                //                $("#tdSEnd").width("170px");

                //                $("#tdIpTitle").width("724px");
                //                $("#tdIpTotalWin").width("100px");
                //                $("#tdIpEnd").width("165px");
                //$("#tfoot").width("905px");
            }
            //-------

            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function GotoPage1() {
    var txtPage = obj('txtpage');
    if (txtPage) {
        GotoPage(txtPage.value);
    }
}

function GotoPage(num) {
    ViewMainReport2(num);
}

function BL_ChangeOption() {
    var rdTransID = obj('rdTransID').checked;
    var rdBettype = obj('rdBetType').checked;
    if (rdTransID == true) {
        obj('dlBettype').disabled = true;
        obj('spanFromDate').disabled = true;
        obj('spanToDate').disabled = true;
        obj('fdate_trigger').disabled = true;
        obj('tdate_trigger').disabled = true;
        obj('txtTransID').disabled = false;
    }
    else {
        obj('dlBettype').disabled = false;
        obj('spanFromDate').disabled = false;
        obj('spanToDate').disabled = false;
        obj('fdate_trigger').disabled = false;
        obj('tdate_trigger').disabled = false;
        obj('txtTransID').disabled = true;
    }
}

function ChangeDateTime() {
    var fdate = obj('fdate').value;
    var tdate = obj('tdate').value;
    var urlLink = '../Handlers/GenBetList.ashx?requesttype=6' + '&fdate=' + fdate + '&tdate=' + tdate;
    $.ajax({
        url: urlLink + '&nocache=' + new Date().getTime(),
        dataType: 'html',
        success: function (data) {
            if (data != "") {
                obj('spanBettype').innerHTML = "<select id='dlBettype'><option value='0'>All</option>" + data + "</select>";
                obj('bettypeArea').style.display = 'inline';
            }
            else
                obj('bettypeArea').style.display = 'none';
        }
    });
}

function reset_date() {
    var dayNoReset = 7;
    var fdateId = 'fdate';
    var tdateId = 'tdate';
    var td = new Date();
    var fd = new Date(td.getTime() - dayNoReset * 24 * 60 * 60 * 1000);
    document.getElementById(fdateId).value = (fd.getMonth() + 1) + "/" + fd.getDate() + "/" + fd.getFullYear();
    document.getElementById(tdateId).value = (td.getMonth() + 1) + "/" + td.getDate() + "/" + td.getFullYear();
}

function GetBetListFromCheckBet(transId, custId) {
    $('#boxheader').addClass('hidden');
    $('#maincontent_betlist').addClass('hidden');
    $('#maincontent_systemparlay').addClass('hidden');
    ViewMainReportByTransIDFromCheckBet(transId, custId);

}

function ExportBetListToExcel() {
    //try { parent.BlockUI(); } catch (e) { }
    var fdate = gup("fdate");
    var userName = gup("custname");
    var transTypeId = gup("transtypeid");

    var downloadUrl = 'ExportExcel.aspx?requesttype=5' + '&fdate=' + fdate + "&username=" + userName + "&transtypeid=" + transTypeId;
    var iframe = $('<iframe width="0px" height="0px" class="display-none"></iframe>');
    iframe.appendTo('body').attr('src', downloadUrl);
    setTimeout(function () { iframe.remove(); }, 5000);
    // GeneralAjaxCall(urlLink);
}

function showColossusBetsDetails(refno, winlossDate, transId, custId, eventId) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        TransId: transId,
        CustId: custId,
        EventId: eventId,
        RoleId: 0,
        AccId: _accId
    });

    var width = $(window).width();
    var widthForDetail = 1120;
    if (width > widthForDetail) {
        width = widthForDetail;
    }

    parent.OpenNextPopup(
        "ColossusBetsDetails",
        'ColossusBets/BetDetails?' + params,
        'Ticket details of ' + refno,
        width,
        600,
        true,
        true,
        window,
        0,
        null,
        function () {
            parent.$("#innerFrameColossusBetsDetails").contents().find(".memberchoice-betdetail").addClass("display-none");
        });
}

function showMicroGamingRnGBetsDetails(refno, winlossDate, custId, transType) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        TransType: transType,
        CustId: custId,
        SportType: 205,
        PageSize: 500
    });

    parent.OpenNextPopup(
        "MicrogamingDetails",
        'MicroGamingBetList/BetDetails?' + params,
        'Microgaming Details',
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

function showAGCasinoDetails(refno, winlossDate, custId, transType) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        TransType: transType,
        CustId: custId
    });

    parent.OpenNextPopup(
        "AGCasinoDetails",
        'AGCasinoBetList/BetDetails?' + params,
        'AG Casino Details',
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

function ViewAllBetResult(transDesc, betCheck, betTypeId) {
    var width = 945;
    var url = 'Report/ViewAllBetResult.aspx?transDesc=' + transDesc + "&matchCode=" + betCheck + "&betTypeId=" + betTypeId;
    parent.OpenNextPopup("ViewAllBetResult", url, 'Allbet Result', width, 500, true, false, window, 0, null, null);
}

function showNG2Details(transId, refno, custId) {
    var width = 660;
    parent.OpenNextPopup("NG2Result", 'Report/ViewNG2ParlayResult.aspx?refno=' + refno, '', width, 500, true, false, window, 0, null, null);
}

function showTodayRunningBetsDetails(refno, winlossDate, custId, transType) {
    var params = $.param({
        Refno: refno,
        WinLossDate: winlossDate,
        CustId: custId,
        TransType: transType
    });

    parent.OpenNextPopup(
        'AllBetDetails',
        'AGCasinoBetList/BetDetails?' + params,
        'AllBet - Detailed Bet list - ' + winlossDate.split(' ')[0],
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

// -------------------- Functions whose name must name by BetListCore
function showRnGCasinoDetails(transId, winlossDate, custId, transType) {
    var params = $.param({
        TransId: transId,
        TransType: transType,
        CustId: custId,
        SportType: 209, //[208,209,210]
        PageSize: 500
    });

    parent.OpenNextPopup(
        "RNGCasinoDetails",
        'RNGCasinoBetList/BetDetails?' + params,
        "RNG Casino Details",
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

function ViewBettradeDetails(transId, refNo) {
    var betTradeParam = {
        url: "Betlist/GetBetListBetTradeDetails",
        title: "Details",
        jsonParam: { TransId: transId, Refno: refNo },
        width: 600,
        height: 200
    };

    openResultsPopUp(betTradeParam);
}

function showVoidBridgeDetails(transId, winlossDate, custId, transType) {
    var params = $.param({
        TransId: transId,
        TransType: transType,
        CustId: custId,
        SportType: 212,
        PageSize: 500
    });

    parent.OpenNextPopup(
        "VoidbridgeDetails",
        'VoidbridgeBetList/BetDetails?' + params,
        "Macau Games Details",
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

function showGlobalGaming(transId, winlossDate, custId, transType) {
    var params = $.param({
        TransId: transId,
        TransType: transType,
        CustId: custId,
        SportType: 219,
        PageSize: 500
    });

    parent.OpenNextPopup(
        "RNGCasinoDetails",
        'RNGCasinoBetList/BetDetails?' + params,
        "Fishing World Details",
        1030,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}

// -------------------- Functions whose name must name by Fanex.Toco
function ViewRNGCasinoResult(matchId, race, betType, sportType, refNo, winlostDate, custId) {
    var params = $.param({
        Refno: refNo,
        RoleId: 0,
        MatchId: matchId,
        Race: race,
        BetType: betType,
        SportType: sportType,
        WinLostDate: winlostDate
    });

    var isFw = betType >= 1011 && betType <= 1020;

    var title = isFw ? "Fishing World Result" : "RNG Casino Result";

    if (isFw === true) {
        ViewGlobalGamingResult(custId, refNo);
    } else {
        parent.OpenNextPopup(
            "RNGCasinoResult",
            'RNGCasinoBetList/GetHandHistory?' + params,
            title,
            835,
            675,
            true,
            false,
            window,
            0,
            null,
            null);
    }
}

function ViewRNGCasinoBrokenGame(matchId, race, betType, sportType, refNo, winlostDate) {
    var params = $.param({
        Refno: refNo,
        RoleId: 0,
        MatchId: matchId,
        Race: race,
        BetType: betType,
        SportType: sportType,
        WinLostDate: winlostDate
    });

    parent.OpenNextPopup(
        "RNGCasinoResult",
        'RNGCasinoBetList/GetHandHistory?' + params,
        "RNG Casino Result",
        835,
        675,
        true,
        false,
        window,
        0,
        null,
        null);
}

function ViewGlobalGamingResult(custId, betCheck) {
    var url = "../Report/ViewGlobalGamingResult.aspx?custId=" + custId + "&gameId=" + betCheck;
    $.ajax({
        url: url,
        beforeSend: function () { parent.OpenNextPopup("ViewGlobalGamingResult", '_loading.htm', 'Fishing Wolrd Result', 945, 500, true, false, window, 0) }
    }).complete(function (result) {
        if (result.responseText.indexOf("<table") < 0) {
            parent.document.getElementById('innerFrameViewGlobalGamingResult').src = result.responseText;
        }
        else {
            parent.document.getElementById('innerFrameViewGlobalGamingResult').contentDocument.write(result.responseText);
        }
    });
}

function ViewVoidbridgeBrokenGame(custId, betCheck) {
    var params = $.param({
        custId: custId,
        betCheck: betCheck
    });

    var url = '../Report/ViewVoidbridgeResult.aspx?';
    $.ajax({ url: url + params, beforeSend: function () { parent.OpenNextPopup("ViewVoidbridgeResult", '_loading.htm', 'Macau Games Result', 945, 500, true, false, window, 0) } }).complete(function (result) {
        if (result.responseText.indexOf("<table") < 0) {
            parent.document.getElementById('innerFrameViewVoidbridgeResult').src = result.responseText;
        }
        else {
            parent.document.getElementById('innerFrameViewVoidbridgeResult').contentDocument.write(result.responseText);
        }
    });
}

function ViewVoidbridgeResult(custId, betCheck) {
    var url = '../Report/ViewVoidbridgeResult.aspx?custId=' + custId + '&betCheck=' + betCheck;
    $.ajax({ url: url, beforeSend: function () { parent.OpenNextPopup("ViewVoidbridgeResult", '_loading.htm', 'Macau Games Result', 945, 500, true, false, window, 0) } }).complete(function (result) {
        if (result.responseText.indexOf("<table") < 0) {
            parent.document.getElementById('innerFrameViewVoidbridgeResult').src = result.responseText;
        }
        else {
            parent.document.getElementById('innerFrameViewVoidbridgeResult').contentDocument.write(result.responseText);
        }
    });
}

function ViewFishingWorldResult(custId, betCheck) {
    ViewGlobalGamingResult(custId, betCheck);
}

// --------------------
function ViewResult(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, betid) {
    var resultParam;

    if (sporttype === 161) { // Number Game.
        resultParam = getNumberGameParam(matchid);
    }
    else if (181 <= sporttype && sporttype <= 185) {
        resultParam = getVirtualHRParam(matchid, race, sporttype);
    }
    else if (sporttype === 186) { // Virtual Tennis.
        resultParam = getVirtualTennisParam(matchid, race, sporttype);
    }
    else {
        if (bettype === 9 || bettype === 29) { // System Parlay
            resultParam = getSystemParlayParam(matchid, refno_mixparlay, bettype, betid);
        }
        else if (bettype === 31 || bettype === 32 || bettype === 31 ||
            bettype === 41 || bettype === 42 || bettype === 43) { // Horse Racing.
            resultParam = getHRParam(race, winlostdate, league, betid);
        }
        else if (bettype === 1701 || bettype === 1702 || bettype === 1703) { // Gold Deluxe.
            resultParam = getGoldDeluxeParam(bettype, refno);
        }
            //else if (bettype === 1231 || bettype === 1232 || bettype === 1233) { // Virtual Horse Racing.
            //    //resultParam = viewVirtualHRResult(matchid, race, sporttype);
            //}
        else {
            resultParam = {
                url: "Report/ViewResult.aspx",
                jsonParam: { matchid: matchid, bettype: bettype, betid: betid, sporttype: sporttype },
                title: "Result"
            };
        }
    }

    //if (2501 <= bettype && bettype <= 2699) {
    //    width = 500;
    //    url = 'Report/ViewNG2Result.aspx';
    //}

    openResultsPopUp(resultParam);
}

function getNumberGameParam(matchid) {
    return {
        url: "Betlist/BetListNumberGameResult.aspx",
        title: "Number Game",
        jsonParam: { matchid: matchid },
        width: 750,
        height: 200
    };
}

function getVirtualTennisParam(matchid, race, sporttype) {
    return {
        url: "Report/GetVirtualHRPopup.aspx",
        title: "Tennis - Virtual Tennis",
        jsonParam: { matchid: matchid, raceno: race, sporttype: sporttype },
        width: 700,
        height: 180
    };
}

function getVirtualHRParam(matchid, race, sporttype) {
    var resultParam = {
        url: "Report/GetVirtualHRPopup.aspx",
        title: "",
        jsonParam: { matchid: matchid, raceno: race, sporttype: sporttype },
        width: 750,
        height: 200
    };

    switch (sporttype) {
        case 181:
            resultParam.title = "Virtual Horse Racing";
            break;
        case 182:
            resultParam.title = "Virtual Greyhound";
            break;
        case 183:
            resultParam.title = "Virtual Speedway";
            break;
        case 184:
            resultParam.title = "Virtual Motor Racing";
            break;
        case 185:
            resultParam.title = "Virtual Cycling";
            break;
    }

    return resultParam;
}

function getSystemParlayParam(matchid, refno, bettype, betid) {
    return {
        url: "Report/ViewMixParlayResult.aspx",
        title: "System Parlay",
        jsonParam: { refno: refno, matchid: matchid, bettype: bettype, betid: betid },
        width: 945,
        height: 500
    };
}

function getHRParam(race, winlostdate, league, betid) {
    return {
        url: "Report/HRRaceResult.aspx",
        title: "Race Result",
        jsonParam: { leagueid: league, raceno: race, eventdate: winlostdate, hrbettype: betid },
        width: 750,
        height: 250
    };
}

function getGoldDeluxeParam(bettype, refno) {
    return {
        url: "Report/ViewGoldDeluxeResult.aspx",
        title: "Gold Deluxe",
        jsonParam: { transId: refno, bettype: bettype },
        width: 945,
        height: 500
    };
}

function openResultsPopUp(resultParam) {
    var url = resultParam.url + "?" + $.param(resultParam.jsonParam);
    var width = resultParam.width || 600;
    var height = resultParam.height || 500;
    var popupName = resultParam.title.replace(/\s/g, "");

    parent.OpenNextPopup(popupName, url, resultParam.title, width, height, true, false, window, 0, null, null);
}

function ViewCashOutDetails(transId, refNo, betType, originalTransId, title) {
    var params = $.param({
        transId: transId,
        refNo: refNo,
        betType: betType,
        originalTransId: originalTransId,
        title: title
    });

    parent.OpenNextPopup(
        "CashOutDetails",
        "BetList/GetCashOutDetails?" + params,
        title + " Details",
        840,
        600,
        true,
        false,
        window,
        0,
        null,
        null);
}