<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Encryption.aspx.cs" Inherits="Admin_Encryption" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 500px">  
            <tr>
                <td>Value</td>
                <td><input type="text" name="txtEncryptValue" value="" style="width:200px" runat="server" id="txtEncryptValue" /></td>
                <td><asp:Button 
                        ID="btnEncrypt1" runat="server" Text="Encrypt" 
                        onclick="btnEncrypt1_Click" />
                </td>
            </tr>
            <tr>
                <td>Encrypted Value</td>
                <td colspan="2">                    
                    <label id="lblEncrypt" runat="server" style="width:200px"></label>
                </td>
            </tr>

             <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                
                <td colspan="3" style="font-weight:bold;">                    
                   Security Code Section
                </td>
            </tr>

             <tr>
                <td>CustId</td>
                <td><input type="text" name="txtEncryptValue" value="" style="width:200px" runat="server" id="txtCustid" /></td>
                <td>
                    
                 </td>
            </tr>

            <tr>
                <td>Security Code</td>
                <td><input type="text" name="txtEncryptValue" value="" style="width:200px" runat="server" id="txtSC" /></td>
                <td>
                    
                 </td>
            </tr>

             <tr>
                <td><asp:Button ID="btnSC" runat="server" onclick="btnSC_Click" Text="Generate" /></td>
                <td></td>
                <td>
                    
                 </td>
            </tr>
            <tr>
                <td>Encrypted Value</td>
                <td colspan="2">                    
                    <label id="lblSc" runat="server" style="width:200px"></label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
