$(function () {
    var scroller = $('#body-scroll-detail');
    preTop = scroller.scrollTop();
    preLeft = scroller.scrollLeft();
    scroller.scroll(function () {
        var top = $(this).scrollTop();
        if (preTop != top) {
            $("#body-frozen-detail").attr("scrollTop", top);
            preTop = top;
            return;
        }
        var left = $(this).scrollLeft();
        if (preLeft != left) {
            $("#header-scroll-detail").attr("scrollLeft", left);
            $("#footer-scroll-detail").attr("scrollLeft", left);
            preLeft = left;
        }
    });
 
});



function showhiderow(obj, className) {
    if ($(obj).hasClass("tree-expand")) {
        $(obj).removeClass("tree-expand");
        $("#tblmatchwinlost tr." + className).removeClass("hidden");
        $("#tblmatchwinlost tr." + className + " span.tree-expanded").removeClass("tree-expand");
    }
    else {
        $("#tblmatchwinlost tr." + className).addClass("hidden");
        $(obj).addClass("tree-expand");
    }
}

function viewdetail(_id, _bettypename, _matchid) {
    id = _id;
    bettypename = _bettypename;
    matchid = _matchid;
    GetDataReportDetail();
}

function backtomain() {
    $("#err").remove();
    $("#backbtn").hide();
    $("#matchwinlostdetail").hide();
    $("#matchwinlost").show();
}

function ExportToExcel(type) {
    //try{parent.BlockUI();}catch(e){}
    
    var _id = id.split("_");
    var matchcode = _id[0];
    var islive = _id[1];
    var bettype = _id[2];
    
    var params = 'type=' + type;
    params += '&date=' + $('#date').val();
    params += '&isLive=' + islive;
    params += '&matchid=' + matchcode;
    params += '&bettype=' + bettype;
    $("#frm").attr('src', 'MatchWinloss.aspx?' + params);
    //parent.jQuery.unblockUI();
}

function ExportToExcel(type, bettype, isLive, matchId) {
    //try{parent.BlockUI();}catch(e){}
    var _id = id.split("_");
    var matchId = _id[0];
    var isLive = _id[1];
    var bettype = _id[2];

    var params = 'type=' + type;
    params += '&date=' + $('#date').val();
    params += '&isLive=' + isLive;
    params += '&matchid=' + matchId;
    params += '&bettype=' + bettype;

    $("#frm").attr('src', 'MatchWinloss.aspx?' + params);
    //parent.jQuery.unblockUI();
}

function GetDataReport() {

    try { parent.BlockUI(); } catch (e) { }
    fromcache = false;
    _pageIndex = 1;
    _pageSize = 50;
    $("#err").remove();
    $("#backbtn").hide();
    $("#matchwinlostdetail").hide();
    var arr = [];
    $.ajax({
        type: "Post",
        url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/WinLoss_ByMatch_NumberGame_Filter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"date":"' + $("#date").val() + '", "SiteID":"' + $("#hiddenCompnayFilter").val() + '", "Currency":"' + $("#hiddenCurrencyFilter").val() + '"}',
        success: function (rs) {
            //var start = new Date().getTime();
            if (rs.rows != null && rs.rows.length > 0) {
                $("#gridtitle").html("Number Game Win Loss By Match - Date " + $("#date").val());
                var tbl = $("#tblmatchwinlost");
                tbl.empty();
                $.each(rs.rows, function (i, n) {
                    arr.push(renderMatchRow(n));
                    if (n.children.length > 0) {
                        $.each(n.children, function (i1, n1) {
                            arr.push(renderLiveRow(n.Id, n1));
                            if (n1.children.length > 0) {
                                $.each(n1.children, function (i2, n2) {
                                    arr.push(renderBettypeRow(n.Id, n1.Id, n2));
                                });
                            }
                        });
                    }
                });

                tbl.html(arr.join(''));
                assignFooter(rs.totalrow);
                var matchwinlost = $("#matchwinlost");
                matchwinlost.show();
                
                var gridwidth = tbl.width() + 17; //17:scrollbar width
                var headerscroll = matchwinlost.find("div.header-scroll");
                var footerscroll = matchwinlost.find("div.footer-scroll");
                var gridheight = matchwinlost.find("div.content-toolbox").height() + headerscroll.height() + footerscroll.height() + 611; //3+608:border+div scroll height
                matchwinlost.css({ width: gridwidth + 'px', height: gridheight + 'px' });
                matchwinlost.find("div.body-scroll").css({ width: gridwidth + 'px' });
                headerscroll.css({ width: gridwidth + 'px' });
                footerscroll.css({ width: gridwidth + 'px' });

                try { parent.jQuery.unblockUI(); } catch (e) { }

                //var end = new Date().getTime();
                //alert(end - start);
            }
            else {
                $("#matchwinlost").hide();
                $('<div id="err"></div>').appendTo($("#mainreport"));
                showAlertMessage("err", "There is no data.", 1);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        }
    });
}

function renderMatchRow(n) {
    var strrow = "";
    strrow += '<tr>';
    strrow += '<td ><div class="col160 b"><span class="tree-expanded" onclick="showhiderow(this, \'' + n.Id + '\')"></span>' + n.Name + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
    strrow += '<td><div class="col100 r b' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
    strrow += '</tr>';
    return strrow;
}

function renderLiveRow(matchid, n) {
    var strrow = "";
    strrow += '<tr class="' + matchid + '">';
    strrow += '<td ><div class="col160"><span class="indent"></span><span class="tree-expanded" onclick="showhiderow(this, \'' + n.Id + '\')" ></span>' + n.Name + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
    strrow += '</tr>';
    return strrow;
}

function renderBettypeRow(matchid, liveId, n) {
    var strrow = "";
    strrow += '<tr class="' + matchid + ' ' + liveId + '">';
    strrow += '<td ><div class="col160"><span class="indent"></span><span class="indent"></span><span class="canchange" onclick="viewdetail(\'' + n.Id + '\',\'' + n.Name + '\',' + n.MatchId + ');">' + n.Name + '</span></div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Winlost) < 0 ? ' red' : '') + '">' + n.Winlost + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Position) < 0 ? ' red' : '') + '">' + n.Position + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.LineTotal) < 0 ? ' red' : '') + '">' + n.LineTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.VirtualLineTotal) < 0 ? ' red' : '') + '">' + n.VirtualLineTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Variance) < 0 ? ' red' : '') + '">' + n.Variance + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Margin) < 0 ? ' red' : '') + '">' + n.Margin + '</div></td>';
    strrow += '</tr>';
    return strrow;
}

function assignFooter(row) {
    if (parseFloat(row.Winlost) < 0) {
        $("#totalWinlost").html('<span style="color:red;">' + row.Winlost + '</span>');
    }
    else {
        $("#totalWinlost").html(row.Winlost);
    }

    if (parseFloat(row.Position) < 0) {
        $("#totalPosition").html('<span style="color:red;">' + row.Position + '</span>');
    }
    else {
        $("#totalPosition").html(row.Position);
    }

    if (parseFloat(row.Commission) < 0) {
        $("#totalCommission").html('<span style="color:red;">' + row.Commission + '</span>');
    }
    else {
        $("#totalCommission").html(row.Commission);
    }

    if (parseFloat(row.LineTotal) < 0) {
        $("#totalLineTotal").html('<span style="color:red;">' + row.LineTotal + '</span>');
    }
    else {
        $("#totalLineTotal").html(row.LineTotal);
    }

    if (parseFloat(row.VirtualLineTotal) < 0) {
        $("#totalVirtualLineTotal").html('<span style="color:red;">' + row.VirtualLineTotal + '</span>');
    }
    else {
        $("#totalVirtualLineTotal").html(row.VirtualLineTotal);
    }
    
    if (parseFloat(row.Variance) < 0) {
        $("#totalVariance").html('<span style="color:red;">' + row.Variance + '</span>');
    }
    else {
        $("#totalVariance").html(row.Variance);
    }

    if (parseFloat(row.TurnOver) < 0) {
        $("#totalTurnOver").html('<span style="color:red;">' + row.TurnOver + '</span>');
    }
    else {
        $("#totalTurnOver").html(row.TurnOver);
    }

    if (parseFloat(row.Margin) < 0) {
        $("#totalMargin").html('<span style="color:red;">' + row.Margin + '</span>');
    }
    else {
        $("#totalMargin").html(row.Margin);
    }
}

function GetDataReportDetail() {
    try { parent.BlockUI(); } catch (e) { }
    var scrollablenoscroll = 309;
    var scrollablewidth = scrollablenoscroll + 17;

    $("#err").remove();
    $("#matchwinlost").hide();
    var _id = id.split("_");
    var matchcode = _id[0];
    var islive = _id[1];
    var bettype = _id[2];

    var arrfrozen = [];
    var arrscroll = [];
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/WinLoss_Detail_NumberGame_Filter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //data: '{"date":"' + $("#date").val() + '","islive":' + (islive == "Live" ? true : false) + ',"bettype":' + bettype + ',"matchid":' + matchid + ',"pageIndex":' + _pageIndex + ',"pageSize":' + _pageSize + ',"fromcache":' + fromcache + '}',
        data: '{"date":"' + $("#date").val() 
        + '", "islive":' + (islive == "Live" ? true : false) 
        + ', "bettype":' + bettype 
        + ', "matchid":' + matchid 
        + ', "SiteID":"' + $("#selectCompany").val() 
        + '", "Currency":"' + $("#selectCurrency").val() +'"}',
        success: function (rs) {
            if (rs.rows.length > 0) {
                $("#gridtitledetail").html("Number Game Win Loss By Match " + matchcode + " - " + (islive == "Live" ? "Live" : "None Live") + " - " + bettypename + " - Date " + $("#date").val());
                var tblfrozen = $("#tblfrozendetail");
                var tblscroll = $("#tblscrolldetail");
                tblscroll.empty();
                tblfrozen.empty();

                $.each(rs.rows, function (i, n) {
                    arrscroll.push(renderWinLossDetail(n));
                    arrfrozen.push(renderWinLossDetailFrozen(n));
                });

                tblfrozen.html($(arrfrozen.join('')));
                tblscroll.html($(arrscroll.join('')));

                assignFooterDetail(rs.footer[0]);

                //Adjust the size of grid
                //var start = new Date().getTime();
                var matchwinlostdetail = $("#matchwinlostdetail");
                matchwinlostdetail.show();

                var gridwidth = matchwinlostdetail.find(".tbl-header-frozen").width() + scrollablewidth;
                var scrollheight = 21;
                var headerfrozenheight = $("#header-scroll-detail").height();
                if (jQuery.browser.msie) {
                    if (parseFloat(jQuery.browser.version) == 7) {
                        scrollheight = -1;
                        headerfrozenheight -= 2;
                    }
                    if (parseFloat(jQuery.browser.version) == 8) {
                        scrollheight = 22;
                    }
                }

                matchwinlostdetail.find(".tbl-header-frozen tr").css("height", headerfrozenheight + "px");
                var gridheight = matchwinlostdetail.height() - scrollheight;

                matchwinlostdetail.css({ width: gridwidth + 'px', height: gridheight + 'px' });
                matchwinlostdetail.find("div.body-scroll").css("width", scrollablewidth + "px");
                matchwinlostdetail.find("div.header-scroll, div.footer-scroll").css("width", scrollablenoscroll + "px");
                //var end = new Date().getTime();
                //alert(end - start);

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
            else {
                $("#matchwinlostdetail").hide();
                $("#backbtn").show();
                $('<div id="err"></div>').appendTo($("#detailreport"));
                showAlertMessage("err", "There is no data.", 1);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        }
    });
}

function renderWinLossDetail(n) {
    var strrow = "";
    strrow += '<tr>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MemberWinlost) < 0 ? ' red' : '') + '">' + n.MemberWinlost + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MemberComm) < 0 ? ' red' : '') + '">' + n.MemberComm + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MemberTotal) < 0 ? ' red' : '') + '">' + n.MemberTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.AgentWinlost) < 0 ? ' red' : '') + '">' + n.AgentWinlost + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.AgentComm) < 0 ? ' red' : '') + '">' + n.AgentComm + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.AgentTotal) < 0 ? ' red' : '') + '">' + n.AgentTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MasterTotal) < 0 ? ' red' : '') + '">' + n.MasterTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MasterComm) < 0 ? ' red' : '') + '">' + n.MasterComm + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.MasterTotal) < 0 ? ' red' : '') + '">' + n.MasterTotal + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.SuperWinlost) < 0 ? ' red' : '') + '">' + n.SuperWinlost + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.SuperComm) < 0 ? ' red' : '') + '">' + n.SuperComm + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.SuperTotal) < 0 ? ' red' : '') + '">' + n.SuperTotal + '</div></td>';
    strrow += "</tr>";
    return strrow;
}

function renderWinLossDetailFrozen(n) {
    var strrow = "";
    strrow += '<tr>';
    strrow += '<td><div class="col40">' + n.Id + '</div></td>';
    strrow += '<td><div class="col160">' + n.Username + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.TurnOver) < 0 ? ' red' : '') + '">' + n.TurnOver + '</div></td>';
    strrow += '<td><div class="col100 r' + (parseFloat(n.Commission) < 0 ? ' red' : '') + '">' + n.Commission + '</div></td>';
    strrow += "</tr>";
    return strrow;
}

function assignFooterDetail(row) {
    if (parseFloat(row.TurnOver) < 0) {
        $("#detailtotalTurnover").html('<span style="color:red;">' + row.TurnOver + '</span>');
    }
    else {
        $("#detailtotalTurnover").html(row.TurnOver);
    }

    if (parseFloat(row.Commission) < 0) {
        $("#detailtotalComm").html('<span style="color:red;">' + row.Commission + '</span>');
    }
    else {
        $("#detailtotalComm").html(row.Commission);
    }


    //Member
    if (parseFloat(row.MemberWinlost) < 0) {
        $("#mdetailtotalwinloss").html('<span style="color:red;">' + row.MemberWinlost + '</span>');
    }
    else {
        $("#mdetailtotalwinloss").html(row.MemberWinlost);
    }

    if (parseFloat(row.MemberComm) < 0) {
        $("#mdetailtotalcomm").html('<span style="color:red;">' + row.MemberComm + '</span>');
    }
    else {
        $("#mdetailtotalcomm").html(row.MemberComm);
    }

    if (parseFloat(row.MemberTotal) < 0) {
        $("#mdetailtotal").html('<span style="color:red;">' + row.MemberTotal + '</span>');
    }
    else {
        $("#mdetailtotal").html(row.MemberTotal);
    }
    //End member

    //Agent
    if (parseFloat(row.AgentWinlost) < 0) {
        $("#adetailtotalwinloss").html('<span style="color:red;">' + row.AgentWinlost + '</span>');
    }
    else {
        $("#adetailtotalwinloss").html(row.AgentWinlost);
    }

    if (parseFloat(row.AgentComm) < 0) {
        $("#adetailtotalcomm").html('<span style="color:red;">' + row.AgentComm + '</span>');
    }
    else {
        $("#adetailtotalcomm").html(row.AgentComm);
    }

    if (parseFloat(row.AgentTotal) < 0) {
        $("#adetailtotal").html('<span style="color:red;">' + row.AgentTotal + '</span>');
    }
    else {
        $("#adetailtotal").html(row.AgentTotal);
    }
    //End agent

    //Master
    if (parseFloat(row.MasterWinlost) < 0) {
        $("#madetailtotalwinloss").html('<span style="color:red;">' + row.MasterWinlost + '</span>');
    }
    else {
        $("#madetailtotalwinloss").html(row.MasterWinlost);
    }

    if (parseFloat(row.MasterComm) < 0) {
        $("#madetailtotalcomm").html('<span style="color:red;">' + row.MasterComm + '</span>');
    }
    else {
        $("#madetailtotalcomm").html(row.MasterComm);
    }

    if (parseFloat(row.MasterTotal) < 0) {
        $("#madetailtotal").html('<span style="color:red;">' + row.MasterTotal + '</span>');
    }
    else {
        $("#madetailtotal").html(row.MasterTotal);
    }
    //End master

    //Super
    if (parseFloat(row.SuperWinlost) < 0) {
        $("#sdetailtotalwinloss").html('<span style="color:red;">' + row.SuperWinlost + '</span>');
    }
    else {
        $("#sdetailtotalwinloss").html(row.SuperWinlost);
    }

    if (parseFloat(row.SuperComm) < 0) {
        $("#sdetailtotalcomm").html('<span style="color:red;">' + row.SuperComm + '</span>');
    }
    else {
        $("#sdetailtotalcomm").html(row.SuperComm);
    }

    if (parseFloat(row.SuperTotal) < 0) {
        $("#sdetailtotal").html('<span style="color:red;">' + row.SuperTotal + '</span>');
    }
    else {
        $("#sdetailtotal").html(row.SuperTotal);
    }
    //End super
}

