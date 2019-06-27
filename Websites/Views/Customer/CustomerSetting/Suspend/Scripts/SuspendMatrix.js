$(document).ready(function () {
    var DOMElements = {
        btnSubmit: $('#btnSubmit'),
        txtUsername: $('#txtUsername'),
        errMsg: $('#err'),
        tblCustInfo: $('#tblCustInfo'),
        tblSuspendMatrix: $('#tblSuspendMatrix')
    };

    var RoleName = {
        1: 'Member',
        2: 'Agent',
        3: 'Master',
        4: 'Super'
    };

    DOMElements.txtUsername.keypress(function (e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key == 13) DOMElements.btnSubmit.click();
        return;
    });

    DOMElements.btnSubmit.click(function () {
        try { parent.BlockUI(); } catch (e) { }
        DOMElements.btnSubmit.attr("disabled", true);
        clearMessage();
        var params = {
            userName: DOMElements.txtUsername.val()
        };

        $.ajax({
            type: 'POST',
            url: "GetSuspendMatrix",
            dataType: "json",
            data: params,
            success: function (result) {
                var htmlCustinfoRow = '', htmlSuspendMatrixRow = '';
                for (var i = 0 ; i < result.CustomerInfo.length; i++) {
                    var dataRow = result.CustomerInfo[i];
                    htmlCustinfoRow += '<tr><td style="text-transform:uppercase">' +
                        (i == 0 ? 'Customer' : RoleName[dataRow.RoleId]) + '</td><td>' +
                        (i == 0 ? '<b>' + dataRow.UserName + '</b>' : dataRow.UserName) + '&nbsp;(' + dataRow.CustId + ')</td></tr>';
                }

                for (var i = 0 ; i < result.SuspendMatrix.length; i++) {
                    var dataRow = result.SuspendMatrix[i];
                    var j = 0;
                    htmlSuspendMatrixRow += '<tr class="trbgov">';
                    htmlSuspendMatrixRow += '<td class="ViewPTHeader l">' + RoleName[dataRow.RoleId] + ' [' + dataRow.RoleId + ']</td>';
                    htmlSuspendMatrixRow += '<td>' + (dataRow.Suspend ? 'Yes' : 'No') + '</td>';
                    htmlSuspendMatrixRow += RenderSuspendMatrixCell(j++, i, dataRow.sSuspend);
                    htmlSuspendMatrixRow += RenderSuspendMatrixCell(j++, i, dataRow.mSuspend);
                    htmlSuspendMatrixRow += RenderSuspendMatrixCell(j++, i, dataRow.aSuspend);
                    htmlSuspendMatrixRow += RenderSuspendMatrixCell(j++, i, dataRow.pSuspend);
                    htmlSuspendMatrixRow += '</tr>';
                }

                $('tbody', DOMElements.tblCustInfo).html(htmlCustinfoRow);
                $('tbody', DOMElements.tblSuspendMatrix).html(htmlSuspendMatrixRow);
            },
            complete: function () {
                DOMElements.btnSubmit.removeAttr("disabled");
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    });

    var RenderSuspendMatrixCell = function (col, row, value) {
        if (col <= row) {
            return '<td>' + (value ? '1' : '0') + '</td>';
        }
        else {
            return '<td class="disabled"></td>';
        }
    }

    var clearMessage = function () {
        DOMElements.errMsg.empty().hide();
    }


    DOMElements.btnSubmit.click();
});
