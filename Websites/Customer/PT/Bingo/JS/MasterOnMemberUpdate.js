
$(document).ready(function() {
    getCustomerSetting();
});
var Customer;
//-----------Load data-----------------//
function getCustomerSetting(isReload) {
   
    var custid = gup("custId");
    if (custid!=null)
    {
        PTBlockUI();
        var param = '{"custID":' + custid + ',"updatedByRoleID":3}';
        $.ajax({
            type: "POST",
            url: "Services/BingoService.svc/GetCustomerSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                 LoadData(rs);
                 PTUnBlockUI(); 
            }
        });
    }
   
}

function LoadData(rs)
{
    if (rs != null && rs.GameKindList != null) {
         if (rs.Result.ErrorCode>0)
        {
            $("#err").html('<div class="alertboxerr"><div>' + rs.Result.ErrorMessage + '</div></div>');
            $(".bg_table").addClass("hidden");
        }
        else
        {
            $(".bg_table").removeClass("hidden");
            var SettingContent = $("#SettingContent");
            SettingContent.setTemplateURL("../../../App_Templates/Default/Customer/PT/Bingo/MasterOnMemberUpdateBingoPT.htm");
            SettingContent.setParam("IsGroupSetting",rs.IsUsedGameKind);
            SettingContent.processTemplate(rs);
            AssignCommission();
            AssignPT();
            Customer=rs;
        }
    }
    else {
        $("#err").html('<div class="alertboxerr"><div>There is no information available.</div></div>');
        $(".bg_table").addClass("hidden");
    }
    
}

//----------End load data----------------//

//----------Event------------------------//
function updatePT() {
    if (Customer!=null)
    {
        PTBlockUI();
        var totalGameKind=Customer.GameKindList.length;
        var totalGameType;
        var totalBettingRange;
        //Loop to get PT setting GameKind
        for (var i=0;i<totalGameKind;i++)
        {
            var gamekind=Customer.GameKindList[i];
            var gamekindid=gamekind.ID;
            totalGameType=gamekind.GameTypeList.length;
            Customer.GameKindList[i].PositionTaking = roundNumber($("#" + strListMaster + "_" + gamekindid).val(), 2);
         
            //Loop to get PT setting GameType
            for (var j=0;j<totalGameType;j++)
            {
                var gametype=Customer.GameKindList[i].GameTypeList[j];
                var gametypeid=gametype.ID;
                totalBettingRange=gametype.BettingRangeList.length;
                if (!Customer.IsUsedGameKind)
                {
                    Customer.GameKindList[i].GameTypeList[j].PositionTaking = roundNumber($("#" + strListMaster + "_" + gamekindid + "_" + gametypeid).val(), 2);
                }
                //Loop to get BettingRangeList
                for (var z=0;z<totalBettingRange;z++)
                {
                    var bettingrange=Customer.GameKindList[i].GameTypeList[j].BettingRangeList[z];
                    var bettingrangeid=bettingrange.ID;
                    Customer.GameKindList[i].GameTypeList[j].BettingRangeList[z].IsChecked=$("#" + strChkStake + "_" + gamekindid + "_" + gametypeid + "_" + bettingrangeid).attr("checked");
                }
            }
        }
        
        var rstext=JSON.stringify(Customer);
        var data = '{"customer":' + rstext + ',"updatedByRoleID":3' + '}';
          $.ajax({
                type: "POST",
                url: "Services/BingoService.svc/UpdateCustomerSetting",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data:  data,
                success: function (rs) {
                   if (rs.ErrorCode==0)
                    {
                        showAlertMessage("err","Updated Successfully",0);
                        afterUpdate(true,2000);
                        
                    }
                    else if  (rs.ErrorCode==500)
                    {
                        showAlertMessage("err", rs.ErrorMessage,1);
                        afterUpdate(false,2000);
                    }
                    else
                    {
                         showAlertMessage("err", rs.ErrorMessage,1);
                    }
                    PTUnBlockUI();
                }
            });
    }
}

function ReducePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");
    if (!obj.attr("disabled")) {
        if (obj.val() > min) {
            var newvalue = roundNumber((curr * 100 - step) / 100, 2);
            var max = obj.attr("max");
            if (newvalue < min) newvalue = curr;

            if ($("#" + id + " option").size() <= 1) {
                CreateOption(obj, newvalue, max, min, 0.01, 2);
            }
            else {
                $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
            }
            
            if (id.indexOf(strListMaster)>=0)
            {
                on_ChangeMasterPT(document.getElementById(id));
            }
        }
        
    }
   
}

function IncreasePT(id, step) {
    var obj = $("#" + id);
    if (!obj.attr("disabled")) {
        var curr = obj.val();
        var min = obj.attr("min");
        var max = obj.attr("max");
        if (obj.val() < max) {
            var newvalue = roundNumber((curr * 100 + step) / 100, 2);
            if (newvalue > max) newvalue = curr;
            if ($("#" + id + " option").size() <= 1) {
                CreateOption(obj, newvalue, max, min, 0.01, 2);
            }
            else {
                $("#" + id + " option[value='" + newvalue + "']").attr("selected", "selected");
            }
            if (id.indexOf(strListMaster)>=0)
            {
                on_ChangeMasterPT(document.getElementById(id));
            }
        }
    }
}

function on_ChangeMasterPT(obj)
{
    var oMaster=$(obj);
    var strMasterID=oMaster.attr("id");
    var MasterValue=oMaster.val();
     jQuery.each($("." + strMasterID.replace(strListMaster,"MasterPT")), function() {
            if ($(this).attr("id").indexOf(strMasterID)>=0)
            {
                $(this).val(MasterValue);
            }
        });
}
//----------End Event--------------------//

