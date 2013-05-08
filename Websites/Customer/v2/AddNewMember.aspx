<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AddNewMember.aspx.vb" Inherits="AddNewMember" %>
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
    <script type="text/javascript" src="../../js/Customer.js"></script>
    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=AccCommon.Version() %>" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript">
       
        function IncreasePT(ddl, step) {
            var max = 0.9;
            if (obj(ddl) || obj(ddl) != 'undefined') {
                max = obj(ddl).options[0].value;
            }
            Increase(ddl, step, max);
        }

        function ReducePT(ddl, step) {
            var min = obj(ddl).options[obj(ddl).length - 1].value;
            Reduce(ddl, step, min);
        }

        var arr1Super1 = ['listM1OUD', 'listM11stHdpD', 'listM11stOUD', 'listM1OE'];
        var arr1Master1 = [];
        var arr2Super1 = ['listM1CS', 'listM1TG', 'listM1MP', 'listM1OR'];
        var arr2Master1 = [];
        var arr3Super1 = ['listM1OUL', 'listM11stHdpL', 'listM11stOUL'];
        var arr3Master1 = [];
        var arrSuper2 = ['listM2OU', 'listM2OE', 'listM2MP', 'listM2ML'];
        var arrMaster2 = [];
        var arrchk = ['listM1HdpL', 'listM2Hdp', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'chkCopy11Hdp', 'listM11x2', 'chkCopyBasket', 'txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'listM201OU', 'listM201OE', 'chkAuto201OE', 'txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201', 'txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151', 'chkCopy201OU'];
        var arrchkBaskball = ['chkCopyAll', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'listM201OU', 'listM201OE', 'chkAuto201OE', 'txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201', 'txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151', 'chkCopy201OU'];
        var arrSuper3 = ['listM3OU', 'listM3OE', 'listM3ML'];
        var arrMaster3 = [];
        var arrSuper5 = ['listM5OU', 'listM5OE', 'listM5ML'];
        var arrMaster5 = [];
        var arrSuper10 = ['listM10OU', 'listM10OE', 'listM10ML'];
        var arrMaster10 = [];
        var arrSuper99 = ['listM99OU', 'listM99OE', 'listM99ML'];
        var arrMaster99 = [];
        var arrSuper8 = ['listM8OU', 'listM8ML'];
        var arrMaster8 = [];
        var arrSuper11 = ['listM11ML'];
        var arrMaster11 = [];
        var arrSuper201 = ['listM201OU', 'listM201OE'];
        var arrMaster201 = [];
        var arr2Super201 = ['listM201OE']
        var arr2Master201 = []
        //chkCopy : checkbox copy all
        //arrSuper: array ids of PT
        //chk: checkbox copy from Handicap
        function CheckCopyPT(chkCopy, arrSuper, chk) {
            var c = document.getElementById(chk);
            if (!c) chk = 0;
            for (var i = 0; i < arrSuper.length; i++) {
                if (obj(arrSuper[i]) != null) {
                    obj(arrSuper[i]).disabled = (chkCopy.checked) ? true : ((!chk) ? false : ((obj(chk).checked) ? true : false));
                }
            }
        }

        //Copy all position taking settings from Soccer Handicap
        function CopyAllPT(chkCopyAll) {
            if (chkCopyAll.checked) {
                obj("panelSoccer").className = 'show';
            }
            for (i = 0; i < arrchk.length; i++) {
                if (obj(arrchk[i])) {
                    obj(arrchk[i]).disabled = chkCopyAll.checked;
                }
            }
            CheckCopyPT(chkCopyAll, arr1Super1, 'chkCopy1HdpDeadball');
            CheckCopyPT(chkCopyAll, arr2Super1, 'chkCopy11x2');
            CheckCopyPT(chkCopyAll, arr3Super1, 'chkCopy1HdpLive');
            CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');
            CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
            CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
            CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
            CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
            CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
            CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
            CheckCopyPT(chkCopyAll, arr2Super201, 'chkCopy201OU');
            //CheckCopyPT(chkCopyAll, arrSuper12, 'chkCopy12Hdp');
            //disable161DDL(chkCopyAll);
        }

        //Copy all position taking settings from Basketball Handicap
        function CopyBasketPT(chkCopyAll) {
            if (chkCopyAll.checked) {
                obj("panelBasket").className = 'show';
            }
            for (i = 0; i < arrchkBaskball.length; i++) {
                if (obj(arrchkBaskball[i])) {
                    obj(arrchkBaskball[i]).disabled = chkCopyAll.checked;
                }
            }
            CheckCopyPT(chkCopyAll, arrSuper2, 'chkCopy2Hdp');
            CheckCopyPT(chkCopyAll, arrSuper3, 'chkCopy3Hdp');
            CheckCopyPT(chkCopyAll, arrSuper5, 'chkCopy5Hdp');
            CheckCopyPT(chkCopyAll, arrSuper10, 'chkCopy10Hdp');
            CheckCopyPT(chkCopyAll, arrSuper99, 'chkCopy99Hdp');
            CheckCopyPT(chkCopyAll, arrSuper8, 'chkCopy8Hdp');
            CheckCopyPT(chkCopyAll, arrSuper11, 'chkCopy11Hdp');
            CheckCopyPT(chkCopyAll, arr2Super201, 'chkCopy201OU');
            //CheckCopyPT(chkCopyAll, arrSuper12, 'chkCopy12Hdp');
            //disable161DDL(chkCopyAll);
        }
        var flag = false;
        function BtnOnClick() {
            ClearAllMsg();
            if (!CheckInfoValue(false)) return false;
            if (!CheckMaxPerBall161()) return false;
            if (!CheckLimitMinMaxBet(1)) return false;
            if (!obj("chkCopyAll").checked) {
                if (!CheckLimitMinMaxBet(2)) return false;
                if (!obj("chkCopyBasket").checked) {
                    if (!CheckLimitMinMaxBet(3)) return false;
                    if (!CheckLimitMinMaxBet(5)) return false;

                    if (!CheckLimitMinMaxBet(8)) return false;
                    if (!CheckLimitMinMaxBet(10)) return false;
                    if (!CheckLimitMinMaxBet(11)) return false;

                    if (obj('txtMinbet161')) {
                        if (!CheckLimitMinMaxBet(161)) return false;
                    }
                    if (!CheckLimitMinMaxBet('99_9')) return false;
                    if (obj("txtMinbet201") || obj("txtMaxbet201") || obj("txtMaxPerMatch201")) {
                        if (!CheckLimitMinMaxBet(201)) return false;
                    }
                    if (!CheckLimitMinMaxBet(99)) return false;
                    if (obj("chkdisableHR")) {
                        if (!CheckLimitMinMaxBet(151)) return false;
                    }
                }
            }
            PTBlockUI();
            document.fUpdate.submit();
            return true;
        }
    </script>

</head>
<body >
     <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC fixfont">
                    Add New Member
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg"></div>
        </div>
        <div class="bg_table">
             <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>
            <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>
            <iframe name="InsertData" id="InsertData" style="display: none"></iframe>
            <form id="fUpdate" name="fUpdate" method="post" action="UpdateNewMember.aspx?agentID=<%=agentID %>"
            target="InsertData">
            <input id="lblPasswordRequire" type="hidden" runat="server" name="lblPasswordRequire" />
            <%=content.ToString()%>
            
            </form>
        </div>
     </div>
  <script language="javascript" type="text/javascript">
      roleId = 1;
  </script>
</body>
</html>
