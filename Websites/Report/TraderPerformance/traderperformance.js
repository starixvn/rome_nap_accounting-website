var _pageIndex = 1;
var _pageSize = 25;
var _totalPage = 0;
var _totalRows = 0;
var _width = 1200;
var _height = 600;

function getTranderPerformanceReport(actionType) {
    var fromDate = $("#fdate").val();
    var toDate = $("#tdate").val();
    var sortCol = $("#sortColName").val();
    var order = $("#sortType").val();
    var isSoccer = $("#IsSoccer").val();
    if (isSoccer == "1") {
        GetSoccerData(fromDate, toDate, sortCol, order, isSoccer, actionType);
    }
    else {
        GetOtherSportsData(fromDate, toDate, sortCol, isSoccer, actionType);
    }
}

function fortmatColumn() {
    $(".datagrid-view2 .datagrid-body table tr:last td").removeAttr("class");
    $(".datagrid-view1 .datagrid-body .datagrid-body-inner table tr:last td.Use .datagrid-cell ").html("Grand Total");
    $(".datagrid-view2 .datagrid-body table tr:last").attr("style", "font-weight:bold;background-color:#CCFFCC");
    $(".datagrid-view1 .datagrid-body .datagrid-body-inner table tr:last").attr("style", "font-weight:bold;background-color:#CCFFCC");
}

function SwitchViewTrader(object) {
    var soccer = obj('soccer');
    var other = obj('other');
    var IsSoccer = obj('IsSoccer');


    IsSoccer.value = (object.id == 'soccer') ? '1' : '0';

    if (object.id == 'soccer') {
        soccer.className = '';
        soccer.onlick = '';

        other.className = 'lnk';
        other.onlick = 'SwitchViewTrader(this);';
    }
    else {
        soccer.className = 'lnk';
        soccer.onlick = 'SwitchViewTrader(this);';

        other.className = '';
        other.onlick = '';
    }

    DoPostBack();
}

function DoPostBack() {
    //getTranderPerformanceReport(1);
    getTranderPerformanceReport(0);
}

function GetSoccerData(fromDate, toDate, sortCol, order, isSoccer, actionType) {
    var data = {
        'fromDate': fromDate,
        'toDate': toDate,
        'sortingCol': sortCol,
        'order': order,
        'isSoccer': isSoccer,
        'action': actionType,
        'pageIndex': _pageIndex,
        'pageSize': _pageSize
    };
    //try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/TraderPerformanceService.svc/GetTraderPerformance",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $("#dvTraderPerformance").empty();
            _totalRows = rs.rows.length - 1;

            if (rs.rows.length > 0) {
                $("<table id='tblTraderPerformance'></table>").appendTo($("#dvTraderPerformance"));
                $('#tblTraderPerformance').datagrid({
                    width: _width,
                    height: _height,
                    nowrap: false,
                    collapsible: true,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    idFied: 'Trader',
                    frozenColumns: [[
                                    { field: 'UserName', title: 'Trader', width: 100 },
                                    { field: 'MatchNo', title: 'Match', width: 50, align: 'center'
                                    }
                                ]],

                    columns: [[
                                    { title: 'WinLost', colspan: 14 },
                                    { title: 'TurnOver', colspan: 14 },
                                    { title: 'Live BetCount', colspan: 14 },
                                    { title: 'Margin', colspan: 14 },
                                    { title: 'Commission', colspan: 14}],

                                    [
                                    { field: 'WinLost1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'WinLostHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'WinLost1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'WinLostOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'WinLost1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'WinLostNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'WinLost1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'WinLostNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'WinLost15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'WinLost1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'TurnOver1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'TurnOverHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'TurnOverOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'TurnOver1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'TurnOverNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'TurnOver1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'TurnOverNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'TurnOver15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'LiveBetCount1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCountHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'LiveBetCount1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'LiveBetCountNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'LiveBetCount1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'LiveBetCountNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'LiveBetCount15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Margin1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Margin1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Margin1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'MarginHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Margin1stOu', title: '1st O/U', width: 80, align: 'right' },
                                        { field: 'MarginOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'Margin1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'MarginNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'Margin1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'MarginNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'Margin15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'Margin1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'MarginOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'MarginOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Comm1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Comm1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Comm1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'CommHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Comm1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'CommOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'Comm1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'CommNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'Comm1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'CommNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'Comm15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'Comm1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'CommOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'CommOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'UserName', title: 'Trader', width: 100}]
                                 ]
                });
                $('#tblTraderPerformance').datagrid('loadData', rs);

                var pager = $('#tblTraderPerformance').datagrid('getPager');    // get the pager of datagrid
                _totalPage = rs.TotalRows;
                pager.pagination({
                    pageList: [10, 20, 25, 30],
                    total: _totalPage,
                    pageNumber: _pageIndex,
                    pageSize: _pageSize,
                    onSelectPage: function (pageNumber, pageSize) {
                        $(this).pagination('loading');
                        _pageIndex = pageNumber;
                        _pageSize = pageSize;
                        getTranderPerformanceReport('0');
                    }
                });
                fortmatColumn();
            }
            else {
                $('<div id="errmsg" class="errmsg" style="width: 958px;"><div class="alertboxerr"><div>There is no infomation.</div></div></div>').appendTo($("#dvTraderPerformance"));
            }

            //parent.jQuery.unblockUI();
        }
    });
}

function GetOtherSportsData(fromDate, toDate, sortCol, order, isSoccer, actionType) {
    var data = {
        'fromDate': fromDate,
        'toDate': toDate,
        'sortingCol': sortCol,
        'order': order,
        'isSoccer': isSoccer,
        'action': actionType,
        'pageIndex': _pageIndex,
        'pageSize': _pageSize
    };
    //try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/TraderPerformanceService.svc/GetTraderPerformance",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $("#dvTraderPerformance").empty();
            _totalRows = rs.rows.length - 1;

            if (rs.rows.length > 0) {
                $("<table id='tblTraderPerformance'></table>").appendTo($("#dvTraderPerformance"));
                $('#tblTraderPerformance').datagrid({
                    width: _width,
                    height: _height,
                    nowrap: false,
                    collapsible: true,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    idFied: 'Trader',
                    frozenColumns: [[
                                    { field: 'UserName', title: 'Trader', width: 100 },
                                    { field: 'MatchNo', title: 'Match', width: 50, align: 'center'
                                    }
                                ]],

                    columns: [[
                                    { title: 'WinLost', colspan: 9 },
                                    { title: 'TurnOver', colspan: 9 },
                                    { title: 'Live BetCount', colspan: 9 },
                                    { title: 'Margin', colspan: 9 },
                                    { title: 'Commission', colspan: 9}],

                                    [
                                    { field: 'WinLost1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'WinLostHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'WinLost1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'WinLostOu', title: 'O/U', width: 80, align: 'right' },
                                    { field: 'WinLost1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'TurnOver1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'TurnOverHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'TurnOverOu', title: 'O/U', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'LiveBetCount1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCountHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOu', title: 'O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Margin1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Margin1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Margin1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'MarginHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Margin1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'Margin1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'MarginOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'MarginOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Comm1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Comm1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Comm1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'CommHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Comm1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'CommOu', title: 'O/U', width: 80, align: 'right' },
                                    { field: 'Comm1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'CommOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'CommOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'UserName', title: 'Trader', width: 100}]
                                 ]
                });
                $('#tblTraderPerformance').datagrid('loadData', rs);

                var pager = $('#tblTraderPerformance').datagrid('getPager');    // get the pager of datagrid
                _totalPage = rs.TotalRows;
                pager.pagination({
                    pageList: [10, 20, 25, 30],
                    total: _totalPage,
                    pageNumber: _pageIndex,
                    pageSize: _pageSize,
                    onSelectPage: function (pageNumber, pageSize) {
                        $(this).pagination('loading');
                        _pageIndex = pageNumber;
                        _pageSize = pageSize
                        getTranderPerformanceReport('0');
                    }
                });
                fortmatColumn();
            }
            else {
                $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no infomation.</div></div></div>').appendTo($("#dvTraderPerformance"));
            }

            //parent.jQuery.unblockUI();
        }
    });
}


function GetSoccerDataByType(fromDate, toDate, isSoccer, actionType) {
    var type = $('#ddlSelectType').value();
    var data = {
        'fromDate': fromDate,
        'toDate': toDate,
        'isSoccer': isSoccer,
        'action': actionType,
        'type' : type

    };
    //try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "../../Services/WCF/Report/TraderPerformanceService.svc/GetTraderPerformanceByType",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            $("#dvTraderPerformance").empty();
            _totalRows = rs.rows.length - 1;

            if (rs.rows.length > 0) {
                $("<table id='tblTraderPerformance'></table>").appendTo($("#dvTraderPerformance"));
                $('#tblTraderPerformance').datagrid({
                    width: _width,
                    height: _height,
                    nowrap: false,
                    collapsible: true,
                    pagination: true,
                    singleSelect: true,
                    rownumbers: true,
                    idFied: 'Trader',
                    frozenColumns: [[
                                    { field: 'UserName', title: 'Trader', width: 100 },
                                    { field: 'MatchNo', title: 'Match', width: 50, align: 'center'
                                    }
                                ]],

                    columns: [[
                                    { title: 'WinLost', colspan: 14 },
                                    { title: 'TurnOver', colspan: 14 },
                                    { title: 'Live BetCount', colspan: 14 },
                                    { title: 'Margin', colspan: 14 },
                                    { title: 'Commission', colspan: 14}],

                                    [
                                    { field: 'WinLost1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'WinLost1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'WinLostHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'WinLost1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'WinLostOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'WinLost1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'WinLostNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'WinLost1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'WinLostNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'WinLost15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'WinLost1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'WinLostOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'TurnOver1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'TurnOver1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'TurnOverHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'TurnOverOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'TurnOver1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'TurnOverNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'TurnOver1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'TurnOverNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'TurnOver15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'TurnOver1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'TurnOverOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'LiveBetCount1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCountHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'LiveBetCount1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'LiveBetCountNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'LiveBetCount1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'LiveBetCountNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'LiveBetCount15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'LiveBetCount1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'LiveBetCountOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Margin1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Margin1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Margin1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'MarginHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Margin1stOu', title: '1st O/U', width: 80, align: 'right' },
                                        { field: 'MarginOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'Margin1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'MarginNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'Margin1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'MarginNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'Margin15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'Margin1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'MarginOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'MarginOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'Comm1st1x2', title: '1st 1x2', width: 80, align: 'right' },
                                    { field: 'Comm1x2', title: '1x2', width: 80, align: 'right' },
                                    { field: 'Comm1stHdp', title: '1st HDP', width: 80, align: 'right' },
                                    { field: 'CommHdp', title: 'HDP', width: 80, align: 'right' },
                                    { field: 'Comm1stOu', title: '1st O/U', width: 80, align: 'right' },
                                    { field: 'CommOu', title: 'O/U', width: 80, align: 'right' },
                                        { field: 'Comm1stNoOfCorners', title: '1st No. of Corner', width: 80, align: 'right' },
                                        { field: 'CommNoOfCorners', title: 'No. of Corner', width: 80, align: 'right' },
                                        { field: 'Comm1stNoOfYellowCards', title: '1st No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'CommNoOfYellowCards', title: 'No. of Yellow Card', width: 80, align: 'right' },
                                        { field: 'Comm15minOu', title: '15mins O/U', width: 80, align: 'right' },
                                    { field: 'Comm1stOe', title: '1st O/E', width: 80, align: 'right' },
                                    { field: 'CommOe', title: 'O/E', width: 80, align: 'right' },
                                    { field: 'CommOverall', title: 'Overall', width: 80, align: 'right' },

                                    { field: 'UserName', title: 'Trader', width: 100}]
                                 ]
                });
                $('#tblTraderPerformance').datagrid('loadData', rs);

                var pager = $('#tblTraderPerformance').datagrid('getPager');    // get the pager of datagrid
                _totalPage = rs.TotalRows;
                pager.pagination({
                    pageList: [10, 20, 25, 30],
                    total: _totalPage,
                    pageNumber: _pageIndex,
                    pageSize: _pageSize,
                    onSelectPage: function (pageNumber, pageSize) {
                        $(this).pagination('loading');
                        _pageIndex = pageNumber;
                        _pageSize = pageSize
                        getTranderPerformanceReport('0');
                    }
                });
                fortmatColumn();
            }
            else {
                $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no infomation.</div></div></div>').appendTo($("#dvTraderPerformance"));
            }

            //parent.jQuery.unblockUI();
        }
    });
}