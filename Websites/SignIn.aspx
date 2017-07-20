<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="SignIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SignIn</title>
    <link rel="stylesheet" type="text/css" id="linkLogin" />
    <asp:PlaceHolder runat="server">
        <link rel="stylesheet" type="text/css" href="FPS/Style?guid=<%=Guid.NewGuid() %>" />
    </asp:PlaceHolder>
</head>
<body onload="signIn.bindingLoginTheme();">
    <form action="PreLogIn/ProcessLogin.ashx" method="post" id="LoginForm" target="controlFr" onsubmit="return signIn.login();" autocomplete="off">
    <center>
        <div id="table_main" class="detecas-visible" style="visibility: collapse">
        </div>
        <div class="content" id="message" style="display: none">
        <div class="icon-killer"></div> 
        <div class="clear"></div>        
    </div>
    </center>
    </form>     
    <iframe id="controlFr" name="controlFr" style="display: none;"></iframe>
    <script src="js/encode.js?v=<% =Sunplus.Accounting.Core.Configurations.WebSettings.Version%>" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var plsInputUserNameErr = '<%=Resources.ErrorMsg.lblPlsEnterUserName %>';
        var plsInputPassErr = '<%=Resources.ErrorMsg.lblPlsEnterPassword %>';       
        var submitting = false;
        var signIn = {
            // Robot Trap TplDefaulLoginTheme
            TplDefaulLoginTheme: '<div class="contents_box"><div class="lgLeft"></div><div class="key"></div><div class="lgRight"></div><div class="lgRp"><div class="war_box"><div class="title"></div></div><div class="war_box"><div class="comment" id="ErrorMessage" style="visibility: hidden;">The New Password is required</div></div><div class="form_log"><table width="100%" border="0" align="left" cellpadding="0" cellspacing="0"><tr><td width="91" height="40" class="log_text">Username</td><td width="266"><input type="text" name="TxtUsername" id="TxtUsername" autocomplete="off" style="width: 180px;" /></td></tr><tr><td height="40" class="log_text">Password</td><td><input type="password" name="TxtPassword" id="TxtPassword" autocomplete="off" style="width: 180px;" /></td></tr><tr><td height="40"></td><td><input value="" class="log_button" type="submit" id="submit"/><input id="code" name="code" type="hidden" value="<% =Fanex.Tarus.Security.Services.Encryptor.Code %>" /><input type="hidden" id="guid" name="guid" value="<%= Guid.NewGuid() + " " + DateTime.Now.ToString("s")%>" /></td></tr></table></div></div></div><div class="text_copyright"><div style="float: left; !padding-left: 12px;"><table border="0px" cellpadding="0px" cellspacing="0px" class="bestview"><tr><td>Best view in</td><td class="viewIE"></td><td class="viewFF"></td></tr></table></div></div>',
                      
            bindingLoginTheme: function () {
                document.getElementById('table_main').innerHTML = '';
                document.getElementById('table_main').innerHTML = this.TplDefaulLoginTheme;
                document.getElementById("TxtUsername").focus(); 
            },
            login: function () {
                if (submitting) { return false; }
                submitting = true; 
                document.getElementById('ErrorMessage').style.visibility = "hidden";
                if (document.getElementById('TxtUsername').value == '') {
                    this.loginError(plsInputUserNameErr);
                    document.getElementById('TxtUsername').focus();
                    submitting = false;
                    return false;
                }
                else if (document.getElementById('TxtPassword').value == '') {
                    this.loginError(plsInputPassErr);
                    document.getElementById('TxtPassword').focus();
                    submitting = false;
                    return false;
                }

                document.getElementById('TxtPassword').value = document.getElementById('TxtPassword').value.ec(document.getElementById('code').value).hc();

                return true;
            },
            loginError: function (errMsg) {
                submitting = false;
                document.getElementById('TxtPassword').value = "";
                document.getElementById('ErrorMessage').innerHTML = errMsg;
                document.getElementById('ErrorMessage').style.visibility = "visible";
            }
        }   
        signIn.bindingLoginTheme();
    </script>
</body>
</html>