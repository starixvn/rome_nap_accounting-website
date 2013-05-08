var systemParlayDetail = {
    viewBetList: function () {
        parent.gotoURL('centerFr', 'BetList/BetList.aspx');
        BlockUI();
    },
    submit: function () {
        $('#errMsg2').html('');
        $('#divcontainer_systemparlay').addClass('hidden');
        $('#systemparlay').addClass('hidden');
        $('#systemparalydetail').addClass('hidden');

        var transid = $("#txtRefNo").val();
        if (transid != "") {
            if (isNaN(transid) == true) {
                showAlertMessage("errMsg2", "Ref No should be a number!", 1);
                $("#txtRefNo").focus();
            }
            else
                systemParlayDetail.viewSystemParlayDetailByRefNo();
        }
        else {
            showAlertMessage("errMsg2", "Please input a Ref No!", 1);
            $("#txtRefNo").focus();
        }
    },
    viewSystemParlayDetailByRefNo: function () {
        parent.BlockUI();
        var transId = $("#txtRefNo").val();
        
        
        //var param = '{"transId":3027306513, "custId":3230192}';
        var param = '{"transId":' + transId + ', "custId":' + custId + '}';
        $.ajax({
            type: 'POST',
            url: '../Services/WCF/BetList/SystemParlayDetail.svc/GetBetListSystemParlayDetailByTransId',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: param,
            success: function (rs) {
                //alert(" rs");

                after_success(rs);

            }, //end of success:
            error: function () {
                showAlertMessage("errMsg2", "There is no information available.", 1);
            },
            complete: function () {
                parent.jQuery.unblockUI();
            }
        }); //end of ajax
    },
    preSubmit: function (e) {
        e = (e) ? e : window.event;
        var key = (e.keyCode) ? e.keyCode : e.which;
        if (key == 13)
            systemParlayDetail.viewSystemParlayDetailByRefNo();
    }
};

function after_success(rs) {
    if (rs && rs.BetList && rs.BetList.length > 0) {
        
        displayMainGrid(rs);
        //begin Combination details
        displayCombinationDetailGrid(rs);
        

    }
    else {
        showAlertMessage("errMsg2", "There is no data.", 1);
    }

}

function displayMainGrid(rs) {
    var rows = [];
    for (var i = 0; i < rs.BetList.length; i++) {
        var j = i + 1;
        var item = rs.BetList[i];
        var trDetail = '<tr valign="top">' +
                            '<td > <div class="col40 c">' + j + '</div></td>' +
                            '<td><div class="col130 c">' + item.TransDate + '</div></td>' +
                            '<td><div class="col280 r">' +
                            item.Choice + '<br/>' +
                            item.Event +
                            '</div></td>' +

                            '<td><div class="col50 r">' + item.Odds + '</div></td>' +
                            '<td><div class="col60 r"> ' + item.Stake + '</div></td>' +
                            '<td><div class="col70 r">' + item.PlayerWinLoss + '</div></td>' +
                            '<td><div class="col90 c">' + item.Status + '</div></td>' +
                            '<td><div class="col70 r">' + item.SuperPT + item.SuperWinLost + item.SuperDiscount + item.SuperComm + '</div></td>' +
                            '<td><div class="col70 r">' + item.MasterPT + item.MasterWinLost + item.MasterDiscount + item.MasterComm + '</div></td>' +
                            '<td><div class="col70 r">' + item.AgentPT + item.AgentWinLost + item.AgentDiscount + item.AgentComm + '</div></td>' +
                            '</tr>';
        rows.push(trDetail);
    }
    $('#tblsystemparlay').empty();
    $('#tblsystemparlay').append(rows.join(''));



    $('#divcontainer_systemparlay').removeClass('hidden');
    $('#systemparlay').removeClass('hidden').reRenderScroller(960, 30000, true);
    //alert('end of tblsystemparlay');

}

function displayCombinationDetailGrid(rs) {
    if (rs && rs.SystemParlayCombinationDetailList && rs.SystemParlayCombinationDetailList.length > 0) {
        var rows2 = [];
        for (var i2 = 0; i2 < rs.SystemParlayCombinationDetailList.length; i2++) {
            var items = rs.SystemParlayCombinationDetailList[i2].Items;
            var totalItems = rs.SystemParlayCombinationDetailList[i2].TotalItems;
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                var wlDateCol = i == 0 ? '<td rowspan="' + totalItems + '"><div class="col80 c">' + item.WinLostDate + '</div></td>' : '';
                var trDetail2 = '<tr>'+
                            wlDateCol +
                            '<td>\
                            <div class="col200 l">' + item.Event + '</div>\
                            </td>\
                            <td>\
                            <div class="col50 r">' + item.Odds + '</div>\
                            </td>\
                            <td>\
                            <div class="col60 r">' + item.Stake + '</div>\
                            </td>\
                            <td>\
                            <div class="col70 r">' + item.WinLoss + '</div>\
                            </td>\
                            <td>\
                            <div class="col70 c">' + item.Status + '</div>\
                            </td>\
                            </tr>';
                rows2.push(trDetail2);
            }

            var subTotalRow = '<tr>\
                                    <td colspan="4"><div class="r b">Sub total&nbsp;</div></td>\
                                    <td><div class="col70 r b">' + rs.SystemParlayCombinationDetailList[i2].SubTotal.WinLoss + '</div></td>\
                                    <td><div class="col70">&nbsp;</div></td>\
                               </tr>';
            rows2.push(subTotalRow);
        }
        $('#tblsystemparlaydetail').empty();
        $('#tblsystemparlaydetail').append(rows2.join(''));



        //$('#divcontainer_systemparlay').removeClass('hidden');
        $('#systemparalydetail').removeClass('hidden').reRenderScroller(548, 300, true);
        //alert('end of tblsystemparlay');
    }
}

//hàm chỉ cho người dùng nhập số
function keypress(e) {
    //Hàm dùng để ngăn người dùng nhập các ký tự khác ký tự số vào TextBox
    var keypressed = null;

    if (window.event) {
        keypressed = window.event.keyCode; //IE
    }
    else {
        keypressed = e.which; //NON-IE, Standard
    }

    if (keypressed == 2 || keypressed == 8 || keypressed == 127 || keypressed == 86 || keypressed == 67) {//2=CTRL,86=V, 67=C, Phím Delete và Phím Back
        return;
    }
    if (keypressed < 48 || keypressed > 57) { //CharCode của 0 là 48 (Theo bảng mã ASCII)
        //CharCode của 9 là 57 (Theo bảng mã ASCII)
        return false;
    }
}

function keydown(e) {
        return true;
}

function ViewCombinationMixParlayResult(refno) {
    var width = 800;
//    if (parent.ibcWnd) {
//        parent.ibcWnd.open('Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', 100, 100, width, 400, 'class1', 'class2')
//    }
//    else {
//        ibcWnd.open('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', 100, 100, width, 400, 'class1', 'class2')
//    }

    OpenPopup('../Report/ViewMixParlayResult.aspx?refno=' + refno, 'System Parlay', width, 500, true, false, window, 0, null, null);
}
function viewSystemParlayTab() {
    //change tab
    $('#Li1').removeClass('current');
    $('#Li2').addClass('current');
    //system parlay display
    $('#maincontent_systemparlay').removeClass('hidden');
    $('#divcontainer_systemparlay').removeClass('hidden');
    //betlist hidden
    $('#maincontent_betlist').addClass('hidden');
    $('#divcontainer').addClass('allhidden');

    
}

function viewBetListTab() {
    //change tab
    $('#Li1').addClass('current');
    $('#Li2').removeClass('current');
    //system parlay hidden
    $('#maincontent_systemparlay').addClass('hidden');
    $('#divcontainer_systemparlay').addClass('hidden');
    //betlist display
    $('#maincontent_betlist').removeClass('hidden');
    $('#divcontainer').removeClass('allhidden');
}

//copy từ file Betlist.js cũ
//<< betlist cũ