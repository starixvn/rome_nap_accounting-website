var fromcache = 0;
var transdate = '', type = '', creator = '', status = '', verifyBy = '';
jQuery(document).ready(function() {
    GetDailyTransaction(1, 0);
    $.historyInit(pageload, "DailyTransaction.aspx");
    
    //add enter event
    $('#bannerFilter').bind('keypress', function(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if(code==13){
            GetDailyTransaction(1, 0);
        }
    });

});

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
    var selectedStatus = $("#ddlStatus option:selected").val();
    //if filtering by verified status, always disable button verify
    if (selectedStatus == 2) {
        $('#chkAll').attr('disabled', 'disabled');
    }

    //bind event for pager
    $(".pagenav_ibtn").click(function() {
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

    $(".pagerlink").click(function() {
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
        url: "../Services/WCF/ServiceDailyTran.svc/GetDailyTransData",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: postdata,
        success: BindData
    });
}


function CreateParams(pageIndex, fromcache) {
    transdate = $("#txndate").val();
    status = $("#ddlStatus option:selected").val();
    creator =  $("#txtCreator").val();
    type = $("#txtType").val();
    verifyBy = $("#txtVerifyBy").val();
    var params = '{"pageIndex":' + pageIndex.toString() +
                 ',"fromcache":' + fromcache.toString() +
				 ',"date":"' + transdate +
				 '","type":"' + type +
				 '","status":' + status +
				 ',"creator":"' + creator +
				 '","verifyBy":"' + verifyBy +
				 '"}';
    transdate = $("#txndate").val();
    return params;
}

function GetSelectedRefNo() {
    var refNos;
    refNos = "";

    $("input[type='checkbox'][id!='chkAll']").each(function() {
        
        if (this.checked == true && !this.disabled) {                  
            refNos += this.id + ",";
         }
    });


    return refNos;
}


function VerifyTrans(btn) {

    var bCconfirm = window.confirm("Are you sure you want to verify selected transaction(s)?");
    if (!bCconfirm)
        return bCconfirm;

    var selectedRefNo;
    if (btn.id.toLowerCase() == "btnverify") {
        //verify multiple
        selectedRefNo = '{"refNoList":"' + GetSelectedRefNo() + '"}';
    }
    else {
        selectedRefNo = '{"refNoList":"' + btn.id + '"}';
    }
    
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceDailyTran.svc/VerifyTransaction",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: selectedRefNo,
        success: VerifyTransCallBack
    });
}

function UnVerifyTrans(trans) {

    var bCconfirm = window.confirm("Are you sure you want to unverify selected transaction(s)?");
    if (!bCconfirm)
        return bCconfirm;

    var selectedRefNo = '{"refNo":' + trans.id + '}';
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../Services/WCF/ServiceDailyTran.svc/UnVerifyTransaction",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: selectedRefNo,
        success: VerifyTransCallBack
    });
}

function VerifyTransCallBack(data) {

    //request to process data successful
    if (data == true) {
        window.alert("Update data successfully");
        GetDailyTransaction(1, 0);
    }
    else {
        window.alert("Cannot update data! Please contact Adminstrator");
        parent.jQuery.unblockUI();
    }
}

function GetDailyTransaction(pageIndex, fromcache) {

    var params = CreateParams(pageIndex, fromcache);
    callAjax(params, fromcache);
}

function GetDailyTransactionByPager(pageIndex) {
    if (pageIndex == null) {
        if (isNumberNotNull("txtpage"))
            pageIndex = $("#txtpage").val();
        else
            return;
    }

    var params;
    if (pageIndex != "") {
        params = CreateParams(pageIndex, 1);

    }
    else {
        params = CreateParams(1, 0);
    }

    callAjax(params);
}

function rowover(obj) {
    $(".tbllayout .overrow").removeClass("overrow");
    $(obj).addClass("overrow");
}
function rowout() {
    $(".tbllayout .overrow").removeClass("overrow");
}

function ItemAllClicked() {
    $("input[type='checkbox']").attr('checked', $('#chkAll').is(':checked'));
    $("input[type='checkbox'][disabled]").attr('checked', 'checked');
    SetEnableVerify();
}

function CheckSelectItem(chkItem) {

    //at least checkbox item checked is false => checkall.checked must be false;
    if ($("#chkAll").attr('checked') == true && chkItem.checked == false)
        $("#chkAll").attr('checked', false);

    //if current checkbox item is true
    //must check all item checkbox, if all are checked => checkall is true
    //else checkall is false
    if (chkItem.checked == true) {
        //enable verified button
        $("#btnVerify").removeAttr("disabled");
        $("#btnVerify").attr("class", "verified");
        CheckSelectAll();
    }
    else {
        SetEnableVerify();
    }
}

//enable verify button checkbox if and only if
//the number checked checkbox > disabled checkbox 
//disalble checkbox always checked
function SetEnableVerify() {

    var totalCheckedCheckbox = $("input:checked").length;
    var totalDisabledCheckbox = $("input[type='checkbox'][disabled]").length;

    if (totalCheckedCheckbox > totalDisabledCheckbox) {
        $("#btnVerify").removeAttr("disabled");
        $("#btnVerify").attr("class", "verified");
    }
    else {
        $("#btnVerify").attr("disabled", "true");
        $("#btnVerify").attr("class", "verifiedDisabled");
    }
}

function CheckSelectAll() {

    var flag = true;
    //explore all check box except chkAll
    $("input[type='checkbox'][id!='chkAll']").each(function() {
        if (this.checked == false && !this.disabled)
            flag = false;
    });

    $("#chkAll").attr('checked', flag);
}

function ExportExcel(){
    try { parent.BlockUI(); } catch (e) { }
    /*var url = 'DailyTransaction.aspx?IsExport=1&transDate=' +  $("#txndate").val() + '&type=' +
          $("#txtType").val() + '&status' + $("#ddlStatus option:selected").val() +'&creator'  + $("#txtCreator").val() + '&verifyBy='  + $("#txtVerifyBy").val();*/   
    var url = 'DailyTransaction.aspx?IsExport=1&transDate=' + transdate + '&type=' +
          type + '&status' + status +'&creator'  + creator + '&verifyBy=' + verifyBy; 

    $('#frm').attr('src', url);
    try { parent.jQuery.unblockUI(); } catch (e) { }
}

        