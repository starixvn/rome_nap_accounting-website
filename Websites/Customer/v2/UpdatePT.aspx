<%@ Page Language="C#" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml"><head id="Head1" runat="server">
    <title></title>     
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script type="text/javascript" src="../PT/Racing/JS/CommonPT.js"></script>   
    <script language="javascript" type="text/javascript">
        function openPage(url) {
            $('#fContent').attr({ src: url });            
        }
        </script>
</head>
<frameset rows="30,*" frameborder="0" >
    <frame name="fMenu" frameborder="0" noresize="noresize" scrolling="no" src="Menu.aspx" />
    <%--<frame id="fContent" name="fContent" frameborder="0" noresize="noresize" scrolling="auto" src="" />--%>

    <frame id="fContent" onload="PTUnBlockUI()" name="fContent" src="../../_blankpage.htm"
                    scrolling="auto" marginwidth="0" marginheight="0" frameborder="0" vspace="0"
                    hspace="0" allowTransparency="true" noresize="noresize" >
                    
            </frame>
</frameset>
</html>