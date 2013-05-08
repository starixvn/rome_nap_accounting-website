
function ViewDNS(ip)
{
    url = 'http://anonym.to/?http://legacytools.dnsstuff.com/tools/ipall.ch?tool_id=67&token=&toolhandler_redirect=0&ip=' + ip + '&j=1';
    window.open(url,'DNS',' scrollbars=yes, toolbar=yes, menubar=yes,location=yes,status=yes,resizable=yes');
}
function onKeyEnter(e) {
    if (e.keyCode == 13) {
        LatestBet.viewBetList();
    }
}
function IsNumeric(sText)

{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
   
   }
   
var LatestBet = {
    fromDate:'',
    toDate:'',
    refNo:'',
    userName:'',
    gridTitle:'',
    currency:0,
    bettype:0,
    fromStake:0,
    toStake:0,
    excludeTest:0,
    pageIndex:1,
    pageSize:50,
    totalItem:1000,
    
    getOptionDate: function(vl){
        if (vl != 8) {
        var data = {
            dateOption: vl
        };
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetDatetimeOption",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                $('#txtfromdate').val(rs.fdate);
                $('#txttodate').val(rs.tdate);
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
      }
    },
    mappingData: function () {
       
        var param = {
            pagesize:LatestBet.pageSize,
            pageindex:LatestBet.pageIndex,
            fromdate:LatestBet.fromDate,
            todate: LatestBet.toDate,
            refno:LatestBet.refNo,
            username:LatestBet.userName,
            currency:LatestBet.currency,
            bettype:LatestBet.bettype,
            fromStake:LatestBet.fromStake,
            toStake:LatestBet.toStake,
            excludeTest:LatestBet.excludeTest
            
        };
        
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetBABetList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend:function(xhr){
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function (rs) {
                if (rs.Items!=null)
                {
                    LatestBet.totalItem = parseInt(rs.totalItem);
                    if (rs.Items.length > 0) {
                        $("#tblatestbet").html(LatestBet.renderListRows(rs.Items,LatestBet.pageSize,LatestBet.pageIndex));
                        //Render Pager
                        if(LatestBet.refNo != '' && LatestBet.refNo != null){
                             $("#pagination").html('&nbsp;'); 
                        }
                        else{
                             
                             LatestBet.initPager(LatestBet.totalItem, LatestBet.pageSize, LatestBet.pageIndex-1);
                        }
                            
                       
                    }
                    else
                    {
                       // $('#contentPagination').addClass("hidden");
                        $("#pagination").html('&nbsp;'); 
                        $("#tblatestbet").html('<tr><td colspan="9"><div class="c" style="width:1210px;">There is no information available.</div></td></tr>');
                    }
                   $('#latestbet').removeClass("hidden").reRenderScroller(1173, 580, true);  
                }
                else
                {
                    $("#pagination").html('&nbsp;'); 
                    $("#tblatestbet").html('<tr><td colspan="9"><div class="c" style="width:1210px;">There is no information available.</div></td></tr>');
                    $('#latestbet').removeClass("hidden").reRenderScroller(1173, 580, true);  
                }
            },
            complete: function () {
               try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderMP:function(row)
    {
    
      var html='<div style="border-bottom:dotted 1px; margin-bottom:5px;" class="' + (row.BettranDetail.Liveindicator ? 'event-live':'') + '"><span class="bl-choice b" >' + row.SportEvent.Choice + '</span>\
                    <span class="bl-choice b">' +  row.SportEvent.Hdp + " @" + row.Odds + '</span>\
                    <span class="bl-bettype">' + row.SportEvent.LiveScore + '</span>\
                    <span class="b">' + row.SportEvent.BetType + '</span>\
                    <span >'+ row.SportEvent.Event + '</span>\
                    <span class="bl-sporttype" >'+ row.SportEvent.SportType + '</span>\
                    <span class="bl-leaguename">'+ row.SportEvent.LeagueName + '</span></div>'; 
     return html;                
    },
    viewBetList:function(){
        $('#trMsg').hide();
        $('#latestbet').addClass("hidden");
//       if(parseInt($("input[name=rdb]:checked").val()) == 1 && isNumeric($("#txtTransID").val())){
//            $('#trMsg').show();
//            showAlertMessage("dvMsg", "TransID must be numberic.", 1);
//        } 
       if(!IsNumeric($("#txtfromstake").val()) || !IsNumeric($("#txttostake").val())){
            $('#trMsg').show();
            showAlertMessage("dvMsg", "Stake must be numberic.", 1);
           
        }
        else{
           
            LatestBet.fromDate=$("#txtfromdate").val();
            LatestBet.toDate=$("#txttodate").val();
            LatestBet.refNo=$("#txtTransID").val();
            LatestBet.userName=$("#txtUsername").val();
            LatestBet.currency = parseInt($("#dlCurrency").val());
            LatestBet.isChecked = parseInt($("input[name=rdExcludeTesting]:checked").val());
            LatestBet.bettype = parseInt($("#dlBettype").val());
            if($("#txtfromstake").val() != "")
                LatestBet.fromStake = parseInt($("#txtfromstake").val());
            else 
                LatestBet.fromStake = 0;
            if($("#txttostake").val() != "")
                LatestBet.toStake = parseInt($("#txttostake").val());
            else
                LatestBet.toStake = 0;
            if($("#cbExclude").attr("checked")){
                LatestBet.excludeTest = 1;
            }
            else 
                LatestBet.excludeTest = 0;
            if (LatestBet.refNo!='' && LatestBet.refNo != null)
            {
                 LatestBet.gridTitle="Bet List Detail - TransId: " + LatestBet.refNo + (LatestBet.userName!='' ? ' - UserName: ' + LatestBet.userName:'');
            }
            else
            {
                LatestBet.gridTitle="Bet List Detail - From: " + LatestBet.fromDate + " To: " + LatestBet.toDate + (LatestBet.userName!='' ? ' - UserName: ' + LatestBet.userName:'');
                
            }
            
            if ($("#rdTransID").attr("checked") && $("#txtTransID").val()=='')
            {
                 $("#pagination").html('&nbsp;'); 
                 $("#tblatestbet").html('<tr><td colspan="9"><div class="c" style="width:1210px;">There is no information available.</div></td></tr>');
                 $('#latestbet').removeClass("hidden").reRenderScroller(1110, 580, true);  
                    return;
            }
            $("#gridtitle").html(LatestBet.gridTitle);
            LatestBet.mappingData();
        }
       
    },
    manualRefresh:function()
    {
         LatestBet.mappingData();
    },
    initPager: function (total_item, items_per_page, current_page) {
        var opt = { 
            start_per_page:50,
            step_per_page:50,
            max_items_per_page:500,
            item_per_page_arr:[50,100,200,300,400,500],
            callback: LatestBet.pageSelectCallback };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(LatestBet.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        LatestBet.pageIndex = page_index + 1;
        LatestBet.pageSize = _items_per_page;
        LatestBet.mappingData();
        return false;
    },
    renderListRows:function(rows, _pageSize,_pageIndex)
    {
        var arr=[];
        var count = 0;
        var len=rows.length;
        
        for (var i=0;i<len;i++)
        {
           count ++;
           var index = _pageSize * (_pageIndex - 1) + count;
           
            var row=rows[i];
            var stake = "<div class='col60 r'>" + row.Stake + "</div>"
            var odds = row.Odds;
            if(row.BettranDetail.Bettype == 29){
                 stake = "<div class='col60 r'>" + row.actualStake + "<br/>("+ row.Stake +")</div>";
                 odds = row.Modds;
            }
            var html='<tr class="' + (row.BettranDetail.Liveindicator ? 'event-live':'') + '">\
                          <td><div class="col40 c">' + index  + '</td>\
                          <td><div class="col100 c">' + row.Username + '</div></td>\
                          <td><div class="col100 c"><b>Ref No: ' + row.Transid + '</b><br/><span class="bl_time">' + row.TransDate + '</span></div></td>\
                          <td>\
                            <div class="col360 r">\
                                <span class="bl-choice b" >' + row.SportEvent.Choice + '</span>\
                                <span class="bl-choice b0">' +  row.SportEvent.Hdp + '</span>\
                                <span class="bl-bettype">' + row.SportEvent.LiveScore + '</span>\
                                <span class="b bl-bettype">' + row.SportEvent.BetType + '</span>\
                                <span class="b" >'+ row.SportEvent.Event + '</span>\
                                <span class="bl-sporttype" >'+ row.SportEvent.SportType + '</span>\
                                <span class="bl-leaguename">'+ row.SportEvent.LeagueName + '</span>\
                              </div>\
                          </td>\
                          <td><div class="col60 r ' + (parseFloat(odds)<0 ? 'red':'0') + '">' + odds + (row.OddName!=''? ('<br/><span class="bl_oddtype">&nbsp;' + row.OddName + '</span>'):'') + '</div></td>\
                          <td>' + stake + '</td>\
                          <td><div class="col100 r ' + (parseInt(row.AgentWinloss)<0 ? 'red':'0') + '">' + row.AgentWinloss + '</div></td>\
                          <td><div class="col100 r">' + row.AgentDiscount + '<br/>' + row.AgentComm + '</div></td>\
                          <td><div class="col60 r">' + row.Virtualrate + '<br/>' + row.Virtual + '</div></td>\
                          <td><div class="col100 c">' + row.BettranDetail.Status + '<br /><span class="lnk" onclick="ViewDNS(\'' + row.Ip + '\')">'+ row.Ip + '</span></div></td>\
                          <td><div class="col60 c">' + row.BettranDetail.CurrencyName + '</div></td>\
                        </tr>';
             
               
            arr.push(html);
        }
        return arr.join('');
        
    },
    showMP:function(refNo, transid)
    {
        var obj=$("#mpwarper");
        var param = {
            refNo: refNo
        };
           $.ajax({
                type: "POST",
                url: "Service/BettransService.svc/GetMixParlayTickets",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(param),
                beforeSend:function(xhr){
                    //try { parent.BlockUI(); } catch (e) { }
                },
                success: function (rs) {
                    var len=rs.length;
                    var arr=[];
                    for(var i=0;i<len;i++)
                    {
                        arr.push(LatestBet.renderMP(rs[i]));
                    }
                    obj.empty().html(arr.join(''));
                   
                    obj.removeClass("hidden").dialog( "option", "title", 'Mix Parlay Info Ref.No '  + transid);
                    obj.dialog("open");
                   //try { parent.jQuery.unblockUI(); } catch (e) { }  
                }
            });
     },
     
     showCombinationMP: function(refNo, transid)
     {
        var obj=$("#mpwarper");
        var param = {
            refNo: refNo
        };
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetMixParlayTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function(xhr){
            },
            success: function(rs){
                var len = rs.length;
                var arr = [];
                for(var i = 0; i<len; i++)
                {
                    arr.push(LatestBet.renderMP(rs[i])); 
                }
                obj.empty().html(arr.join(''));
                   
                obj.removeClass("hidden").dialog( "option", "title", 'System Parlay Info Ref.No '  + transid);
                obj.dialog("open");
            }
        });
     },
     
     showCombinationDetailMP: function(refNo, selection)
     {
        try { parent.BlockUI(); } catch (e) { }
        var param = {
            refNo: refNo
        };
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetCombinationDetailMPTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function(xhr){
            },
            success: function(rs){
                var len = rs.length;
                var arr = [];
                for(var i = 0; i<len; i++)
                {
                    arr.push(LatestBet.renderCombinationDetailMP(rs[i])); 
                }
                 
                $("#dialogCombinationDetailMP").dialog( "option", "title", 'System Parlay - '  + selection);
                var len = rs.items.length;
                for(var i = 0; i< len; i++)
                {
                     arr.push(LatestBet.renderCombinationDetailMP(rs.items[i]));
                }
                $("#tblbodyscrollcombination").html(arr.join(''));
                $("#totalWLComm").html(LatestBet.renderTotal(rs.TotalWLComm));
                $("#dialogCombinationDetailMP").removeClass("hidden");
                $("#dialogCombinationDetailMP").dialog("open");

                $("#gridcombination").reRenderScroller(718,600, true); //1033+11*1+11*2
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }

        });
     },
     
     renderCombinationDetailMP: function(row)
     {
        var html = '';
        var event='';
        var len=row.CombinationDetails.length;
        if (len>0)
        {
             //event='<table>';
             for (var i=0;i<len;i++)
                {
                    var detail=row.CombinationDetails[i];
                   //event += '<tr><td style="border:none 0px">' + detail.HomeName + '</td><td style="border:none 0px">-vs-</td><td style="border:none 0px">' + detail.AwayName + '</td></tr>';
                    event += '<div >' + detail.HomeName + ' -vs- ' + detail.AwayName + '</div>'
                }
             //event+='</table>';   
        }
        else
            event = 'Total of losing tickets';
        
         html = '<tr>\
                    <td><div class="col100 c">' + row.TransShortDate + '</div></td>\
                    <td><div class="col200">' + event + '</div></td>\
                    <td><div class="col100 c" style="font-weight:bold">' + row.Odds + '</div></td>\
                    <td><div class="col100 c" style="font-weight:bold">' + row.Stake+ '</div></td>\
                    <td><div class="col100 c" style="font-weight:bold">' + row.AgentWinloss + '</div></td>\
                    <td><div class="col100 c" style="font-weight:bold">' + row.BettranDetail.Status + '</div></td>\
                    </tr>';
          
        return html;
     },
     
     renderOutstanding: function(outStanding)
     {
        var html = '';
        if(parseInt(outStanding) != 0)
             html = '<tr>\
                        <td colspan="4"><div class="r b outstanding" style="width:509px;">Outstanding</div></td>\
                        <td><div class="col100 c b outstanding" >' + outStanding + '</div></td>\
                        <td><div class="col100 outstanding">&nbsp;</div></td>\
                    </tr>'
        return html;
     },
     
     renderTotal: function(TotalWLComm)
     {
        var html = '<tr>\
                        <td colspan="4"><div class="r b" style="width:509px;">Total</div></td>\
                        <td><div class="col100 c b" >' + TotalWLComm + '</div></td>\
                        <td><div class="col100">&nbsp;</div></td>\
                    </tr>'
        return html;
     },
     
     BL_changeOption: function(){
        var rdTransID = obj('rdTransID').checked;
        var rdBettype = obj('rdBetType').checked;
        if (rdTransID == true) {
            obj('dlBettype').disabled = true;
            obj('txtUsername').disabled = true;
            obj('txtfromdate').disabled = true;
            obj('txttodate').disabled = true;
            obj('dlCurrency').disabled = true;
            obj('txtfromstake').disabled = true;
            obj('txttostake').disabled = true;
            obj('cbExclude').disabled = true;
            obj('txtTransID').disabled = false;
            //$(".content").slideUp();
        }
        else {
            obj('dlBettype').disabled = false;
            obj('txtUsername').disabled = false;
            obj('txtfromdate').disabled = false;
            obj('txttodate').disabled = false;
            obj('dlCurrency').disabled = false;
            obj('txtfromstake').disabled = false;
            obj('txttostake').disabled = false;
            obj('cbExclude').disabled = false;
            obj('txtTransID').disabled = true;
            $('#txtTransID').val('');
            //$(".content").slideDown();
        }
        }
};

$(window).load(function () {
     $('#tbllatestbet').delegate('tr','hover', function( event ) {
        $(this).toggleClass("mouseon");  
     });
 
     helper.renderCalendarFromTo("#txtfromdate, #txttodate");
     helper.renderBettype(); 
     helper.renderCurrency();      
     $("#mpwarper").dialog({
        title: 'Mix Parlay Info',
        autoOpen: false,
        height: 300,
        width: 310,
        modal: false
    });
    $("#dialogCombinationDetailMP").dialog({
        title: '',
        autoOpen: false,
        height: 300,
        width: 780, 
        modal: true
    });
     try { parent.jQuery.unblockUI(); } catch (e) { }  
     //LatestBet.mappingData();
});

var helper={
     renderCalendar: function(id, opts) {
         $("#" + id).datepicker(opts);
     },
     renderCalendarFromTo: function(ids) {
         var defaults = {
             onSelect: function(selectedDate) {
                 var option = this.id == "txtfromdate" ? "minDate" : "maxDate",
                     instance = $(this).data("datepicker"),
                     date = $.datepicker.parseDate(
                         instance.settings.dateFormat ||
                             $.datepicker._defaults.dateFormat,
                         selectedDate, instance.settings);
                 dates.not(this).datepicker("option", option, date);
             }
         };
         //var new_opts = $.extend(true, defaults, opts);
         var dates = $(ids).datepicker(defaults);
         var dt = new Date();
         //dt.addDays(-1);
         var fdate=$(ids.split(',')[0]);
         var tdate=$(ids.split(',')[1]);
         fdate.datepicker("setDate", dt);
         //fdate.datepicker("option","maxDate", dt);
         tdate.datepicker("setDate", dt);
         tdate.datepicker("option","minDate",dt);
         //tdate.datepicker("option","maxDate",dt);
     },
     
     renderBettype: function() {
       $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetBettype",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "",
            success: function (rs) {
                var len = rs.length;
                var cbbettype = $('#dlBettype');
                for( var i = 0; i < len; i++){
                     cbbettype.append("<option value='" + rs[i].BettypeId + "'>" + rs[i].BettypeName + "</option>");
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
     },
     
     renderCurrency: function() {
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetCurrency",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "",
            success: function (rs) {
                var len = rs.length;
                var cbcurrency = $('#dlCurrency');
                for( var i = 0; i < len; i++){
                     cbcurrency.append("<option value='" + rs[i].CurrencyId + "'>" + rs[i].CurrencyName + "</option>");
                }
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
     }
};
