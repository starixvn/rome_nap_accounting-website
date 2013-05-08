var _pageIndex = 1;
var _pageSize = 50;
var _totalItems = 0;
function pageload(hash) {
    if (hash) {
        // restore ajax loaded state
        if ($.browser.msie) {
            hash = encodeURIComponent(hash);
        }
        GetDailyTransactionByPager(hash.toString());
    } else {
        // start page
        document.getElementById("sContent").innerHTML = "";
    }
}
function BindData(rs) {
    var rsvalue = $.evalJSON(rs);
    $("#sContent").html(rsvalue.Content);
    if ($('#dailytrantable').height() > 609) {
        $('#dailytrantable').Scrollable(609, $('#dailytrantable').innerWidth());
    }
    //bind event for pager
    $(".pagenav_ibtn").click(function () {
        if (isNumberNotNull("txtpage")) {
            var pageIndex = $("#txtpage").val();
            if (parseInt(pageIndex) <= parseInt(rsvalue.Totalpages)) {
                var hash = this.href + pageIndex;
                hash = hash.replace(/^.*#/, '');
                $.historyLoad(hash);
            }
            return false;
        }
        else {
            return false;
        }
    });

    $(".pagerlink").click(function () {
        var hash = this.href;
        hash = hash.replace(/^.*#/, '');
        $.historyLoad(hash);
        return false;
    });
    try { parent.jQuery.unblockUI(); } catch (e) { }

}
function callAjax(postdata) {
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceDailyTran.svc/GetDailyAllTransData",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        data: postdata,
        success: BindData
    });
}
function GetDailyTransaction(fromcache) {
    var params = '{"pageIndex":1,"date":"' + $("#txndate").val() + '","type":"' + $("#txtType").val() + '","creator":"' + $("#txtCreator").val() + '"}';    
    callAjax(params);
}
function GetDailyTransactionByPager() {
    if (pageIndex == null) {
        if (isNumberNotNull("txtpage"))
            pageIndex = $("#txtpage").val();
        else
            return;
    }
    if (pageIndex != "") {
        var params = '{"pageIndex":' + pageIndex + ',"date":"' + $("#txndate").val() + '","type":"' + $("#txtType").val() + '","creator":"' + $("#txtCreator").val() + '"}';
        callAjax(params);
    }
}

jQuery(document).ready(function () {
    GetDailyTransaction();
    $.historyInit(pageload, "DailyAllTransaction.aspx");
});

function rowover(obj) {
    $(".tbllayout .overrow").removeClass("overrow");
    $(obj).addClass("overrow");
}
function rowout() {
    $(".tbllayout .overrow").removeClass("overrow");
}

function exportToExcel(){
    var pageIndex = $("#txtpage").val();
    var date = $("#txndate").val();
    var type = $("#txtType").val();
    var creator = $("#txtCreator").val();
     var url ='DailyAllTransaction.aspx?IsExport=1&pageIndex=' + pageIndex + '&date=' + date + '&type=' +
            type + '&creator=' + creator;
       $('#frm').attr('src', url);
}