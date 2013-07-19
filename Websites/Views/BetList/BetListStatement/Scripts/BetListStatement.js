var BetListStatement = {
    gridOpts_Soccer: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 80,
        freezeWidth: 510, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    },
    pagerOpts: {
        callback: null,
        start_per_page: 50,
        step_per_page: 50,
        max_items_per_page: 200,
        items_per_page: 50,
        current_page: 1
    },
    params_SportType:
    {
        CustID: '',
        IP: '',
        FromDate: null,
        IsSoccer: null,
        PageIndex: 1,
        PageSize: 50,
        IsCache: false
    },

    getBetListByIP_Soccer: function (custID, fromdate, ip, isSoccer) {
        if (BetListStatement.params_SportType.IP == ip && BetListStatement.params_SportType.FromDate == fromdate && BetListStatement.params_SportType.CustID == custID && BetListStatement.params_SportType.IsSoccer == isSoccer) {
            BetListStatement.params_SportType.IsCache = true;
        }
        else {
            BetListStatement.params_SportType.CustID = custID;
            BetListStatement.params_SportType.IP = ip;
            BetListStatement.params_SportType.FromDate = fromdate
            BetListStatement.params_SportType.IsSoccer = isSoccer;
            BetListStatement.params_SportType.IsCache = false;
            BetListStatement.params_SportType.PageIndex = 1;
            BetListStatement.params_SportType.PageSize = 50;
        }

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetListByIP_Soccer",
            data: BetListStatement.params_SportType,
            success: function (result) {
                $("#BetlistByIP_Username").remove();
                $("#betListContent").html(result);

                $("#BetlistByIP_Soccer").initGrid(BetListStatement.gridOpts_Soccer);
                $('#BetlistByIP_Soccer').removeClass("display-none").gridScroller(BetListStatement.gridOpts_Soccer);
                BetListStatement.initPager(_lengthSprortype, BetListStatement.params_SportType.PageSize, BetListStatement.params_SportType.PageIndex - 1);

                for (var i = 0; i < _arrayTransID.length; i++) {
                    var heightChoice = $("#choice_left_" + _arrayTransID[i]).height();
                    var heightStatus = $("#stautus_" + _arrayTransID[i]).height();
                    if (heightStatus > heightChoice) {
                        $("#choice_left_" + _arrayTransID[i]).attr('style', 'height:' + heightStatus + 'px');
                    }
                }
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },


    initPager: function (total_items, items_per_page, current_page) {
        BetListStatement.pagerOpts.callback = BetListStatement.pageSelectCallback;
        BetListStatement.pagerOpts.items_per_page = items_per_page;
        BetListStatement.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, BetListStatement.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        BetListStatement.params_SportType.PageIndex = page_index + 1;
        BetListStatement.params_SportType.PageSize = _items_per_page;
        BetListStatement.getBetListByIP_Soccer(BetListStatement.params_SportType.CustID, BetListStatement.params_SportType.FromDate, BetListStatement.params_SportType.IP, BetListStatement.params_SportType.IsSoccer);
        return false;
    }
};

var Helper = {
    renderCalendar: function () {
        var defaults = {
            showOtherMonths: true,
            selectOtherMonths: true,
            onSelect: function (selectedDate) {
                var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                dates.not(this).datepicker("option", option, date);
            }
        };
        var dates = $("#txtfromdate").datepicker(defaults);
        $("#txtfromdate").datepicker("setDate", _today);

    }
};

function showCombMPDetail(tranid, date, refno, betid, matchid) {
    var username = $(".combinationLink span:last").html();
    showDetailsCMP(tranid, "System Parlay - " + username, parseFloat(refno), parseInt(betid), _custid)
}

function showMP(transid, refno) {
    if ($("#td_right_" + transid).hasClass("isClient")) {
        if (!$("#td_right_" + transid).hasClass("isShow")) {
            $(".parlay_" + transid).each(function () {
                $(this).hide();
                var idTdParent = $(this).parent().parent().attr('id');
                $("#" + idTdParent).addClass("isShow");
                $("#BetlistByIP_Soccer").gridScroller(BetListStatement.gridOpts_Soccer);

                var heightChoice = $("#choice_left_" + transid).height();
                var heightStatus = $("#stautus_" + transid).height();
                var heightComm = $("#comm_" + transid).height();
                var heightRight = heightStatus > heightComm ? heightStatus : heightComm;
                if (heightRight > heightChoice) {
                    $("#choice_left_" + transid).attr('style', 'height:' + heightRight + 'px');
                }
            });
        }
        else {
            $(".parlay_" + transid).each(function () {
                $(this).show();
                $("#choice_left_" + transid).css("height", "");
                var idTdParent = $(this).parent().parent().attr('id');
                $("#" + idTdParent).removeClass("isShow");
                $("#BetlistByIP_Soccer").gridScroller(BetListStatement.gridOpts_Soccer);
            });
        }
    }
    else {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetMixParlayDetail",
            data: "refno=" + refno,
            success: function (result) {
                $(".parlay_" + transid).each(function () {
                    $(this).html(result);
                    var id = $(this).parent().attr('id');
                    $("#" + id).css("height", "");

                    var idTdParent = $(this).parent().parent().attr('id');
                    $("#" + idTdParent).addClass("isClient");
                });
                $("#BetlistByIP_Soccer").gridScroller(BetListStatement.gridOpts_Soccer);
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
}

function showCombMP(transid, custid, refno) {
    showMP(transid, refno);
}

head.ready(function (event) {
    var gridOpts_Soccer = {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 10,
        freezeWidth: 510, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    };

    $("#BetlistByIP_Soccer").initGrid(gridOpts_Soccer);
    $('#BetlistByIP_Soccer').removeClass("display-none").gridScroller(gridOpts_Soccer);

});

