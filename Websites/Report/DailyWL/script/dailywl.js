var obj = {
    items: [
        {
            Bettype: "HDP",
            Winloss: "-71,090.04",
            Position: "112.45",
            Comm: "112.45",
            LineTotal: "112.45",
            Turnover: "112.45",
            Margin: "112.45"
        },
        {
            Bettype: "HDP",
            Winloss: "-71,090.04",
            Position: "112.45",
            Comm: "112.45",
            LineTotal: "112.45",
            Turnover: "112.45",
            Margin: "-112.45"
        },
        {
            Bettype: "HDP",
            Winloss: "-71,090.04",
            Position: "112.45",
            Comm: "112.45",
            LineTotal: "112.45",
            Turnover: "112.45",
            Margin: "-112.45"
        }
    ]
    };

   /* $(function () {
        //$("#gridbettype").scroller();
        dailyWL.getDailyWLRacing();
        resize_spliter()
        $("#splitterContainer").splitter({ minAsize: 10, splitVertical: true, A: $('#leftPane'), B: $('#rightPane'), closeableto: 0 });
        $(".splitbarV").addClass("hidden");
     
        $(window).resize(function () {
            resize_spliter();
        });
    });*/
    function resize_spliter() {
        var splittercontainerheight = $(window).height() - 70;
        $("#splitterContainer").css("height", splittercontainerheight + "px");
        var rightPaneWidth = $(window).width() - $("#leftPane").width() - $(".splitbarV").width() - 5;
        $("#rightPane").css("width", rightPaneWidth + "px");
    }
    var dailyWL = {
        date: '',
        sporttypeId: '',
        bettypeId: '',
        arr: [],
        title: '',
        renderDailyWLRow: function (row) {
            var html = '';
            html = '<tr>\
                        <td class="l"><span class="blue b" onclick="dailyWL.getDailyWLBySport(\'' + row.Id + '\',\'' + row.Name + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Margin.replace('%', '')) < 0 ? '' : 'red') + '">' + row.Margin + '</span></td>\
                    </tr>';
            dailyWL.arr.push(html);
        },
        renderDailyWLTotal: function (row) {
            var html = '';
            html = '<tr class="">\
                        <td class="l b"><span >' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</span></td>\
                        <td class="linetotal r b"><span class="' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</span></td>\
                        <td class="r b"><span class="' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '<span></td>\
                        <td class="r"><span class="' + ((row.Margin == 'N/A' || parseFloat(row.Margin.replace('%', '')) < 0) ? '' : 'red') + '">' + row.Margin + '</span></td>\
                    </tr>';
            dailyWL.arr.push(html);
        },
        renderDailyWLBettypeTotal: function (row) {
            var html = '';
            html = '<tr class="">\
                        <td class="l b"><span >Total</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</span></td>\
                        <td class="linetotal r b"><span class="' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</span></td>\
                        <td class="r b"><span class="' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '<span></td>\
                        <td class="r"><span class="' + ((row.Margin == 'N/A' || parseFloat(row.Margin.replace('%', '')) < 0) ? '' : 'red') + '">' + row.Margin + '</span></td>\
                    </tr>';
            dailyWL.arr.push(html);
        },
        renderDailyWLBettypeGrandTotal: function (row) {
            var html = '';
            html = '<tr class="title">\
                        <td class=" l b"><div class="w80">Grand Total</div></td>\
                        <td class="r"><div class="w80 ' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</div></td>\
                        <td class="r"><div class="w80 ' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</div></td>\
                        <td class="r"><div class="w80 ' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</div></td>\
                        <td class="linetotal r b"><div class="w80 ' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</div></td>\
                        <td class="r b"><div class="w80 ' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '<span></td>\
                        <td class="r"><div class="w80 ' + ((row.Margin == 'N/A' || parseFloat(row.Margin.replace('%', '')) < 0) ? '' : 'red') + '">' + row.Margin + '</div></td>\
                    </tr>'
            //dailyWL.arr.push(html);
            return html;
        },
        renderDailyWLRowSport: function (index, row) {
            var html = '';
            html = '<tr class="' + (index % 2 == 0 ? 'evenrow' : '') + '">\
                        <td class="l"><span class="blue b" onclick="dailyWL.getDailyWLBySportBettype(\'' + row.Id + '\',\'' + row.Name + '\');">' + row.Name + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</span></td>\
                        <td class="r"><span class="' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '</span></td>\
                        <td class="r"><span class="' + ((row.Margin == 'N/A' || parseFloat(row.Margin.replace('%', '')) < 0) ? '' : 'red') + '">' + row.Margin + '</span></td>\
                    </tr>';
            dailyWL.arr.push(html);
        },
        renderDailyWLRowBettype: function (index, row) {
            var html = '';
            html = '<tr class="' + (index % 2 == 0 ? 'evenrow' : '') + '">\
                        <td><div class="w80 b c">' + row.Name + '<span></td>\
                        <td><div class="w80 r ' + (parseFloat(row.WinLoss) < 0 ? '' : 'red') + '">' + row.WinLoss + '</div></td>\
                        <td><div class="w80 r ' + (parseFloat(row.Position) < 0 ? '' : 'red') + '">' + row.Position + '</div></td>\
                        <td><div class="w80 r ' + (parseFloat(row.Comm) < 0 ? '' : 'red') + '">' + row.Comm + '</div></td>\
                        <td><div class="w80 r ' + (parseFloat(row.LineTotal) < 0 ? '' : 'red') + '">' + row.LineTotal + '</div></td>\
                        <td><div class="w80 r ' + (parseFloat(row.Turnover) < 0 ? '' : 'red') + '">' + row.Turnover + '</div></td>\
                        <td><div class="w80 r ' + ((row.Margin == 'N/A' || parseFloat(row.Margin.replace('%', '')) < 0) ? '' : 'red') + '">' + row.Margin + '</div></td>\
                    </tr>';
            dailyWL.arr.push(html);
        },
        ViewReport: function (date) {
            var d;
            if (date == 1) {
                d = $("#btnToday").val();
                $("#date").val(d);
            }
            else if (date == 2) {
                d = $("#btnYesterday").val();
                $("#date").val(d);
            }
            else
                d = $("#date").val();

            dailyWL.getDailyWLRacing(d);

        },
        getDailyWLRacing: function (date) {
            try { parent.BlockUI(); } catch (e) { }
            dailyWL.arr = [];
            $("#err").empty();
            //dailyWL.date = $("#date").val();
            dailyWL.date = date;
            dailyWL.showMainReport(true);
            $.ajax({
                type: "Post",
                url: "../../Services/WCF/Report/ServiceReportDailyWL.svc/GetWinlossRacingReport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"winlossdate":"' + dailyWL.date + '"}',
                success: function (rs) {
                    if (rs.Items != null && rs.Items.length > 0) {
                        var total = rs.Items.length;
                        for (var i = 0; i < total; i++) {
                            dailyWL.renderDailyWLRow(rs.Items[i]);
                        }
                        dailyWL.arr.push('<tr><td colspan="7">&nbsp;</td></tr>');
                        for (var j = 0; j < rs.Totals.length; j++) {
                            dailyWL.renderDailyWLTotal(rs.Totals[j]);
                        }
                        $("#dailyWLRacing").find("tr:not(.header)").remove();
                        $("#dailyWLRacing").append(dailyWL.arr.join(''));
                    }
                    else {
                        dailyWL.arr.push('<tr><td colspan="7">There is no data.</td></tr>');
                        $("#dailyWLRacing").find("tr:not(.header)").remove();
                        $("#dailyWLRacing").append(dailyWL.arr.join(''));

                    }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        },
        getDailyWLBySport: function (sporttype, sportname) {
            try { parent.BlockUI(); } catch (e) { }
            dailyWL.arr = [];
            dailyWL.showMainReport(false);
            dailyWL.title = "Daily WinLoss Racing - " + sportname;
            $("#dailyWLBySportTitle").html(dailyWL.title);
            dailyWL.sporttypeId = sporttype;
            $.ajax({
                type: "Post",
                url: "../../Services/WCF/Report/ServiceReportDailyWL.svc/DailyWLDirectRacingBySport",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"sporttype":"' + sporttype + '","winlossdate":"' + dailyWL.date + '"}',
                success: function (rs) {
                    if (rs.Items != null && rs.Items.length > 0) {
                        var total = rs.Items.length;
                        for (var i = 0; i < total; i++) {
                            dailyWL.renderDailyWLRowSport(i, rs.Items[i]);
                        }
                        dailyWL.arr.push('<tr><td colspan="7">&nbsp;</td></tr>');
                        for (var j = 0; j < rs.Totals.length; j++) {
                            dailyWL.renderDailyWLTotal(rs.Totals[j]);
                        }
                        $("#dailyWLBySport").find("tr:not(.header)").remove();
                        $("#dailyWLBySport").append(dailyWL.arr.join(''));
                    }
                    else {
                        dailyWL.arr.push('<tr><td colspan="7">There is no data.</td></tr>');
                        $("#dailyWLBySport").find("tr:not(.header)").remove();
                        $("#dailyWLBySport").append(dailyWL.arr.join(''));

                    }
                },
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        },
        getDailyWLBySportBettype: function (bettype, bettypename) {
            parent.BlockUI();
            dailyWL.arr = [];
            $("#gridtitlebettype").html(dailyWL.title + " - " + bettypename);
            dailyWL.bettypeId = bettype;
            $.ajax({
                type: "Post",
                url: "../../Services/WCF/Report/ServiceReportDailyWL.svc/DailyWLDirectRacingBySportDetails",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"bettype":"' + bettype + '","winlossdate":"' + dailyWL.date + '","sporttype":"' + dailyWL.sporttypeId + '"}',
                success: function (rs) {
                    if (rs.Items != null && rs.Items.length > 0) {
                        var total = rs.Items.length;
                        for (var i = 0; i < total; i++) {
                            var item = rs.Items[i]
                            var total1 = item.Items.length;
                            dailyWL.arr.push('<tr><td colspan="7"><div class="l b" style="width:578px;">' + item.Totals[0].Name + '</div></td></tr>');
                            for (var j = 0; j < total1; j++) {
                                dailyWL.renderDailyWLRowBettype(j, item.Items[j]);
                            }
                            dailyWL.renderDailyWLBettypeTotal(item.Totals[0]);
                        }
                        $("#tblfooterscrollbettype").html(dailyWL.renderDailyWLBettypeGrandTotal(rs.Totals[0]));
                        $("#gridbettype").removeClass("hidden");
                        $("#tblbodyscrollbettype").html(dailyWL.arr.join(''));
                    }
                    else {
                        dailyWL.arr.push('<tr><td colspan="7"><div style="width:578px;">There is no data.</div></td></tr>');
                    }
                },
                complete: function () {
                    $("#tblbodyscrollbettype").html(dailyWL.arr.join(''));
                    dailyWL.showRacingBettypeReport(true);
                    $("#gridbettype").reRenderScroller(581, 485, true); //width=560+7*1+7*2|height:485px
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        },
        showMainReport: function (isshow) {
            var dailyWLBySport = $("#dailyWLBySport");
            var dailyWLRacing = $("#dailyWLRacing");
            if (isshow) {
                dailyWLRacing.removeClass("hidden");
                dailyWLBySport.addClass("hidden");
                dailyWL.showRacingBettypeReport(false);
            }
            else {
                dailyWLBySport.removeClass("hidden");
                dailyWLRacing.addClass("hidden");
            }

            $("#leftPane").css("width", "620px").show();
            resize_spliter();
        },
        showRacingBettypeReport: function (isshow) {
            if (isshow) {
                $("#gridbettype").removeClass("hidden");
                $(".splitbarV").removeClass("hidden");


            }
            else {
                $("#gridbettype").addClass("hidden");
                $(".splitbarV").addClass("hidden");
            }
        },
        ExportToExcel: function (reporttype) {
            var url = 'DailyWL.aspx?reporttype=' + reporttype + '&date=' + dailyWL.date + '&sporttype=' + dailyWL.sporttypeId + '&bettype=' + dailyWL.bettypeId + '&isExpExcel=1';
            $('#frm').attr('src', url);
        },
        backToMain: function () {
            window.location = '../DailyWL_Sporttype.aspx?fdate=' + dailyWL.date + '&db=Rep&mode=';
        }

    };
