
var betList={
    pageIndex:1,
    pageSize:50,
    totalItem:1000,
    reportTitle:'',
    testmapptingData:function(matchId, licenseeId, licenseeName){
        $("#tickestListing").addClass("hidden");
       
        betList.totalItem = parseInt(data.totalItem);
            if (data.Items.length > 0) {
                $("#tblbodybetlist").html(betList.renderListRows(data.Items,betList.pageSize,betList.pageIndex));
                //Render Pager
                betList.initPager(betList.totalItem, betList.pageSize, betList.pageIndex-1);
            }
            else
            {
                $("#pagination").html('&nbsp;'); 
                $("#tblbodybetlist").html('<tr><td colspan="9"><div class="c" style="width:984px;">There is no data.</div></td></tr>');
            }
             betList.reportTitle="Bet List - Match ID: " + matchId + " - Licensee: " + licenseeName;
        $("#gridbetlist #gridtitle").html(betList.reportTitle);
           $('#gridbetlist').removeClass("hidden").reRenderScroller(987, 580, true); 
    },
    mappingData: function () {
        $("#tickestListing").addClass("hidden");
        var param = {
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
                    betList.totalItem = parseInt(rs.totalItem);
                    if (rs.Items.length > 0) {
                        $("#tblbodybetlist").html(betList.renderListRows(rs.Items,LatestBet.pageSize,LatestBet.pageIndex));
                        //Render Pager
                        betList.initPager(betList.totalItem, betList.pageSize, betList.pageIndex-1);
                    }
                    else
                    {
                        $("#pagination").html('&nbsp;'); 
                        $("#tblbodybetlist").html('<tr><td colspan="9"><div class="c" style="width:984px;">There is no data.</div></td></tr>');
                    }
                    $("#tickestListing").addClass("hidden");
                    betList.reportTitle="Bet List - Match ID: " + matchId + " - Licensee: " + licenseeName;
                   $('#gridbetlist').removeClass("hidden").reRenderScroller(987, 580, true);  
                }
                else
                {
                    $("#pagination").html('&nbsp;'); 
                    $("#tblbodybetlist").html('<tr><td colspan="9"><div class="c" style="width:984px;">There is no data.</div></td></tr>');
                    $('#gridbetlist').removeClass("hidden").reRenderScroller(987, 580, true);  
                }
            },
            complete: function () {
               try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    initPager: function (total_item, items_per_page, current_page) {
        var opt = { 
            start_per_page:50,
            step_per_page:50,
            max_items_per_page:500,
            item_per_page_arr:[50,100,200,300,400,500],
            callback: betList.pageSelectCallback };
        opt["items_per_page"] = items_per_page;
        opt["current_page"] = current_page;
        $("#pagination").pagination(total_item, opt);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        var max_page = Math.ceil(betList.totalItem / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        betList.pageIndex = page_index + 1;
        betList.pageSize = _items_per_page;
        betList.mappingData();
        return false;
    },
    goBack:function(){
        $('#gridbetlist').addClass("hidden")
        $("#tickestListing").removeClass("hidden");
    },
    exportExcel:function(){
    
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
            var html = '<tr class="' + (row.BettranDetail.Liveindicator ? 'event-live':'') + '" >\
                          <td><div class="col20 c">' + index  + '</td>\
                          <td><div class="col120 c"><b>Ref No: ' + row.Transid + '</b><br/><span class="bl_time">' + row.TransDate + '</span></div></td>\
                          <td><div class="col100 c">' + row.Username + '</div></td>\
                          <td>\
                            <div class="col360 r">\
                                <span class="bl-choice b">' + row.SportEvent.Choice+ '</span>\
                                <span class="bl-choice b">' + row.SportEvent.Hdp + '</span>\
                                <span class="bl-bettype">' + row.SportEvent.BetType + '</span>\
                              </div>\
                          </td>\
                          <td><div class="col60 r ' + (parseFloat(odds)<0 ? 'red':'0') + '">' + odds + (row.OddName!=''? ('<br/><span class="bl_oddtype">&nbsp;' + row.OddName + '</span>'):'') + '</div></td>\
                          <td>' + stake + '</td>\
                          <td><div class="col80 r bl-choice b">' + row.PlayerWinloss + '</div></td>\
                          <td><div class="col60 c">' + row.BettranDetail.CurrencyName + '</div></td>\
                          <td><div class="col100 c">' + row.BettranDetail.Status + '<br /><span class="lnk" onclick="ViewDNS(\'' + row.Ip + '\')">'+ row.Ip + '</span></div></td>\
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
     }
};