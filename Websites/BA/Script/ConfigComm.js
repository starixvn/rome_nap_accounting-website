$(function () {
    if (showLog) {
        ConfigComm.gridWidth = 990;
    }
    else {
        ConfigComm.gridWidth = 947;
        $("#colViewLog").hide();
        //$(".maincontent_box").css("width", "725px");
    }
    ConfigComm.initGrid();
});
var ConfigComm = {
    gridWidth: 990,
    defaultData: null,
    item: function (item) {
        this.Id = item.Id;
        this.Name = item.Name;
        this.ValueGroupA = ko.observable(item.ValueGroupA);
        this.DefaultGroupA = ko.observable(item.ValueGroupA);
        this.ValueGroupB = ko.observable(item.ValueGroupB);
        this.DefaultGroupB = ko.observable(item.ValueGroupB);
        this.ValueGroupC = ko.observable(item.ValueGroupC);
        this.DefaultGroupC = ko.observable(item.ValueGroupC);
        this.ValueGroupD = ko.observable(item.ValueGroupD);
        this.DefaultGroupD = ko.observable(item.ValueGroupD);
        this.Value1X2Comm = ko.observable(item.Value1X2Comm);
        this.Default1X2Comm = ko.observable(item.Value1X2Comm);
        this.ValueOtherComm = ko.observable(item.ValueOtherComm);
        this.DefaultOtherComm = ko.observable(item.ValueOtherComm);
        this.ValueGroup = ko.observable(item.ValueGroup);
        this.DefaultGroup = ko.observable(item.ValueGroup);
        this.EnableBA = ko.observable(item.EnableBa);
        this.DefalueEnableBA = ko.observable(item.EnableBa);
        this.ModifyBy = ko.observable(item.ModifyBy);
        this.ModifyDate = ko.observable(item.ModifyDate);
      
    },
     createDependentObservable: function (item) {
        item.isEnable = ko.dependentObservable(function () {          
            if (
                    item.ValueGroupA() != item.DefaultGroupA() ||
                    item.ValueGroupB() != item.DefaultGroupB() ||
                    item.ValueGroupC() != item.DefaultGroupC() ||
                    item.ValueGroupD() != item.DefaultGroupD() ||
                    item.Value1X2Comm() != item.Default1X2Comm() ||
                    item.ValueOtherComm() != item.DefaultOtherComm() ||
                    item.ValueGroup() != item.DefaultGroup() ||
                    item.EnableBA() != item.DefalueEnableBA()
                )
                return true;
            return false;
           
        }, ConfigComm.VM);
    },
    VM: {
        items: ko.observableArray(),
        ConfigValue: ko.observableArray(),
        group : ko.observableArray(['a','b','c','d']),
        deleteConfig: function (item) {
            var param = { "licenseeId": item.Id }
            $.ajax({
                type: "POST",
                url: "Service/LIcenseeConfig.svc/DelBALicenseeConfig",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    if (rs.ErrorCode == 0) {
                        showAlertMessage("dvMsg", "Deleted Successfully", 0);
                        ConfigComm.VM.items.remove(function (i) { return parseInt(i.Id) == item.Id });
                        $("#gridConfigComm").reRenderScroller(ConfigComm.gridWidth, 603, true);
                    }
                    else {
                        showAlertMessage("dvMsg", rs.ErrorMessage, 1);
                    }
                    var t = setTimeout(function () {
                        $("#dvMsg").empty();
                    }, 4000);

                }
            });
        },
        updateConfig: function (item) {
            
            try { parent.BlockUI(); } catch (e) { }
            var param = { "licenseeId": item.Id, "groupa": item.ValueGroupA(), "groupb": item.ValueGroupB(), "groupc": item.ValueGroupC(), "groupd": item.ValueGroupD(), "ugroup": item.ValueGroup(), "oneXTwoComm": item.Value1X2Comm(), "otherComm": item.ValueOtherComm(), "enableBA": item.EnableBA() }
            var str = "";
            if(item.EnableBA() != item.DefalueEnableBA())
            {
                if(item.DefalueEnableBA() == true){
                    var paramCheckBA = {"licenseeid": item.Id};
                     $.ajax({
                        type: "POST",
                        url: "Service/LIcenseeConfig.svc/CheckDownlineBA",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async:false,
                        data: JSON.stringify(paramCheckBA),
                        success: function (rs) {
                          str = rs + ' BA members of this licensee will be turned to normal members. Are you sure you want to proceed?';                          
                        }
                     });
                     
                }
                else
                {
                    str = "Are you sure you want to change BA status for this Licensee?";
                }
            }
            else 
            {
                if(item.ValueGroupA() != item.DefaultGroupA() || item.ValueGroupB() != item.DefaultGroupB() || item.ValueGroupC() != item.DefaultGroupC() || item.ValueGroupD() != item.DefaultGroupD() || item.Value1X2Comm() != item.Default1X2Comm() || item.ValueOtherComm() != item.DefaultOtherComm() || item.ValueGroup() != item.DefaultGroup())
                {
                    if(item.DefalueEnableBA())
                        str = "Are you sure you want to adjust commission value? This adjustment will be applied for all members under this Licensee immediately.";
                    else 
                        str = "Are you sure you want to adjust commission value?";
                }
               
            }
            
            
            if(confirm(str))
            {
            $.ajax({
                type: "POST",
                url: "Service/LIcenseeConfig.svc/UpdBALicenseeConfig",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                success: function (rs) {
                    if (rs.ErrorCode == 0) {
                        ConfigComm.initGrid();
                        showAlertMessage("dvMsg", "Updated Successfully", 0);
                    }
                    else {
                        showAlertMessage("dvMsg", rs.ErrorMessage, 1);
                    }
                    var t = setTimeout(function () {
                        $("#dvMsg").empty();
                    }, 4000);
                },
                
                complete: function () {
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                
                }
            });
           
            }
            else 
                 try { parent.jQuery.unblockUI(); } catch (e) { }
            
        }

    },
    mappingData: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/LIcenseeConfig.svc/SelBaLicenseeConfig",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (rs) {
                ConfigComm.defaultData = rs;
                ConfigComm.VM.items.removeAll();
                for (i = 0; i < rs.length; i++) {
                    var item=new ConfigComm.item(rs[i])
                    ConfigComm.createDependentObservable(item);
                    ConfigComm.VM.items.push(item);
                }
                
                var t=setTimeout(function(){
                    ko.applyBindings(ConfigComm.VM, document.getElementById("CommLicensee"));
                     $("#gridConfigComm").removeClass('hidden').reRenderScroller(ConfigComm.gridWidth, 603, true);
                },100);
                

               
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });

    },

    initGrid: function () {
        ConfigComm.mappingData();
    },
    viewLog: function (licenseeId) {
        try { parent.BlockUI(); } catch (e) { }
        var title = 'Log Configuration';
        var url = 'ViewLogComm.aspx?licenseeId=' + licenseeId;
        var width = 990;
        var height = 700;
        OpenPopup(url, title, width, height, false, true, window, 0, null, null);
    },
    addBALicensee: function () {
        var id = $("#add_licensee option:selected").attr("value");
        var enable = $("#enableBA").attr("checked");
        var HdpComm = $("#HDPComm").val();
        var oneTwoComm = $("#1x2Comm").val();
        var otherComm = $("#otherComm").val();
        var gameComm = $("#gameComm").val();
        var param = { "licenseeId": id, "hdpComm": HdpComm, "oneXTwoComm": oneTwoComm, "otherComm": otherComm, "gameComm": gameComm, "enableBA": enable }
        $.ajax({
            type: "POST",
            url: "Service/LIcenseeConfig.svc/InsBALicenseeConfig",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            success: function (rs) {
                if (rs.ErrorCode == 0) {
                    $('#errMsg').show();
                    showAlertMessage('errMsg', "Inserted Successfully", 0);
                    //setTimeout('hidePopup();', 200);
                    hidePopUp();
                    ConfigComm.initGrid();
                }
                else {
                    $('#errMsg').show();
                    showAlertMessage('errMsg', rs.ErrorMessage, 1);
                }
            }
        });
    }
};

function getCommissionValues(min, max, step) {
    
    var arr = [];
    for (var i = parseFloat(max); i >= parseFloat(min); i = roundNumber(i-step,4)) {
        arr.push(i);
    }
    return arr;
}

function getBACommissionValues(type) {
    var arr;
    switch (type)
    {
        case 1:
            arr=getCommissionValues(MinGroupAComm, MaxGroupAComm, StepGroupAComm);
            break;
        case 2:
            arr=getCommissionValues(MinGroupBComm, MaxGroupBComm, StepGroupBComm);
            break;
        case 3:
            arr = getCommissionValues(MinGroupCComm, MaxGroupCComm, StepGroupCComm);
            break;
        case 4:
            arr = getCommissionValues(MinGroupDComm, MaxGroupDComm, StepGroupDComm);
            break;
        case 5:
            arr = getCommissionValues(Min1x2Comm, Max1x2Comm, Step1x2Comm);
            break;
        case 6: 
            arr = getCommissionValues(MinOtherComm, MaxOtherComm, StepOtherComm);
            break;
        case 7: 
            arr = getCommissionValues(MinGroupBComm, MaxGroupBComm, StepGroupBComm);
            break;
    }
    return arr;
}

function roundNumber(num, dec) {
    var result = Math.round(Math.round(num * Math.pow(10, dec + 1)) / Math.pow(10, 1)) / Math.pow(10, dec);
    return result;
}

$(function () {
      $('#tblComm').delegate('tr','mouseover', function( event ) {
        $(this).addClass("mouseon");  
     });
    $('#tblComm').delegate('tr','mouseout', function( event ) {
        $(this).removeClass("mouseon");  
     });
    $('#addLicenseeBA').dialog({
        title: "Add  BALicensee",
        autoOpen: false,
        height: 180,
        width: 400,
        model: true
    });
    
});

function showPopup() {
    $('#errMsg').hide();
    $('#add_licensee').find('option').remove();
    $.ajax({
        type: "POST",
        url: "Service/LIcenseeConfig.svc/SelAllLicensee",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rs) {
            var objSelect = $('#add_licensee');
            for (i = 0; i < rs.length; i++) {
                var records = jlinq.from(ConfigComm.defaultData).equals("Name", rs[i].Name).select();
                if (records.length == 0) {
                    objSelect.append("<option value='" + rs[i].Id + "'>" + rs[i].Name + "</option>");
                }
            }
            $('#addLicenseeBA').removeClass("hidden").dialog("open");
        }
    });
}

function hidePopUp() {
    $('#addLicenseeBA').dialog("close");
}


$(window).load(function () {
     try { parent.jQuery.unblockUI(); } catch (e) { }  
});
