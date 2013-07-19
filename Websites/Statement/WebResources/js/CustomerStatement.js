function KeyPress(e)
{
     var code = (e.keycode ? e.keycode : e.which);
        if(code==13){
            GetDataReport();
        }
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

function obj(id) {
    return document.getElementById(id);
}

var custName = '';
function GetDataReport() {
    var fdate, tdate
    if (obj('fdate')) {
        fdate = obj('fdate').value;
    }
    else {
        fdate = '';
    }
    if (obj('tdate')) {
        tdate = obj('tdate').value;
    }
    else {
        tdate = '';
    }

    //alert(encodeURIComponent($("#txtCustName").attr("value")));

    try { parent.BlockUI(); } catch (e) { }
    custName = encodeURIComponent($("#txtCustName").attr("value"));
    $.ajax({

        url: 'GetData.ashx?fdate=' + fdate + '&tdate=' + tdate + '&repno=' + obj('repno').value + "&custname=" + custName,
        dataType: 'html',
        success: function(data) {

            $("#statementCotent").attr("style", "display:block");
            obj('sData').innerHTML = data;
            
//            if ($('#rptMemberStatement').height() > 500) {
//                $('#rptMemberStatement').Scrollable(500, $('#rptMemberStatement').innerWidth());
//            }

            parent.jQuery.unblockUI();
        }
    });

    //$("#btnVerify").css("backgound-color", "");
    //$("#btnVerify").css("backgound-color", "url(../images/buttons/bgred_btn.jpg) repeat-x left");
      
}

function ViewCustomerStatement(reportNo) {

    var fdate, tdate
    if (obj('fdate')) {
        fdate = obj('fdate').value;
    }
    else {
        fdate = '';
    }
    if (obj('tdate')) {
        tdate = obj('tdate').value;
    }
    else {
        tdate = '';
    }

    var url = "CustomerStatement.aspx?repno=" + reportNo.toString() 
    + "&fdate=" + fdate.toString()
    + "&tdate=" + tdate.toString()
    + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));

    window.location.href = url;
}

function SBD(transtype, fdate) {
    url = 'StatementByDay.aspx?transtype='
        + transtype + '&fdate=' + fdate
    + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));
    document.location.href = url;
}
function VBL(fdate) {
    url = '../BetList/BetList.aspx?bltype=2&fdate=' + fdate
    + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));
    document.location.href = url;
}

function ViewStatement(repno) {
    url = "CustomerStatement.aspx?repno=" + repno
    + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));
    document.location.href = url;
}

function ExportToExcel() {
    var fdate = '';
    var tdate = '';
    if (obj('fdate')) {
        fdate = obj('fdate').value;
    }
    if (obj('tdate')) {
        tdate = obj('tdate').value;
    }

    location.href = 'CustomerStatement.aspx?fdate=' + fdate + '&tdate=' + tdate + '&repno=' + obj('repno').value + '&IsExport=1'
    + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));
}

function DelTrans(refno) {
    if (confirm("Do you want to delete the transaction with Ref.No: " + refno + "?")) {
        if (confirm("To double check, do you want to delete the transaction with Ref.No: " + refno + "?")) {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'DelTrans.ashx?refno=' + refno + '&custName=' + custName,
                dataType: 'html',
                success: function(data) {
                    var arr = data.split('|');
                    if (arr[0] == "1") {
                        obj('sData').innerHTML = arr[1];

                        parent.jQuery.unblockUI();
                    }
                    else {
                        setTimeout("GetDataReport()", 3000);
                    }

                }
            });
        }
    }

}

function GetSelectedRefNo() {
    var refNos;
    refNos = "";

    $("input[type='checkbox'][id!='chkAll']").each(function() {
        if (this.checked == true && !this.disabled)
            refNos += this.id + ",";
    });

    return refNos;
}


function VerifyTrans(btn) {    
    var bCconfirm = window.confirm("Are you sure you want to verify the selected transaction(s)?");
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
    var bCconfirm = window.confirm("Are you sure you want to unverify the selected transaction(s)?");
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
        GetDataReport();
    }
    else {
        window.alert("Cannot update data! Please contact Adminstrator");
        parent.jQuery.unblockUI();
    }
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

var isonauto = false;
jQuery(document).ready(function () {

    var txtusername = $('#txtCustName');

    //Auto complete code
    var autosearch = txtusername.autocomplete({ serviceUrl: '../Handlers/QueryUserName.ashx', minChars: 2 });
    $('.autocomplete').mouseover(function () {
        isonauto = true;
    }).mouseout(function () {
        isonauto = false;
    });


    txtusername.blur(function () {
        if (!isonauto) {
            $('#Autocomplete_1').css('display', 'none');
        }
        else {
            txtusername.focus();
        }
    });

    //add enter event
    $('#bannerfilter').bind('keypress', function (e) {
        var code = (e.keycode ? e.keycode : e.which);
        if (code == 13) {
            GetDataReport();
        }
    });
});


function viewDetail(fdate, transtypeid) {
    url = 'BetList/GetBetListStatement?CustID=1176092&TransType=' + transtypeid + '&FromDate=' + fdate;
    parent.OpenPopup(url, "Bet list", 1050, 700, true, false, window, 2000, null, null);

}
