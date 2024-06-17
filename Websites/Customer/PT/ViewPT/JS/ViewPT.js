$.extend({
    URLEncode: function (c) {
        var o = ''; var x = 0; c = c.toString(); var r = /(^[a-zA-Z0-9_.]*)/;
        while (x < c.length) {
            var m = r.exec(c.substr(x));
            if (m != null && m.length > 1 && m[1] != '') {
                o += m[1]; x += m[1].length;
            } else {
                if (c[x] == ' ') o += '+'; else {
                    var d = c.charCodeAt(x); var h = d.toString(16);
                    o += '%' + (h.length < 2 ? '0' : '') + h.toUpperCase();
                } x++;
            }
        } return o;
    },
    URLDecode: function (s) {
        var o = s; var binVal, t; var r = /(%[^%]{2})/;
        while ((m = r.exec(o)) != null && m.length > 1 && m[1] != '') {
            b = parseInt(m[1].substr(1), 16);
            t = String.fromCharCode(b); o = o.replace(m[1], t);
        } return o;
    }
});

$(document).ready(function () {
    $('#rdoPT').attr('checked', true);
    $("#txtUsername").val($.URLDecode(gup("username")));
    Custom.init();
    ViewPT.getSporttype();
    $('#btnSubmit').attr('disabled', true);
});

var ViewPT = {
    data: null,
    TplCustInfo: '<table>\
                    <tr>\
                        <td>CUSTOMER</td>\
                        <td><b>${Customer.UserName}</b>&nbsp;(${Customer.CustId})</td>\
                    </tr>\
                    {{if (Upline != null)}}\
                        {{each(i, upline) Upline}}\
                            <!--upline-->\
                            <tr>\
                                <td style="text-transform:uppercase">${RoleName}</td>\
                                <td>${UserName}&nbsp;(${CustId})</td>\
                            </tr>\
                            <!--/upline-->\
                        {{/each}}\
                    {{/if}}\
                </table>',
    TplPtInfo: '{{each(i, pt) PotionTakingDetail}}\
                    <div style="color:#0000FF; padding: 6px 0px;">${SportName}&nbsp;(${TypeName})&nbsp;&nbsp;&nbsp;&nbsp;[${SportType}-${Bettype}]</div>\
                    <table class="layout_table ViewPTTable c">\
                        <tr class="ViewPTHeader b">\
                            <th width="70"><font color="#FF0000">${LiveGame}</font></th>\
                            <th width="40">s</th>\
                            <th width="40">m</th>\
                            <th width="40">a</th>\
                            <th width="40">us</th>\
                            <th width="40">is</th>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <th width="40">um</th>\
                                <th width="40">im</th>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <th width="40">n</th>\
                            <th width="40">l</th>\
                        </tr>\
                        <!--r4-->\
                        {{if (Super != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Super [4]</td>\
                            <td>${Super.Super}</td>\
                            <td>${Super.Master}</td>\
                            <td>${Super.Agent}</td>\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="disable"></td>\
                                <td class="disable"></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Super.MinPt}</td>\
                            <td>${Super.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r4-->\
                        <!--r3-->\
                        {{if (Master != null)}}\
                        <tr  class="trbgov" >\
                            <td class="ViewPTHeader l">Master [3]</td>\
                            <td>${Master.Super}</td>\
                            <td>${Master.Master}</td>\
                            <td class="disable"></td>\
                            <td>${Master.SuperAuto}</td>\
                            <td><input type="checkbox" disabled="disabled" ${Master.SuperIsAuto} /></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="${DisableFlagCss}"></td>\
                                <td class="${DisableFlagCss}"></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Master.MinPt}</td>\
                            <td>${Master.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r3-->\
                        <!--r2-->\
                        {{if (Agent != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Agent [2]</td>\
                            <td class="disable"></td>\
                            <td>${Agent.Master}</td>\
                            <td>${Agent.Agent}</td>\
                            <!--DisableAutoMPT-->\
                            {{if (!EnableAutoMaster)}}\
                                <td>${Agent.SuperAuto}</td>\
                                <td><input type="checkbox" disabled="disabled" ${Agent.SuperIsAuto}/></td>\
                            <!--/DisableAutoMPT-->\
                            <!--AutoMPT-->\
                            {{else}}\
                                <td class="disable"></td>\
                                <td class="disable"></td>\
                                <td class="${DisableAutoCss}">${Agent.MasterAuto}</td>\
                                <td class="${DisableAutoCss}"><input type="checkbox" disabled="disabled" ${Agent.MasterIsAuto} /></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Agent.MinPt}</td>\
                            <td>${Agent.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r2-->\
                        <!--r1-->\
                        {{if (Member != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Member [1]</td>\
                            <td>${Member.Super}</td>\
                            <td>${Member.Master}</td>\
                            <td>${Member.Agent}</td>\
                            <td>${Member.SuperAuto}</td>\
                            <td><input type="checkbox" disabled="disabled" ${Member.SuperIsAuto} /></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="${DisableAutoCss}">${Member.MasterAuto}</td>\
                                <td class="${DisableAutoCss}"><input type="checkbox" disabled="disabled" ${Member.MasterIsAuto} /></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                        </tr>\
                        {{/if}}\
                        <!--/r1-->\
                    </table>\
                    <br />\
                    <!--/tbl-->\
                {{/each}}',
    TplNoAutoPtInfo: '{{each(i, pt) PotionTakingDetail}}\
                    <div style="color:#0000FF; padding: 6px 0px;">${SportName}&nbsp;(${TypeName})&nbsp;&nbsp;&nbsp;&nbsp;[${SportType}-${Bettype}]</div>\
                    <table class="layout_table ViewPTTable c">\
                        <tr class="ViewPTHeader b">\
                            <th width="70"><font color="#FF0000">${LiveGame}</font></th>\
                            <th width="40">s</th>\
                            <th width="40">m</th>\
                            <th width="40">a</th>\
                            <th width="40">n</th>\
                            <th width="40">l</th>\
                        </tr>\
                        <!--r4-->\
                        {{if (Super != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Super [4]</td>\
                            <td>${Super.Super}</td>\
                            <td>${Super.Master}</td>\
                            <td>${Super.Agent}</td>\
                            <td>${Super.MinPt}</td>\
                            <td>${Super.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r4-->\
                        <!--r3-->\
                        {{if (Master != null)}}\
                        <tr  class="trbgov" >\
                            <td class="ViewPTHeader l">Master [3]</td>\
                            <td>${Master.Super}</td>\
                            <td>${Master.Master}</td>\
                            <td class="disable"></td>\
                            <td>${Master.MinPt}</td>\
                            <td>${Master.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r3-->\
                        <!--r2-->\
                        {{if (Agent != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Agent [2]</td>\
                            <td class="disable"></td>\
                            <td>${Agent.Master}</td>\
                            <td>${Agent.Agent}</td>\
                            <td>${Agent.MinPt}</td>\
                            <td>${Agent.MaxPt}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r2-->\
                        <!--r1-->\
                        {{if (Member != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Member [1]</td>\
                            <td>${Member.Super}</td>\
                            <td>${Member.Master}</td>\
                            <td>${Member.Agent}</td>\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                        </tr>\
                        {{/if}}\
                        <!--/r1-->\
                    </table>\
                    <br />\
                    <!--/tbl-->\
                {{/each}}',
    TplAGCasinoPtInfo: '{{each(i, pt) PotionTakingDetail}}\
                    <div style="color:#0000FF; padding: 6px 0px;">${SportName}&nbsp;</div>\
                    <table class="layout_table ViewPTTable c">\
                        <tr class="ViewPTHeader b">\
                            <th width="70"><font color="#FF0000">${LiveGame}</font></th>\
                            <th width="40">s</th>\
                            <th width="40">m</th>\
                            <th width="40">a</th>\
                            <th width="40">us</th>\
                            <th width="40">is</th>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <th width="40">um</th>\
                                <th width="40">im</th>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <th width="40">n</th>\
                            <th width="40">comm</th>\
                        </tr>\
                        <!--r4-->\
                        {{if (Super != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Super [4]</td>\
                            <td>${Super.Super}</td>\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                            <td class="disable"></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="disable"></td>\
                                <td class="disable"></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Super.MinPt}</td>\
                            <td>${Super.Com}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r4-->\
                        <!--r3-->\
                        {{if (Master != null)}}\
                        <tr  class="trbgov" >\
                            <td class="ViewPTHeader l">Master [3]</td>\
                            <td>${Master.Super}</td>\
                            <td>${Master.Master}</td>\
                            <td class="disable"></td>\
                            <td>${Master.SuperAuto}</td>\
                            <td><input type="checkbox" disabled="disabled" ${Master.SuperIsAuto} /></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="${DisableFlagCss}"></td>\
                                <td class="${DisableFlagCss}"></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Master.MinPt}</td>\
                            <td>${Master.Com}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r3-->\
                        <!--r2-->\
                        {{if (Agent != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Agent [2]</td>\
                            <td class="disable"></td>\
                            <td>${Agent.Master}</td>\
                            <td>${Agent.Agent}</td>\
                            <!--DisableAutoMPT-->\
                            {{if (!EnableAutoMaster)}}\
                                <td>${Agent.SuperAuto}</td>\
                                <td><input type="checkbox" disabled="disabled" ${Agent.SuperIsAuto}/></td>\
                            <!--/DisableAutoMPT-->\
                            <!--AutoMPT-->\
                            {{else}}\
                                <td class="disable"></td>\
                                <td class="disable"></td>\
                                <td class="${DisableAutoCss}">${Agent.MasterAuto}</td>\
                                <td class="${DisableAutoCss}"><input type="checkbox" disabled="disabled" ${Agent.MasterIsAuto} /></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td>${Agent.MinPt}</td>\
                            <td>${Agent.Com}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r2-->\
                        <!--r1-->\
                        {{if (Member != null)}}\
                        <tr class="trbgov" >\
                            <td class="ViewPTHeader l">Member [1]</td>\
                            <td>${Member.Super}</td>\
                            <td>${Member.Master}</td>\
                            <td>${Member.Agent}</td>\
                            <td>${Member.SuperAuto}</td>\
                            <td><input type="checkbox" disabled="disabled" ${Member.SuperIsAuto} /></td>\
                            <!--AutoMPT-->\
                            {{if (EnableAutoMaster)}}\
                                <td class="${DisableAutoCss}">${Member.MasterAuto}</td>\
                                <td class="${DisableAutoCss}"><input type="checkbox" disabled="disabled" ${Member.MasterIsAuto} /></td>\
                            {{/if}}\
                            <!--/AutoMPT-->\
                            <td class="disable"></td>\
                            <td>${Member.Com}</td>\
                        </tr>\
                        {{/if}}\
                        <!--/r1-->\
                    </table>\
                    <br />\
                    <!--/tbl-->\
                {{/each}}',
    TplPt: '<div style="padding-left: 20px;">\
            {{tmpl($data.CustomerInfo) ViewPT.TplCustInfo}}\
            <div style="padding-left: 20px;">\
            {{if ($data.PotionTakingDetail != null)}}\
                {{if ($data.IsAGCasino)}}\
                        {{tmpl($data) ViewPT.TplAGCasinoPtInfo}}\
                {{else}}\
                    {{tmpl($data) ViewPT.TplPtInfo}}\
                {{/if}}\
            {{/if}}\
            </div>\
        </div>',
    TplCom: '<div style="padding-left: 20px;">\
            {{tmpl($data.CustomerInfo) ViewPT.TplCustInfo}}\
            {{if ($data.Comm != null && $data.Comm.length > 0)}}\
                <table class="layout_table ViewPTTable c" style="width: 968px;">\
                    <tr class="ViewPTHeader b">\
                        <td width="60">&nbsp;</td>\
                        <td >Group A</td>\
                        {{if ($data.Comm[0].MyanmarOdds != null && !$data.Comm[0].MyanmarOdds.Status)}}\
                            <td>Group B</td>\
                            <td>Group C</td>\
                            <td>Group D</td>\
                        {{/if}}\
                        <td>Other</td>\
                        <!--1X2-->\
                            <td>1x2</td>\
                        <!--/1X2-->\
                        <!--myanmarodds-->\
                        {{if ($data.Comm[0].MyanmarOdds != null && $data.Comm[0].MyanmarOdds.Status)}}\
                            <td>Myanmar Odds</td>\
                        {{/if}}\
                        <!--/myanmarodds-->\
                    </tr>\
                    <!--row-->\
                    {{each(i, current) $data.Comm}}\
                        <tr class="trbgov">\
                            <td class="tdtitle b">${RoleName}</td>\
                            <td>${GroupA.Comm}</td>\
				            {{if (MyanmarOdds != null && !MyanmarOdds.Status)}}\
                                <td>${GroupB.Comm}</td>\
                                <td>${GroupC.Comm}</td>\
                                <td>${GroupD.Comm}</td>\
                            {{/if}}\
                            <td>${Other.Comm}</td>\
                            <!--1X2-->\
                            {{if (OneXTwo != null && OneXTwo.Status)}}\
                                <td>${OneXTwo.Comm}</td>\
                            {{/if}}\
                            <!--/1X2-->\
                            <!--/numbergame-->\
                            <!--myanmarodds-->\
                            {{if (MyanmarOdds != null && MyanmarOdds.Status)}}\
                                <td>${MyanmarOdds.Comm}</td>\
                            {{/if}}\
                            <!--/myanmarodds-->\
                        </tr>\
                    {{/each}}\
                    <!--/row-->\
                </table>\
            {{/if}}\
            </div>',
    getSporttype: function () {
        var p = {
            userName: $('#txtUsername').val()
        };
        $.ajax({
            type: "POST",
            url: "../../../Services/WCF/PT/ViewPT/ViewPTService.svc/GetPtSportType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(p),
            success: function (rs) {
                if (rs != null) {
                    ViewPT.data = rs.SportsList;
                    ViewPT.bindSportType(rs);
                    if ($('#txtUsername').val() != '')
                        ViewPT.getPt();
                    $("#btnSubmit").removeAttr("disabled", false);
                }
            }
        });
    },
    getProducts: function () {
        $.ajax({
            type: "POST",
            url: "../../../Services/WCF/PT/ViewPT/ViewPTService.svc/GetProducts",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rs) {
                if (rs != null) {
                    var products = '';
                    $.each(rs, function (i, product) {
                        products += '<option value="' + product.ProductId + '">' + product.ProductName + '</option>';
                    });
                    $("#ddlProduct").append($(products));
                }
            }
        });
    },
    getBettype: function (ind) {
        var sport = ViewPT.getCurrentSportIndex(ind);
        var betTypes = sport.Bettype ? sport.Bettype.BettypeList : null;

        var bettypeSelect = $('#ddlBettype');
        var options = bettypeSelect.attr('options');
        $('option', bettypeSelect).remove();

        if (betTypes != null) {
            $.each(betTypes, function (ind, val) {
                options[options.length] = new Option(val.BettypeName, val.BettypeId);
            });
        }

        if (parseInt(sport.SportId) != 251 && parseInt(sport.SportId) != 252)
            options[options.length] = new Option("..:: ALL ::..", 0);
    },
    bindSportType: function (rs) {
        var sports = '';
        $.each(rs.SportsList, function (i, n) {
            if (n.Children != null) {
                sports += '<optgroup label="' + n.SportName + '">';
                $.each(n.Children, function (iC, nC) {
                    sports += '<option value="' + (i + '_' + iC) + '">' + nC.SportName + '</option>';
                });
                sports += '</optgroup>';
            }
            else {
                sports += '<option value="' + i + '">' + n.SportName + '</option>';
            }
        });

        $("#ddlSporttype").append($(sports)).bind("change", function () {
            ViewPT.getBettype($(this).val());
        });

        this.getBettype('0');
    },
    getPt: function () {
        $("#err").html("");
        $("#sContent").html("");

        var sportIndex = $("#ddlSporttype").val();
        var betTypeId = $("#ddlBettype").val();
        var userName = $("#txtUsername").val();
        var sport = ViewPT.getCurrentSportIndex(sportIndex);
        var sportId = sport == undefined ? 0 : sport.SportId;

        if (userName == "") {
            showAlertMessage("err", "Please input username!", 1);
            return;
        }
        try { parent.BlockUI(); } catch (e) { }

        switch (sportId) {
            case 43: // esport
            case 44: // Sportsbook 2
            case 50: // Cricket
            case 161: // Number Game
            case 167: // CG
            case 169: // Sports Lottery
            case 170: // PP Live Casino
            case 202: // RNG Keno
            case 211: // Allbet
            case 241: // BBIN
            case 242: // GT Casino
            case 243: // Sexy Gaming
            case 244: // SA Gaming
            case 214:
            case 215:
            case 216:
            case 217:
            case 218:
            case 220:
            case 207:
            case 222: // Table Game
            case 224: // Funky Games
            case 225: // ION
            case 233: // WM
            case 237: // MaxGame
            case 239: // BigGame
            case 238: // Habanero
            case 172: // Advantplay
            case 174: // Advantplay Mini
            case 159: // YeeBet
            case 9911: // IBC Live Casino
            case 9906: // RNG Casino
            case 9918: // Other RNG Casino
            case 9909: //RNG Games
                ViewPT.viewPTBasedOnKindOfCustomer(userName, sportId, betTypeId);
                break;
            case 9908: // Virtual Sports
                ViewPT.viewPTBasedOnKindOfCustomer(userName, sportId, betTypeId, 5);
                break;

            case 23:
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 23);
                break;
            case 9906: // RNG Slot
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 18);
                break;
            case 245://Virtual Games
            case 176: // FGG
                ViewPT.viewPTBasedOnKindOfCustomer(userName, sportId, betTypeId, 64);
                break;
            case 200://Saba.games
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 42);
                break;

            case 171: //VGaming
            case 173: //VGaming
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 66);
                break;
            case 175: //Bitcoin Betting
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 69);
                break;
            case 208: // Saba Casino
            case 209: // Saba Casino
            case 210: // Saba Casino
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 6);
                break;
            case 9913:
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, 1);
                break;
            default:
                ViewPT.getViewPTOldFormat(userName, sportId, betTypeId);
        }
    },
    viewPTBasedOnKindOfCustomer: function (userName, sportId, betTypeId, productId) {
        var params = '{"userName":"' + userName + '"}';
        $.ajax({
            type: "POST",
            url: "../../../ViewPT/CheckDepositCustomer",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (isDepositCustomer) {
                if (isDepositCustomer != null) {
                    isDepositCustomer ? ViewPT.getViewPTOldFormat(userName, sportId, betTypeId) : ViewPT.getViewPTFromDynamicSettingFormat(userName, sportId, betTypeId, productId);
                }
                else {
                    showAlertMessage("err", "There is no information available.", 1);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    getViewPTFromDynamicSettingFormat: function (userName, sportId, betTypeId, productId) {
        $('#sContent').html('');
        $("#frmSportsbook2").attr('src', "../../../Matrix/ViewCustomerSettingMatrix?productId=" + productId + "&showFilter=false&settingTypeId=1&sportTypeId=" + sportId + "&betTypeId=" + betTypeId + "&customerName=" + $("#txtUsername").val());
        $("#frmSportsbook2").show();
        parent.jQuery.unblockUI();
    },
    getViewPTOldFormat: function (userName, sportId, betTypeId) {
        var params = '{"userName":"' + userName + '","sportId":"' + sportId + '","bettypeId":"' + betTypeId + '"}';
        $.ajax({
            type: "POST",
            url: "../../../Services/WCF/PT/ViewPT/ViewPTService.svc/ViewPositionTaking",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (resp) {
                $("#frmSportsbook2").attr('src', "");
                $("#frmSportsbook2").hide();
                if (resp != null) {
                    if (resp.ErrorMessage != null && resp.ErrorMessage.length > 0) {
                        showAlertMessage("err", resp.ErrorMessage, 1);
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        return;
                    }
                    $.tmpl(ViewPT.TplPt, resp).appendTo('#sContent');
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }
                else {
                    showAlertMessage("err", "There is no information available.", 1);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function (xhr) {
                try {
                    if (xhr.status === 403) {
                        var denyObject = $.parseJSON(xhr.responseText);
                        showAlertMessage("err", denyObject.ErrorMessage, 1);
                    }
                    parent.jQuery.unblockUI();
                } catch (e) { }
            }
        });
    },

    getCurrentSportIndex: function (indexValue) {
        var indexArr = indexValue.split('_');
        var sport = null;
        if (indexArr.length > 1) {
            sport = this.data[indexArr[0]].Children[indexArr[1]];
        } else {
            sport = this.data[indexValue];
        }
        return sport;
    },
    viewCommBasedOnKindOfSetting: function () {
        if ($("#ddlProduct").val() == '0') {
            ViewPT.viewComm()
        }
        else {
            ViewPT.viewCommFromDynamicSettingFormat();
        }
    },
    viewCommFromDynamicSettingFormat: function () {
        $('#err').html('');
        $('#sContent').html('');
        $("#frmSportsbook2").attr('src', "../../../Matrix/ViewCustomerSettingMatrix?productId=" + $("#ddlProduct").val() + "&showFilter=false&settingTypeId=3&customerName=" + $("#txtUsername").val());
        $("#frmSportsbook2").show();
        parent.jQuery.unblockUI();
    },
    viewComm: function () {
        $("#err").html("");
        $("#sContent").html("");

        var userName = $("#txtUsername").val();
        var params = '{"userName":"' + userName + '"}';
        if (userName == "") {
            showAlertMessage("err", "Please input username!", 1);
            return;
        }

        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "../../../Services/WCF/PT/ViewPT/ViewPTService.svc/ViewCommission",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (resp) {
                $("#frmSportsbook2").attr('src', "");
                $("#frmSportsbook2").hide();
                if (resp != null) {
                    if (resp.ErrorMessage != null && resp.ErrorMessage.length > 0) {
                        showAlertMessage("err", resp.ErrorMessage, 1);
                        try { parent.jQuery.unblockUI(); } catch (e) { }
                        return;
                    }
                    $.tmpl(ViewPT.TplCom, resp).appendTo('#sContent');
                }
                else {
                    showAlertMessage("err", "There is no information available.", 1);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function (xhr) {
                try {
                    if (xhr.status === 403) {
                        var denyObject = $.parseJSON(xhr.responseText);
                        showAlertMessage("err", denyObject.ErrorMessage, 1);
                    }
                    parent.jQuery.unblockUI();
                } catch (e) { }
            }
        });
    },
    viewData: function () {
        var rdPtChecked = $('#rdoPT') == undefined ? 1 : ($('#rdoPT').attr('checked'));

        if (rdPtChecked) {
            ViewPT.getPt();
        }
        else {
            ViewPT.viewCommBasedOnKindOfSetting();
        }
    }
};

function KP(e) {
    e = (e) ? e : window.event;
    var key = (e.keyCode) ? e.keyCode : e.which;
    if (key == 13) ViewPT.viewData();
    return;
}

function gup(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.href);
    if (results == null)
        return "";
    else
        return results[1];
}

function ChangeType(rdo) {
    if (rdo.id == 'rdoComm') {
        var numberOfOptions = $('#ddlProduct option').length;
        if (numberOfOptions == 0) {
            ViewPT.getProducts();
        }
    }

    if (rdo.id == 'rdoPT' && $('#tblPT').css('display', '') && $('#tblProducts').css('display', 'none')) return;
    if (rdo.id == 'rdoComm' && $('#tblPT').css('display', 'none') && $('#tblProducts').css('display', '')) return;

    $('#tblPT').css('display', $('#rdoPT').css('display') = 'none' ? '' : 'none');
    $('#tblProducts').css('display', $('#rdoPT').css('display') = 'none' ? 'none' : '');

    $('#sContent').html('');
}