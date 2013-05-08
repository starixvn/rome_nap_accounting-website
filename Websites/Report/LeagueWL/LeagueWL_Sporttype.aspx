<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeagueWL_Sporttype.aspx.cs"
    Inherits="DailyWL_Sporttype" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-1.3.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.blockUI.1.33.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.jscrollable.js" type="text/javascript"></script>
    <script src="../../js/jQuery/scrolltable/webtoolkit.scrollabletable.js" type="text/javascript"></script>
    <script src="../../js/Report/LeagueWL/LeagueWL.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/commonIOM.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../../App_Themes/Public/input.css" />
    <link href="../../App_Themes/Report/Report.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ViewReport() {
            $('#IsExport').val('0');
            var param = 'mode=1';
            var type = $('#type').val();
            switch (type) {
                case '1':
                    GetLeagueWLSport(param, 1);
                    break;
                case '2':
                    var sporttype = $('#sporttype').val();
                    var leagueid = $('#leagueid').val();
                    var name = $('#sport_league_name').val();
                    GetLeagueWLBettype(sporttype, leagueid, name);
                    break;
                default:
                    GetLeagueWLSport(param, 1);
                    break;
            }
            $('#tdLine').attr('bgcolor', '#E6E9F0');
        }
        function ExportToExcel() {
            $('#IsExport').val('1');
            //alert($('#sContentSub').html());
            if ($('#sContentSub').html().length > 0)
                $('#type').val('2');
            $('#form').submit();
        }

        
    </script>
</head>
<body>
    <img id="displayBox" src="../../App_Themes/Images/waiting7.gif" alt="" style="display: none;" />
    <form id="form" action="LeagueWL_Sporttype.aspx" method="post">
    <input type="hidden" id="IsExport" name="IsExport" value="1" />
    <input type="hidden" id="type" name="type" value="1" />
    <input type="hidden" id="sporttype" name="sporttype" value="" />
    <input type="hidden" id="leagueid" name="leagueid" value="" />
    <input type="hidden" id="sport_league_name" name="sport_league_name" value="" />
    <div class="main">
        <div id="divMainHeader" class="">
            <div id="left_tab1">
                <ul>
                    <li class="current"><span>League Win Lost</span></li>
                </ul>
            </div>
        </div>
        <div class="maincontent">
            <asp:literal id="headerDatetime" runat="server"></asp:literal>
        </div>
        <div style="margin-top: 2px">
            <div id="sErrMsg" class="error-message" style="font-weight: bold">
            </div>
            <div style="width: 1460px;">
                <div id="sContent" style="float: left; width: 760px; margin-right: 10px; margin-bottom: 6px;">
                    <%=sContent%></div>
                <div id="sContentSub" style="float: left;">
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        InitFixedHeaderLeagueWL(800, 1);
    </script>
    </form>
</body>
</html>
