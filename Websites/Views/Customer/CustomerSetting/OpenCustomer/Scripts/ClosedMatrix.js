$(document).ready(function () {
    var DOMElements = {
        btnSubmit: $('#btnSubmit'),
        txtUsername: $('#txtUsername'),
        errMsg: $('#err'),
        tblCustInfo: $('#tblCustInfo'),
        tblClosedMatrix: $('#tblClosedMatrix')
    };

    var RoleName = {
        1: 'Member',
        2: 'Agent',
        3: 'Master',
        4: 'Super'
    };

    DOMElements.txtUsername.keypress(function (e) {
        e = e ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key === 13) DOMElements.btnSubmit.click();
    });

    DOMElements.btnSubmit.click(function () {
        try { parent.BlockUI(); } catch (e) { return; }

        DOMElements.btnSubmit.attr("disabled", true);
        clearMessage();
        var params = {
            userName: DOMElements.txtUsername.val()
        };

        $.ajax({
            type: 'POST',
            url: "GetClosedMatrix",
            dataType: "json",
            data: params,
            success: function (result) {
                var htmlCustinfoRow = '',
                    htmlClosedMatrixRow = '';
                for (var i = 0; i < result.CustomerInfoList.length; i++) {
                    var customerInfo = result.CustomerInfoList[i];
                    htmlCustinfoRow += '<tr><td style="text-transform:uppercase">' +
                        (i === 0 ? 'Customer' : RoleName[customerInfo.RoleId]) + '</td><td>' +
                        (i === 0 ? '<b>' + customerInfo.UserName + '</b>' : customerInfo.UserName) + '&nbsp;(' + customerInfo.CustId + ')</td></tr>';
                }

                for (var j = 0; j < 4 ; j++) {                
                    var closedMatrix = result.ClosedMatrixList.find(item => item.RoleId == (4 - j));
                    if (closedMatrix != null) {
                        var k = 0;
                        htmlClosedMatrixRow += '<tr class="trbgov">';
                        htmlClosedMatrixRow += '<td class="ViewPTHeader l">' + RoleName[closedMatrix.RoleId] + ' [' + closedMatrix.RoleId + ']</td>';
                        htmlClosedMatrixRow += '<td>' + (closedMatrix.Closed ? 'Yes' : 'No') + '</td>';
                        htmlClosedMatrixRow += RenderClosedMatrixCell(closedMatrix.AgentId, k++, j, closedMatrix.SClosed);
                        htmlClosedMatrixRow += RenderClosedMatrixCell(closedMatrix.AgentId, k++, j, closedMatrix.MClosed);
                        htmlClosedMatrixRow += RenderClosedMatrixCell(closedMatrix.AgentId, k++, j, closedMatrix.AClosed);
                        htmlClosedMatrixRow += RenderClosedMatrixCell(closedMatrix.AgentId, k++, j, closedMatrix.PClosed);
                        htmlClosedMatrixRow += '</tr>';
                    }                   
                }

                $('tbody', DOMElements.tblCustInfo).html(htmlCustinfoRow);
                $('tbody', DOMElements.tblClosedMatrix).html(htmlClosedMatrixRow);
            },
            complete: function () {
                DOMElements.btnSubmit.removeAttr("disabled");
                try { parent.jQuery.unblockUI(); } catch (e) { return; }
            }
        });
    });

    var RenderClosedMatrixCell = function (agentId, col, row, value) {
        if (col <= row) {
            if (agentId == -4 && (col == 1 || col == 2)) {
                return '<td class="disabled"></td>';
            }
            else if (agentId == -3 && col == 2) {
                return '<td class="disabled"></td>';
            }
            else {
                return '<td>' + (value ? '1' : '0') + '</td>';
            }
        }
        else {
            return '<td class="disabled"></td>';
        }
    };

    var clearMessage = function () {
        DOMElements.errMsg.empty().hide();
    };

    DOMElements.btnSubmit.click();
});