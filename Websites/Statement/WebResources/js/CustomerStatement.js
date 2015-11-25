function KeyPress(e) {
    var code = (e.keycode ? e.keycode : e.which);
    if (code == 13) {
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
    try { parent.BlockUI(); } catch (e) { }
    custName = encodeURIComponent($("#txtCustName").attr("value"));

    statementReportManager.callAjax(
        domain + 'NapStatement/StatementReport/CheckInternalByUsername',
        {
            Username: custName
        },
        completeUpdateIsInternal);
}

function completeUpdateIsInternal(rs) {
    isInternalCustomer = rs;
    var fdate, tdate;
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

    var criteria = {
        fromDate: moment(new Date(fdate)).format("MM/DD/YYYY"),
        toDate: moment(new Date(tdate)).format("MM/DD/YYYY"),
        custId: 0,
        roleId: 0,
        balanceUpDay: balanceUpDay,
        moveBetDay: moveBetsDay,
        username: $('#txtCustName').val()
    };
    statementReportManager.initFunction(
                function () {
                    parent.jQuery.unblockUI();
                },
                function (options) {
                    parent.OpenPopup(options.url,
                        options.title,
                        options.width,
                        options.height,
                        true,
                        true,
                        window,
                        2000,
                        null,
                        null);
                },
                function (options) {
                    var criteria = {
                        username: unescape(custName),
                        transType: options.transactionType,
                        fromDate: options.statementDate
                    };

                    var url = domain + "BetList/GetBetListStatement?" +
                        jQuery.param(criteria);
                    parent.OpenPopup(url,
                        options.title,
                        1024,
                        options.height,
                        true,
                        true,
                        window,
                        2000,
                        null,
                        null);
                });

    var reportNo = obj('repno').value;
    if (!isInternalCustomer && reportNo == 1 && isNewStatementTab == "1") {
        if (!statementReportManager.checkCriteria(criteria)) {
            try { parent.jQuery.unblockUI(); } catch (e) { }
            return;
        }

        if (statementReportManager.IsUseNewLayout(criteria)) {
            jQuery('#statementCotent').addClass("hidden");
            statementReportManager.getStatementReport(criteria);
            return;
        }
    }

    jQuery('#GridContainer').addClass("hidden");
    jQuery('#statementCotent').removeClass("hidden");
    $.ajax({

        url: 'GetData.ashx?fdate=' + fdate + '&tdate=' + tdate + '&repno=' + reportNo + "&custname=" + custName,
        dataType: 'html',
        success: function (data) {

            $("#statementCotent").attr("style", "display:block");
            obj('sData').innerHTML = data;
            parent.jQuery.unblockUI();
        }
    });
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

var statementReportCriteria = null;
function ExportToExcel() {
    statementReportManager.callAjax(
        domain + 'NapStatement/StatementReport/CheckInternalByUsername',
        {
            Username: $('#txtCustName').val()
        },
        function (rs) {
            isInternalCustomer = rs;
            var fdate = '';
            var tdate = '';
            if (obj('fdate')) {
                fdate = obj('fdate').value;
            }
            if (obj('tdate')) {
                tdate = obj('tdate').value;
            }

            var criteria = {
                fromDate: moment(new Date(fdate)).format("MM/DD/YYYY"),
                toDate: moment(new Date(tdate)).format("MM/DD/YYYY"),
                custId: 0,
                roleId: 0,
                username: custName
            };

            var reportNo = obj('repno').value;
            if (!isInternalCustomer && reportNo == 1 && isNewStatementTab == "1") {
                if (!statementReportManager.checkCriteria(criteria)) {
                    parent.jQuery.unblockUI();
                    return;
                }

                if (statementReportManager.IsUseNewLayout(criteria)) {
                    statementReportCriteria = criteria;
                    var url = domain + 'NapStatement/StatementReport/GetCustomerInfo';
                    statementReportManager.callAjax(url, criteria, exportStatementReport);
                    return;
                }
            }

            location.href = 'CustomerStatement.aspx?fdate=' + fdate + '&tdate=' + tdate + '&repno=' + obj('repno').value + '&IsExport=1'
                + "&custname=" + encodeURIComponent($("#txtCustName").attr("value"));
        });
}

function exportStatementReport(rs) {
    statementReportCriteria.custId = rs.CustomerId;
    statementReportCriteria.roleId = rs.RoleId;
    statementReportManager.exportStatementReport(statementReportCriteria);
}

function DelTrans(refno) {
    __confirm("Delete transaction", "Do you want to delete the transaction with Ref.No: " + refno + "?", 250, 150, 'cfm_1', function () {
        __close(null, null, 'cfm_1', null);
        __confirm("Delete transaction", "To double check, do you want to delete the transaction with Ref.No: " + refno + "?", 250, 180, 'cfm_2', function () {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'DelTrans.ashx?refno=' + refno + '&custName=' + custName,
                dataType: 'html',
                success: function (data) {
                    __close(null, null, 'cfm_2', null);
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
        })
    });
}

function GetSelectedRefNo() {
    var refNos;
    refNos = "";

    $("input[type='checkbox'][id!='chkAll']").each(function () {
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
    $("input[type='checkbox'][id!='chkAll']").each(function () {
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

    if (isNewStatementTab == "0") {
        $("#notemessage").addClass("display-none");
    }
});

function viewDetail(fdate, transtypeid) {
    var username = $('#txtCustName').val();
    url = 'BetList/GetBetListStatement?Username=' + unescape(username) + '&TransType=' + transtypeid + '&FromDate=' + fdate;
    parent.OpenNextPopup("popupStatement", url, "Bet list", 1050, 700, true, true, window, 2000, null, null);
}

function viewGuidelines() {
    var criteria = {
        username: $('#txtCustName').val()
    };
    var url = domain + 'NapStatement/StatementReport/GetCustomerInfo';
    statementReportManager.callAjax(url, criteria, function (rs) {
        parent.OpenPopup(
            window.rootUrl + "NapStatement/StatementReport/Guideline?roleId=" + rs.RoleId + '&canViewBetListStatement =  true',
            statementResources.New_Statement_Guidelines,
            900,
            400,
            true,
            false,
            window,
            2000,
            null,
            null);
    });
}