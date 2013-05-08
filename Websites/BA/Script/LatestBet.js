var LatestBet = {
    timeOut:null,
    isAuto:true,
    duration:2000,
    lastBet:30,
    Stake:0,
    lastTransId:0,
    maxItem:1000,
    isFirstLoad:true,
    item: function(item) {
        this.TransId = item.Code;
        this.TransDate = item.TransDate;
        this.BetType = item.SportEvent.BetType;
        this.RefNo = item.BettranDetail.Refno;
        this.Liveindicator = item.BettranDetail.Liveindicator;

        this.CurrencyName = item.BettranDetail.CurrencyName;
        this.UserName = item.BettranDetail.UserName;

        this.BetTeam = item.SportEvent.Choice;
        this.LiveScore = item.SportEvent.LiveScore;
        this.Hdp = item.SportEvent.Hdp;
        this.Event = item.SportEvent.Event;
        this.Odds = item.Odds;
        this.OddName = item.OddName
        this.Stake = item.Stake;
    },
    VM: {
        items: ko.observableArray()
    },
    mappingData: function () {
        var param = {
            totalRow: LatestBet.lastBet,
            amount:LatestBet.Stake,
            lastTransId:LatestBet.lastTransId
        };
        LatestBet.isAuto=false;
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetLatestBABetList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend:function(xhr){
                $(".loader").removeClass("hidden");
            },
            success: function (rs) {
                var len=rs.length;
                if (LatestBet.isFirstLoad)
                {
                    ko.applyBindings(LatestBet.VM, document.getElementById("latestbetcontent"));
                }
               
                if (len > 0) {
                    var total=LatestBet.VM.items().length;
                    var balance= (len + total) - LatestBet.maxItem;
                    LatestBet.lastTransId=parseInt(rs[0].Code);
                     for (var j=0;j<balance;j++)
                     {
                        LatestBet.VM.items.pop();
                     }
                     if (LatestBet.isFirstLoad)
                     {
                         for (var i = 0; i < len; i++) {
                                LatestBet.VM.items.push(new LatestBet.item(rs[i]));
                         }
                         LatestBet.isFirstLoad=false;
                     }
                     else
                     {
                        rs.reverse();
                        for (var i = 0; i < len; i++) {
                             LatestBet.VM.items.unshift(new LatestBet.item(rs[i]));
                         }
                     }
                }
                
              
                $('#latestbet').removeClass("hidden").reRenderScroller(1124, 702, true);
                 var t=setTimeout(function(){
                         //Auto Refresh
                         LatestBet.isAuto=$("#chkAutoRefresh").attr("checked");
                         if (LatestBet.isAuto)
                         {
                                LatestBet.timeOut=setTimeout(function(){
                                    LatestBet.mappingData();
                                },LatestBet.duration);
                         }
                    },100);
            },
            complete: function () {
               $(".loader").addClass("hidden");
            }
        });
    },
    changeStake:function(obj)
    {
        clearTimeout(LatestBet.timeOut);
        LatestBet.Stake=parseInt($(obj).val());
        LatestBet.mappingData();
    },
    changeDuration:function(obj)
    {
        LatestBet.duration=parseInt($(obj).val());
    },
    changeDisplay:function(obj)
    {
         clearTimeout(LatestBet.timeOut);
         LatestBet.lastBet=parseInt($(obj).val());
         LatestBet.mappingData();
    },
    ToggleAutoRefresh:function(obj){
         LatestBet.isAuto=$("#chkAutoRefresh").attr("checked");
             if (LatestBet.isAuto)
             {
                    LatestBet.timeOut=setTimeout(function(){
                        LatestBet.mappingData();
                    },LatestBet.duration);
             }
             else
             {
                clearTimeout(LatestBet.timeOut);
             }
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
    manualRefresh:function(){
         clearTimeout(LatestBet.timeOut);
         LatestBet.mappingData();
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
     
     showCombinationMP: function(refNo, transid){
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
     }
};

$(window).load(function () {
     $('#tbllatestbet').delegate('tr','hover', function( event ) {
        $(this).toggleClass("mouseon");  
     });

     $("#mpwarper").dialog({
        title: 'Mix Parlay Info',
        autoOpen: false,
        height: 300,
        width: 300,
        modal: false
    });
     try { parent.jQuery.unblockUI(); } catch (e) { }  
     LatestBet.mappingData();
});

