<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeFile="MatchWL.aspx.cs"
    Inherits="Report_MatchWL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Resources.Language.lblAccountingSystem%></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../../js/jQuery/json2.js" type="text/javascript"></script>
    <script src="js/MatchWL.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/BlockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        var currpage = 1;
        var IsFirst = true;

        function LoadingMatchWL() {


            var data = {
                'wldate': $('#fdate').val(),
                'currpage': currpage
            };

            $.ajax({
                type: "POST",
                url: "../Service/ReportService.svc/SelMatchWL2a",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (rs) {
                    $.tmpl(tmplMatchWLContent, rs.rows).appendTo("#tbodyContentMatchWL");
                    if (!rs.IsFinish || rs.IsFinish && currpage == 1) {
                        currpage = rs.currpage;

                        if (!rs.IsFinish)
                            LoadingMatchWL();

                        if (IsFirst) {
                            $('#btnExport').show();
                            $('.wrapper').show();
                            $(".tblbody").css("width", ($("#headerWLbyBettype").width() + 18) + "px");
                            $(".tblbodyMatchWL").css("width", ($("#headerMatchWL").width() + 18) + "px");

                            $.tmpl(tmplWL8RM, rs).appendTo("#tfooterContent");
                            $.tmpl(tmplMatchWLContent, rs.rows3rd).appendTo("#tfooterContent");
                            $.tmpl(tmplMatchWLContent, rs.rowsMatchWLTotal).appendTo("#tblfooterMatchWL");
                            IsFirst = false;

                            $.tmpl(tmplWLbyBettype, rs.rowsWLbyBettype).appendTo("#tbodyContentWLbyBettype");
                            $.tmpl(tmplWLbyBettype, rs.rowsWLbyBettypeTotal).appendTo("#footerWLbyBettype");

                        }

                        if ($(".tblbodyMatchWL").height() > 650)
                            $(".tblbodyMatchWL").css("height", "650px");

                        if ($(".tblbody").height() > 400)
                            $(".tblbody").css("height", "400px");

                    }
                    jQuery.unblockUI();
                }
            });
        }

        function ViewReport() {
            BlockUI();
            onLoad();

            IsFirst = true;
            $('#tbodyContentMatchWL').html('');
            $('#tfooterContent').html('');
            $('#tbodyContentWLbyBettype').html('');
            $('#footerWLbyBettype').html('');
            $('#tblfooterMatchWL').html('');
            currpage = 1;

            $('.wrapper').hide();
            LoadingMatchWL();
        }

        function onLoad() {
            $('#trGetData').hide();
            $('#btnExport').hide();
        }

        function ExportToExcel() {
            try { BlockUI(); } catch (e) { }
            $("#frm").attr('src', 'MatchWL.aspx?IsExport=1&fdate=' + $('#fdate').val());

            jQuery.unblockUI();
        }
    </script>
</head>
<body>
    <form id="form" name="form" method="post">
    <div class="main">
        <div id="divMainHeader" class="">
            <div id="left_tab1">
                <ul>
                    <li class="current"><span>M<%=Resources.Language.lblMathWinloss %></span></li>
                </ul>
            </div>
        </div>
        <div class="maincontent">
            <div id="headerDatetime" runat="server">
            </div>
        </div>
        <div class="bg_table">
            <div class="wrapper" style="display: none">
                <div class="tblheader">
                    <table id="headerWLbyBettype" class="tblfixed">
                        <thead>
                            <tr class="title">
                                <th id="bt2bettype" rowspan="2" class="w120">
                                    Bet Type
                                </th>
                                <th id="Th12" colspan="2">
                                    Soccer Live
                                </th>
                                <th id="Th13" colspan="2">
                                    Soccer Non-Live
                                </th>
                                <th id="Th14" colspan="2">
                                    Other Live
                                </th>
                                <th id="Th15" colspan="2">
                                    Other Non-Live
                                </th>
                                <th id="Th16" colspan="2">
                                    Line Total
                                </th>
                                <th id="bt2variance" rowspan="2" class="w80">
                                    Variance
                                </th>
                            </tr>
                            <tr class="title">
                                <th id="bt21" class="w80">
                                    Real
                                </th>
                                <th id="bt22" class="w80">
                                    Virtual
                                </th>
                                <th id="bt23" class="w80">
                                    Real
                                </th>
                                <th id="bt24" class="w80">
                                    Virtual
                                </th>
                                <th id="bt25" class="w80">
                                    Real
                                </th>
                                <th id="bt26" class="w80">
                                    Virtual
                                </th>
                                <th id="bt27" class="w80">
                                    Real
                                </th>
                                <th id="bt28" class="w80">
                                    Virtual
                                </th>
                                <th id="bt29" class="w80">
                                    Real
                                </th>
                                <th id="bt210" class="w80">
                                    Virtual
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="tblbody" style="overflow: auto;">
                    <table id="table1" class="tblfixed">
                        <tbody id="tbodyContentWLbyBettype">
                        </tbody>
                    </table>
                </div>
                <div class="tblfooter">
                    <table id="footerWLbyBettype" class="tblfixed">
                    </table>
                </div>
                <p>
                </p>
                <div class="wrapper" style="display: none">
                    <div class="tblheader">
                        <table id="headerMatchWL" class="tblfixed" style="width: 1167px;">
                            <thead>
                                <tr class="title">
                                    <th style="width: 120px" id="Th1">
                                        Bet Type
                                    </th>
                                    <th style="width: 80px" id="Th2">
                                        Date
                                    </th>
                                    <th style="width: 306px" id="Th3" colspan="3">
                                        Match
                                    </th>
                                    <th style="width: 90px" id="Th4">
                                        Winloss
                                    </th>
                                    <th class="w90" id="Th5">
                                        Position
                                    </th>
                                    <th class="w90" id="Th6">
                                        Comm
                                    </th>
                                    <th class="w90" id="Th7">
                                        Virtual
                                        <br />
                                        Comm
                                    </th>
                                    <th class="w90" id="Th8">
                                        Line Total
                                    </th>
                                    <th class="w90" id="Th9">
                                        Virtual
                                        <br />
                                        Line Total
                                    </th>
                                    <th class="w90" id="Th10">
                                        Variance
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="tblbodyMatchWL" style="overflow: auto;">
                        <table id="tbldata" class="tblfixed" style="width: 1167px;">
                            <tbody id="tbodyContentMatchWL">
                            </tbody>
                            <tfoot id="tfooterContent">
                            </tfoot>
                        </table>
                    </div>
                    <div class="tblfooter">
                        <table id="tblfooterMatchWL" class="tblfixed" style="width: 1167px;">
                        </table>
                    </div>
                </div>
                <input type="hidden" id="Export" name="Export" />
            </div>
        </div>
        <iframe id="frm" width="0" height="0" frameborder="0" style="display: none"></iframe>
        <script type="text/javascript">
            onLoad();
        </script>
    </div>
    </form>
</body>
</html>
