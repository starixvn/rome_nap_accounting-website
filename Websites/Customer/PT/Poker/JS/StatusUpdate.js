$(function () {
   pokerData.getCustomerStatus(custId);
});

var pokerStatus={
    data:{},
    initPokerStatus:function(){
        $("#SettingContent").removeClass("hidden");
        $("#custinfo").html(getRoleName(pokerStatus.data.CustInfo.RoleId) + ": " + pokerStatus.data.CustInfo.UserName);
       
		switch (parseInt(pokerStatus.data.CustInfo.RoleId))
        {
            case 1:
                $("#tddl, #tddlfun").addClass("hidden");
                $("#chkEnable").attr("checked",!pokerStatus.data.disableP2P).attr("value",!pokerStatus.data.disableP2P); 
                $("#chkEnable").attr("disabled",pokerStatus.data.disableUplineP2P);
                $("#chkEnableFun").attr("checked",!pokerStatus.data.disableFunP2P).attr("value",!pokerStatus.data.disableFunP2P); 
                $("#chkEnableFun").attr("disabled",pokerStatus.data.disableUplineFunP2P);
				if (pokerStatus.data.disableUplineFunP2P && pokerStatus.data.disableUplineFunP2P)
				{
					$("#btnAdd").attr("disabled",true);
				}
                break;
            case 2:
                $("#chkEnable").attr("checked",!pokerStatus.data.disableP2P).attr("value",!pokerStatus.data.disableP2P); 
                $("#chkEnable").attr("disabled",pokerStatus.data.disableUplineP2P);   
                $("#chkEnableDownline").attr("checked",!pokerStatus.data.disableDLP2P).attr("value",!pokerStatus.data.disableDLP2P);
                
                $("#chkEnableFun").attr("checked",!pokerStatus.data.disableFunP2P).attr("value",!pokerStatus.data.disableFunP2P); 
                $("#chkEnableFun").attr("disabled",pokerStatus.data.disableUplineFunP2P);  
                $("#chkEnableFunDownline").attr("checked",!pokerStatus.data.disableDLFunP2P).attr("value",!pokerStatus.data.disableDLFunP2P);
				
				if (pokerStatus.data.disableUplineFunP2P && pokerStatus.data.disableUplineFunP2P)
				{
					$("#btnAdd").attr("disabled",true);
				}
                 break;   
            case 3:
                $("#chkEnable").attr("checked",!pokerStatus.data.disableP2P).attr("value",!pokerStatus.data.disableP2P); 
                $("#chkEnable").attr("disabled",pokerStatus.data.disableUplineP2P);   
                $("#chkEnableDownline").attr("checked",!pokerStatus.data.disableDLP2P).attr("value",!pokerStatus.data.disableDLP2P);
                
                $("#chkEnableFun").attr("checked",!pokerStatus.data.disableFunP2P).attr("value",!pokerStatus.data.disableFunP2P); 
                $("#chkEnableFun").attr("disabled",pokerStatus.data.disableUplineFunP2P);  
                $("#chkEnableFunDownline").attr("checked",!pokerStatus.data.disableDLFunP2P).attr("value",!pokerStatus.data.disableDLFunP2P);
			
				if (pokerStatus.data.disableUplineFunP2P && pokerStatus.data.disableUplineFunP2P)
				{
					$("#btnAdd").attr("disabled",true);
				}
                 break;  
            case 4:
                $("#chkEnable").attr("checked",!pokerStatus.data.disableP2P).attr("value",!pokerStatus.data.disableP2P);             
                $("#chkEnableDownline").attr("checked",!pokerStatus.data.disableDLP2P).attr("value",!pokerStatus.data.disableDLP2P);
                $("#chkEnableFun").attr("checked",!pokerStatus.data.disableFunP2P).attr("value",!pokerStatus.data.disableFunP2P);                 
                $("#chkEnableFunDownline").attr("checked",!pokerStatus.data.disableDLFunP2P).attr("value",!pokerStatus.data.disableDLFunP2P);
                break;         
            default:
                break;
        }
    },
    setStatusParams:function()
    {
        pokerStatus.data.disableP2P=!$("#chkEnable").attr("checked");
        pokerStatus.data.disableFunP2P=!$("#chkEnableFun").attr("checked");
        pokerStatus.data.disableDLP2P=!$("#chkEnableDownline").attr("checked");
        pokerStatus.data.disableDLFunP2P=!$("#chkEnableFunDownline").attr("checked");
        
        return pokerStatus.data;
    }
};

function ChkEnalbe(obj) {
    if (parseInt(pokerStatus.data.CustInfo.RoleId)>1)
    {
        var checked = $(obj).attr("checked");
        var oldvalue = $(obj).attr("value") == "true" ? true : false;

        var downlineId = $(obj).attr("id")+ "Downline";
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
}


function BtnOnClick() {
    $(".errmsg").empty();
    pokerData.updateStatus();
}
