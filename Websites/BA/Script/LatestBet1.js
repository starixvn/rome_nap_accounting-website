var LatestBet = {
    connected:false,
    container:null,
    Stake:0,
    maxLen: 30,
    item: function(item, action) {
        this.TransId = item.Code;
        this.TransDate = item.TransDate;
        //this.Bettype = item.BettranDetail.Bettype;
        this.RefNo = item.BettranDetail.Refno;
        this.Liveindicator = item.BettranDetail.Liveindicator;

        this.CurrencyName = item.BettranDetail.CurrencyName;
        this.UserName = item.BettranDetail.UserName;

        this.BetTeam = item.SportEvent.Choice;
        this.LiveScore = item.SportEvent.LiveScore;
        this.Hdp = item.SportEvent.Hdp;
        this.BetType = item.SportEvent.BetType;
        this.Event = item.SportEvent.Event;
        this.Odds = item.Odds;
        this.OddName = item.OddName
        this.Stake = item.Stake;
       
        this.Action = action;
        switch (action) {
            case 1: //Insert
                this.IsUpdate = ko.observable(false);
                this.IsInsert = ko.observable(true);
                this.IsDelete = ko.observable(false);
                break;
            case 2: //Update
                this.IsUpdate = ko.observable(true);
                this.IsInsert = ko.observable(false);
                this.IsDelete = ko.observable(false);
                break;
            case 3: //Delete
                this.IsUpdate = ko.observable(false);
                this.IsInsert = ko.observable(false);
                this.IsDelete = ko.observable(true);
                break;
            default:
                this.IsUpdate = ko.observable(false);
                this.IsInsert = ko.observable(false);
                this.IsDelete = ko.observable(false);
                break;
        }
    },
    VM: {
        items: ko.observableArray()
    },
    parseData: function(r) {
        
        var data = r.Data;
        var len = LatestBet.VM.items().length;
        if (r) {
            switch (parseInt(r.Action)) {
                case 1: // Insert
                    if (len >= LatestBet.maxLen) {
                        LatestBet.VM.items.pop();
                    }
                    LatestBet.VM.items.unshift(new LatestBet.item(data, 1));
                    var item = LatestBet.VM.items()[0];
                    LatestBet.container.reRenderScroller(1124, 702, true);
                    var t1 = setTimeout(function() {
                        item.IsInsert(false);
                    }, 3000);
                    break;
                case 2: //Update
                    for (var i = 0; i < len; i++) {
                        if (parseInt(LatestBet.VM.items()[i].TransId) == parseInt(data.Transid)) {
                            var item = LatestBet.VM.items()[i];
                            item.IsUpdate(true);
                            var t1 = setTimeout(function() {
                                item.IsUpdate(false);
                            }, 3000);
                            return;
                        }
                    }
                    break;
                case 3: // delete (response data as TransId)
                    for (var i = 0; i < len; i++) {
                        if (parseInt(LatestBet.VM.items()[i].TransId) == parseInt(data)) {
                            var item=LatestBet.VM.items()[i];
                            item.IsDelete(true);
                            var t1 = setTimeout(function() {
                                LatestBet.VM.items.remove(item);
                                LatestBet.container.reRenderScroller(1124, 702, true);
                            }, 3000);
                            return;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    },
    mappingData: function() {
        var param = {
            totalRow: 30,
            amount: LatestBet.Stake,
            lastTransId:0
        };
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetLatestBABetList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function(xhr) {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function(rs) {
                ko.applyBindings(LatestBet.VM, document.getElementById("latestbetcontent"));
                var len = rs.length;
             
                if (len > 0) {
                    LatestBet.VM.items.removeAll();
                    $("#tblatestbet").empty();
                    for (var i = 0; i < len; i++) {
                        LatestBet.VM.items.push(new LatestBet.item(rs[i], 0));
                    }
                }
                else {
                    $("#tblatestbet").html('<tr><td colspan="8"><div class="c" style="width:1121px;">There is no data.</div></td></tr>');
                }
                var t = setTimeout(function() {
                    LatestBet.container.removeClass("hidden").reRenderScroller(1124, 702, true);
                    if (!LatestBet.connected)
                    {
                        window.setTimeout(function() {
                            var sk = typeof (PSK)==undefined? '' : PSK, maxQ = 100, speedInterval = $.browser.msie ? 5 : 0;
                            try {
                                ConnectNotify({ SK: sk, MaxQ: maxQ, Interval: parseInt(speedInterval) }, function(data) {
                                    LatestBet.parseData(data);
                                });
                            } catch (e) { }
                        });
                        LatestBet.connected=true;
                    }
                    try { parent.jQuery.unblockUI(); } catch (e) { }
                }, 100);
            },
            complete: function() {
            }
        });
    },
    changeStake:function(obj)
    {
        LatestBet.Stake=parseInt($(obj).val());
        Notify.Send(new Filter(3, '>=|' + LatestBet.Stake));
        LatestBet.mappingData();
    },
    initGrid: function() {
        LatestBet.mappingData();
    },
    renderMP: function(row) {
        var html = '<div><span class="bl-choice b" >' + row.SportEvent.Choice + '</span>\
                    <span class="bl-choice b">' + row.SportEvent.Hdp + " @" + row.Odds + '</span>\
                    <span class="bl-bettype">' + row.SportEvent.LiveScore + '</span>\
                    <span class="b">' + row.SportEvent.BetType + '</span>\
                    <span >' + row.SportEvent.Event + '</span>\
                    <span class="bl-sporttype" >' + row.SportEvent.SportType + '</span>\
                    <span class="bl-leaguename">' + row.SportEvent.LeagueName + '</span></div>';
        return html;
    },
    showMP: function(refNo, transid) {
        var obj = $("#mpwarper");
        var param = {
            refNo: refNo
        };
        $.ajax({
            type: "POST",
            url: "Service/BettransService.svc/GetMixParlayTickets",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(param),
            beforeSend: function(xhr) {
                try { parent.BlockUI(); } catch (e) { }
            },
            success: function(rs) {
                var len = rs.length;
                var arr = [];
                for (var i = 0; i < len; i++) {
                    arr.push(LatestBet.renderMP(rs[i]));
                }
                obj.empty().html(arr.join(''));
                obj.removeClass("hidden").dialog("option", "title", 'Mix Parlay Info Ref.No ' + transid);
                obj.dialog("open");
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    }
};

$(window).load(function () {
      $("#mpwarper").dialog({
        title: 'Mix Parlay Info',
        autoOpen: false,
        height: 300,
        width: 300,
        modal: false
    });
     LatestBet.container= $('#latestbet');    
     try { parent.jQuery.unblockUI(); } catch (e) { }  
     LatestBet.initGrid();
});

