var CheckBetNormalList=false;
var CheckBetRejectList=false;
function renderCalendar(ids, opts, chosenDate) {
    var defaults = {
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    };
    var new_opts = $.extend(true, defaults, opts);
    var dates = $("#" + ids).datepicker(new_opts);
    $("#" + ids).datepicker("setDate", chosenDate);
}

function setupCalendar(id, chosenDate, mindate, maxdate) {
    var limitDate = {
        minDate: mindate,
        maxDate: maxdate
    };
    renderCalendar(id, limitDate, chosenDate);
    $('#' + id).click(function () {
        $('#' + id).datepicker("show");
    });
}

$(document).ready(function () {
    setupCalendar("txtfromdate", chosenDate, mindate, currdate);
    var gridOpts = {
        tblContentId: 'gridDetail'
    };
    $("#CheckBet").initGrid(gridOpts);
    gridOpts.tblContentId = 'gridNormalDetail';
    $("#CheckNormalBet").initGrid(gridOpts);
    gridOpts.tblContentId = 'gridRejectDetail';
    $("#CheckRejectBet").initGrid(gridOpts);
});

var CheckBet = {
    date: '',
    title: '',
    GetCheckBetReport: function () {
        try { parent.BlockUI(); } catch (e) { }
        var arr = [];
        var grid = $('#CheckBet');
        $('#err').hide();
        $("#CheckNormalBet").addClass("hidden");
        $("#CheckRejectBet").addClass("hidden");
        CheckBet.date = $('#txtfromdate').val();
        var data = '{"date":"' + CheckBet.date + '"}';
        $.ajax({
            type: "POST",
            url: "GetCheckBet",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (rs) {
                CheckBet.title = 'Check Bet Date ' + CheckBet.date;
                $('#gridtitleCheckBet').empty().append(CheckBet.title);
                grid.addClass("hidden");
                $("#gridDetail tbody").empty().append(CheckBet.RenderBodyContent(rs.NormalTickets, rs.RejectTickets));
                $("#gridrowheaderDetail tbody").empty().append(CheckBet.RenderGridRowHeader(CheckBet.date));
                var gridOpts = {
                    tblContentId: 'gridDetail',
                    manualCopyHeader: true,
                    fullWidth: true,
                    gridHeight: 100,
                    freezeWidth: 120,
                    gridWidth: 375,
                    numberFreezeCol: 1
                };
                grid.removeClass("hidden").gridScroller(gridOpts);
                CheckBetNormalList = false;
                CheckBetRejectList = false;

                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },

    RenderBodyContent: function (NormalTickets, RejectTickets) {
        var html = [];
        html.push('<tr >');
        html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >&nbsp;</div></td>');
        if (parseInt(NormalTickets) > 0)
            html.push('<td ><div style="width:120px; height:34px;" onclick="CheckNormalBet.GetCheckNormalBet()" class=" table-cell c link">' + NormalTickets + '</div></td>');
        else
            html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >' + NormalTickets + '</div></td>');
        if (parseInt(RejectTickets))
            html.push('<td ><div style="width:120px; height:34px;" onclick="CheckRejectBet.GetCheckRejectBet()" class=" table-cell c link">' + RejectTickets + '</div></td>');
        else
            html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >' + RejectTickets + '</div></td>');
        html.push('</tr>');
        return html.join('');
    },
    RenderGridRowHeader: function (date) {
        var html = [];

        html.push('<tr >');
        html.push('<td ><div style="width:120px; height:34px" class="c table-cell" >' + date + '</div></td>');
        html.push('</tr >');

        return html.join('');
    }

};

var CheckNormalBet = {
    title: '',
    GetCheckNormalBet: function () {
        $("#CheckRejectBet").addClass("hidden");
        var grid = $("#CheckNormalBet");
        if (!CheckBetNormalList) {
            try { parent.BlockUI(); } catch (e) { }
            var data = '{"date":"' + CheckBet.date + '"}';
            $.ajax({
                type: "POST",
                url: "GetCheckNormalBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data,
                success: function (rs) {
                    if (rs.listTicket && rs.listTicket.length > 0) {
                        CheckNormalBet.title = 'Normal Tickets ' + CheckBet.date;
                        $('#gridtitleCheckNormalBet').empty().append(CheckNormalBet.title);
                        grid.addClass("hidden");
                        $("#gridNormalDetail tbody").empty().append(CheckNormalBet.RenderBodyContent(rs.listTicket));
                        grid.find(".col_footer_content thead").empty().append(CheckNormalBet.RenderFooterColContent(rs.TotalChangedStake, rs.TotalChangedOdds, rs.TotalChangedBetteam, rs.TotalChangedPT));
                        CheckNormalBet.initGrid();
                        CheckBetNormalList = true;
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
            CheckNormalBet.initGrid();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }

    },
    initGrid: function () {
        var gridOpts = {
            tblContentId: 'gridNormalDetail',
            manualCopyHeader: true,
            fullWidth: true,
            gridWidth: 1055, //=totalColumnWidth + totalColumn*4 +totalColumn
            gridHeight: 100,
            freezeWidth: 0,
            numberFreezeCol: 0
        };
        $("#CheckNormalBet").removeClass("hidden").gridScroller(gridOpts);
    },
    RenderBodyContent: function (rows) {
        var html = [];
        var length = rows.length;
        for (var i = 0; i < length; i++) {
            var row = rows[i];
            var arr = [];
            arr = row.TransDate.split(' ');
            var time = arr[1] + " " + arr[2];
            html.push('<tr>');
            html.push('<td ><div style="width:120px; height:34px" class="c table-cell link" onclick="ViewDetailRefNo(\'' + rows[i].RefNo + '\', \'' + rows[i].Username + '\')" >' + rows[i].RefNo + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" ><span>' + arr[0] + '<br/></span><span>' + time + '</span></div></td>');
            html.push('<td ><div style="width:50px; height:34px; " class="table-cell c" >' + row.CurrencyName + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Bettype + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Username + '</div></td>');
            if (parseFloat(row.OldStake) != parseFloat(row.NewStake))
                html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" ><span>' + row.OldStake + '<br/></span><span style="color:Red">' + row.NewStake + '</span></div></td>');
            else
                html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" >' + row.OldStake + '</div></td>');
            if (parseFloat(row.OldOdds) != parseFloat(row.NewOdds))
                html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" ><span>' + row.OldOdds + '<br/></span><span style="color:Red">' + row.NewOdds + '</span></div></td>');
            else
                html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" >' + row.OldOdds + '</div></td>');
            if (row.NewBetteam != row.OldBetteam)
                html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" ><span>' + row.OldBetteam + '<br/></span><span style="color:Red">' + row.NewBetteam + '</span></div></td>');
            else
                html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >' + row.OldBetteam + '</div></td>');
            if (parseFloat(row.OldTotalPT) != parseFloat(row.NewTotalPT))
                html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" ><span>' + row.OldTotalPT + '<br/></span><span style="color:Red">' + row.NewTotalPT + '</span></div></td>');
            else
                html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >' + row.OldTotalPT + '</div></td>');
            html.push('<td ><div style="width:60px; height:34px;" class="table-cell c" >' + row.OldStatus + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Creator + '</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },
    RenderFooterColContent: function (Stake, Odds, Betteam, PT) {
        var html = [];
        html.push('<tr><th><div class="table-cell c" style="width:120px; height: 23px;">Count</div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;"></div></th>');
        html.push('<th><div class="table-cell c" style="width:60px; height: 23px;"></div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;"></div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;"></div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" >' + Stake + '</div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" >' + Odds + '</div></th>');
        html.push('<th ><div style="width:120px; height: 23px;" class="c table-cell" >' + Betteam + '</div></th>');
        html.push('<th ><div style="width:120px; height: 23px;" class="c table-cell" >' + PT + '</div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" ></div></th>');
        html.push('<th ><div style="width:100px; height: 23px;" class="c table-cell" ></div></th>');
        html.push('</tr>');
        return html.join('');
    }
}

var CheckRejectBet = {
    title: '',
    initGrid: function () {
        var gridOpts = {
            tblContentId: 'gridRejectDetail',
            manualCopyHeader: true,
            fullWidth: true,
            gridHeight: 200,
            freezeWidth: 0,
            numberFreezeCol: 0
        };
        $("#CheckRejectBet").removeClass("hidden").gridScroller(gridOpts);
    },
    GetCheckRejectBet: function () {
        $("#CheckNormalBet").addClass("hidden");
        var rejectBet = $("#CheckRejectBet");
        if (!CheckBetRejectList) {
            try { parent.BlockUI(); } catch (e) { }
            var data = '{"date":"' + CheckBet.date + '"}';
            $.ajax({
                type: "POST",
                url: "GetCheckRejectBet",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: data,
                success: function (rs) {
                    if (rs.listTicket.length > 0) {
                        CheckNormalBet.title = 'Void/Reject Tickets ' + CheckBet.date;
                        $('#gridtitleCheckRejectBet').empty().append(CheckNormalBet.title);
                        $("#CheckRejectBet").addClass("hidden");
                        $("#gridRejectDetail tbody").empty().append(CheckRejectBet.RenderBodyContent(rs.listTicket));
                        rejectBet.find(".col_footer_content thead").empty().append(CheckRejectBet.RenderFooterColContent(rs.TotalChangedStatus));
                        CheckRejectBet.initGrid();
                        CheckBetRejectList = true;
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
            });
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
            CheckRejectBet.initGrid();
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    },

    RenderBodyContent: function (rows) {
        var html = [];
        var length = rows.length;
        for (var i = 0; i < length; i++) {
            var row = rows[i];
            var arr = [];
            arr = row.TransDate.split(' ');
            var time = arr[1] + " " + arr[2];
            html.push('<tr>');
            html.push('<td ><div style="width:120px; height:34px" class="c table-cell link" onclick="ViewDetailRefNo(\'' + rows[i].RefNo + '\', \'' + rows[i].Username + '\')" >' + rows[i].RefNo + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" ><span>' + arr[0] + '<br/></span><span>' + time + '</span></div></td>');
            html.push('<td ><div style="width:50px; height:34px; " class="table-cell c" >' + row.CurrencyName + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Bettype + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Username + '</div></td>');
            html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" >' + row.OldStake + '</div></td>');
            html.push('<td ><div style="width:60px; height:34px; " class="table-cell c" >' + row.OldOdds + '</div></td>');
            html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" >' + row.OldBetteam + '</div></td>');
            html.push('<td ><div style="width:120px; height:34px; " class="table-cell c" ><span>' + row.OldTotalPT + '</div></td>');
            if (row.OldStatus != row.NewStatus)
                html.push('<td ><div style="width:60px; height:34px;" class="table-cell c" >' + row.OldStatus + '<br/></span><span style="color:Red">' + row.NewStatus + '</span></div></td>');
            else
                html.push('<td ><div style="width:60px; height:34px;" class="table-cell c" >' + row.OldStatus + '</div></td>');
            html.push('<td ><div style="width:100px; height:34px; " class="table-cell c" >' + row.Creator + '</div></td>');
            html.push('</tr>');
        }
        return html.join('');
    },

    RenderFooterColContent: function (Status) {
        var html = [];
        html.push('<tr><th><div class="table-cell c" style="width:120px; height: 23px;">Count</div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;">&nbsp;</div></th>');
        html.push('<th><div class="table-cell c" style="width:60px; height: 23px;">&nbsp;</div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;">&nbsp;</div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;">&nbsp;</div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" >&nbsp;</div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" >&nbsp;</div></th>');
        html.push('<th ><div style="width:120px; height: 23px;" class="c table-cell" >&nbsp;</div></th>');
        html.push('<th ><div style="width:120px; height: 23px;" class="c table-cell" >&nbsp;</div></th>');
        html.push('<th ><div style="width:60px; height: 23px;" class="c table-cell" >' + Status + '</div></th>');
        html.push('<th><div class="table-cell c" style="width:100px; height: 23px;">&nbsp;</div></th>');
        html.push('</tr >');
        return html.join('');
    }
};

function ViewDetailRefNo(refno, custName) {
    try { parent.BlockUI(); } catch (e) { }
    var title = "Bet list of " + refno;
//    var url = window.rootUrl + 'BetList/BetListSystemparlay.aspx?refno=' + refno + '&custName=' + custName;
    var width = 1020;
    var height = 250;
    gridOpts = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: false,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 100,
        freezeWidth: 460, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    };
    $.ajax({
        type: "POST",
        url: window.rootUrl + "BetList/GetBetListByRefNo",
        data: 'refno=' + refno,
        success: function (result) {
            $("#betListContent").removeClass("hidden");
            $("#betListContent").html(result);
            $("#BetlistByRefNo").initGrid(gridOpts);
            $('#BetlistByRefNo').gridScroller(gridOpts);
            if ($('div.combinationLink')) {
                var divReplace = '<span style="color: #0066CC;">' + $('div.combinationLink a:first').html() + '</span>';
                $('div.combinationLink a:first').replaceWith(divReplace);
            }
            if ($('#choice_left a:first').html() == 'Mix Parlay') {
                var divReplace = '<span style="color: #0066CC;">' + $('#choice_left a:first').html() + '</span>';
                $('#choice_left a:first').replaceWith(divReplace);
            }
            var heightChoice = $("#choice_left").height();
            var heightStatus = $("#stautus").height();
            if ((heightStatus + 5) > heightChoice) {
                $("#choice_left").attr('style', 'height:' + heightStatus + 'px;margin-top:5px;');
            }
            $("#hidden0").removeAttr("href");
            $("#result").attr('style', 'cursor: pointer;cursor: hand;');

            bCconfirm = $("#betListContent").html();
            $('#tblconfirm').empty().append(bCconfirm);
            $("#tblconfirm").dialog({
                title: 'Bet list of ' + refno,
                autoOpen: false,
                height: "auto",
                width: "auto",
                modal: true,
                buttons: {

                },
                close: function (event, ui) {
                }
            });
            $('a.ui-dialog-titlebar-close').show();
            $("#tblconfirm").dialog("open");
            $("#betListContent").addClass("hidden");
            //            var html = $("#betListContent").html();
            //            OpenPopup(html, title, width, height, false, true, window, 0, null, null);
        },
        error: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
//    var html = BetList.GetBetListByRefNo(refno);
//    OpenPopup(html, title, width, height, false, true, window, 0, null, null);
    try { parent.jQuery.unblockUI() } catch (e) { };
}
