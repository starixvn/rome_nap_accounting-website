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
    jQuery("#errBox").hide();
    jQuery("#sData").show();
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
                1220,
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
    var isReward = $('input[name="radWallet"]:checked').val() == '1';
    $.ajax({
        url: 'GetData.ashx?fdate=' + fdate + '&tdate=' + tdate + '&repno=' + reportNo + "&custname=" + custName + "&isReward=" + isReward,
        dataType: 'html',
        success: function (data) {
            $("#statementCotent").attr("style", "display:block");
            obj('sData').innerHTML = data;
            parent.jQuery.unblockUI();
        },
        error: function (jqXHR) {
            if (jqXHR.status === 403) {
                $('#txtCustName').val("");
                jQuery('#GridContainer').addClass("hidden");             
                $("#statementCotent").addClass("hidden");  
                var denyObject = $.parseJSON(jqXHR.responseText);
                var errorBox = $("#errBox");
                errorBox.find("div").html(denyObject.ErrorMessage);
                errorBox.show();
                parent.jQuery.unblockUI();
            }
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

function VBL(fdate) {
    url = '../BetList/BetList.aspx?bltype=2&fdate=' + fdate
        + "&custname=" + encodeURIComponent($("#`").attr("value"));
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
                username: custName,
                isReward: $("input[name='radWallet']:checked").val() == '1'
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
                + "&custname=" + encodeURIComponent($("#txtCustName").attr("value")) + "&isReward=" + criteria.isReward;
        });
}

function exportStatementReport(rs) {
    statementReportCriteria.custId = rs.CustomerId;
    statementReportCriteria.roleId = rs.RoleId;
    statementReportManager.exportStatementReport(statementReportCriteria);
}

function ReverseTrans(refno, isReward) {
    __confirm("Reverse transaction", "Do you want to reverse the transaction with Ref.No: " + refno + "?", 250, 150, 'cfm_1', function () {
        __close(null, null, 'cfm_1', null);
        __confirm("Reverse transaction", "To double check, do you want to reverse the transaction with Ref.No: " + refno + "?", 250, 180, 'cfm_2', function () {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'DelTrans.ashx?refno=' + refno + '&custName=' + custName + '&isReward=' + isReward,
                dataType: 'html',
                success: function (data) {
                    __close(null, null, 'cfm_2', null);
                    var arr = data.split('|');
                    if (arr[0] == "1") {
                        __showMessage("", arr[1], 350, 250);
                        parent.jQuery.unblockUI();
                    }
                    else {
                        __showMessage("", arr[1], 350, 250);
                        setTimeout("GetDataReport()", 3000);
                    }
                }
            });
        })
    });
}

function GetSelectedRefNo() {
    var refNos = "";

    $("input[type='checkbox'][id!='chkAll']").each(function () {
        if (this.checked == true && !this.disabled)
            refNos += this.id + ",";
    });

    return refNos;
}


function VerifyTrans(btn) {
    __confirm("Verify transaction", "Are you sure you want to verify the selected transaction(s)?", 400, 250, 'cfm_1', function () {
        __close(null, null, 'cfm_1', null);

        let selectedRefNo;
        let isReward = btn.dataset.isReward == 'true';
        if (btn.id.toLowerCase() == "btnverify") {
            //verify multiple
            selectedRefNo = GetSelectedRefNo();
        }
        else {
            selectedRefNo = btn.id;
        }

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/UpdateVerifyTrans",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                RefNo: (isReward ? "" : selectedRefNo),
                RewardRefNo: (isReward ? selectedRefNo : ""),
                IsVerify: true
            }),
            success: VerifyTransCallBack
        });
    });
}

function UnVerifyTrans(trans) {
    __confirm("UnVerify transaction", "Are you sure you want to unverify the selected transaction(s)?", 400, 250, 'cfm_1', function () {
        __close(null, null, 'cfm_1', null);
        let isReward = trans.dataset.isReward == 'true';
        var selectedRefNo = trans.id;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/UpdateVerifyTrans",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({
                RefNo: (isReward ? "" : selectedRefNo),
                RewardRefNo: (isReward ? selectedRefNo : ""),
                IsVerify: false
            }),
            success: VerifyTransCallBack
        });
    });

}

function VerifyTransCallBack(data) {

    //request to process data successful
    if (data.ErrorResults.length == 0) {
        setTimeout(() => {
            GetDataReport();
            __showMessage("", "Update data successfully", 350, 250);
        }, 3000);
    }
    else {
        __showMessage("", "Cannot update data! Please contact Adminstrator", 350, 250);
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

    $('input[name="radWallet"]').change(GetDataReport);
});

function viewDetail(fdate, transtypeid) {
    var username = $('#txtCustName').val();
    url = 'BetList/GetBetListStatement?Username=' + unescape(username) + '&TransType=' + transtypeid + '&FromDate=' + fdate;
    parent.OpenNextPopup("popupStatement", url, "Bet list", 1250, 700, true, true, window, 2000, null, null);
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

function onClickAllCheckBox() {
    var checkedAll = $('#chkAll').is(':checked');
    $("input[name='chkReverse']").each(function () {
        this.checked = checkedAll;
    });

    this.updateBuntonStateAndSelectedCount();
}

function ReverseTransAll() {

    let transIds = [];
    $("input[name='chkReverse']").each(function () {
        if (this.checked) {
            transIds.push(this.id.replace('chkReverse_', '') + '_' + (this.dataset.isReward == 'true' ? 1 : 0));
        }
    });
    const refsno = transIds.join(', ');
    const _this = this;
    __confirm("Reverse transaction", "Do you want to reverse the transaction(s) with Ref.No: " + refsno.replaceAll('_0', '').replaceAll('_1', '') + "?", 350, 250, 'cfm_1', function () {
        __close(null, null, 'cfm_1', null);
        __confirm("Reverse transaction", "To double check, do you want to reverse the transaction(s) with Ref.No: " + refsno.replaceAll('_0', '').replaceAll('_1', '') + "?", 350, 250, 'cfm_2', function () {
            try { parent.BlockUI(); } catch (e) { }
            $.ajax({
                url: 'ReverseMultipleTrans.ashx?refsno=' + refsno + '&custName=' + custName,
                dataType: 'html',
                success: function (data) {
                    __close(null, null, 'cfm_2', null);
                    const arr = data.split('|');
                    if (arr[0] == "1") {
                        __showMessage("", arr[1], 350, 250);
                        parent.jQuery.unblockUI();
                        return;
                    }

                    let info = arr[1] && arr[1];
                    arr[2] && (info += arr[2]);

                    setTimeout(() => {
                        __showMessage("", info, 350, 250);
                        _this.GetDataReport();
                    }, 3000);

                }
            });
        })
    });

}

function onClickCheckBoxItem(id) {
    this.updateSelectAll();
    this.updateBuntonStateAndSelectedCount();
}

function updateSelectAll() {
    var flag = true;
    //explore all check box except chkAll
    $("input[type='checkbox'][id!='chkAll']").each(function () {
        if (this.checked == false)
            flag = false;
    });

    $("#chkAll").attr('checked', flag);
}

function updateBuntonStateAndSelectedCount() {
    let count = 0;
    $("input[type='checkbox'][id!='chkAll']").each(function () {
        if (this.checked)
            count++;
    });

    if (count == 0) {
        $("#selectedCount").text("");
        $("#btnReverse").addClass("verifiedDisabled");
        $("#btnReverse").attr("disabled", "disabled");
    } else {
        $("#selectedCount").text(count + ' selected');
        $("#btnReverse").removeClass("verifiedDisabled");
        $("#btnReverse").removeAttr("disabled");
    }
}