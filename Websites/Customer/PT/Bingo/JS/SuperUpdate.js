//Global variable declaretion
var Customer;

/*--End global variable declaretion--*/
$(document).ready(function() {
    getCustomerSetting();
});

//-----------Load data-----------------//
function getCustomerSetting(isReload) {
    
    var custid = gup("custId");
    
    var isGroupSetting;
    if (custid!=null)
    {
        PTBlockUI();
        var param = '{"custID":' + custid + ',"updatedByRoleID":4}';
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
    //Assign  Info
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
            SettingContent.setTemplateURL("../../../App_Templates/Default/Customer/PT/Bingo/SuperUpdateBingoPT.htm");
            SettingContent.setParam("IsGroupSetting",rs.IsUsedGameKind);
            SettingContent.processTemplate(rs);
            AssignCommission();
            if (!isShowBtSave) {
                $('#tblHideUpdate').css('display', 'none');
            }
            if (rs.SyncBingo)
            {
                 $("#tblUpdate").click(function(){
                    updateSuperPT();
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
            updateSuperPT();
        });
        
    }
    else
    {
        $("#tblUpdate").css("opacity","0.5");
        $('#tblUpdate').unbind('click');

    }
}
function updateSuperPT() {
                   
 if (Customer!=null)
    {
        var maxTransferCredit=$("#txtMaxTransferCredit").val()
        if (!uInteger(maxTransferCredit))
        {
            showAlertMessage("err", "Invalid Max Transder Credit",1);
            return ;
        }
        if  (Customer.MinLimit>maxTransferCredit)
        {
            showAlertMessage("err", "Invalid Max Transder Credit",1);
            return ;
        }
        PTBlockUI();
        var totalGameKind=Customer.GameKindList.length;
        var totalGameType;
        var totalBettingRange;
        var gamekindid, gametypeid, bettingrangeid;
        
        //Loop to get PT setting GameKind
        Customer.MaxTransferCredit=maxTransferCredit;
        
        Customer.IsAutoReset = $("#cbxAutoReset").attr("checked");
        Customer.IsUsedGameKind = $("#chkGroupSetting").attr("checked"); 
        
        for (var i=0;i<totalGameKind;i++)
        {
            var gamekind=Customer.GameKindList[i];
            gamekindid=gamekind.ID;
            totalGameType=gamekind.GameTypeList.length;
            Customer.GameKindList[i].PositionTaking=roundNumber($("#" + strListMax + "_" + gamekindid).val(),2);
            Customer.GameKindList[i].MinPositionTaking=roundNumber($("#" + strListMin + "_" + gamekindid).val(),2);
            Customer.GameKindList[i].Commission=roundNumber($("#" + strListComm + "_" + gamekindid).val(),4);            
            //Loop to get PT setting GameType
            for (var j=0;j<totalGameType;j++)
            {
                var gametype=Customer.GameKindList[i].GameTypeList[j];
                gametypeid=gametype.ID;
                totalBettingRange=gametype.BettingRangeList.length;
                if (!Customer.IsUsedGameKind)
                {
                    Customer.GameKindList[i].GameTypeList[j].PositionTaking=roundNumber($("#" + strListMax + "_" + gamekindid + "_" + gametypeid).val(),2);
                    Customer.GameKindList[i].GameTypeList[j].MinPositionTaking=roundNumber($("#" + strListMin + "_" + gamekindid + "_" + gametypeid).val(),2);
                    Customer.GameKindList[i].GameTypeList[j].Commission=roundNumber($("#" + strListComm + "_" + gamekindid + "_" + gametypeid).val(),4);
                }
                //Loop to get BettingRangeList
                for (var z=0;z<totalBettingRange;z++)
                {
                    var bettingrange=Customer.GameKindList[i].GameTypeList[j].BettingRangeList[z];
                    bettingrangeid=bettingrange.ID;
                    Customer.GameKindList[i].GameTypeList[j].BettingRangeList[z].IsChecked=$("#" + strChkStake + "_" + gamekindid + "_" + gametypeid + "_" + bettingrangeid).attr("checked");
                }
            }
        }
        
        var rstext=JSON.stringify(Customer);
        var data = '{"customer":' + rstext + ',"updatedByRoleID":4' + '}';
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

// Max PT onChange
function On_ChangeMax(obj) {
    var Maxlist = $(obj);
    var MaxId=Maxlist.attr("id");
    var Minlist = $("#" + MaxId.replace(strListMax, strListMin));
   
    //Change  Min PT
    var Minvalue = Minlist.val();
    var Max_Minvalue = Maxlist.val();
    var Min_Minvalue = Minlist.attr("min");

    if (Max_Minvalue < Minvalue) Minvalue = 0;
    
    Minlist.attr("max", Max_Minvalue);
    CreateOption(Minlist, Minvalue, Max_Minvalue, Min_Minvalue, 0.01, 2);
    
    //Change Game Type  Max PT 
    var PTlists=$(".select-pt");
    PTlists.each(function()
    {
       if ($(this).attr("id").indexOf(MaxId + "_")>=0)
       {
            $(this).val(Maxlist.val());
            On_ChangeMaxGameType(document.getElementById($(this).attr("id")));
       }
    });
    
}

// Min PT onChange
function On_ChangeMin(obj)
{
    
    var Minlist = $(obj);
    var MinId=Minlist.attr("id");
    var MinValue=Minlist.val();
    var PTlists=$(".select-pt");
    PTlists.each(function()
    {
       if ($(this).attr("id").indexOf(MinId + "_")>=0)
       {
            if ($(this).attr("max")>=MinValue)
            {
                $(this).val(MinValue);
                $("#" + $(this).attr("id").replace(strListMin,strerrPT)).css("visibility","hidden");
            }
            else
            {
               $("#" + $(this).attr("id").replace(strListMin,strerrPT)).css("visibility","visible");
            }
            
       }
    });
}



//Game Type  Max onChange
function On_ChangeMaxGameType(obj) {
   
    var Maxlist = $(obj);
    var MaxId=Maxlist.attr("id");
     $("#" + MaxId.replace(strListMax,strerrPT)).css("visibility","hidden");
    var Minlist = $("#" + MaxId.replace(strListMax, strListMin));
   
    //Change  Min PT
    var Minvalue = Minlist.val();
    var Max_Minvalue = Maxlist.val();
    var Min_Minvalue = Minlist.attr("min");

    if (Max_Minvalue < Minvalue) Minvalue = 0;
    
    Minlist.attr("max", Max_Minvalue);
    CreateOption(Minlist, Minvalue, Max_Minvalue, Min_Minvalue, 0.01, 2);
    
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

function ReducePT(id, step) {
    var obj = $("#" + id);
    var curr = obj.val();
    var min = obj.attr("min");

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
        
        if (id.indexOf(strListMax)>=0)
        {
            On_ChangeMax(document.getElementById(id));
        }
        else
        {
            On_ChangeMin(document.getElementById(id));
        }
    }
}

function IncreasePT(id, step) {
    var obj = $("#" + id);
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
        if (id.indexOf(strListMax)>=0)
        {
            On_ChangeMax(document.getElementById(id));
        }
        else if (id.indexOf(strListMin)>=0)
        {
            On_ChangeMin(document.getElementById(id));
        }
    }
}


function showhideSetting(obj)
{
    if (obj.checked)
     {
        $(".gametypePT").css("display","none");
     }
     else
        {
            $(".gametypePT").css("display","");
        }
}
//-------------End Event-------------------//



