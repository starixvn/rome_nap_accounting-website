$(function () {
    var TplStatusUpdate = '<td class="w150 l">\
                                   &nbsp;<input type="checkbox" onclick="ChkEnalbe(this)" {{if !EnableUpline}}disabled="disabled"{{/if}} {{if Enable}}checked="true"{{/if}} value="${Enable}" id="${getToteId()}_chkEnable" />&nbsp;Enable ${this.parent.data.Name}\
                            </td><td class="w150 l">\
                                    {{if this.parent.parent.parent.data.CustInfo.CustLevel>1}}<input type="checkbox" id="${getToteId()}_chkEnableDownline" {{if EnableDownline}}checked="true"{{/if}} value="${EnableDownline}" disabled="disabled" />&nbsp;Enable Downlines{{/if}}\
                            </td>';
    //*******Override  Global template for Racing Setting
    TplMain = '<table class="tblcollapse w300 marginT6"><tbody>\
                <tr class="title1">\
                    <td> ${getRoleName(CustInfo.CustLevel)}: ${CustInfo.UserName}</td>\
                </tr>\
            </tbody></table>\
            {{tmpl(RacingSportType) TplRacingBox}}\
            <table cellspacing="0" cellpadding="0" border="0" class="marginT6">\
                <tr>\
                    <td class="custBtn1"></td>\
                    <td onclick="return BtnOnClick();" id="btnAdd" class="custBtn2 hand">Update</td>\
                </tr>\
            </table><br>';

    TplRacingBox = '<div class="{{if !Visible}}hidden{{/if}}"><table class="tblcollapse w300 marginT6">\
                            <tr class="title1">\
                                <td class="l" colspan="2"><span>${Name}</span></td>\
                            </tr>\
                            {{if Totes}}\
                            {{tmpl($data.Totes) TplRacingType}}\
                            {{/if}}\
                        </table>\
                    </div>';

    TplRacingType = '<tr class="{{if !Visible}}hidden{{/if}}">\
                        {{tmpl($data) RacingSetting.TplRacingTypeSetting}}\
                    </tr>';
    //************* End Global template*******************
    $.extend(RacingSetting, { TplRacingTypeSetting: TplStatusUpdate });
    RacingData.getCustomerStatus(parseInt(gup("custId")), RacingSetting.initStatus);
});


function ChkEnalbe(obj) {
    var checked = $(obj).attr("checked");
    var oldvalue = $(obj).attr("value") == "true" ? true : false;

    var downlineId = $(obj).attr("id").replace(chkEnable, chkEnableDownline);
    var oldvaluedownline = $("#" + downlineId).attr("value") == "true" ? true : false;
    if (checked != oldvalue) {
        if (checked) {
            $("#" + downlineId).removeAttr("disabled");
        }
        else {
            $("#" + downlineId).attr({ "disabled": "disabled", "checked": false });
        }
    }
    else {
        $("#" + downlineId).attr({ "disabled": "disabled", "checked": oldvaluedownline });
    }
}


function BtnOnClick() {
    $(".errmsg").empty();
    RacingSetting.updateStatus();
}
