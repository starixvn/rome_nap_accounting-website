var pokerData={
    isReloaded:false,
    isFirstSuccess:true,
	isFirstNoPermissionMsg:true,
    getCustomerStatus:function (Id)
    {
       try { PTBlockUI() } catch (e) { }
       var param = '{"custId":' + Id + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/GetPokerStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                if (rs.errCode>0)
                {
                    showAlertMessage("err",rs.errMessage,1);
                }
                else
                {
                    pokerStatus.data=rs;
                    pokerStatus.initPokerStatus();
                }
                try {  PTUnBlockUI();  } catch (e) { }
            }
        }); 
    },
    getDefaultSetting:function ()
    {
       $("#err").empty();
       try { PTBlockUI() } catch (e) { }
       var param = {};
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/GetPokerDefaultSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                if (rs.errCode>0)
                {
                    showAlertMessage("err",rs.errMessage,1);
                }
                else
                {
                    DefaultSetting.mappingData(rs.items);
                    
                }
                try {  PTUnBlockUI();  } catch (e) { }
            }
        }); 
    },
    getCustomerSetting:function(custId, roleId){
       
       try { PTBlockUI() } catch (e) { }
       var param = '{"custId":' + custId + ',"roleId":' + roleId  + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/GetPokerSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                if (rs.errCode>0)
                {
                    showAlertMessage("err",rs.errMessage,1);
                }
                else
                {
                    pokerManager.mappingPokerData(rs, custId);
                    pokerManager.createDependentObservable();
                    pokerManager.initPoker();
                    
                }
                try {  PTUnBlockUI();  } catch (e) { }
            }
        }); 
    },
    updateGameSetting:function(data, type, isPartial)
    {
       try { PTBlockUI() } catch (e) { }
       pokerData.isFirstSuccess=true;
       for (var i=0;i<data.GameSettings().length;i++)
       {
        var gameSetting=data.GameSettings()[i];
        //if (ko.toJSON(gameSetting) != JSON.stringify(pokerManager.pokerDefaultData.GameSettings[i]))
        //{
            var param = '{"gameSetting":' + ko.toJSON(gameSetting) + ',"custDetail":' + ko.toJSON(data.CustInfo) + ',"type":' + type  + '}';
            $.ajax({
                type: "POST",
                url: "Services/PokerService.svc/UpdatePokerGameType",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data:  param,
                success: function (rs) {
                    //pokerManager.pokerVM.isAllowUpdate(false);
                    pokerData.afterUpdate(rs,isPartial,true);
                }
            }); 
        //}
       }
       
    },
    updateCustomerSetting:function(data, type, isPartial)
    {
       try { PTBlockUI() } catch (e) { }
       var param = '{"setting":' + ko.toJSON(data) + ',"type":' + type  + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/UpdatePoker",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                //pokerManager.pokerVM.isAllowUpdate(false);
                pokerData.afterUpdate(rs,isPartial);
            }
        }); 
    },
    updateStatus:function()
    {
     if (isDisabled)
            {
                showAlertMessage("err",msgDisabled,1);
                return;
            }
       var data = pokerStatus.setStatusParams();
       try { PTBlockUI() } catch (e) { }
       var param = '{"status":' + JSON.stringify(data) + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/UpdatePokerStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                //pokerData.afterUpdate(rs,isPartial);
                if (rs.ErrorCode==0)
                {
                    showAlertMessage("err","Poker status updated successfully",0);
                    //pokerManager.pokerVM.isAllowUpdate(false);
                    window.setTimeout(function() {
                        try { parent.ClosePopup(); } catch (e) { }
                    }, 6000);
                }
                else
                {
                    showAlertMessage("err",rs.ErrorMessage,1);
                }
            },
            complete:function(){
                try {  PTUnBlockUI();  } catch (e) { }
            }
        });
    },
    updateDefaultSetting:function(data)
    {
        data.MaxCredit(data.MaxCredit().replace(/,/g,''));
        data.MinCredit(data.MinCredit().replace(/,/g,''));
       if (!isInt(data.MaxCredit()) || !isInt(data.MinCredit()))
       {
             showAlertMessage("err","Invalid input data.",1);
             try {  PTUnBlockUI();  } catch (e) { }
             return;
       }
       try { PTBlockUI() } catch (e) { }
     
       var param = '{"item":' + ko.toJSON(data) + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/UpdatePokerDefaultSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                //pokerData.afterUpdate(rs,isPartial);
                if (rs.ErrorCode==0)
                {
                    showAlertMessage("err","P2P default setting updated successfully",0);
                    var t=setTimeout(function(){ pokerData.getDefaultSetting();},3000);
                }
                else
                {
                    showAlertMessage("err",rs.ErrorMessage,1);
                }
            },
            complete:function(){
                try {  PTUnBlockUI();  } catch (e) { }
            }
        });
    },
    updateDefaultSettingMultiple:function()
    {
        
       var arrErr=[]; 
       var data=[];
       var len=DefaultSetting.VM.items().length;
        for (var i=0;i<len;i++)
        {
            var item=DefaultSetting.VM.items()[i];
            item.MaxCredit(item.MaxCredit().replace(/,/g,''));
            item.MinCredit(item.MinCredit().replace(/,/g,''));
            if (item.isUpdate())
            {
                if (isInt(item.MinCredit()) && isInt(item.MaxCredit()))
                {
                    data.push({
                        Currency:item.Currency,
                        CurrencyID:item.CurrencyID,
                        MinCredit:item.MinCredit().replace(/,/g,''),
                        MaxCredit:item.MaxCredit().replace(/,/g,''),
                        Maxsdiscount:item.Maxsdiscount(),
                        Maxadiscount:item.Maxadiscount(),
                        Maxdiscount:item.Maxdiscount(),
                        Maxplayerdiscount:item.Maxplayerdiscount()
                    });
                 }
                 else
                 {
                    arrErr.push({Key:parseInt(item.CurrencyID)+1000,Value:item.Currency + " - invalid input data."});
                 }
                 
            }
        } 
        if (data.length==0)
        {
             showEnrichMessage("err",arrErr);
             return;
        }
       try { PTBlockUI() } catch (e) { }
       var param = '{"items":' + JSON.stringify(data) + '}';
       $.ajax({
            type: "POST",
            url: "Services/PokerService.svc/UpdatePokerDefaultSettingMultiple",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data:  param,
            success: function (rs) {
                //pokerData.afterUpdate(rs,isPartial);
                
                    var len=rs.ErrorHandler.length;                  
                    if (rs.ErrorHandler != null && len > 0) {
                        for (var i=0;i<len;i++)
                        {
                            arrErr.push(rs.ErrorHandler[i]);
                        }
                        showEnrichMessage("err",arrErr); 
                    }
                    else
                    {
                        arrErr.push({Key:0,Value:"P2P default setting updated successfully"});
                        showEnrichMessage("err",arrErr);
                        //showAlertMessage("err","P2P default setting updated successfully",0);
                        var t=setTimeout(function(){ pokerData.getDefaultSetting();},6000);
                    }
            },
            complete:function(){
                try {  PTUnBlockUI();  } catch (e) { }
            }
        });
    },
    afterUpdate:function(result, isPartial, isGameSetting)
    {
        if (result.ErrorHandler != null && result.ErrorHandler.length > 0) {
               if (isPartial)
               {
                    showEnrichMessage("err",result.ErrorHandler);
                    var t=setTimeout(function(){
                         pokerData.getCustomerSetting(custId,roleId);
                    },6000);
               }
               else
               {
                    var isSuccess = true;
                    var resultStr = '';
                    if (result.ErrorHandler != null && result.ErrorHandler.length > 0) {
                         $.each(result.ErrorHandler, function (index, value) {
                             if (value.Key > 0) {
								if (pokerData.isFirstNoPermissionMsg)
								 {
									resultStr += formatAlertMessage(value.Value, 1);
								 }
                                 isSuccess = false;
                             }
                             else {                               
                                 
									 if (isGameSetting)
									 {
										if (pokerData.isFirstSuccess)
										{
											resultStr += formatAlertMessage(value.Value, 0);
											pokerData.isFirstSuccess=false;
										}
									 }
									 else
									 {   
										resultStr += formatAlertMessage(value.Value, 0);
									 }
								 }
							 if (value.Key==500) pokerData.isFirstNoPermissionMsg=false;
                         });
                         if (!isSuccess)
                         {
                            $("#box").removeClass("boxscc").addClass("boxerr");
                         }
                         $("#box").removeClass("hidden").append(resultStr);
                     }
               }
               $(".btnUpdateSetting").attr("disabled",true);
               $(".btnUpdate").attr("disabled",true);
               
               var t=setTimeout(function(){
                    if (!pokerData.isReloaded)
                    {
                        pokerData.getCustomerSetting(custId,roleId);
                        pokerData.isReloaded=true;
                    }
               },6000);
             }
            try {  PTUnBlockUI();  } catch (e) { }
    }
};