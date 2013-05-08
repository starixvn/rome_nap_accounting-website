<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Others_Menu" theme=""%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        function LinkOnClick(id) {
            $("a.current").removeClass("current");
            $("#" + id).addClass("current");
        }       
    </script>
    <style>
        body
        {
        	margin-top:2px;
        	}
    </style>
</head>
<body >
    <asp:Literal ID="sMenu" runat="server"></asp:Literal>
    <script type="text/javascript">
        var fr =parent.frames['fContent'];
        var lfr=parent.parent.frames['leftPanel'];                
        var TabLog='', UserName='';      
        if (typeof lfr !='undefined' && fr) {
            var usernamectrl=lfr.document.getElementById('box_02_headertext');
            var tablogsctrl = lfr.document.getElementById('TabLogs');
            if (usernamectrl!=null)
                UserName=usernamectrl.innerHTML; //UserName
            if (tablogsctrl!=null)
                TabLog = tablogsctrl.value;
        }
        if (TabLog=='Credit')
        {        
            LinkOnClick('Cre');
            if (typeof fr !='undefined' && fr) 
                fr.location.replace('CreditLog.aspx?UserName='+UserName+'&NewWindow=0');
        }
        else    
        {
            LinkOnClick('Log');
            if (TabLog!='')
            {
                if (typeof fr !='undefined' && fr) 
                    fr.location.replace('Logs.aspx?UserName='+UserName+'&NewWindow=0');
            }
            else
            {
                if (typeof fr !='undefined' && fr) 
                    fr.location.replace('Logs.aspx?UserName='+UserName+'&NewWindow=1');
            }
        }   
    </script>
</body>
</html>
