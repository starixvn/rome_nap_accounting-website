function GetData() {
    parent.BlockUI();
    $('#errMsg').html('');
    if (!ValidateData()) {
        $('#gridContainer').hide();
        parent.jQuery.unblockUI();
        return;
    }

    var params = {
        UserName: $("#txtUserName").val(),
    };

    $.ajax({
        type: "POST",
        url: "Search",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(params),
        dataType: "json",
        beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
        success: function (data) {
            if (data.ErrCode != 0) {
                RenderErrMsg(data.ErrMsg);
            }
            else {
                var bodyRows = [];
                var row = { Columns: [] };
                var isHighLight = data.TotalLeftPanel != data.TotalCashGeneralWallet;
                if (data.UserName != undefined && data.UserName != '') {
                    row.Columns.push({ Name: 'UserName', Value: data.UserName, CellAlign: 'left' });
                    row.Columns.push({ Name: 'TotalCash', Value: data.TotalCash, CellAlign: 'right' });
                    row.Columns.push({ Name: 'OutStanding', Value: data.OutStanding, CellAlign: 'right' });
                    row.Columns.push({ Name: 'TotalLeftPanel', Value: data.TotalLeftPanel, CellAlign: 'right', TdClass: isHighLight ? 'ui-state-highlight' : '' });
                    row.Columns.push({ Name: 'CreditSports', Value: data.CreditSports, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitSports', Value: data.DebitSports, CellAlign: 'right' });
                    row.Columns.push({ Name: 'CreditCasino', Value: data.CreditCasino, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitCasino', Value: data.DebitCasino, CellAlign: 'right' });
                    row.Columns.push({ Name: 'CreditOthers', Value: data.CreditOthers, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitOthers', Value: data.DebitOthers, CellAlign: 'right' });
                    row.Columns.push({ Name: 'CreditTransfer', Value: data.CreditTransfer, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitTransfer', Value: data.DebitTransfer, CellAlign: 'right' });
                    row.Columns.push({ Name: 'TotalCashGeneralWallet', Value: data.TotalCashGeneralWallet, CellAlign: 'right', TdClass: isHighLight ? 'ui-state-highlight' : '' });
                    row.Columns.push({ Name: 'CreditBonusWallet', Value: data.CreditBonusWallet, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitBonusWallet', Value: data.DebitBonusWallet, CellAlign: 'right' });
                    row.Columns.push({ Name: 'TotalCashBonusWallet', Value: data.TotalCashBonusWallet, CellAlign: 'right' });
                    row.Columns.push({ Name: 'CreditNetPosition', Value: data.CreditNetPosition, CellAlign: 'right' });
                    row.Columns.push({ Name: 'DebitNetPosition', Value: data.DebitNetPosition, CellAlign: 'right' });
                    row.Columns.push({ Name: 'TotalNetPosition', Value: data.TotalNetPosition, CellAlign: 'right' });
                }
                else {
                    row.Columns.push({ Name: 'UserName', Value: 'There is no information', CellAlign: 'center', TdClass: 'red', MergeColNumber: 15 });
                }

                bodyRows.push(row);


                var gridOptions =
                {
                    customizeHeader: true,
                    groupHeaderRows: [
                        {
                            Columns: [
                                { HeaderText: "User Name", Name: "UserName", MergeRowNumber: 2 },
                                { HeaderText: "Left Panel", Name: "TotalCash", MergeColNumber: 3 },
                                { HeaderText: "General Wallet", Name: "CreditSports", MergeColNumber: 9 },
                                { HeaderText: "Bonus Wallet", Name: "CreditBonusWallet", MergeColNumber: 3 },
                                { HeaderText: "Net Position", Name: "CreditNetPosition", MergeColNumber: 3 }
                            ]
                        }
                    ],
                    columns: [
                        { HeaderText: "User Name", Width: 200, Name: 'UserName', HeaderAlign: "center", CellAlign: "center", AllowTruncate: true },
                        { HeaderText: "Total Balance", Width: 90, Name: 'TotalCash', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Outstanding", Width: 90, Name: 'OutStanding', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Total", Width: 90, Name: 'TotalLeftPanel', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit Sports", Width: 90, Name: 'CreditSports', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit Sports", Width: 90, Name: 'DebitSports', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit Casino", Width: 90, Name: 'CreditCasino', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit Casino", Width: 90, Name: 'DebitCasino', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit Others", Width: 90, Name: 'CreditOthers', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit Others", Width: 90, Name: 'DebitOthers', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit Transfer", Width: 90, Name: 'CreditTransfer', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit Transfer", Width: 90, Name: 'DebitTransfer', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Total", Width: 90, Name: 'TotalCashGeneralWallet', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit", Width: 90, Name: 'CreditBonusWallet', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit", Width: 90, Name: 'DebitBonusWallet', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Total", Width: 90, Name: 'TotalCashBonusWallet', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Credit", Width: 90, Name: 'CreditNetPosition', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Debit", Width: 90, Name: 'DebitNetPosition', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' },
                        { HeaderText: "Total", Width: 90, Name: 'TotalNetPosition', HeaderAlign: "center", CellAlign: "center", FormatNumber: "0,0.00", FormatNegative: '$' }
                    ],
                    bodyRows: bodyRows,
                    gridExpandHeight: 200,
                    showDistinctRow: false
                };

                $('#gridContainer').show();
                $('#gridId').fanexGrid(gridOptions);
            }
        },
        error: function (xhr) {
            if (xhr.status === 403) {
                $('#gridContainer').hide();
                RenderErrMsg(lang.AccessDenied);
            }
        },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
    });
}

function ValidateData() {
    var usernamesValue = $('#txtUserName').val();
    if (usernamesValue.trim().length == 0) {
        RenderErrMsg('Please enter Username.');
        return false;
    }

    return true;
}

function RenderErrMsg(errMsg) {
    $('#errMsg').append('<div class="alertboxerr"><div>' + errMsg + '</div></div>');
}

function ShowTips() {
    $('#infoMessage').removeClass('hidden');
    $('#infoMessage').dialog({
        width: 'auto'
    });
}

function RenderResultTable(data) {

}