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
    TplCasinoPt: '{{each(i, currentPt) PotionTakingDetail}}\
                    <div style="padding-left: 20px;">\
                    <!--tbl-->\
                    {{if (SportName=="Casino")}}\
                    <font color="#0000FF">${SportName}&nbsp;(${SportType})&nbsp;&nbsp;&nbsp;&nbsp;[${TypeName}]</font>\
                    {{else}}\
                    <font color="#0000FF">${SportName}&nbsp;</font>\
                    {{/if}}\
                    <table class="layout_table ViewPTTable c">\
                        <tr class="ViewPTHeader b">\
                            <th width="70"><font color="#FF0000"></font></th>\
                            <th width="40">s</th>\
                            <th width="40">m</th>\
                            <th width="40">a</th>\
                            <th width="40">us</th>\
                            <th width="40">is</th>\
                            <th width="40">um</th>\
                            <th width="40">im</th>\
                            <th width="40">n</th>\
                            <th width="40">Comm</th>\
                        </tr>\
                        <!--r4-->\
                        {{if (Super != null)}}\
                            <tr class="trbgov">\
                                <td class="ViewPTHeader l">Super [4]</td>\
                                <td>${Super.Super}</td>\
                                <td>${Super.Master}</td>\
                                <td>${Super.Agent}</td>\
                                <td class="disable"></td>\
		                        <td class="disable"></td>\
		                        <!--AutoMPT-->\
		                            <td class="disable"></td>\
		                            <td class="disable"></td>\
		                        <!--/AutoMPT-->\
		                        <td>${Super.MinPt}</td>\
                                <td>${Super.Com}</td>\
                            </tr>\
                        {{/if}}\
                        <!--/r4-->\
                        <!--r3-->\
                        {{if (Master != null)}}\
                            <tr class="trbgov">\
                                <td class="ViewPTHeader l">Master [3]</td>\
                                <td>${Master.Super}</td>\
                                <td>${Master.Master}</td>\
                                <td></td>\
                                <td>${Master.SuperAuto}</td>\
                                <td><input type="checkbox" disabled="disabled" ${Master.SuperIsAuto}/></td>\
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
                            <tr class="trbgov">\
                                <td class="ViewPTHeader b l">Agent [2]</td>\
                                <td></td>\
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
                            <tr class="trbgov">\
                                <td class="ViewPTHeader l">Member [1]</td>\
                                <td>${Member.Super}</td>\
                                <td>${Member.Master}</td>\
                                <td>${Member.Agent}</td>\
                                <td>${Member.SuperAuto}</td>\
                                <td><input type="checkbox" disabled="disabled" ${Member.SuperIsAuto}/></td>\
                                <!--AutoMPT-->\
                                {{if (EnableAutoMaster)}}\
		                            <td class="${DisableAutoCss}">${Member.MasterAuto}</td>\
		                            <td class="${DisableAutoCss}"><input type="checkbox" disabled="disabled" ${Member.MasterIsAuto} /></td>\
                                {{/if}}\
		                        <!--/AutoMPT-->\
		                        <td class="disable"></td>\
		                        <td class="disable">${Member.Com}</td>\
                            </tr>\
                        {{/if}}\
                        <!--/r1-->\
                    </table>\
                    <br />\
                    <!--/tbl-->\
                    </div>\
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
    TplGoldDeluxePtInfo: '{{each(i, pt) PotionTakingDetail}}\
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
    TplMicroGamingPtInfo: '{{each(i, pt) PotionTakingDetail}}\
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
    TplPt: '<div style="padding-left: 20px;">\
            {{tmpl($data.CustomerInfo) ViewPT.TplCustInfo}}\
            <div style="padding-left: 20px;">\
            {{if ($data.PotionTakingDetail != null)}}\
                {{if ($data.IsCasino)}}\
                    {{tmpl($data) ViewPT.TplCasinoPt}}\
                {{else}}\
                    {{if ($data.IsGoldDeluxe)}}\
                        {{tmpl($data) ViewPT.TplGoldDeluxePtInfo}}\
                    {{else}}\
                        {{if ($data.IsMicroGaming)}}\
                            {{tmpl($data) ViewPT.TplMicroGamingPtInfo}}\
                        {{else}}\
                            {{if ($data.IsAGCasino)}}\
                                {{tmpl($data) ViewPT.TplAGCasinoPtInfo}}\
                            {{else}}\
                                {{tmpl($data) ViewPT.TplPtInfo}}\
                            {{/if}}\
                        {{/if}}\
                    {{/if}}\
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
                        <!--numbergame-->\
                        {{if ($data.Comm[0].GameComm != null)}}\
                            <td>Number Game </br> Comm</td>\
                        {{/if}}\
                        <!--/numbergame-->\
                         <!--myanmarodds-->\
                        {{if ($data.Comm[0].MyanmarOdds != null && $data.Comm[0].MyanmarOdds.Status)}}\
                            <td>% Odds</td>\
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
			                <!--numbergame-->\
			                {{if (GameComm != null && GameComm.Status)}}\
				                <td>${GameComm.Comm}</td>\
			                {{/if}}\
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
                    var products = '<option value="0">SB/NG</option>';
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
        var isLive = (($('#chkLive').attr("checked")) ? '1' : '0');
        var sport = ViewPT.getCurrentSportIndex(sportIndex);
        var sportId = sport == undefined ? 0 : sport.SportId;

        if (userName == "") {
            showAlertMessage("err", "Please input username!", 1);
            return;
        }
        try { parent.BlockUI(); } catch (e) { }

        switch (sportId) {
            case 44: // Sportsbook 2           
            case 50: // Cricket
            case 211: // Allbet
            case 213: // NG2            
            case 214:
            case 215:
            case 216:
            case 217:
            case 218:
            case 220:
            case 207:
                ViewPT.viewPTBasedOnKindOfCustomer(userName, sportId, betTypeId, isLive);
                break;
            case 180: // Virtual Soccer 
            case 181: // Virtual Horse Racing 
            case 182: // Virtual Greyhounds
            case 183: // Virtual Speedway
            case 184: // Virtual Motorsports
            case 185: // Virtual Cycling
            case 186: // Virtual Tennis
            case 190: // Virtual Soccer League
            case 191: // Virtual Soccer Nation
                ViewPT.viewPTBasedOnKindOfCustomer(userName, sportId, betTypeId, isLive, 5);
                break;
            case 23:
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, isLive, 23);
                break;
            case 212: // Voidbridge
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, isLive, 22);
                break;
            case 219: // Fishing World
                ViewPT.viewPTBasedOnKindOfCustomer(userName, 0, 0, isLive, 24);
                break;
            default:
                ViewPT.getViewPTOldFormat(userName, sportId, betTypeId, isLive);

        }
    },
    viewPTBasedOnKindOfCustomer: function (userName, sportId, betTypeId, isLive, productId) {
        var params = '{"userName":"' + userName + '"}';
        $.ajax({
            type: "POST",
            url: "../../../ViewPT/CheckDepositCustomer",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (isDepositCustomer) {
                if (isDepositCustomer != null) {
                    isDepositCustomer ? ViewPT.getViewPTOldFormat(userName, sportId, betTypeId, isLive) : ViewPT.getViewPTFromDynamicSettingFormat(userName, sportId, betTypeId, isLive, productId);
                }
                else {
                    showAlertMessage("err", "There is no information available.", 1);
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            },
            error: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    getViewPTFromDynamicSettingFormat: function (userName, sportId, betTypeId, isLive, productId) {
        $('#sContent').html('');
        $("#frmSportsbook2").attr('src', "../../../Matrix/ViewCustomerSettingMatrix?productId=" + productId + "&isLive=" + (isLive == 1 ? "true" : "false") + "&showFilter=false&settingTypeId=1&sportTypeId=" + sportId + "&betTypeId=" + betTypeId + "&customerName=" + $("#txtUsername").val());
        $("#frmSportsbook2").show();
        parent.jQuery.unblockUI();
    },
    getViewPTOldFormat: function (userName, sportId, betTypeId, isLive) {
        var params = '{"userName":"' + userName + '","sportId":"' + sportId + '","bettypeId":"' + betTypeId + '", "isLive": "' + isLive + '"}';
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
            error: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
        });
    },
    renderPTSetting: function (requestParams, result) {
        if (result && result.length > 0) {
            var ptHtmlResult = '';
            switch (requestParams.SportId) {
                case 252:
                    ptHtmlResult = viewPT_Bingo.renderPTSetting(requestParams, result);
                    break;
            }

            $('#sContent').append(ptHtmlResult);
        } else {
            showAlertMessage("err", "There is no information available.", 1);
        }
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
        $('#sContent').html('');
        $("#frmSportsbook2").attr('src', "../../../Matrix/ViewCustomerSettingMatrix?productId=" + $("#ddlProduct").val() + "&showFilter=false&settingTypeId=" + ($("#ddlProduct").val() == "20" ? "5" : $("#ddlProduct").val() == "26" ? "14" : "3") + "&customerName=" + $("#txtUsername").val());
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
            error: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
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

