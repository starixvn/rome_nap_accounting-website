var financeStatement = {
    Model: '',
    StatementDetail: '',
    tplMain: '',
    tplLef: '',
    optionGrid: '',
    Params: {
        refNo: -1,
        custName: -1,
        filterBy: '',
        fromdate: '',
        toDay: '',
        balanceUpDay: '',
        includeAccBall: 0
    },
    arrayMark: new Array(),
    VerifyParams: {
        RefNo: '',
        IsVerify: false
    },
    getFinanceStatement: function () {
        //        this.getSatementMain();
        if (financeStatement.getParams() == false) return false;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/GetFinanceStatement",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(financeStatement.Params),
            success: function (result) {
                if (result.length > 0) {
                    $("#btnExportNew").removeAttr("disabled");
                    $("#err").hide();
                    $("#mainreport").show();
                    financeStatement.getSatementMain(result);
                    financeStatement.Model = result;
                    financeStatement.arrayMark = new Array();
                    //                    var heightleft = $("#tbstatementrowheader tbody tr:first").height() - 1;
                    ////                    $("#tbstatementrowheader tbody tr:first").height(heightleft);
                    //                    $('#tbstatementrowheader tbody tr:first').attr('cssChrome', 'height: ' + heightleft + 'px !important');
                    //                    //                    $("#tbstatement tbody tr:first").height(heightleft);
                    //                    //                    $("#tbstatement tbody").height($("#tbstatementrowheader tbody").height());

                }
                else {
                    $("#mainreport").hide();
                    if (result.errorMessage) {
                        showAlertMessage("err", result.errorMessage, 1);
                    }
                    else {
                        showAlertMessage("err", "There is no data.", 1);
                    }
                    $("#err").show();
                }
            },
            error: function () {
                $("#mainreport").hide();
                showAlertMessage("err", "There is no data.", 1);
                $("#err").show();
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    getSatementMain: function (result) {
        var mainreport = $('#statementgrid');
        var financeSatementRows = result;
        var arr = [];
        var arrFirstCols = [];
        var totalItems = financeSatementRows.length;

        for (var i = 0; i < totalItems; i++) {
            var css = "";
            var row = financeSatementRows[i];
            if (row.IsAccummulated) {
                row.RefNo = "";
                row.WinLossDate = "";
                row.ListTrans[0].TransDate = "";
                arr.push(this.renderStatementRow(row, "statement-accu-row"));
                arrFirstCols.push(this.renderFirstCols(row, "statement-accu-row"));
            }
            else if (row.IsOpeningBalance) {
                row.RefNo = "";
                arr.push(this.renderStatementRow(row, css));
                arrFirstCols.push(this.renderFirstCols(row, css));
            }
            else {
                if (row.IsReverse) {
                    arr.push(this.renderStatementRow(row, "reversal"));
                    arrFirstCols.push(this.renderFirstCols(row, "reversal"));
                }
                else {
                    arr.push(this.renderStatementRow(row, css));
                    arrFirstCols.push(this.renderFirstCols(row, css));
                }
            }

        }

        $('#tbstatementrowheader tbody').empty().append(arrFirstCols.join(''));
        $('#tbstatement tbody').empty().append(arr.join(''));
        var isChrome = window.chrome;
        if (isChrome) {
             $('#tbstatement tbody tr:first').attr('style', 'height: 47px !important');
        }
        this.recalcuGrid(false);
        mainreport.removeClass("hidden");
        mainreport.gridScroller(this.optionGrid);
    },
    renderFirstCols: function (row, cssClass) {
        var rowspan = row.ListTrans.length;

        this.row = row;
        var winlossdate = row.WinLossDate;
        var html = '';
        html = '<tr id="' + row.RefNo + '" name="' + row.RefNo + '_MainLeft"  class="' + cssClass + '"><td rowspan ="' + rowspan + '"><div class="c  table-cell" style="width:80px;' + (row.ListTrans[0].AccumTotal.search('NOT EQUAL') == -1 ? 'min-height:42px;' : 'height:56px;') + ' ">' + row.WinLossDate + '</div></td>';
        var TransHTML = '';
        if (row.IsExpand) {
            TransHTML = '<td colspan="2"><div class="l b  table-cell" style="width:350px;' + (Helper.isExistLongWord(row.ListTrans[0].Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '"><a winlostdate="' + winlossdate + '" onclick="financeStatement.getStatementDetail(true, this,' + row.RefNo + ')" class="tree-expand2"> </a>' + row.ListTrans[0].Remark + '</div></td></tr>';
        }
        else {
            TransHTML = '<td colspan="2"><div class="l b table-cell " style="width:350px;' + (Helper.isExistLongWord(row.ListTrans[0].Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '">' + row.ListTrans[0].Remark + '</div></td></tr>';
        }

        html += TransHTML;
        if (rowspan > 1) {
            for (var i = 1; i < rowspan; i++) {
                html += '<tr  id="' + row.RefNo + '" name="' + row.RefNo + '_MainLeft" class="' + cssClass + ' b"><td colspan="2"><div class="l table-cell" style="width:350px;' + (Helper.isExistLongWord(row.ListTrans[i].Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '">' + row.ListTrans[i].Remark + '</div></td></tr>';
            }
        }
        return html;
    },
    renderStatementRow: function (row, cssClass) {
        var row = row;
        var rowspanWinLoss = row.ListTrans.length;
        var rowspanCreator = 0; // for Creator colum
        var tran; // Keep value of every trans 

        rowspanCreator = rowspanWinLoss;
        if (row.IsReverse && !row.IsAccummulated && !row.IsOpeningBalance) {
            for (var i = 0; i < row.ListTrans.length; i++) {
                row.ListTrans[i].AccumTotal = "";
            }
            rowspanCreator = rowspanWinLoss / 2;
        }

        var winlossdate = row.WinLossDate;

        var html = '';

        html = '<tr id="' + row.RefNo + '" name="' + row.RefNo + '_Main" class="' + cssClass + '"><td rowspan ="' + rowspanWinLoss + '"><div class="c  table-cell" style="width:80px;">' + row.WinLossDate + '</div></td>';


        var TransHTML = '';
        var firstTran = row.ListTrans[0];

        if (row.IsExpand) {
            TransHTML = '<td colspan="2"><div class="l b table-cell" style="width:350px;' + (Helper.isExistLongWord(firstTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '"><a winlostdate="' + winlossdate + '" onclick="financeStatement.getStatementDetail(true, this,' + row.RefNo + ')" class="tree-expand2"></a>' + firstTran.Remark + '</div></td>';
        }
        else {
            TransHTML = '<td colspan="2"><div class="l b table-cell" style="width:350px;' + (Helper.isExistLongWord(firstTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '">' + firstTran.Remark + '</div></td>';
        }
        html += TransHTML;
        html += '<td><div class="r" style="width:110px;">' + Helper.formatCSS(firstTran.Total) + '</div></td>';
        if (row.IsOpeningBalance) {
            html += ' <td><div class="r table-cell" style="width:110px;' + (row.ListTrans[0].AccumTotal.search('NOT EQUAL') == -1 ? 'min-height:42px;' : 'height:56px;') + '">' + firstTran.AccumTotal + '</div></td>';
        }
        else {
            html += ' <td><div class="r table-cell" style="width:110px;' + (row.ListTrans[0].AccumTotal.search('NOT EQUAL') == -1 ? 'min-height:42px;' : 'height:56px;') + '">' + Helper.formatCSS(firstTran.AccumTotal) + '</div></td>';
        }

        html += '<td><div class="c" style="width:70px;font-size: 10px;">' + firstTran.TransDate + '</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:100px;">' + row.RefNo + '</div></td><td rowspan ="' + rowspanCreator + '"><div class="c" style="width:80px;">' + firstTran.Creator + '</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:80px;">' + row.CheckerName + '</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:80px;">' + row.CheckerTime + '</div></td>';

        var ReverseHTML = '';

        if (!row.IsShowReversal) {
            var ActionHTML = '';
            ActionHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c " style="width:55px;">&nbsp;</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c " style="width:55px;">&nbsp;</div></td>';
            html += ActionHTML;
            ReverseHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;">&nbsp;</div></td></tr>';

        }
        else {
            var ActionHTML = '';
            if (row.IsVerify) {
                if (row.CheckerName == '') {
                    ActionHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;"><input id="' + row.RefNo + '" type="checkbox" onclick="financeStatement.checkSelectAll(this)"></div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;"><input id="' + row.RefNo + '" class="verified" type="button" onclick="financeStatement.VerifyTrans(' + row.RefNo + ');" title="Verify" value="Verify"></div></td>';
                }
                else {
                    ActionHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;"><input id="' + row.RefNo + '" type="checkbox" disabled="disabled" checked = "checked" onclick="financeStatement.checkSelectAll(this)"</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;"><input id="' + row.RefNo + '" class="unverify" type="button" onclick="financeStatement.UnVerifyTrans(' + row.RefNo + ');" title="Verify" value="Unverify"></div></td>';
                }
            }
            else {
                ActionHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;">&nbsp;</div></td><td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;">&nbsp;</div></td>';
            }
            html += ActionHTML;
            ReverseHTML = '<td rowspan ="' + rowspanWinLoss + '"><div class="c" style="width:55px;"><input id="' + row.RefNo + '" class="reversed" type="button" onclick="financeStatement.DeleteTrans(' + row.RefNo + ');" title="Reverse" value="Reverse"></div></td></tr>';
        }
        html += ReverseHTML;

        for (var i = 1; i < rowspanCreator && i < rowspanWinLoss ; i++) {
           
            html += '<tr id="' + row.RefNo + '"  name="' + row.RefNo + '_Main" class="' + cssClass + '"><td colspan="2"><div class="l b table-cell" style="width:350px;' + (Helper.isExistLongWord(row.ListTrans[i].Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '">' + row.ListTrans[i].Remark + '</div></td><td><div class="r" style="width:110px; ">' + Helper.formatCSS(row.ListTrans[i].Total) + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(row.ListTrans[i].AccumTotal) + '</div></td><td><div class="c" style="width:70px;">' + row.ListTrans[i].TransDate + '</div></td></tr>';
        }

        if (rowspanWinLoss > rowspanCreator ) {
            for (var i = rowspanCreator; i < rowspanWinLoss; i++) {
                html += '<tr id="' + row.RefNo + '"  name="' + row.RefNo + '_Main" class="' + cssClass + '"><td colspan="2"><div class="l b table-cell" style="width:350px;' + (Helper.isExistLongWord(row.ListTrans[i].Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '">' + row.ListTrans[i].Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(row.ListTrans[i].Total) + '</div></td><td><div class="r" style="width:110px;">' + row.ListTrans[i].AccumTotal + '</div></td><td><div class="c" style="width:70px;">' + row.ListTrans[i].TransDate + '</div></td>';
                if (i == rowspanCreator) {
                    html += '<td rowspan ="' + rowspanCreator + '"><div class="c" style="width:80px;">' + row.ListTrans[i].Creator + '</div></td></tr>';
                }
                else {
                    html += '</tr>';
                }
            }
        }
        return html;
    },
    checkSelectItem: function (id) {
        if ($("#chkAll").attr('checked') == true && chkItem.checked == false)
            $("#chkAll").attr('checked', false);

        if (chkItem.checked == true) {
            $("#btnVerify").removeAttr("disabled");
            $("#btnVerify").attr("class", "verified");
            this.checkSelectAll();
        }
        else {
            this.setEnableVerify();
        }
    },
    checkSelectAll: function () {
        var flag = true;
        //explore all check box except chkAll
        $("input[type='checkbox'][id!='chkAll']").each(function () {
            if (this.checked == false && !this.disabled)
                flag = false;
        });

        $("#chkAll").attr('checked', flag);
    },
    setEnableVerify: function () {
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
    },
    itemAllClicked: function () {
        $("input[type='checkbox']").attr('checked', $('#chkAll').is(':checked'));
        $("input[type='checkbox'][disabled]").attr('checked', 'checked');
        this.setEnableVerify();
    },
    getParams: function () {
        if ($('input[name=type]:checked').val() == "-1") {
            financeStatement.Params.custName = '';
            if ($('#txtRefNo').val() == '') {
                $("#mainreport").hide();
                showAlertMessage("err", "Please input Ref No.", 1);
                $("#err").show();
                return false;
            }
            else {
                financeStatement.Params.refNo = Helper.trim($('#txtRefNo').val());
            }

            //            $('#txtCustName').val("");
        }
        else {
            financeStatement.Params.custName = Helper.trim($('#txtCustName').val());
            financeStatement.Params.refNo = -1;
            //            $('#txtRefNo').val("") ;
        }
        financeStatement.Params.fromdate = $('#txtfromdate').val();
        financeStatement.Params.toDay = $('#txttodate').val();
        financeStatement.Params.filterBy = $('#FilterBy').val();
        financeStatement.Params.balanceUpDay = _balanceUpDay;
        financeStatement.Params.includeAccBall = 0;

        //        ? '' : Helper.trim($('#txtRefNo').val())
    },
    GetSelectedRefNo: function () {
        var refNos;
        refNos = "";

        $("input[type='checkbox'][id!='chkAll']").each(function () {
            if (this.checked == true && !this.disabled)
                refNos += this.id + ",";
        });

        return refNos;
    },
    VerifyTrans: function (id) {
        var bCconfirm = window.confirm("Are you sure you want to verify the selected transaction(s)?");
        if (!bCconfirm)
            return bCconfirm;

        var selectedRefNo;
        if (id == "btnVerify") {
            //verify multiple
            selectedRefNo = financeStatement.GetSelectedRefNo();
        }
        else {
            selectedRefNo = id;
        }

        financeStatement.VerifyParams.RefNo = selectedRefNo;
        financeStatement.VerifyParams.IsVerify = true;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/UpdateVerifyTrans",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(financeStatement.VerifyParams),
            success: function (result) {
                if (result.ErrorCode == 0) {
                    window.alert("Update data successfully");
                    setTimeout(financeStatement.getFinanceStatement(), 60000);

                }
                else {
                    window.alert("Cannot update data! Please contact Adminstrator");
                    parent.jQuery.unblockUI();
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    UnVerifyTrans: function (id) {
        var bCconfirm = window.confirm("Are you sure you want to unverify the selected transaction(s)?");
        if (!bCconfirm)
            return bCconfirm;

        selectedRefNo = id;

        financeStatement.VerifyParams.RefNo = selectedRefNo;
        financeStatement.VerifyParams.IsVerify = false;
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/UpdateVerifyTrans",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(financeStatement.VerifyParams),
            success: function (result) {
                if (result.ErrorCode == 0) {
                    window.alert("Update data successfully");
                    setTimeout(financeStatement.getFinanceStatement(), 60000);
                }
                else {
                    window.alert("Cannot update data! Please contact Adminstrator");
                    parent.jQuery.unblockUI();
                }
            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    DeleteTrans: function (refno) {
        financeStatement.VerifyParams.RefNo = refno;
        if (confirm("Do you want to delete the transaction with Ref.No: " + refno + "?")) {
            if (confirm("To double check, do you want to delete the transaction with Ref.No: " + refno + "?")) {
                try { parent.BlockUI(); } catch (e) { }
                $.ajax({
                    type: "POST",
                    url: window.rootUrl + "CustomerStatement/DeleteTrans",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(financeStatement.VerifyParams),
                    success: function (result) {
                        if (result.ErrorCode == 0) {
                            window.alert("Delete successfully");
                            //                            var timeOutId = setTimeout(function () { },)
                            setTimeout(financeStatement.getFinanceStatement(), 80000);

                        }
                        else {
                            window.alert("Cannot Delete data! Please contact Adminstrator");
                            parent.jQuery.unblockUI();
                        }
                    },
                    error: function () {
                        showAlertMessage("err", "There is no data.", 1);
                    },
                    complete: function () {
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                    }
                });

            }
        }
    },
    Template: function () {


        var row = this.StatementDetail;
        var countTrans = row.ListTrans.length;

        var rowspanWinLoss = 0; // for Winloss Date colum
        var rowspanCreator = 0; // for Creator colum
        var tran, subTran; // Keep value of every trans and subtrans

        for (var i = 0; i < countTrans; i++) { // Calcu for Winloss rowspan
            rowspanWinLoss += row.ListTrans[i].ListSubTrans.length;
        }

        if (row.IsReverse) { // If reversal not show value of Accum
            for (var i = 0; i < row.ListTrans.length; i++) {
                row.ListTrans[i].AccumTotal = "";
            }
        }

        rowspanCreator = rowspanWinLoss;
        if (row.IsReverse && !row.IsAccummulated && !row.IsOpeningBalance) {
            rowspanCreator = rowspanWinLoss / 2;
        }

        var tplMain = '<tr  name="' + row.RefNo + '_Detail"><td rowspan=' + rowspanWinLoss + '><div class="c table-cell" style="width:80px;">' + row.WinLossDate + '</div></td>'; // for the First <tr>

        for (var i = 0; i < countTrans; i++) {
            tran = row.ListTrans[i];
            var subrowspan = tran.ListSubTrans.length;
            if (i == 0) {
                for (var j = 0; j < subrowspan; j++) {
                    subTran = tran.ListSubTrans[j];
                    if (j == 0) {
                        tplMain += '<td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell" ><a onclick="financeStatement.showHide(false, this,' + row.RefNo + ')"  class="tree-expanded"> </a>' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.Total) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.AccumTotal) + '</div></td><td ><div class="c" style="width:70px;font-size: 10px;">' + tran.TransDate + '</div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:100px;">' + row.RefNo + '</div></td><td rowspan=' + rowspanCreator + '><div class="c" style="width:80px;">' + tran.Creator + '</div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:80px;">' + row.CheckerName + '</div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:80px;">' + row.CheckerTime + '</div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:55px;"></div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:55px;"></div></td><td rowspan=' + rowspanWinLoss + '><div class="c" style="width:55px;"></div></td></tr>';
                    }
                    else {
                        tplMain += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell">' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td><div class="c" style="width:70px; font-size: 10px;">' + Helper.formatCSS(tran.TransDate) + '</div></td></tr>';
                    }
                }
            }
            else if (i == 2) {
                for (var j = 0; j < subrowspan; j++) {
                    subTran = tran.ListSubTrans[j];
                    if (j == 0) {
                        tplMain += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell" >' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.Total) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.AccumTotal) + '</div></td><td ><div class="c" style="width:70px;font-size: 10px;">' + tran.TransDate + '</div></td><td rowspan=' + rowspanCreator + '><div class="c" style="width:80px;">' + tran.Creator + '</div></td></tr>';
                    }
                    else {
                        tplMain += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell">' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td><div class="c" style="width:70px;font-size: 10px;">' + Helper.formatCSS(tran.TransDate) + '</div></td></tr>';
                    }
                }
            }
            else {
                for (var j = 0; j < subrowspan; j++) {
                    subTran = tran.ListSubTrans[j];
                    if (j == 0) {
                        tplMain += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell" >' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.Total) + '</div></td><td rowspan=' + subrowspan + '><div class="r" style="width:110px;">' + Helper.formatCSS(tran.AccumTotal) + '</div></td><td ><div class="c" style="width:70px;font-size: 10px;">' + tran.TransDate + '</div></td>';
                        if (countTrans < 3 && row.IsReverse) {
                            tplMain += '<td rowspan=' + rowspanCreator + '><div class="c" style="width:80px;">' + tran.Creator + '</div></td></tr>';
                        }
                        else {
                            tplMain += '</tr>';
                        }
                    }
                    else {
                        tplMain += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell">' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td><td><div class="c" style="width:70px;font-size: 10px;">' + Helper.formatCSS(tran.TransDate) + '</div></td></tr>';
                    }
                }
            }
        }


        var tplLef = '<tr  name="' + row.RefNo + '_Detail"><td rowspan=' + rowspanWinLoss + '><div class="c table-cell" style="width:80px;">' + row.WinLossDate + '</div></td>'; // for the First <tr>
        for (var i = 0; i < countTrans; i++) {
            tran = row.ListTrans[i];
            var subrowspan = tran.ListSubTrans.length;
            if (i == 0) {
                for (var j = 0; j < subrowspan; j++) {
                    subTran = tran.ListSubTrans[j];
                    if (j == 0) {
                        tplLef += '<td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell" ><a onclick="financeStatement.showHide(false, this,' + row.RefNo + ')"  class="tree-expanded"> </a>' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td></tr>';
                    }
                    else {
                        tplLef += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell">' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td></tr>';
                    }
                }
            }
            else {
                for (var j = 0; j < subrowspan; j++) {
                    subTran = tran.ListSubTrans[j];
                    if (j == 0) {
                        tplLef += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell" >' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td></tr>';
                    }
                    else {
                        tplLef += '<tr name="' + row.RefNo + '_Detail"><td><div style="width:235px;' + (Helper.isExistLongWord(subTran.Remark) ? 'min-height:42px;display:block' : 'height:42px;') + '" class="b table-cell">' + subTran.Remark + '</div></td><td><div class="r" style="width:110px;">' + Helper.formatCSS(subTran.Amount) + '</div></td></tr>';
                    }
                }

            }
        }

        financeStatement.tplMain = tplMain;
        financeStatement.tplLef = tplLef;
    },
    findTrans: function (refno, winlossDate) {
        var length = financeStatement.Model.length;
        for (var i = 0; i < length; i++) {
            var row = financeStatement.Model[i];
            if (row.RefNo == refno && row.WinLossDate == winlossDate) {
                return row;
            }
        }
    },
    getStatementDetail: function (isMain, obj, refNo) {
        for (var i = 0; i < financeStatement.arrayMark.length; i++) {
            if (refNo == financeStatement.arrayMark[i]) {
                financeStatement.showHide(isMain, obj, refNo, false);
                return false;
            }
        }
        var winlossDate = $(obj).attr("winlostdate");
        financeStatement.StatementDetail = financeStatement.findTrans(refNo, winlossDate);
        var param = { statementDetail: financeStatement.StatementDetail, username: $('#txtCustName').val() };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "CustomerStatement/GetFinanceStatementDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (result) {
                if (result.ListTrans.length > 0) {
                    $("#err").hide();
                    $("#mainreport").show();
                    financeStatement.StatementDetail = result;
                    financeStatement.showHide(isMain, obj, refNo, true);
                    financeStatement.arrayMark.push(refNo);

                }
                else {
                    showAlertMessage("err", "There is no data.", 1);
                    $("#err").show();
                    financeStatement.recalcuGrid(true);
                    $("#statementgrid").gridScroller(financeStatement.optionGrid);
                }

            },
            error: function () {
                showAlertMessage("err", "There is no data.", 1);
                $("#err").show();
                financeStatement.recalcuGrid(true);
                $("#statementgrid").gridScroller(financeStatement.optionGrid);
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },
    showHide: function (isMain, obj, refNo, isGetData) {
        $("#err").hide();
        grid = $("#tbstatement");
        grid1 = $("#tbstatementrowheader");
        var main = $('tr[name=' + refNo + '_Main]');
        var mainLeft = $('tr[name=' + refNo + '_MainLeft]');

        var detail = $('tr[name=' + refNo + '_Detail]');
        var detailLeft = $('tr[name=' + refNo + '_DetailLeft]');
        if (isMain) {
            if (isGetData) {

                var main = $('tr[name=' + refNo + '_Main]');
                var mainLeft = $('tr[name=' + refNo + '_MainLeft]');

                financeStatement.Template();
                var classname = $("#" + refNo).attr("class");

                main.hide().last().after(financeStatement.tplMain);
                mainLeft.hide().last().after(financeStatement.tplLef);

                var detail = $('tr[name=' + refNo + '_Detail]');
                var detailLeft = $('tr[name=' + refNo + '_DetailLeft]');
                detail.addClass(classname);
                detailLeft.addClass(classname);

            }
            else {
                main.hide();
                mainLeft.hide();
                detail.show();
                detailLeft.show();
            }
        }
        else {
            detail.hide();
            detailLeft.hide();
            main.show();
            mainLeft.show();
        }

        this.recalcuGrid(false);
        $('#statementgrid').removeClass("hidden");
        $('#statementgrid').gridScroller(this.optionGrid);
    },
    recalcuGrid: function (error) {
        if (error) {
            this.optionGrid = {
                tblContentId: 'tbstatement',
                manualCopyHeader: true,
                fullWidth: true,
                fullHeight: true,
                gridWidth: 1285,
                gridHeight: 113,
                freezeWidth: 435,
                numberFreezeCol: 2
            };
        }
        else {
            this.optionGrid = {
                tblContentId: 'tbstatement',
                manualCopyHeader: true,
                fullWidth: true,
                fullHeight: true,
                gridWidth: 1285,
                gridHeight: 90,
                freezeWidth: 435,
                numberFreezeCol: 2
            };
        }

    },
    disabledFilter: function () {
        if ($('input[name=type]:checked').val() == "-2") {
            $("#txtRefNo").attr("disabled", "disabled");

            $("#txtCustName").removeAttr("disabled", "disabled");
            $("#txttodate").removeAttr("disabled", "disabled");
            $("#txtfromdate").removeAttr("disabled", "disabled");
            $("#btnToday").removeAttr("disabled", "disabled");
            $("#FilterBy").removeAttr("disabled", "disabled");

        }
        else {
            $("#txtCustName").attr("disabled", "disabled");
            $("#txttodate").attr("disabled", "disabled");
            $("#txtfromdate").attr("disabled", "disabled");
            $("#btnToday").attr("disabled", "disabled");
            $("#FilterBy").attr("disabled", "disabled");

            $("#txtRefNo").removeAttr("disabled", "disabled");

        }
    },
    exportExcel: function () {
        this.getParams();
        var url = 'Index?isExport=true&title=' + financeStatement.Params.custName + '_Statement' + '&refno=' + financeStatement.Params.refNo +
        '&custName=' + financeStatement.Params.custName + '&filterby=' + financeStatement.Params.filterBy + '&fdate="' + financeStatement.Params.fromdate +
        '"&tdate="' + financeStatement.Params.toDay + '"&balupday=' + financeStatement.Params.balanceUpDay + '&includeaccbal=' + financeStatement.Params.includeAccBall;

        $('#frm').attr('src', url);
    }
};

var Helper = {
    formatCSS: function (num) {
        if (num) {
            if (num.indexOf("-") != -1) {
                return '<div style="color:red; overflow: hidden; width:110px;">' + num + '</div>'
            }
            return num;
        }
        return num;
    },
    trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    reset_date: function () {
        var dayNoReset = 7;
        var fdateId = 'txtfromdate';
        var tdateId = 'txttodate';
        var td = new Date();
        var fd = new Date(td.getTime() - dayNoReset * 24 * 60 * 60 * 1000);
        document.getElementById(fdateId).value = (fd.getMonth() + 1) + "/" + fd.getDate() + "/" + fd.getFullYear();
        document.getElementById(tdateId).value = (td.getMonth() + 1) + "/" + td.getDate() + "/" + td.getFullYear();
    },
    isExistLongWord: function (words) {
        var arr = words.replace(/  /g, " ").split(' ');
        var len = arr.length;
        if (len < 1) {
            return false;
        }
        for (var i = 0; i < len; i++) {
            if (arr[i].length > 30)
                return true;
        }
        return false;
    }
};

head.ready(function (event) {
      Calendar.setup({
         animation: false,
         showTime :false,
         bottomBar:true,
         trigger: "fdateTrigger",
         inputField: "txtfromdate",
         dateFormat: "%m/%d/%Y",
         min: "01012008",
         max: _maxDate,
         selection: [_selectFromDate],
         onSelect: function() {
            ""
            this.hide();
            }
    });

    Calendar.setup({
        animation: false,
        showTime: false,
        bottomBar: true,
        trigger: "tdateTrigger",
        inputField: "txttodate",
        dateFormat: "%m/%d/%Y",
        min: "01012008",
        max: _maxDate,
        selection: [_selectToDate],
        onSelect: function () {
            ""
            this.hide();
        }
    });

    financeStatement.getFinanceStatement();
    $("#txtRefNo").attr("disabled", "disabled");
    $(document).keypress(function (e) {
        if (e.which == 13) {
            financeStatement.getFinanceStatement();
        }
    });
    $('#statementgrid').initGrid({ tblContentId: 'tbstatement' });


});
