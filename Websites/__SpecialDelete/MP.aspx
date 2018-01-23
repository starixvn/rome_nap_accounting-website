<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MP.aspx.cs" Inherits="SpecialDelete_MP" Title="Untitled Page" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">
          window.location.href = "V2/Mixparlay.aspx";
    </script>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7"/>
    <script type="text/javascript" src="../js/common.js"></script>   
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />  
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/SpecialDelete/SpecialDelete.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/BetList/BetList.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript" src="../js/prototype-full.js"></script>
    <style type="text/css">
        input[type=checkbox]
        {
         cursor:pointer;	
        }
    </style>
    
    <script type="text/javascript">

        function BeginLoad() {
            $('inProgressBar').show();
        }

        function FinishLoad() {
            $('inProgressBar').hide();
        }

        function on_submit() {
            if (!isNumberNotNull('matchid')) {
                //$('ErrMsg').innerHTML = "*Please enter a valid Match Id (number)!";
                showAlertMessage("ErrMsg", "Please enter a valid Match Id (number)!", 1);
                $('matchid').focus();
                return false;
            }
            if (!isNumberNotNull('frefno')) {
                //$('ErrMsg').innerHTML = "*Please enter a valid From Ref Number!";
                showAlertMessage("ErrMsg", "Please enter a valid From Ref Number!", 1);
                $('frefno').focus();
                return false;
            }
            if (!isNumberNotNull('trefno')) {
                //$('ErrMsg').innerHTML = "Please enter a valid To Ref Number!";
                showAlertMessage("ErrMsg", "Please enter a valid To Ref Number!", 1);
                $('trefno').focus();
                return false;
            }

            if ($('hdp').value.length > 0) {
                if (isNaN($('hdp').value)) {
                    //$('ErrMsg').innerHTML = "Please enter a valid To HDP Number!";
                    showAlertMessage("ErrMsg", "Please enter a valid To HDP Number!", 1);
                    $('hdp').focus();
                    return false;
                }
            }

            $('ErrMsg').innerHTML = '';

            var param = '?matchid=' + $('matchid').value;
            param += '&bettype=' + $('bettype').value;
            param += '&frefno=' + $('frefno').value;
            param += '&trefno=' + $('trefno').value;
            param += '&betteam=' + $('choice').value;
            param += '&hdp=' + $('hdp').value;

             try{parent.BlockUI();}catch(e){}

            var myajax = new Ajax.Request("GetDataMP.ashx", {
                asynchronous: true,
                method: 'get',
                parameters: param,
                onSuccess: function(rs) {
                    $('sData').innerHTML = rs.responseText;
                    //FinishLoad();
                     parent.jQuery.unblockUI();
                }
            });

            return true;
        }

        function on_changeBetype() {

            var bettype = $('bettype').value;

            switch (bettype) {
                case "1":
                case "3":
                case "7":
                case "8":
                    var choice = $('choice');
                    choice.disabled = '';
                    $('hdp').disabled = '';

                    var arrTypeValue = ['All', 'h', 'a'];
                    var arrTypeText = ['All', 'Home/Over', 'Away/Under'];
                    choice.options.length = 0;
                    for (i = 0; i < arrTypeValue.length; i++) {
                        choice.options.add(new Option(arrTypeText[i], arrTypeValue[i]));
                    }
                    break;
                case "5":
                case "15":
                    var arrItem = ['All', '1', 'X', '2'];
                    var choice = $('choice');
                    choice.disabled = '';
                    choice.options.length = 0;
                    for (i = 0; i < arrItem.length; i++) {
                        choice.options.add(new Option(arrItem[i], arrItem[i]));
                    }

                    $('hdp').disabled = 'disabled';
                    $('hdp').value = '';
                    break;
                default:
                    var arrItem = ['All'];
                    var choice = $('choice');
                    choice.disabled = '';
                    choice.options.length = 0;
                    for (i = 0; i < arrItem.length; i++) {
                        choice.options.add(new Option(arrItem[i], arrItem[i]));
                    }

                    $('choice').disabled = 'disabled';

                    $('hdp').value = '';

                    $('hdp').disabled = 'disabled';
                    break;
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
                <td class="maincontent_headerC">
                    Mix Parlay
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box">
            <div id="ErrMsg" class="errMsg"></div>
        </div>
        <div class="bg_table">
            <table class="table">
                <tr>
                    <td>
                        <asp:Literal ID="HtmlHolder" runat ="server"></asp:Literal>
                        <div id="sData"></div>
                    </td>
                </tr>
            </table>
            
            <script type ="text/javascript">
                on_changeBetype();
            </script>
        </div>
     </div>
    
   
</body>

</html>

