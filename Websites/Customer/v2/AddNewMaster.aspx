<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddNewMaster.aspx.vb" Inherits="Customer_V2_AddNewMaster"
    Title="Untitled Page" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../js/common.js"></script>

    <script type="text/javascript" src="../../js/agent/Customer.js?v=<%=AccCommon.Version() %>"></script>

    <script type="text/javascript" src="../../js/agent/EditMaster.js?v=<%=AccCommon.Version() %>"></script>

    <script src="../../js/showhidepopup.js" type="text/javascript"></script>

    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>

    <script type="text/javascript">
        var errMsgMinPT = ""; //alertLessthanMinPT;			

        //declare array that contain id of Super Preset Position dropdowns
        var arrDdl1 = ["listS1HdpD", "listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE", "listS11x2", "listS1CS", "listS1TG", "listS1MP", "listS1OR", "listS1HdpL", "listS1OUL", "listS11stHdpL", "listS11stOUL"];
        var arrDdl2 = ["listS2Hdp", "listS2OU", "listS2OE", "listS2MP", "listS2ML"]; // basketball
        var arrDdl3 = ["listS3Hdp", "listS3OU", "listS3OE", "listS3ML"]; // football
        var arrDdl5 = ["listS5Hdp", "listS5OU", "listS5OE", "listS5ML"]; // tennis
        var arrDdl8 = ["listS8Hdp", "listS8OU", "listS8ML"]; // baseball
        var arrDdl10 = ["listS10Hdp", "listS10OU", "listS10OE", "listS10ML"]; // golf
        var arrDdl11 = ["listS11Hdp", "listS11ML"]; // motosports
        var arrDdl201 = ["listS201OU"]; // Financial
        var arrDdl99 = ["listS99Hdp", "listS99OU", "listS99OE", "listS99ML"]; // othersports
        //declare array that contain id of
        var arr1Super1 = ["listS1OUD", "listS11stHdpD", "listS11stOUD", "listS1OE"];
        var arr2Super1 = ["listS1CS", "listS1TG", "listS1MP", "listS1OR"];
        var arr3Super1 = ["listS1OUL", "listS11stHdpL", "listS11stOUL"];

        var arr1Master1 = ["listM1OUD", "listM11stHdpD", "listM11stOUD", "listM1OE"];
        var arr2Master1 = ["listM1CS", "listM1TG", "listM1MP", "listM1OR"];
        var arr3Master1 = ["listM1OUL", "listM11stHdpL", "listM11stOUL"];

        var arrSuper2 = ["listS2OU", "listS2OE", "listS2MP", "listS2ML"]; // basketball
        var arrSuper3 = ["listS3OU", "listS3OE", "listS3ML"]; // football
        var arrSuper5 = ["listS5OU", "listS5OE", "listS5ML"]; // tennis
        var arrSuper8 = ["listS8OU", "listS8ML"]; // baseball
        var arrSuper10 = ["listS10OU", "listS10OE", "listS10ML"]; // golf
        var arrSuper11 = ["listS11ML"]; // motosports
        var arrSuper201 = ["listS201OU", "listS201OE"]; // financial
        var arr2Super201 = ["listS201OE"]; // financial
        var arrSuper99 = ["listS99OU", "listS99OE", "listS99ML"]; // othersports

        var arrMaster2 = ["listM2OU", "listM2OE", "listM2MP", "listM2ML"]; // basketball
        var arrMaster3 = ["listM3OU", "listM3OE", "listM3ML"]; // football
        var arrMaster5 = ["listM5OU", "listM5OE", "listM5ML"]; // tennis
        var arrMaster8 = ["listM8OU", "listM8ML"]; // baseball
        var arrMaster10 = ["listM10OU", "listM10OE", "listM10ML"]; // golf
        var arrMaster11 = ["listM11ML"]; // motosports
        var arrMaster201 = ["listM201OU", "listM201OE"]; // financial
        var arr2Master201 = ["listM201OE"]; // financial
        var arrMaster99 = ["listM99OU", "listM99OE", "listM99ML"]; // othersports		
        var flag = false;
        function changeDisableFI(obj)
        { }
        function BtnOnClick() {
            var arrAcc;
            var arrPAcc;
            var arrMin;
            
            if (!CheckInfoValue(false)) return false;
            if (!CheckTime()) return false;
            if (!CheckMaxPerBall161()) return false;
            if (!CheckLimitMinMaxBet(1)) return false;

            if (obj("chkCopyAll").checked) //this case for Copy All from Soccer
            {
                PTBlockUI();
                document.fUpdate.submit();
                return true;
            }
            //Begin check min Position taking for Basketball 
            if (!CheckLimitMinMaxBet(2)) return false;

            if (obj("chkCopyBasket").checked) //this case for Copy All from Basket ball
            {
                try {parent.parent.BlockUI(); } catch (e) { }
                document.fUpdate.submit();
                return true;
            }
            //Begin check min Position taking for Football 
            if (!CheckLimitMinMaxBet(3)) return false;

            //Begin check min Position taking for Tennis
            if (!CheckLimitMinMaxBet(5)) return false;

            //Begin check min Position taking for Baseball
            if (!CheckLimitMinMaxBet(8)) return false;

            //Begin check min Position taking for Golf 
            if (!CheckLimitMinMaxBet(10)) return false;

            //Begin check min Position taking for MotoSports
            if (!CheckLimitMinMaxBet(11)) return false;
            if (obj('txtMinbet161')) {
                if (!CheckLimitMinMaxBet(161)) return false;
            }
            //Begin check min Position taking for mixed sports
            if (!CheckLimitMinMaxBet('99_9')) return false;
            //Begin check min Position taking for Financial
            if (obj("txtMinbet201") || obj("txtMaxbet201") || obj("txtMaxPerMatch201")) {
                if (!CheckLimitMinMaxBet(201)) return false;
            }
            //Begin check min Position taking for Other Sports 
            if (!CheckLimitMinMaxBet(99)) return false;
            if (obj('chkdisableHR')) {
                if (!CheckLimitMinMaxBet(151)) return false;
            }
            PTBlockUI();
            document.fUpdate.submit();
            return true;
            
        }				
    </script>

    <%VariableOfScript()%>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC fixfont">
                    Add New Master
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg">
            </div>
        </div>
        <div class="bg_table">

            <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>

            <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>

            <iframe name="InsertData" id="InsertData" style="display: none"></iframe>
            <form id="fUpdate" name="fUpdate" method="post" action="UpdateNewMaster.aspx" target="InsertData">
            <asp:Literal ID="htmlAddMasterContent" runat="server" EnableViewState="false"></asp:Literal>
            <input type="hidden" name="superId" id="superId" value="<%=superId%>" />
            <input type="hidden" name="set" id="set" value="<%=Request("set")%>" />
            <input type="hidden" name="hidSite" id="hidSite" value="<%=Request("site")%>" />
            </form>

            <script type="text/javascript">selectTranferTime();</script>

        </div>
    </div>
    <input type="hidden" name="roleId2" id="roleId2" value="3" />
</body>
</html>
