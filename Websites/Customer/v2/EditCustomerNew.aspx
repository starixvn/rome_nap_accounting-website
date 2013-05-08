<%@ Page Language="vb" AutoEventWireup="true" EnableViewState="false" CodeFile="EditCustomerNew.aspx.vb"
    Inherits="EditCustomerNew" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />

    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>

    <script type="text/javascript" src="../../js/common.js"></script>

    <script type="text/javascript" src="../../js/Customer.js"></script>
    <script src="../../js/PT/NumberGame/NumberGamePT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/PT/MixedSports/MixedSportsPT.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        roleId = 1;
            function BtnOnClick() {
                ClearAllMsg();
                if (!CheckInfoValue(true)) return false;
                if (!CheckMaxPerBall161()) return false;

                if (!CheckLimitMinMaxBet(1)) return false;
                if (obj("isMult").value == "1" || !obj("chkCopyAll").checked) {
                    if (!CheckLimitMinMaxBet(2)) return false;
                    if (obj("isMult").value == "1" || !obj("chkCopyBasket").checked) {
                        if (!CheckLimitMinMaxBet(3)) return false;
                        if (!CheckLimitMinMaxBet(5)) return false;

                        if (!CheckLimitMinMaxBet(8)) return false;
                        if (!CheckLimitMinMaxBet(10)) return false;
                        if (!CheckLimitMinMaxBet(11)) return false;

                        if (obj('txtMinbet161')) {
                            if (!CheckLimitMinMaxBet(161)) return false;
                        }
                        if (obj("txtMinbet201") || obj("txtMaxbet201") || obj("txtMaxPerMatch201")) {
                            if (!CheckLimitMinMaxBet(201)) return false;
                        }
                        if (!CheckLimitMinMaxBet(99)) return false;
                        if (!CheckLimitMinMaxBet('99_9')) return false;

                        if (obj("chkdisableHR")) {
                            if (!CheckLimitMinMaxBet(151)) return false;
                        }
                    }
                }
                PTBlockUI();
                document.fUpdate.submit();
                return true;
            }
            
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
        var arrchk = ['listM1HdpL', 'listM2Hdp', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'chkCopy11Hdp', 'listM11x2', 'chkCopyBasket', 'txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'listM201OU', 'txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201', 'txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151', 'chkAuto201OE', 'chkCopy201OU']; //Add new PT bet type
        var arrchkBaskball = ['chkCopyAll', 'listM3Hdp', 'listM5Hdp', 'listM10Hdp', 'chkCopy10Hdp', 'listM99Hdp', 'listM8Hdp', 'listM11Hdp', 'chkCopy2Hdp', 'chkCopy3Hdp', 'chkCopy5Hdp', 'chkCopy10Hdp', 'chkCopy99Hdp', 'chkCopy8Hdp', 'txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3', 'txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5', 'txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8', 'txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10', 'txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11', 'txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99', 'listM201OU', 'txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201', 'txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151', 'chkAuto201OE', 'chkCopy201OU']; //Add new bet type for PT
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
                var objSuper = obj(arrSuper[i]);
                if (objSuper != null)
                    objSuper.disabled = (chkCopy.checked) ? true : ((!chk) ? false : ((obj(chk).checked) ? true : false));
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
            CheckCopyPT(chkCopyAll, arr2Super201, 'chkCopy201OU'); //Finance, Odd/Even copy from Over/Under
            //CheckCopyPT(chkCopyAll, arrSuper12, 'chkCopy12Hdp');
            //disable161DDL(chkCopyAll);
        }

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
            CheckCopyPT(chkCopyAll, arr2Super201, 'chkCopy201OU'); //Finance, Odd/Even copy from Over/Under
            //CheckCopyPT(chkCopyAll, arrSuper12, 'chkCopy12Hdp');
            //disable161DDL(chkCopyAll);
        }
      


            var arrComm = ['ddrCommissionH_OU_OE', 'ddrCommissionCS_TG'];

            var arrSoccerBet = ['txtMinbet1', 'txtMaxbet1', 'txtMaxPerMatch1'];
            var arrSoccerPT = ['listM1HdpD', 'listM11x2', 'listM1HdpL', 'chkCopy1HdpDeadball', 'chkCopy11x2', 'chkCopy1HdpLive'];

            var arrBasketballBet = ['txtMinbet2', 'txtMaxbet2', 'txtMaxPerMatch2'];
            var arrBasketballPT = ['listM2Hdp', 'chkCopy2Hdp'];

            var arrFootballBet = ['txtMinbet3', 'txtMaxbet3', 'txtMaxPerMatch3'];
            var arrFootballPT = ['listM3Hdp', 'chkCopy3Hdp'];

            var arrTennisBet = ['txtMinbet5', 'txtMaxbet5', 'txtMaxPerMatch5'];
            var arrTennisPT = ['listM5Hdp', 'chkCopy5Hdp'];

            var arrBaseballBet = ['txtMinbet8', 'txtMaxbet8', 'txtMaxPerMatch8'];
            var arrBaseballPT = ['listM8Hdp', 'chkCopy8Hdp'];

            var arrGolfBet = ['txtMinbet10', 'txtMaxbet10', 'txtMaxPerMatch10'];
            var arrGolfPT = ['listM10Hdp', 'chkCopy10Hdp'];

            var arrMotoBet = ['txtMinbet11', 'txtMaxbet11', 'txtMaxPerMatch11'];
            var arrMotoPT = ['listM11Hdp', 'chkCopy11Hdp'];

            var arrFinancialBet = ['txtMinbet201', 'txtMaxbet201', 'txtMaxPerMatch201'];
            var arrFinancialPT = ['listM201OU'];

            var arrHorseRacingBet = ['txtMinbet151', 'txtMaxbet151', 'txtMaxPerMatch151'];
            var arrHorseRacingPT = [];

            var arrOtherBet = ['txtMinbet99', 'txtMaxbet99', 'txtMaxPerMatch99'];
            var arrOtherPT = ['listM99Hdp', 'chkCopy99Hdp'];

            function AllowUpdate(chkObj, isBet) {
                if (chkObj.id == 'chkUpdStaus') obj('chkClosed').disabled = (chkObj.checked) ? false : true; //Status				
                if (chkObj.id == 'chkUpdComm') SetEnableDisable(arrComm, (chkObj.checked) ? false : true); //Commission
                if (chkObj.id == 'chkUpdSoccerBet') SetEnableDisable(arrSoccerBet, (chkObj.checked) ? false : true); //Soccer Bet
                if (chkObj.id == 'chkUpdSoccerPT') //Soccer PT
                {
                    SetEnableDisable(arr1Super1, (chkObj.checked) ? ((obj('chkCopy1HdpDeadball').checked) ? true : false) : true);
                    SetEnableDisable(arr2Super1, (chkObj.checked) ? ((obj('chkCopy11x2').checked) ? true : false) : true);
                    SetEnableDisable(arr3Super1, (chkObj.checked) ? ((obj('chkCopy1HdpLive').checked) ? true : false) : true);
                    SetEnableDisable(arrSoccerPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd2Bet') SetEnableDisable(arrBasketballBet, (chkObj.checked) ? false : true); //Basketball Bet				
                if (chkObj.id == 'chkUpd2PT') //Basketball PT
                {
                    SetEnableDisable(arrBasketballPT, (chkObj.checked) ? false : true);
                    SetEnableDisable(arrSuper2, (chkObj.checked) ? ((obj('chkCopy2Hdp').checked) ? true : false) : true);
                }

                if (chkObj.id == 'chkUpd3Bet') SetEnableDisable(arrFootballBet, (chkObj.checked) ? false : true); //Football Bet
                if (chkObj.id == 'chkUpd3PT')//Football PT
                {
                    SetEnableDisable(arrSuper3, (chkObj.checked) ? ((obj('chkCopy3Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrFootballPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd5Bet') SetEnableDisable(arrTennisBet, (chkObj.checked) ? false : true); //Tennis Bet
                if (chkObj.id == 'chkUpd5PT')//Tennis PT
                {
                    SetEnableDisable(arrSuper5, (chkObj.checked) ? ((obj('chkCopy5Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrTennisPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd8Bet') SetEnableDisable(arrBaseballBet, (chkObj.checked) ? false : true); //Baseball Bet
                if (chkObj.id == 'chkUpd8PT')//Baseball PT
                {
                    SetEnableDisable(arrSuper8, (chkObj.checked) ? ((obj('chkCopy8Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrBaseballPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd10Bet') SetEnableDisable(arrGolfBet, (chkObj.checked) ? false : true); //Golf Bet
                if (chkObj.id == 'chkUpd10PT')//Golf PT
                {
                    SetEnableDisable(arrSuper10, (chkObj.checked) ? ((obj('chkCopy10Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrGolfPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd11Bet') SetEnableDisable(arrMotoBet, (chkObj.checked) ? false : true); //Moto Bet
                if (chkObj.id == 'chkUpd11PT')//Golf PT
                {
                    SetEnableDisable(arrSuper11, (chkObj.checked) ? ((obj('chkCopy11Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrMotoPT, (chkObj.checked) ? false : true);
                }

                if (chkObj.id == 'chkUpd201Bet') SetEnableDisable(arrFinancialBet, (chkObj.checked) ? false : true); //Financial Bet
                if (chkObj.id == 'chkUpd201PT')//Financial PT
                {
                    SetEnableDisable(arrSuper12, (chkObj.checked) ? ((obj('chkCopy12Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrFinancialPT, (chkObj.checked) ? false : true);
                }
                if (chkObj.id == 'chkUpd151Bet') SetEnableDisable(arrHorseRacingBet, (chkObj.checked) ? false : true); //HorseRacing Bet

                if (chkObj.id == 'chkUpd99Bet') SetEnableDisable(arrOtherBet, (chkObj.checked) ? false : true); //Other Bet
                if (chkObj.id == 'chkUpd99PT')//Other PT
                {
                    SetEnableDisable(arrSuper99, (chkObj.checked) ? ((obj('chkCopy99Hdp').checked) ? true : false) : true);
                    SetEnableDisable(arrOtherPT, (chkObj.checked) ? false : true);
                }
            }
    </script>

</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC fixfont">
                    Update Member
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="err" class="errMsg">
            </div>
            <div id="errMsg" class="error-message">
            </div>
        </div>
        <div class="bg_table">

            <script src="../../js/ToolTip/wz_tooltip/wz_tooltip.js" type="text/javascript"></script>

            <script src="../../js/ToolTip/wz_tooltip/tip_balloon2.js" type="text/javascript"></script>

            <iframe name="InsertData" id="InsertData" style="display: none" width="0" height="0"
                frameborder="0"></iframe>
            <form id="fUpdate" name="fUpdate" method="post" action="UpdateNewMember.aspx?mode=edit&agentID=<%=agentID%>&amp;CID=<%=CID%>"
            target="InsertData">
            <div style="text-align: left;">
                <input type="hidden" id="hidEdit" name="hidEdit" />
                <input type="hidden" id="txtIDs" name="txtIDs" />
                <input type="hidden" id="isMult" name="isMult" value="<%=Mult%>" />
                <input type="hidden" id="txtAllowPT" name="txtAllowPT" value="<%=AllowUdpPT%>" />
                <input type="hidden" id="set" name="set" value="<%=formset%>" />
                <asp:Literal ID="htmlEditMemberContent" runat="server"></asp:Literal>
            </div>
            </form>
            <%=script.ToString()%>
        </div>
    </div>
</body>
</html>
