var BetListByCustInfo = {
    paramGetBetBetlist: {
        FromDate: null,
        ToDate: null,
        CustID: null,
        Bettype: 0,
        PageIndex: 1,
        PageSize: 30,
        IsCache: false
    },
    paramGetBetBetlistbyRefno: {
        CustID: null,
        TransID: null
    },
    paramGetBetlistSystemParlay: {
        CustID: null,
        TransID: null
    },
    URL: null,
    gridOpts: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        isShowPager: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 980, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 120,
        freezeWidth: 410, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 3
    },
    eventGridOpts: {
        tblContentId: 'tblContent',
        manualCopyHeader: true,
        fullWidth: true,
        fullHeight: true,
        gridWidth: 670, //=totalColumnWidth + totalColumn*4 +totalColumn
        gridHeight: 550,
        freezeWidth: 0, //=totalColumnWidth + (totalColumn-1)*4 + (totalColumn-1)
        numberFreezeCol: 0
    },
    pagerOpts: {
        callback: null,
        start_per_page: 10,
        step_per_page: 10,
        max_items_per_page: 100,
        items_per_page: 30,
        current_page: 1
    },

    getBetListSystemParlay: function () {
        BetListByCustInfo.paramGetBetlistSystemParlay.CustID = _custID;
        BetListByCustInfo.paramGetBetlistSystemParlay.TransID = $('#txtRefNoSystemParaly').val();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetListByCustInfo_SystemParLayTab",
            data: BetListByCustInfo.paramGetBetlistSystemParlay,
            success: function (result) {
                BetListByCustInfo.gridOpts.isShowPager = false;
                $("#betListSystemParlayContent").html(result);
                $("#BetlistSystemParlay").initGrid(BetListByCustInfo.gridOpts);
                $('#BetlistSystemParlay').removeClass("display-none").gridScroller(BetListByCustInfo.gridOpts);
                BetListByCustInfo.initPager(_length, BetListByCustInfo.paramGetBetBetlist.PageSize, BetListByCustInfo.paramGetBetBetlist.PageIndex - 1);

                $("#betListSystemParlayContent").find(".detail").remove();
                $("#betListSystemParlayContent").find("#hidden0").removeAttr("href");
                $("#result").attr('style', 'cursor: pointer;cursor: hand;');

                BetListByCustInfo.eventGridOpts.gridHeight = $("#betListSystemParlayContent").height() + 55;
                $("#EventDetail").initGrid(BetListByCustInfo.eventGridOpts);
                $('#EventDetail').removeClass("display-none").gridScroller(BetListByCustInfo.eventGridOpts);
            },
            error: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },


    getBetListByCustInfo: function () {
        var params = BetListByCustInfo.getParams();
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + BetListByCustInfo.URL,
            data: params,
            success: function (result) {
                BetListByCustInfo.gridOpts.isShowPager = true;
                $("#betListContent").html(result);
                $("#BetlistByCustInfo").initGrid(BetListByCustInfo.gridOpts);
                $('#BetlistByCustInfo').removeClass("display-none").gridScroller(BetListByCustInfo.gridOpts);
                BetListByCustInfo.initPager(_length, BetListByCustInfo.paramGetBetBetlist.PageSize, BetListByCustInfo.paramGetBetBetlist.PageIndex - 1);

                for (var i = 0; i < _arrayTransID.length; i++) {
                    var heightChoice = $("#choice_left_" + _arrayTransID[i]).height();
                    var heightStatus = $("#stautus_" + _arrayTransID[i]).height();
                    var heightComm = $("#comm_" + _arrayTransID[i]).height();
                    var heightRight = heightStatus > heightComm ? heightStatus : heightComm;
                    if (heightRight > heightChoice) {
                        $("#choice_left_" + _arrayTransID[i]).attr('style', 'height:' + heightRight + 'px');
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
    getParams: function () {
        if ($('input[name=type]:checked').val() == "-2") {
            var fromDateTmp = $('#txtfromdate').val();
            var toDateTmp = $('#txttodate').val();
            var bettypeTmp = $('#ddlBettype').val() == null ? 0 : $('#ddlBettype').val();

            if (BetListByCustInfo.paramGetBetBetlist.FromDate == fromDateTmp && BetListByCustInfo.paramGetBetBetlist.ToDate == toDateTmp && BetListByCustInfo.paramGetBetBetlist.Bettype == bettypeTmp) {
                BetListByCustInfo.paramGetBetBetlist.IsCache = true;
            }
            else {
                BetListByCustInfo.paramGetBetBetlist.IsCache = false;
                BetListByCustInfo.paramGetBetBetlist.PageIndex = 1;
                BetListByCustInfo.paramGetBetBetlist.PageSize = 30;
            }
            BetListByCustInfo.paramGetBetBetlist.FromDate = fromDateTmp;
            BetListByCustInfo.paramGetBetBetlist.ToDate = toDateTmp;
            BetListByCustInfo.paramGetBetBetlist.CustID = _custID;
            BetListByCustInfo.paramGetBetBetlist.Bettype = bettypeTmp;


            BetListByCustInfo.URL = "TarusBetList/GetBetListByCustInfo_DateRange";
            return BetListByCustInfo.paramGetBetBetlist;
        }
        else {
            BetListByCustInfo.paramGetBetBetlistbyRefno.CustID = _custID;
            BetListByCustInfo.paramGetBetBetlistbyRefno.TransID = $('#txtRefNo').val();

            BetListByCustInfo.URL = "BetList/GetBetListByCustInfo_Refno";
            return BetListByCustInfo.paramGetBetBetlistbyRefno;
        }
    },
    trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    getBettyes: function () {
        var paramGetBetTypes = {
            FromDate: $('#txtfromdate').val(),
            ToDate: $('#txttodate').val(),
            CustID: _custID
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "BetList/GetBetTypes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(paramGetBetTypes),
            success: function (rs) {
                if (rs.length > 1) {
                    $("#divBettype").removeClass("hidden");
                    $('#ddlBettype').empty();
                    $.each(rs, function (i, val) {
                        $('#ddlBettype').append('<option value="' + val.BetTypeID + '">' + val.BetTypeName + '</option>');
                    });
                }
                else {
                    $("#divBettype").addClass("hidden");
                }
            },
            error: function (event, jqXHR, ajaxSettings) {
               
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    disabledFilter: function () {
        if ($('input[name=type]:checked').val() == "-2") {
            $("#txtRefNo").attr("disabled", "disabled");

            $("#txttodate").removeAttr("disabled", "disabled");
            $("#txtfromdate").removeAttr("disabled", "disabled");
            $("#ddlBettype").removeAttr("disabled", "disabled");

        }
        else {
            $("#txttodate").attr("disabled", "disabled");
            $("#txtfromdate").attr("disabled", "disabled");
            $("#ddlBettype").attr("disabled", "disabled");

            $("#txtRefNo").removeAttr("disabled", "disabled");
        }
    },
    initPager: function (total_items, items_per_page, current_page) {
        BetListByCustInfo.pagerOpts.callback = BetListByCustInfo.pageSelectCallback;
        BetListByCustInfo.pagerOpts.items_per_page = items_per_page;
        BetListByCustInfo.pagerOpts.current_page = current_page;

        $("#pagination").pagination(total_items, BetListByCustInfo.pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(_length / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;

        BetListByCustInfo.paramGetBetBetlist.PageIndex = page_index + 1;
        BetListByCustInfo.paramGetBetBetlist.PageSize = _items_per_page;
        BetListByCustInfo.getBetListByCustInfo();
        return false;
    },
    viewSystemParlayTab: function () {
        //change tab
        $('#Li1').removeClass('current');
        $('#Li2').addClass('current');
        //system parlay display
        $('#maincontent_systemparlay').removeClass('hidden');
        $('#betListSystemParlayContent').removeClass('hidden');
        //betlist hidden
        $('#maincontent_betlist').addClass('hidden');
        $('#betListContent').addClass('allhidden');


    },
    viewBetListTab: function () {
        //change tab
        $('#Li1').addClass('current');
        $('#Li2').removeClass('current');
        //system parlay hidden
        $('#maincontent_systemparlay').addClass('hidden');
        $('#betListSystemParlayContent').addClass('hidden');
        //betlist display
        $('#maincontent_betlist').removeClass('hidden');
        $('#betListContent').removeClass('allhidden');
    }
};


var Helper = {
    renderCalendarFromTo: function (ids) {
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
                BetListByCustInfo.getBettyes();
            }
        };

        var dates = $(ids).datepicker(defaults);
        var dt = new Date(_today);
        var fdate = $(ids.split(',')[0]);
        var tdate = $(ids.split(',')[1]);
        fdate.datepicker("setDate", dt);
        tdate.datepicker("setDate", dt);

    },
    reset_date: function () {
        var dayNoReset = 7;
        var fdateId = 'txtfromdate';
        var tdateId = 'txttodate';
        var td = new Date();
        var fd = new Date(td.getTime() - dayNoReset * 24 * 60 * 60 * 1000);
        document.getElementById(fdateId).value = (fd.getMonth() + 1) + "/" + fd.getDate() + "/" + fd.getFullYear();
        document.getElementById(tdateId).value = (td.getMonth() + 1) + "/" + td.getDate() + "/" + td.getFullYear();
        BetListByCustInfo.getBettyes();
    },
    range_date: function () {
        $("#txtfromdate").datepicker("setDate", _fromDate);
        $("#txttodate").datepicker("setDate", _toDate);
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
                $("#BetlistByCustInfo").gridScroller(BetListByCustInfo.gridOpts);

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
                $("#BetlistByCustInfo").gridScroller(BetListByCustInfo.gridOpts);
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
                $(".parlay_" + transid + ":visible").each(function () {
                    $(this).html(result);
                    var id = $(this).parent().attr('id');
                    $("#" + id).css("height", "");

                    var idTdParent = $(this).parent().parent().attr('id');
                    $("#" + idTdParent).addClass("isClient");
                });
                $("#BetlistByCustInfo").gridScroller(BetListByCustInfo.gridOpts);
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
    Helper.renderCalendarFromTo("#txtfromdate, #txttodate");
    Helper.range_date();
    BetListByCustInfo.getBetListByCustInfo();
    BetListByCustInfo.getBettyes();
    $("#txtRefNo").attr("disabled", "disabled");
});

