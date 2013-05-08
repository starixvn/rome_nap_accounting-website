function ExportToExcel(type) {
    //try{parent.BlockUI();}catch(e){}
    var params = 'type=' + type;
    params += '&fdate=' + $('#fdate').val();
    params += '&tdate=' + $('#tdate').val();
    $("#frm").attr('src', 'DateWinloss.aspx?' + params);
    //parent.jQuery.unblockUI();
}

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

function GetDataReport() {
    try { parent.BlockUI(); } catch (e) { }
    $("#err").remove();
    var arr = [];
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/ServiceReportNumberGame.svc/WinLoss_ByDate_NumberGame_Filter",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"fdate":"' + $("#fdate").val() + '","tdate":"' + $("#tdate").val() + '", "SiteID":"' + $("#hiddenCompnayFilter").val() 
            + '", "Currency": "' + $("#hiddenCurrencyFilter").val() + '"}',
        success: function (rs) {
            if (rs.rows != null && rs.rows.length > 0) {
                $("#gridtitle").html("Number Game Win Loss By Date - From " + $("#fdate").val() + " To " + $("#tdate").val());
                var tbl = $("#tblmatchwinlost");
                tbl.empty();
                //document.getElementById("tblmatchwinlost").innerHTML = "";
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
                //document.getElementById("tblmatchwinlost").innerHTML = arr.join('');
                tbl.html(arr.join(''));
                assignFooter(rs.totalrow);
                var matchwinlost = $("#matchwinlost");
                matchwinlost.show();
                var gridwidth = tbl.width() + 17; //17:scrollbar width
                var headerscroll = matchwinlost.find("div.header-scroll");
                var footerscroll = matchwinlost.find("div.footer-scroll");
                var gridheight = matchwinlost.find("div.content-toolbox").height() + headerscroll.height() + footerscroll.height() + 3 + 608; //3+608:border+div scroll height
                matchwinlost.css({ width: gridwidth + 'px', height: gridheight + 'px' });
                matchwinlost.find("div.body-scroll").css({ width: gridwidth + 'px' });
                headerscroll.css({ width: gridwidth + 'px' });
                footerscroll.css({ width: gridwidth + 'px' });

                try { parent.jQuery.unblockUI(); } catch (e) { }

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
    strrow += '<td ><div class="col160"><span class="indent"></span><span class="indent"></span>' + n.Name + '</div></td>';
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
