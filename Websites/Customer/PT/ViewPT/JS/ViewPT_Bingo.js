var ViewPT_Bingo = function () {
    var self = this;

    self.Enums = {
        RoleId: {
            Super: 4,
            Master: 3,
            Agent: 2,
            Member: 1
        }
    };

    self.renderPTSetting_CustomerInfo = function (requestParams, result) {
        var ptLength = result.length;
        var ptInfo;
        var customerInfoTable = '<table class="customerInfoTable">';
        ptInfo = result[ptLength - 1];
        customerInfoTable += '<tr>\
                                    <td>CUSTOMER</td>\
                                    <td>\
                                        <span class="b">' + ptInfo.CustomerInfo.UserName + '</span> ' +
                                        '<span> (' + ptInfo.CustomerInfo.CustId + ')</span>' +
                                    '</td>\
                                </tr>';

        for (var i = ptLength - 2; i >= 0; i--) {
            ptInfo = result[i];
            customerInfoTable += '<tr>\
                                    <td class="colRoleName">' + ptInfo.CustomerInfo.RoleName + '</td>\
                                    <td>\
                                        <span>' + ptInfo.CustomerInfo.UserName + '</span> ' +
                                        '<span> (' + ptInfo.CustomerInfo.CustId + ')</span>' +
                                    '</td>\
                                </tr>';
        }

        customerInfoTable += '</table>';
        return customerInfoTable;
    };

    self.renderPTSetting_PTInfo = function (requestParams, result) {
        var ptLength = result.length;
        var ptInfo;
        var ptInfoTable = '<div style="padding-left: 20px;">\
                                <!--tbl-->\
                                    <font color="#0000FF">Bingo</font>\
                                    <table class="layout_table ViewPTTable c">\
                                        <tr class="ViewPTHeader b">\
                                            <th class="col70"><font color="#FF0000"></font></th>\
                                            <th class="col40">s</th>\
                                            <th class="col40">m</th>\
                                            <th class="col40">a</th>\
                                            <th class="col40">MinPT</th>\
                                            <th class="col40">isAutoPT</th>\
                                            <th class="col40">AutoPT</th>\
                                            <th class="col40">Comm</th>\
                                            <th class="col60">Incentive</th>\
                                        </tr>';

        for (var i = 0; i < ptLength; i++) {
            ptInfo = result[i];
            ptInfoTable += '<!--r' + ptInfo.CustomerInfo.RoleId + '-->';
            ptInfoTable += '<tr class="trbgov">\
                                <td class="ViewPTHeader l">' + ptInfo.CustomerInfo.RoleName + ' [' + ptInfo.CustomerInfo.RoleId + ']</td>\
                                <td>' + (ptInfo.CustomerInfo.RoleId != self.Enums.RoleId.Agent ?
                                        ptInfo.SuperPT :
                                        '') +
                               '</td>\
                                <td>' + ptInfo.MasterPT + '</td>\
                                <td>' + (ptInfo.CustomerInfo.RoleId != self.Enums.RoleId.Master ?
                                        ptInfo.AgentPT :
                                        '') +
                               '</td>\
                                <td>' + ptInfo.MinPt + '</td>\
                                <td><input type="checkbox" disabled="disabled" ' +
                                    (ptInfo.IsAutoPT ? 'checked="checked" ' : '') + '/></td>\
                                <td>' + ptInfo.AutoPT + '</td>\
                                <td>' + ptInfo.Commission + '</td>\
                                <td>' + ptInfo.Incentive + '</td>\
                            </tr>';
            ptInfoTable += '<!--/r' + ptInfo.CustomerInfo.RoleId + '-->';
        }

        ptInfoTable += '</table>\
                    <!--/tbl-->\
                </div>';

        return ptInfoTable;
    };

    self.renderPTSetting = function (requestParams, result) {
        var ptHtmlResult = '';
        var ptLength = result.length;
        var ptInfo;

        var customerInfoTable = self.renderPTSetting_CustomerInfo(requestParams, result);
        var ptInfoTable = self.renderPTSetting_PTInfo(requestParams, result);

        ptHtmlResult += '<div class="padding-20">\
                            <div class="padding-20">';
        ptHtmlResult += customerInfoTable;
        ptHtmlResult += ptInfoTable;
        ptHtmlResult += '</div>\
                 </div>';
        return ptHtmlResult;
    }
};

var viewPT_Bingo = new ViewPT_Bingo();