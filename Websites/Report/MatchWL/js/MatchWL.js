
var tmplMatchWLContent = '{{if typeTotal == 0}}\
                            <tr align="right">\
                                <td style="width:120px" align="center">\
                                    {{if type3rd == 0}}\
                                        {{if liveindicator}} <b>!Live -</b> {{/if}} ${bettype} <br> ${matchcode}\
                                    {{else}}\
                                        {{if type3rd == 2}}\
                                            {{if bettype == "Casino"}}\
                                                <a href="../CasinoDetail_1.aspx?wldate=${eventdate}&bettype=${bettype}" target="_blank">${bettype}</a>\
                                            {{else}}\
                                                {{if bettype == "P2P"}}
                                                    <a href="../Report/Poker/PokerWL.aspx?fdate=${eventdate}&tdate=${eventdate}" target="_blank">${bettype}</a>\
                                                {{else}}
                                                    {{if bettype == "Tote HR"}}\
                                                        <a href="../ToteHRWLDetails.aspx?wldate=${eventdate}&bettype=${bettype}" target="_blank">${bettype}</a>\
                                                    {{else}}\
                                                        ${bettype}\
                                                    {{/if}}\
                                                {{/if}}\
                                            {{/if}}\
                                        {{else}}\
                                            ${bettype}\
                                        {{/if}}\
                                    {{/if}}\
                                </td>\
                                <td align="center" style="width:80px">${eventdate} <br/> ${kickofftime}</td>\
                                {{if type3rd == 0 }}\
                                    <td align="right" style="width:140px">${homename}</td>\
                                    <td align="center" style="width:20px">-vs-</td>\
                                    <td align="left" style="width:140px">${awayname}</td>\
                                {{else}}\
                                    <td colspan="3" style="width:306px;"></td>\
                                {{/if}}\
                                <td class="w90">${winlost}</td>\
                                <td class="w90">${pos}</td>\
                                <td class="w90">${comm}</td>\
                                <td class="w90">{{if bettype == "Casino" || bettype == "P2P" || bettype == "Racing"}} N/A {{else}} ${VirtualComm} {{/if}}</td>\
                                <td class="w90"><b>${LineTotal}</b></td>\
                                <td class="w90">{{if bettype == "Casino" || bettype == "P2P" || bettype == "Racing"}} N/A {{else}} <b>${VirtualLineTotal}</b> {{/if}}</td>\
                                <td class="w90">{{if bettype == "Casino" || bettype == "P2P" || bettype == "Racing"}} N/A {{else}} ${variance} {{/if}}</td>\
                            </tr>\
                        {{else}}\
                            <tr class="total${typeTotal}" align="right">\
                                <td  align="right" colspan="7" style="width:692px;"><b> {{if typeTotal>1}} ${leaguenametc} {{/if}} TOTAL:</b></td>\
                                <td  class="w90">${comm}</td>\
                                <td  class="w90">${VirtualComm}</td>\
                                <td  class="w90"><b>${LineTotal}</b></td>\
                                <td  class="w90"><b>${VirtualLineTotal}</b></td>\
                                <td  class="w90">${variance}</td>\
                           </tr>\
                        {{/if}}';


var tmplWL8RM = '<tr class="bg">\
                            <td colspan ="9" align="right" style="width:874px;"><b>Total 8RM:</b></td>\
                            <td align="right" class="w90"><b>${wl8rm}</b></td>\
                            <td align="right" class="w90"><b>${wl8rm}</b></td>\
                            <td align="right" class="w90">0.00</td>\
                        </tr>';

var tmplWLbyBettype = '<tr class="r {{if IsTotal}} total {{/if}}">\
                                    <td class="c w120">${bettype}</td>\
                                    <td class="w80">${LineTotal_soccer_live}</td>\
                                    <td class="w80">${VirtualLineTotal_soccer_live}</td>\
                                    <td class="w80">${LineTotal_soccer}</td>\
                                    <td class="w80">${VirtualLineTotal_soccer}</td>\
                                    <td class="w80">${LineTotal_others_live}</td>\
                                    <td class="w80">${VirtualLineTotal_others_live}</td>\
                                    <td class="w80">${LineTotal_others}</td>\
                                    <td class="w80">${VirtualLineTotal_others}</td>\
                                    <td class="w80">${LineTotalReal}</td>\
                                    <td class="w80">${LineTotalVirtual}</td>\
                                    <td class="w80">${Variance}</td>\
                                </tr>';