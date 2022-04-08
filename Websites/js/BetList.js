function obj(id) {
    return document.getElementById(id);
}

function showMP(refno, listType, custId, transIndex) {
    var spEvent = obj('spEvent_' + refno);
    if (!spEvent) return;

    if (spEvent.className == 'hide') {
        if (spEvent.innerHTML == "") {
            BlockUI();
            $.ajax({
                url: '../Handlers/GenMixParlay.ashx?refno=' + refno + '&listType=' + listType + '&type=1' + '&custId=' + custId,
                dataType: 'html',
                success: function (data) {
                    spEvent.innerHTML = data;
                    spEvent.className = '';
                    $.unblockUI();
                },
                error: function () {
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
                },
                error: function () {
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

function showNG2Details(transId, refno, custId) {
    var width = 660;
    parent.OpenNextPopup("NG2Result", 'Report/ViewNG2ParlayResult.aspx?refno=' + refno, '', width, 500, true, false, window, 0, null, null);
}

// -------------------- Functions whose name must name by BetListCore
function showRnGCasinoDetails(transId, winlossDate, custId, transType) {
    var rngCasinoParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Saba Casino Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 209,/*[208,209,210]*/ PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(rngCasinoParam);
}

function showRNGWARBetDetails(transId, winlossDate, custId, transType) {
    var rngCasinoParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Funky Games - Detailed Bet list -" + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 224, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(rngCasinoParam);
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
    var voidbridgeParam = {
        url: "VoidbridgeBetList/BetDetails",
        title: "Macau Games Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 212, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(voidbridgeParam);
}

function showPPDetails(transId, winlossDate, custId, transType) {
    var voidbridgeParam = {
        url: "VoidbridgeBetList/BetDetails",
        title: "PP Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 248, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(voidbridgeParam);
}

function showSGDetails(transId, winlossDate, custId, transType) {
    var voidbridgeParam = {
        url: "VoidbridgeBetList/BetDetails",
        title: "Spadegaming Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 247, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(voidbridgeParam);
}

function showPGSoftDetails(transId, winlossDate, custId, transType) {
    var voidbridgeParam = {
        url: "VoidbridgeBetList/BetDetails",
        title: "PG Soft Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 232, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(voidbridgeParam);
}


function showVideoPokerGameDetails(transId, winlossDate, custId, transType) {
    var voidbridgeParam = {
        url: "VoidbridgeBetList/BetDetails",
        title: "Arcadia Gaming - Detail Tickets - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 165, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(voidbridgeParam);
}

function showIONCasinoBetDetails(refno, winlossDate, custId, transType) {
    var param = {
        url: "AGCasinoBetList/BetDetails",
        title: "ION - Detail Tickets - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 225, PageSize: 500 },
        width: 1080,
        height: 600,
        resizable: false
    };

    openResultsPopUp(param);
}

function showGlobalGaming(transId, winlossDate, custId, transType) {
    var globalGamingParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Fishing World Details",
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 219, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(globalGamingParam);
}

function showTodayRunningBetsDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "Allbet - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 211, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showCGBetDetails(transId, winlossDate, custId, transType) {
    var param = {
        url: "RNGCasinoBetList/BetDetails",
        title: "CG - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 167, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showAdvantPlayBetDetails(transId, winlossDate, custId, transType) {
    var param = {
        url: "RNGCasinoBetList/BetDetails",
        title: "AdvantPlay - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 172, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showAdvantPlayMiniBetDetails(transId, winlossDate, custId, transType) {
    var param = {
        url: "RNGCasinoBetList/BetDetails",
        title: "AdvantPlay Mini - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 174, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showMaxGameBetDetails(transId, winlossDate, custId, transType) {
    var jokerParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "MaxGame - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 237, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(jokerParam);
}

function showJokerBetDetails(transId, winlossDate, custId, transType) {
    var jokerParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Joker - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 234, PageSize: 500 },
        width: 1030,
        height: 600
    };

    openResultsPopUp(jokerParam);
}

function showHabaneroBetDetails(transId, winlossDate, custId, transType) {
    var jokerParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Habanero - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: transId, TransType: transType, CustId: custId, SportType: 238, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(jokerParam);
}

function showSexyGamingBetDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "AE Sexy - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 243, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showSAGamingDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "SA Gaming - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 244, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showGTCasinoBetsDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "IBCBet Live Casino - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 242, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showAGCasinoDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "AG Casino Details",
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}
function showWMBetDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "WM - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 233, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}
function showGDGBetDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "GDG - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 236, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}
function showBGBetDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "BG - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 239, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showSabaClubDetails(refno, winlossDate, custId, transType) {
    var rngCasinoParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Saba.games Details",
        jsonParam: { TransId: refno, Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 200, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(rngCasinoParam);
}

function showVGamingDetails(refno, winlossDate, custId, transType) {
    var rngCasinoParam = {
        url: "RNGCasinoBetList/BetDetails",
        title: "Vgaming Details",
        jsonParam: { TransId: refno, Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 171, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(rngCasinoParam);
}

function showBBINBetDetails(refno, winlossDate, custId, transType) {
    var rngCasinoParam = {
        url: "BBinBetList/BetDetails",
        title: "BBIN - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { TransId: refno, Refno: refno, CustId: custId, TransType: transType, SportType: 241, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(rngCasinoParam);
}

function ShowBonusDetails(refCode, transId) {
    var url = "BetList/ViewBonusDetails?refCode=" + refCode + "&transId=" + transId;
    parent.OpenNextPopup("TransactionDetails", url, 'Transaction Details', 1010, 480, true, false, window, 0, null, null);
}

function showProgressiveJackpotWonDetails(transId, custId, betType) {
    var title = "Progressive Jackpot Won Details";
    var param = {
        url: "RNGCasinoBetList/JackpotDetails",
        title: title,
        jsonParam: { TransId: transId, CustId: custId, BetType: betType, Title: title },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showPPJackpotWonDetails(transId, custId, betType) {
    var title = "PP Jackpot Won Details";
    var param = {
        url: "RNGCasinoBetList/JackpotDetails",
        title: title,
        jsonParam: { TransId: transId, CustId: custId, BetType: betType, Title: title },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showSGJackpotWonDetails(transId, custId, betType) {
    var title = "Spadegaming Jackpot Won Details";
    var param = {
        url: "RNGCasinoBetList/JackpotDetails",
        title: title,
        jsonParam: { TransId: transId, CustId: custId, BetType: betType, Title: title },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showVGamingJackpotWonDetails(transId, custId, betType) {
    var title = "Vgaming Jackpot Won Details";
    var param = {
        url: "RNGCasinoBetList/JackpotDetails",
        title: title,
        jsonParam: { TransId: transId, CustId: custId, BetType: betType, Title: title },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showSabaClubJackpotWonDetails(transId, custId, betType) {
    var title = "Saba.games Jackpot Won Details";
    var param = {
        url: "RNGCasinoBetList/JackpotDetails",
        title: title,
        jsonParam: { TransId: transId, CustId: custId, BetType: betType, Title: title },
        width: 1080,
        height: 600
    };

    openResultsPopUp(param);
}

function showPragmaticLiveCasinoBetsDetails(refno, winlossDate, custId, transType) {
    var title = "PP Live Casino - Detailed Bet list - " + winlossDate.split(" ")[0];
    showBetDetailsOnBettransLC(170, refno, winlossDate, custId, transType, title);
}

function showBetDetailsOnBettransLC(sportTypeId, refno, winlossDate, custId, transType, title) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: title,
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: sportTypeId, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
}

function showFGGBetDetails(refno, winlossDate, custId, transType) {
    var params = {
        url: "AGCasinoBetList/BetDetails",
        title: "FGG - Detailed Bet list - " + winlossDate.split(" ")[0],
        jsonParam: { Refno: refno, WinLossDate: winlossDate, CustId: custId, TransType: transType, SportType: 176, PageSize: 500 },
        width: 1080,
        height: 600
    };

    openResultsPopUp(params);
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

    var title = isFw ? "Fishing World Result" : "Saba Casino Result";

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
    var rngCasinoBrokenGameParam = {
        url: "RNGCasinoBetList/GetHandHistory",
        title: "Saba Casino Result",
        jsonParam: {
            Refno: refNo,
            RoleId: 0,
            MatchId: matchId,
            Race: race,
            BetType: betType,
            SportType: sportType,
            WinLostDate: winlostDate
        },
        width: 835,
        height: 675
    };

    openResultsPopUp(rngCasinoBrokenGameParam);
}

function ViewGlobalGamingResult(custId, betCheck) {
    var url = "../Report/ViewGlobalGamingResult.aspx?custId=" + custId + "&gameId=" + betCheck;
    $.ajax({
        url: url,
        beforeSend: function () { parent.OpenNextPopup("ViewGlobalGamingResult", '_loading.htm', 'Fishing World Result', 945, 500, true, false, window, 0) }
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

function ViewCashOutHistory(transId, refNo, betType, originalTransId, title) {
    var cashoutParam = {
        url: "BetList/GetCashOutDetails",
        title: title + " Details",
        jsonParam: { transId: transId, refNo: refNo, betType: betType, originalTransId: originalTransId, title: title },
        width: 840,
        height: 600
    };

    openResultsPopUp(cashoutParam);
}

function ViewNewCashOutHistory(transId, custId, winlostDate) {
    var cashoutParam = {
        url: "BetList/GetCashOutHistory",
        title: "Cash Out History",
        jsonParam: { transId: transId, custId: custId, winlostDate: winlostDate},
        width: 500,
        height: 400
    };

    openResultsPopUp(cashoutParam);
}

function ViewResult(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, betid, matchcode, betcheck, transdesc, siteId) {
    var resultParam = getResultParamBySportType(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, betid, matchcode, betcheck, transdesc, siteId);

    resultParam = resultParam || getResultParamByBetType(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, betid, matchcode);

    var handicap2 = 0
    if (isFM2(bettype)) {
        handicap2 = matchcode;
    }

    var defaultResultParam = {
        url: "Report/ViewResult.aspx",
        jsonParam: { matchid: matchid, bettype: bettype, betid: betid, sporttype: sporttype, handicap2: handicap2 },
        title: "Result",
        width: 750,
        height: 300
    };

    if (resultParam) {
        $.extend(defaultResultParam, resultParam);
    }

    //if (2501 <= bettype && bettype <= 2699) {
    //    width = 500;
    //    url = 'Report/ViewNG2Result.aspx';
    //}

    openResultsPopUp(defaultResultParam);
}

function isFM2(betttype) {
    return (betttype >= 221 && betttype <= 227)
}

function isFM2(betttype) {
    return (betttype >= 221 && betttype <= 227)
}

function getResultParamBySportType(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, currencyId, gameId3rd, custId, transId3rd, siteId) {
    switch (sporttype) {
        case 5: // Tennis.
            return getTennisParam();
        
        case 8: // Baseball.
            return getBaseballParam();
        case 50: // Cricket
            return getCricketParam(matchid, bettype, sporttype, currencyId, gameId3rd);

        case 161: // Number Game.
            return getNumberGameParam(matchid);

        case 164: // Happy5
            return getHappy5Param(matchid);

        case 167: // CG
            return getCGParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 168: // Happy5 Linked Games
            return getLinkedGamesParam(custId, refno, gameId3rd, currencyId, siteId);

        case 169: // Sports Lottery
            return getSportsLotteryParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 170: // PP Live Casino
            return getPPLiveCasinoParam(bettype, custId, transId3rd, gameId3rd, currencyId, siteId, bettype);

        case 171: // VGaming
        case 173: // VGaming
            // custId, gameroundid, gameid, siteId, sporttype, currencyname
            return getVGamingParam(custId, transId3rd, bettype, gameId3rd, sporttype, username);

        case 175: //Bitcoin
            return getBitcoinParam(matchid, sporttype);
        case 172: // AdvantPlay
            return getAdvantPlayParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 174: // AdvantPlay Mini
            return getAdvantPlayMiniParam(custId, transId3rd, gameId3rd, currencyId, siteId);
        case 181:
        case 182:
        case 183:
        case 184:
        case 185:
            return getVirtualHRParam(matchid, sporttype);

        case 186: // Virtual Tennis.
            return getVirtualHRAndTennisParam(matchid, sporttype);
        case 150:
        case 200: // Saba.games
            // custId, gameroundid, gameid, siteId
            return getSabaClubParam(custId, transId3rd, bettype, gameId3rd, sporttype);

        case 208:
        case 209:
        case 210: // RNG Casino.
            return getRNGCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 224: //Funky Games
            return getRNGWarCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 225: // ION Casino
            return getIONCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 231: // Leap Gaming
            return getLeapGamingParam(transId3rd);

        case 232: // PG Soft
            return getPGSoftCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 236: // GDG
            return getGDGParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 239: // BG
            return getBGParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 241: // BBIN
            return getBBinParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 211: // Allbet.        
        case 242: // GTCasino.
        case 243: // SexyGaming.
        case 244: // SaGaming.
        case 233: // WM
            return getLiveCasinoParam(transId3rd, gameId3rd, bettype, sporttype);

        case 212: // Macau Games.
            return getMacauGamesParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 219: // Fishing World.
            return getFishingWorldParam(gameId3rd, custId);

        case 245: //Virtual Games
            return getVirtualGamesParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 247: //Spadegaming
            return getSpadegamingParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 248: //PP
            return getPPParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 250: // AGCasino.
            return getAGCasinoParam(refno, bettype);

        case 165:
            return getCardClubParam(bettype, custId, transId3rd, gameId3rd, currencyId, siteId);

        case 234:
            return getJokerParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 237: //MaxGame
            return getMaxGameParam(custId, transId3rd, gameId3rd, currencyId, siteId, bettype, winlostdate);

        case 238:
            return getHabaneroParam(custId, transId3rd, gameId3rd, currencyId, siteId);

        case 176: // FGG
            return getFGGParam(custId, transId3rd, gameId3rd, currencyId, siteId, winlostdate);

        default:
            return undefined;
    }
}

function getResultParamByBetType(matchid, race, bettype, sporttype, refno, username, winlostdate, refno_mixparlay, league, is_outright, betid, custId) {
    switch (true) {
        case bettype === 9 || bettype === 29 || bettype === 2799: // System Parlay, MixParlayVirtualSports2
            return getSystemParlayParam(matchid, refno_mixparlay, bettype, betid);

        case bettype === 1231 || bettype === 1232 || bettype === 1233: // Virtual Horse Racing.
            return getVirtualHRAndTennisParam(matchid, sporttype);

        case 801 <= bettype && bettype <= 899: // Metric Gaming.
            return getMetricGamingParam(matchid, bettype, sporttype, betid);

        case 2501 <= bettype && bettype <= 2699:
            return getNG2Param(matchid);

        default:
            return undefined;
    }
}

function getTennisParam() {
    return {
        title: "Tennis"
    };
}

function getBaseballParam() {
    return {
        title: "Baseball"
    };
}

function getCricketParam(matchid, bettype, sporttype, betid, transdesc) {
    return {
        url: "Report/ViewResult.aspx",
        jsonParam: { matchid: matchid, bettype: bettype, betid: betid, sporttype: sporttype, transdesc: transdesc },
        title: "Result",
        width: 750,
        height: 300
    };
}

function getNumberGameParam(matchid) {
    return {
        url: "Betlist/BetListNumberGameResult.aspx",
        title: "Number Game",
        jsonParam: { matchid: matchid }
    };
}

function getBitcoinParam(matchid, sporttype) {
    return {
        url: "Report/ViewBitcoinResult.aspx",
        title: "Bitcoin",
        jsonParam: { matchid: matchid, sporttype: sporttype }
    };
}

function getHappy5Param(matchid) {
    return {
        url: "Report/ViewHappy5Result.aspx",
        title: "Happy 5",
        jsonParam: { matchid: matchid },
        width: 800,
        height: 300
    };
}

function getLeapGamingParam(transDesc) {
    return {
        url: "Report/ViewLeapGamingResult.aspx",
        title: "Result",
        jsonParam: { TransDesc: transDesc },
        width: 800,
        height: 300
    };
}

function getVirtualHRParam(matchid, sporttype) {
    var resultParam = {
        url: "Report/GetVirtualHRPopup.aspx",
        title: "",
        jsonParam: { matchid: matchid, sporttype: sporttype }
    };

    switch (sporttype) {
        case 181:
            resultParam.title = "Virtual Horse Racing";
            break;
        case 182:
            resultParam.title = "Virtual Greyhounds";
            break;
        case 183:
            resultParam.title = "Virtual Speedway";
            break;
        case 184:
            resultParam.title = "Virtual Motorsports";
            break;
        case 185:
            resultParam.title = "Virtual Cycling";
            break;
    }

    return resultParam;
}

function getVirtualHRAndTennisParam(matchid, sporttype) {
    return {
        url: "Report/GetVirtualHRPopup.aspx",
        title: "Tennis - Virtual Tennis",
        jsonParam: { matchid: matchid, sporttype: sporttype }
    };
}

function getRNGCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/RNGCasinoResult",
        title: "Saba Casino Result",
        jsonParam: {
            custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, currencyId: currencyId, siteId: siteId, productId: 6
        },
        width: 835,
        height: 675
    };
}

function getLiveCasinoParam(transdesc, betcheck, bettype, sporttype) {
    var sporttypes = {};
    sporttypes["211"] = "Allbet Result";
    sporttypes["242"] = "IBCBet Live Casino Result";
    sporttypes["243"] = "AE Sexy Result";
    sporttypes["244"] = "SA Gaming Result";
    sporttypes["233"] = "WM Result";

    var resultPageBySportTypes = {};
    resultPageBySportTypes["211"] = "Report/ViewAllbetCasinoResult.aspx";
    resultPageBySportTypes["242"] = "Report/ViewGTCasinoResult.aspx";
    resultPageBySportTypes["243"] = "Report/ViewSexyGamingResult.aspx";
    resultPageBySportTypes["244"] = "Report/ViewSAGamingResult.aspx";
    resultPageBySportTypes["233"] = "Report/ViewWMResult.aspx";

    var title = sporttypes[sporttype];
    var url = resultPageBySportTypes[sporttype];

    return {
        url: url,
        title: title,
        jsonParam: { transDesc: transdesc, matchCode: betcheck, betTypeId: bettype },
        width: 945,
        height: 500
    };
}

function getBBinParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/BBINResult",
        title: "BBIN Result",
        jsonParam:
        {
            productId: 47,
            custId: custId,
            TransId3rd: transId3rd,
            GameId3rd: gameId3rd,
            Currency: currencyId,
            SiteId: siteId
        },
        width: 945,
        height: 900
    };
}

function getRNGWarCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/RNGWarResult",
        title: "Funky Games Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, currencyId: currencyId, siteId: siteId, productId: 50},
        width: 945,
        height: 500
    };
}

function getPGSoftCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/PGSoftResult",
        title: "PG Soft Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, currencyId: currencyId, siteId: siteId, productId: 55 },
        width: 945,
        height: 800
    };
}

function getBGParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/BGResult",
        title: "BG Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, currencyId: currencyId, siteId: siteId, productId: 57 },
        width: 850,
        height: 800
    };
}

function getFGGParam(custId, transId3rd, gameId3rd, currencyId, siteId, endTime) {
    return {
        url: "ThirdParty/FGGResult",
        title: "FGG Result",
        jsonParam:
        {
            custId: custId,
            transId3rd: transId3rd,
            GameId3rd: gameId3rd,
            EndTime: endTime,
            currencyId: currencyId,
            siteId: siteId,
            productId: 70
        },
        width: 850,
        height: 800
    };
}

function getGDGParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/GDGResult",
        title: "GDG Result",
        jsonParam: { custId: custId, TransId3rd: transId3rd, GameId3rd: gameId3rd, productId: 59, siteId: siteId, currencyid: currencyId },
        width: 850,
        height: 800
    };
}

function getHabaneroParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/HabaneroResult",
        title: "Habanero Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, productId: 61, siteId: siteId, CurrencyId: currencyId },
        width: 850,
        height: 800
    };
}

function getCGParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/CGResult",
        title: "CG Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, productId: 62, siteId: siteId, currenctyId: currencyId },
        width: 850,
        height: 800
    };
}

function getAdvantPlayParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/AdvantPlayResult",
        title: "AdvantPlay Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, productId: 67, siteId: siteId, currenctyId: currencyId },
        width: 850,
        height: 800
    };
}

function getAdvantPlayMiniParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/AdvantPlayMiniResult",
        title: "AdvantPlay Mini Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 68},
        width: 850,
        height: 800
    };
}

function getCardClubParam(bettype, custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/CardClubResult",
        title: "Arcadia Gaming Result",
        jsonParam: { BetTypeId: bettype, custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 34 },
        width: 850,
        height: 800
    };
}

function getVGamingParam(custId, gameRoundId, gameId, siteId, sportType, currencyName) {

    return {
        url: "../Report/ViewVGamingResult.aspx",
        title: "Vgaming Result",
        jsonParam: { custId: custId, GameRoundId: gameRoundId, GameId: gameId, SiteId: siteId, SportType: sportType, CurrencyName: currencyName },
        width: 945,
        height: 500,
        iframe: "innerFrameVgamingResult"
    };
}

function getSabaClubParam(custId, gameRoundId, gameId, siteId, sportType) {

    return {
        url: "../Report/ViewSabaClubResult.aspx",
        title: "Saba.games Result",
        jsonParam: { custId: custId, GameRoundId: gameRoundId, GameId: gameId, SiteId: siteId, SportType: sportType },
        width: 945,
        height: 500,
        iframe: "innerFrameSabagamesResult"
    };
}

function getMacauGamesParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/VoidbridgeResult",
        title: "Macau Games Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 22},
        width: 945,
        height: 500
    };
}

function getIONCasinoParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/IONResult",
        title: "ION Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 52 },
        width: 945,
        height: 500
    };
}

function getSpadegamingParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/SpadegamingResult",
        title: "Spadegaming Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 36},
        width: 945,
        height: 500
    };
}

function getVirtualGamesParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/VirtualGamesResult",
        title: "Virtual Games Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, siteId: siteId, currenctyId: currencyId, productId: 38 },
        width: 945,
        height: 500
    };
}

function getMaxGameParam(custId, transId3rd, gameId3rd, currencyId, siteId, betType, endTime) {
    return {
        url: "ThirdParty/MaxGameResult",
        title: "MaxGame Result",
        jsonParam:
        {
            productId: 60,
            custId: custId,
            transId3rd: transId3rd,
            gameId3rd: gameId3rd,
            BetTypeId: betType,
            EndTime: endTime,
            SiteId: siteId,
            CurrencyId: currencyId
        },
        width: 945,
        height: 500
    };
}

function getPPLiveCasinoParam(bettype, custId, transId3rd, gameId3rd, currencyId, siteId, bettype) {
    return {
        url: "ThirdParty/PPLiveCasinoResult",
        title: "PP Live Casino Result",
        jsonParam:
        {
            productId: 65,
            custId: custId,
            gameId3rd: gameId3rd,
            TransId3rd: transId3rd,
            betType: bettype,
            CurrencyId: currencyId,
            SiteId: siteId,
            BetTypeId: bettype
        },
        width: 945,
        height: 500
    };
}

function getSportsLotteryParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/SportsLotteryResult",
        title: "Sports Lottery Result",
        jsonParam: { productId: 63, custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, CurrencyId: currencyId, SiteId: siteId },
        width: 945,
        height: 800
    };

}
function getLinkedGamesParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/LinkedGamesResult",
        title: "Happy 5 Linked Games Result",
        jsonParam: { productId: 3, custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, CurrencyId: currencyId, SiteId: siteId},
        width: 945,
        height: 800
    };
}


function getPPParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/PPResult",
        title: "PP Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, productId: 32, CurrencyId: currencyId, SiteId: siteId },
        width: 945,
        height: 500
    };
}

function getJokerParam(custId, transId3rd, gameId3rd, currencyId, siteId) {
    return {
        url: "ThirdParty/JokerResult",
        title: "Joker Result",
        jsonParam: { custId: custId, transId3rd: transId3rd, gameId3rd: gameId3rd, currencyId: currencyId, productId: 56, SiteId: siteId },
        width: 945,
        height: 800
    };
}

function getFishingWorldParam(matchcode, betcheck) {
    return {
        url: "../Report/ViewGlobalGamingResult.aspx",
        title: "Fishing World Result",
        jsonParam: { custId: betcheck, gameId: matchcode },
        width: 945,
        height: 500,
        iframe: "innerFrameFishingWorldResult"
    };
}

function getSystemParlayParam(matchid, refno, bettype, betid) {
    return {
        url: "Report/ViewMixParlayResult.aspx",
        title: bettype === 9 ? "System Parlay" : "Mix Parlay",
        jsonParam: { refno: refno, matchid: matchid, bettype: bettype, betid: betid },
        width: 945,
        height: 500
    };
}

function getNG2Param(matchId) {
    return {
        url: "Report/ViewNG2Result.aspx",
        title: "Result",
        jsonParam: { matchId: matchId },
        width: 500,
        height: 500
    };
}

function getMetricGamingParam(matchid, bettype, sporttype, betid) {
    return {
        url: "Report/ViewMetricGamingResult.aspx",
        title: "Result",
        jsonParam: { matchid: matchid, bettype: bettype, betid: betid, sporttype: sporttype },
        width: 750,
        height: 500
    };
}

function getAGCasinoParam(transId, bettype) {
    return {
        url: "Report/ViewAGCasinoResult.aspx",
        title: "AG Casino Result",
        jsonParam: { transId: transId, BetType: bettype },
        width: 750,
        height: 500
    };
}

function openResultsPopUp(resultParam) {
    var url = resultParam.url + "?" + $.param(resultParam.jsonParam);
    var width = resultParam.width || 600;
    var height = resultParam.height || 500;
    var popupName = resultParam.title.replace(/[^a-z0-9\s]/gi, "").replace(/\s/g, "").replace(/\//g, "").replace(".", "");
    var resizable = resultParam.resizable == undefined ? true : resultParam.resizable;

    if (resultParam.iframe) {
        $.ajax({
            url: url,
            beforeSend: function () {
                parent.OpenNextPopup(popupName, "_loading.htm", resultParam.title, width, height, true, true, window, 0);
            }
        }).complete(function (result) {
            if (result.responseText.indexOf("<table") < 0) {
                parent.document.getElementById(resultParam.iframe).src = result.responseText;
            } else {
                parent.document.getElementById(resultParam.iframe).contentDocument.write(result.responseText);
            }
        });
    } else {
        parent.OpenNextPopup(popupName, url, resultParam.title, width, height, true, resizable, window, 0, null, null);
    }
}