
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
        var result;
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
            SettingContent.setTemplateURL("../../../App_Templates/Default/Customer/PT/Bingo/MasterUpdateBingoPT.htm");
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
        //        param='{"person":{"ID":1,"Name":"Huy","Clothlist":';
        //            param+='[';
        //                param+='{"ID":1,"Name":"Jeans","Colorlist":';
        //                    param+='[';
        //                        param+='{"Name":"Blue"},';
        //                        param+='{"Name":"Red"}';
        //                    param+=']';
        //                param+='},';
        //                param+='{"ID":2,"Name":"Shirt","Colorlist":';
        //                    param+='[';
        //                        param+='{"Name":"Black"},';
        //                        param+='{"Name":"White"}';
        //                    param+=']';
        //                param+='}';
        //            param+=']';
        //        param+='}}';
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
        Customer.IsAutoReset = $("#cbxAutoReset").attr("checked");
        Customer.IncentiveRate = $('#listIncentiveRate').val();


        for (var i=0;i<totalGameKind;i++)
        {
            var gamekind=Customer.GameKindList[i];
            var gamekindid=gamekind.ID;
            totalGameType=gamekind.GameTypeList.length;
            var isAutoGameKind=$("#" + strChkAutoPreset + "_" + gamekindid).attr("checked");
            
            if (isAutoGameKind)
            {
                Customer.GameKindList[i].AutoPositionTaking=roundNumber($("#" + strListAutoPreset + "_" + gamekindid).val(),2);
                Customer.GameKindList[i].PositionTakingUpline=0;
            }
            else
            {
                Customer.GameKindList[i].AutoPositionTaking=0;
                 Customer.GameKindList[i].PositionTakingUpline=roundNumber($("#" + strListPreset + "_" + gamekindid).val(),2);
            }
            Customer.GameKindList[i].IsAutoPositionTaking=isAutoGameKind;
            Customer.GameKindList[i].PositionTaking=roundNumber($("#" + strListMax + "_" + gamekindid).val(),2);
            Customer.GameKindList[i].MinPositionTaking=roundNumber($("#" + strListMin + "_" + gamekindid).val(),2);
            Customer.GameKindList[i].Commission=roundNumber($("#" + strListComm + "_" + gamekindid).val(),4);
            //Loop to get PT setting GameType
            for (var j=0;j<totalGameType;j++)
            {
                var gametype=Customer.GameKindList[i].GameTypeList[j];
                var gametypeid=gametype.ID;
                totalBettingRange=gametype.BettingRangeList.length;
                if (!Customer.IsUsedGameKind)
                {
                    var isAutoGameType=$("#" + strChkAutoPreset + "_" + gamekindid + "_" + gametypeid).attr("checked");
                    if (isAutoGameType)
                    {
                        Customer.GameKindList[i].GameTypeList[j].AutoPositionTaking=roundNumber($("#" + strListAutoPreset + "_" + gamekindid + "_" + gametypeid).val(),2);
                        Customer.GameKindList[i].GameTypeList[j].PositionTakingUpline=0;
                    }
                    else
                    {
                        Customer.GameKindList[i].GameTypeList[j].AutoPositionTaking=0;
                         Customer.GameKindList[i].GameTypeList[j].PositionTakingUpline=roundNumber($("#" + strListPreset + "_" + gamekindid + "_" + gametypeid).val(),2);
                    }
                    Customer.GameKindList[i].GameTypeList[j].IsAutoPositionTaking=isAutoGameType;
                    Customer.GameKindList[i].GameTypeList[j].PositionTaking=roundNumber($("#" + strListMax + "_" + gamekindid + "_" + gametypeid).val(),2);
                    Customer.GameKindList[i].GameTypeList[j].MinPositionTaking=roundNumber($("#" + strListMin + "_" + gamekindid + "_" + gametypeid).val(),2);
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
            if (id.indexOf(strListPreset)>=0)
            {
                on_ChangePreset(document.getElementById(id));
            }
            else if (id.indexOf(strListAutoPreset)>=0)
            {
                On_ChangeAutoPreset(document.getElementById(id));
            }
            else if (id.indexOf(strListMax)>=0)
            {
                on_ChangeMax(document.getElementById(id));
            }
            else if (id.indexOf(strListMin)>=0)
            {
                On_ChangeMin(document.getElementById(id));
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
            else if (id.indexOf(strListAutoPreset)>=0)
            {
                On_ChangeAutoPreset(document.getElementById(id));
            }
            else if (id.indexOf(strListMax)>=0)
            {
                on_ChangeMax(document.getElementById(id));
            }
            else if (id.indexOf(strListMin)>=0)
            {
                On_ChangeMin(document.getElementById(id));
            }
        }
    }
}
function on_ChangePreset(obj)
{
    var oPreset=$(obj);
    var strPresetID=oPreset.attr("id");
    var PresetValue=oPreset.val();
    var PresetValueMax=oPreset.attr("max");
    
     jQuery.each($("." + strPresetID.replace(strListPreset,"Pre")), function() {
            if ($(this).attr("id").indexOf(strPresetID)>=0)
            {
                $(this).val(PresetValue);
                on_ChangePresetGameType(this);
            }
        });
    
    var oMax=$("#" + strPresetID.replace(strListPreset,strListMax));
    var strMaxID=oMax.attr("id");
    var MaxValue=oMax.val();
    
    var MaxValueMax=roundNumber(PresetValueMax-PresetValue,2);
    
    //if (MaxValue>MaxValueMax)
    //{
        MaxValue=MaxValueMax;
    //}
    oMax.attr("max",MaxValueMax);
    CreateOption(oMax, MaxValue, MaxValueMax, 0, 0.01, 2);
    on_ChangeMax(document.getElementById(strMaxID));
    
}
function on_ChangePresetGameType(obj)
{
    var oPreset=$(obj);
    var strPresetID=oPreset.attr("id");
    var PresetValue=oPreset.val();
    var PresetValueMax=oPreset.attr("max");
    $("#" + strPresetID.replace(strListPreset,strerrPT)) .css("visibility","hidden");
    $("#" + strPresetID.replace(strListPreset,strerrPTMin)) .css("visibility","hidden");
    var oMax=$("#" + strPresetID.replace(strListPreset,strListMax));
    var MaxValue=oMax.val();
    
    var MaxValueMax=roundNumber(PresetValueMax-PresetValue,2);
    
    if (MaxValue>MaxValueMax)
    {
        MaxValue=MaxValueMax;
    }
    oMax.attr("max",MaxValueMax);
    CreateOption(oMax, MaxValue, MaxValueMax, 0, 0.01, 2);
    
}

function on_ChangeMax(obj)
{
    var oMax=$(obj);
    var strMaxID=oMax.attr("id");
    var MaxValue=oMax.val();
    
     jQuery.each($("." + strMaxID.replace(strListMax,"Max")), function() {
            if ($(this).attr("id").indexOf(strMaxID)>=0)
            {
                  if ($(this).attr("max")>=MaxValue)
                    {
                         $(this).val(MaxValue);
                         on_ChangeMaxGameType(this);
                         $("#" + $(this).attr("id").replace(strListMax,strerrPT)).css("visibility","hidden");
                    }
                    else
                    {
                        $("#" + $(this).attr("id").replace(strListMax,strerrPT)).css("visibility","visible");
                    }
            
                
            }
        });
    
    var oMin=$("#" + strMaxID.replace(strListMax,strListMin));
    var strMinID=oMin.attr("id");
    var MinValue=oMin.val();
    var MinValueMax=MaxValue;
    
  
    if (MinValue>MinValueMax)
    {
        MinValue=MinValueMax;
    }
    oMin.attr("max",MinValueMax);
    CreateOption(oMin, MinValue, MinValueMax, 0, 0.01, 2);

}

function on_ChangeMaxGameType(obj)
{
  
    var oMax=$(obj);
    var strMaxID=oMax.attr("id");
    var MaxValue=oMax.val();
    $("#" + strMaxID.replace(strListMax,strerrPT)).css("visibility","hidden");
    $("#" + strMaxID.replace(strListMax,strerrPTMin)).css("visibility","hidden");
    var oMin=$("#" + strMaxID.replace(strListMax,strListMin));
     
    var MinValue=oMin.val();
    var MinValueMax=MaxValue;
    
    if (MinValue>MinValueMax)
    {
        MinValue=MinValueMax;
    }
    oMin.attr("max",MinValueMax);
    CreateOption(oMin, MinValue, MinValueMax, 0, 0.01, 2);
}

function CheckAutoPresetPT(obj)
{   
    var chkAuto=$(obj);
    var chkAutoID=chkAuto.attr("id");
    if (chkAuto.attr("checked"))
    {
        $("#" + chkAutoID.replace(strChkAutoPreset,strListPreset)).attr("disabled","disabled");
        $("#" + chkAutoID.replace(strChkAutoPreset,strListAutoPreset)).removeAttr("disabled");
        
        $(".Pre" + chkAutoID.replace(strChkAutoPreset,"")).attr("disabled","disabled");
        $(".AutoPre" + chkAutoID.replace(strChkAutoPreset,"")).removeAttr("disabled");
        
        //Change Max value of Max Min 
        var oMax=  $("#" + chkAutoID.replace(strChkAutoPreset, strListMax));
        var oMin=  $("#" + chkAutoID.replace(strChkAutoPreset, strListMin));
        var oAutoPreset= $("#" + chkAutoID.replace(strChkAutoPreset, strListAutoPreset));
        oMax.attr("max",oAutoPreset.attr("max"));
        oMin.attr("max",oMax.val());
        CreateOption(oMax, oMax.val(),oAutoPreset.attr("max"), 0,0.01,2);
        CreateOption(oMin, oMin.val(),oMax.val(), 0,0.01,2);
        
        jQuery.each($(".chkauto"), function() {
            if ($(this).attr("id").indexOf(chkAutoID)>=0) {
                $(this).attr("checked","checked");
                 CheckAutoPresetPTGameType(this);
                }
        });
    }
    else
    {
        $("#" + chkAutoID.replace(strChkAutoPreset,strListAutoPreset)).attr("disabled","disabled");
        $("#" + chkAutoID.replace(strChkAutoPreset,strListPreset)).removeAttr("disabled");
        
        $(".AutoPre" + chkAutoID.replace(strChkAutoPreset,"")).attr("disabled","disabled");
        $(".Pre" + chkAutoID.replace(strChkAutoPreset,"")).removeAttr("disabled");
        
        jQuery.each($(".chkauto"), function() {
            if ($(this).attr("id").indexOf(chkAutoID)>=0) {
                $(this).removeAttr("checked");
                CheckAutoPresetPTGameType(this);
        }
        });
        
        on_ChangePreset(document.getElementById(chkAutoID.replace(strChkAutoPreset,strListPreset)));
    }
}

function CheckAutoPresetPTGameType(obj)
{
    var chkAuto=$(obj);
    var chkAutoID=chkAuto.attr("id");
    if (chkAuto.attr("checked"))
    {
        $("#" + chkAutoID.replace(strChkAutoPreset,strListPreset)).attr("disabled","disabled");
        $("#" + chkAutoID.replace(strChkAutoPreset,strListAutoPreset)).removeAttr("disabled");
        
        //Change Max value of Max Min
        var oMax=  $("#" + chkAutoID.replace(strChkAutoPreset, strListMax));
        var oMin=  $("#" + chkAutoID.replace(strChkAutoPreset, strListMin));
        var oAutoPreset= $("#" + chkAutoID.replace(strChkAutoPreset, strListAutoPreset));
        oMax.attr("max",oAutoPreset.attr("max"));
        oMin.attr("max",oMax.val());
        CreateOption(oMax, oMax.val(),oAutoPreset.attr("max"), 0,0.01,2);
        CreateOption(oMin, oMin.val(),oMax.val(), 0,0.01,2);
    }
    else
    {
        $("#" + chkAutoID.replace(strChkAutoPreset,strListAutoPreset)).attr("disabled","disabled");
        $("#" + chkAutoID.replace(strChkAutoPreset,strListPreset)).removeAttr("disabled");
        //on_ChangePresetGameType(document.getElementById(chkAutoID.replace(strChkAutoPreset,strListPreset)));
    }
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

function On_ChangeAutoPreset(obj) {
    var Auto = $(obj);
    var AutoID=Auto.attr("id");
    var AutoValue=Auto.val();
    var Autolists=$(".AutoPre" + AutoID.replace(strListAutoPreset,""));
    Autolists.each(function()
    {
       if ($(this).attr("id").indexOf(AutoID + "_")>=0)
       {
           $(this).val(AutoValue);
       }
    });
    
}
function On_ChangeMin(obj)
{
    var Min = $(obj);
    var MinID=Min.attr("id");
    var MinValue=Min.val();
    var Minlists=$(".Min");
    Minlists.each(function()
    {
       if ($(this).attr("id").indexOf(MinID + "_")>=0)
       {
           if (MinValue<=$(this).attr("max"))
           {
            $(this).val(MinValue);
            $("#" + $(this).attr("id").replace(strListMin,strerrPTMin)).css("visibility","hidden");
           }
           else
           {
            $("#" + $(this).attr("id").replace(strListMin,strerrPTMin)).css("visibility","visible");
           }
       }
    });
}
//----------End Event--------------------//

