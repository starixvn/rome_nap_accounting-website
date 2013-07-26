head.ready(function (event) {
    $('#btnExport').click(function () {
        exportExcel();
    });

    var gridOpts = {
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

    $("#BetlistStatement").initGrid(gridOpts);
    $('#BetlistStatement').removeClass("display-none").gridScroller(gridOpts);
});

function exportExcel(){
    url = window.rootUrl + 'BetList/ExportBetlistStatement?CustID=' + _custidMember + '&TransType=' + _transType + '&FromDate=' + _fromDate ;
    $('#frm').attr('src', url);
}


function showMP(transid, refno) {
    var gridOpts = {
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
    if ($("#td_right_" + transid).hasClass("isClient")) {
        if (!$("#td_right_" + transid).hasClass("isShow")) {
            $(".parlay_" + transid).each(function () {
                $(this).hide();
                var idTdParent = $(this).parent().parent().attr('id');
                $("#" + idTdParent).addClass("isShow");
                $("#BetlistStatement").gridScroller(gridOpts);

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
                $("#BetlistStatement").gridScroller(gridOpts);
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
                $("#BetlistStatement").gridScroller(gridOpts);
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

function showCombMPDetail(tranid, date, refno, betid, matchid) {
    var username = $(".combinationLink span:last").html();
    showDetailsCMP(tranid, "System Parlay - " + username, parseFloat(refno), parseInt(betid), _custid)
}