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
        var param = '{"custID":' + custid + ',"updatedByRoleID":1}';
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

    if (rs != null) {
         if (rs.Result.ErrorCode>0)
        {
            $("#err").html('<div class="alertboxerr"><div>' + rs.Result.ErrorMessage + '</div></div>');
            $(".bg_table").addClass("hidden");
        }
        else
        {
            $(".bg_table").removeClass("hidden");
            var SettingContent = $("#SettingContent");
            SettingContent.setTemplateURL("../../../App_Templates/Default/Customer/PT/Bingo/MemberUpdateBingoPT.htm");
            SettingContent.setParam("IsGroupSetting",rs.IsUsedGameKind);
            SettingContent.processTemplate(rs);
            AssignCommission();
            if (rs.SyncBingo)
            {
                 $("#tblUpdate").click(function(){
                    updatePT();
                });
            }
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
function IsSyncData(obj)
{
    if (obj.checked)
    {
        $("#tblUpdate").css("opacity","1");
        $("#tblUpdate").click(function(){
            updatePT();
        });
        
    }
    else
    {
        $("#tblUpdate").css("opacity","0.5");
        $('#tblUpdate').unbind('click');

    }
}

function updatePT() {
     if (Customer!=null)
    {
         var maxTransferCredit=$("#txtMaxTransferCredit").val()
          if (!uInteger(maxTransferCredit))
        {
            showAlertMessage("err", "Invalid Max Transder Credit",1);
            return ;
        }
       if  (Customer.MaxLimit>0)
        {
            if (Customer.MinLimit>maxTransferCredit || maxTransferCredit>Customer.MaxLimit)
            {
                showAlertMessage("err", "Invalid Max Transder Credit",1);
                return ;
            }
        }
        else
        {
            if (Customer.MinLimit>maxTransferCredit)
            {
                showAlertMessage("err", "Invalid Max Transder Credit",1);
                return ;
            }
        }
        PTBlockUI();
        var totalGameKind=Customer.GameKindList.length;
        var totalGameType;
        var totalBettingRange;
        //Loop to get PT setting GameKind
        Customer.MaxTransferCredit=maxTransferCredit;
        Customer.IsAutoReset=$("#cbxAutoReset").attr("checked");
        for (var i=0;i<totalGameKind;i++)
        {
            var gamekind=Customer.GameKindList[i];
            var gamekindid=gamekind.ID;
            totalGameType=gamekind.GameTypeList.length;
            Customer.GameKindList[i].PositionTaking=roundNumber($("#" + strListPreset + "_" + gamekindid).val(),2);
            Customer.GameKindList[i].Commission=roundNumber($("#" + strListComm + "_" + gamekindid).val(),4);
            //Loop to get PT setting GameType
            for (var j=0;j<totalGameType;j++)
            {
                var gametype=Customer.GameKindList[i].GameTypeList[j];
                var gametypeid=gametype.ID;
                totalBettingRange=gametype.BettingRangeList.length;
                if (!Customer.IsUsedGameKind)
                {
                    Customer.GameKindList[i].GameTypeList[j].PositionTaking=roundNumber($("#" + strListPreset + "_" + gamekindid + "_" + gametypeid).val(),2);
                    Customer.GameKindList[i].GameTypeList[j].Commission=roundNumber($("#" + strListComm + "_" + gamekindid + "_" + gametypeid).val(),4);
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
        var data = '{"customer":' + rstext + ',"updatedByRoleID":1' + '}';
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
            if (id.indexOf(strListPreset)>=0)
            {
                on_ChangePreset(document.getElementById(id));
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
            
            if (id.indexOf(strListPreset)>=0)
            {
                on_ChangePreset(document.getElementById(id));
            }
        }
    }
}
function on_ChangePreset(obj)
{
    var oPreset=$(obj);
    var strPresetID=oPreset.attr("id");
    var PresetValue=oPreset.val();
    jQuery.each($("." + strPresetID.replace(strListPreset,"Pre")), function() {
        if ($(this).attr("id").indexOf(strPresetID)>=0)
        {
            $(this).val(PresetValue);
        }
    });
}


// Comm onChange
function On_ChangeComm(obj) {
    var Commlist = $(obj);
    var CommId=Commlist.attr("id");
    var CommValue=Commlist.val();
    var Commlists=$(".select-comm");
    Commlists.each(function()
    {
       if ($(this).attr("id").indexOf(CommId + "_")>=0)
       {
           $(this).val(CommValue);
       }
    });
    
}

//----------End Event--------------------//