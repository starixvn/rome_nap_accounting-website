<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="SignIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SignIn</title>
    <link rel="stylesheet" type="text/css" id="linkLogin" /> 
    <script type="text/javascript">
        
    </script>
</head>
<body>
    <form action="PreLogIn/ProcessLogin.ashx" method="post" id="LoginForm" target="controlFr">
    <center>
        <div id="table_main">
        </div>
    </center>
    </form>
     
    <iframe id="controlFr" name="controlFr" style="display: none;"></iframe>
    <script language="javascript" type="text/javascript">
        var plsInputUserNameErr = '<%=Resources.ErrorMsg.lblPlsEnterUserName %>';
        var plsInputPassErr = '<%=Resources.ErrorMsg.lblPlsEnterPassword %>';
        var theme = '<%=LoginTheme%>';
        
        var signIn = {
            system: '',
            TplDefaulLoginTheme: '<div class="contents_box"><div class="lgLeft"></div><div class="key"></div><div class="lgRight"></div><div class="lgRp"><div class="war_box"><div class="title"></div></div><div class="war_box"><div class="comment" id="ErrorMessage" style="visibility: hidden;">The New Password is required</div></div><div class="form_log"><table width="100%" border="0" align="left" cellpadding="0" cellspacing="0"><tr><td width="91" height="40" class="log_text">Username</td><td width="266"><input type="text" name="TxtUsername" id="TxtUsername" autocomplete="off" style="width: 180px;" /></td></tr><tr><td height="40" class="log_text">Password</td><td><input type="password" name="TxtPassword" id="TxtPassword" autocomplete="off" style="width: 180px;" /></td></tr><tr><td height="40"></td><td><input value="" onclick="return signIn.login();" class="log_button" type="submit" /></td></tr></table></div></div></div><div class="text_copyright"><div style="float: left; !padding-left: 12px;"><table border="0px" cellpadding="0px" cellspacing="0px" class="bestview"><tr><td>Best view in</td><td class="viewIE"></td><td class="viewFF"></td></tr></table></div></div>',
            TplExtLoginTheme: '<div class="contents_box"><div class="lgLeft"></div><div class=""><div class="comment " id="ErrorMessage" style="position:relative;top:70px; left:155px; max-width:380px;visibility: hidden;">The New Password is wrong</div></div><div class="lgRp"><div class="form_log" style="position: fixed;top: 150px;"><table width="280px" align="left" border="0" cellpadding="0" cellspacing="0"><tbody><tr><td height="20" colspan="2" class="text_pad">&nbsp;</td></tr><tr><td height="20" align="right" valign="top" class="icon_user"><span class="text_pad">Username </span></td><td height="20"><input name="TxtUsername" type="text" class="log_text" id="TxtUsername" style="width: 152px;" autocomplete="off" /></td></tr><tr><td height="20" align="left" valign="top" class="icon_pass"><span class="text_pad">Password</span></td><td height="20"><input name="TxtPassword" type="password" class="log_text" id="TxtPassword" style="width:152px;" autocomplete="off" /></td></tr><tr><td width="39" height="40"></td><td align="right" valign="middle"><input value="LOGIN" onclick="return signIn.login();" class="log_button" type="submit"></td></tr></tbody></table></div></div></div><div class="text_copyright"><div style="float: left;"></div></div>',
            bindingLoginTheme: function () {

                var getTheme = '';
                var links = document.getElementsByTagName('link');
                document.getElementById('table_main').innerHTML = '';

                if (this.system == 'login') {                    
                    getTheme = this.TplDefaulLoginTheme;
                }
                else {                    
                    getTheme = this.TplExtLoginTheme;
                }
                document.getElementById('table_main').innerHTML = getTheme;
                document.getElementById("TxtUsername").focus();
            },
            login: function () {
                document.getElementById('ErrorMessage').style.visibility = "hidden";
                if (document.getElementById('TxtUsername').value == '') {
                    this.loginError(plsInputUserNameErr);
                    document.getElementById('TxtUsername').focus();
                    return false;
                }
                else if (document.getElementById('TxtPassword').value == '') {
                    this.loginError(plsInputPassErr);
                    document.getElementById('TxtPassword').focus();
                    return false;
                }
                return true;
            },
            loginError: function (errMsg) {
                document.getElementById('TxtPassword').value = "";
                document.getElementById('ErrorMessage').innerHTML = errMsg;
                document.getElementById('ErrorMessage').style.visibility = "visible";
            }
        }

        signIn.system = theme.toLowerCase() ;
        signIn.bindingLoginTheme();
    </script>
    
</body>
</html>