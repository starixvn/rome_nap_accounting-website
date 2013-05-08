<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectType_New.aspx.cs" Inherits="NewCustomer_SelectType_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>New Customer</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript">
        function OpenFormAdd(parentId, parentRoleId) {
            var roleId = '';
            var tfHeigth = "30";
            obj("errMsg").innerHTML = "";
            if (parentRoleId) {
                roleId = parentRoleId;
                roleId == '4' ? tfHeigth = "130" : tfHeigth = "30";
            }
            else {
                roleId = obj("custRoleId").value;
                parentId = obj("custId").value;
            }
            var f = parent.frames["mainCust"];

            if (roleId == '2') {
                parent.document.body.rows = tfHeigth + ",*";
                //f.location.href="AddNewMember.aspx?agentId="+parentId + "&username="+obj("txtUsername").value;
                //Khoa.tran add new member (Original code from IBCAgent)
                //f.location.href = "v2/AddNewMember.aspx?agentId=" + parentId + "&username=" + obj("txtUsername").value;
                f.location.href = "../AddNewMember/AddNewMemberPage?agentId=" + parentId + "&username=" + obj("txtUsername").value;
            }
            else if (roleId == '3') {
                parent.document.body.rows = tfHeigth + ",*";
                //f.location.href = "v2/AddNewAgent.aspx?masterId=" + parentId + "&username=" + obj("txtUsername").value; ;
                f.location.href = "../AddNewAgent/AddNewAgentPage?masterId=" + parentId + "&username=" + obj("txtUsername").value;
            }
            else if (roleId == '4') {
                parent.document.body.rows = tfHeigth + ",*";
                //var url="AddNewMaster.aspx?";
                //var url = "V2/AddNewMaster.aspx?";
                var url = "../AddNewMaster/AddNewMasterPage?";
                var uname = "";
                var custId = "";
                if (!parentRoleId) {
                    url += "superId=" + parentId;
                }
                else {
                    if (obj("exSuper").checked) {
                        uname = obj("listSuper").options[obj("listSuper").selectedIndex].text;
                        custId = obj("listSuper").options[obj("listSuper").selectedIndex].value;
                        url += "superId=" + custId + "&username=" + uname;
                        f.location.href = url;
                        return;
                    }
                    else if (obj("inSuper").checked) {
                        uname = obj("listCurrency").options[obj("listCurrency").selectedIndex].text;
                        custId = obj("listCurrency").options[obj("listCurrency").selectedIndex].value;
                        var site = obj("listSite").value;
                        url += "superId=" + custId + "&username=" + uname + "&site=" + site;
                        f.location.href = url;
                        return;
                    }
                }
                url += "&set=1";
                f.location.href = url;
            }
        }
        function OpenInputParent() {
            document.getElementById("errMsg").innerHTML = "";
            var selectType = obj("listType").options[obj("listType").selectedIndex].value;
            var f = parent.frames["mainCust"];
            if (selectType == "member" || selectType == "agent") {
                parent.document.body.rows = "30,*";
                f.location.href = "Empty.html";
            }
            else if (selectType == "master") {
                parent.document.body.rows = "130,*";
                f.location.href = "Empty.html";
            }
            else if (selectType == "super") {
                parent.document.body.rows = "30,*";
                //f.location.href="AddNewSuper.aspx";
                //f.location.href = "V2/AddNewSuper.aspx"; //<-- Khoa.Tran add new v2
                f.location.href = "../AddNewSuper/AddNewSuperPage";
            }
            document.getElementById('fSelectType').submit();
        }
        function CheckUserName() {
            document.getElementById("errMsg").innerHTML = "";
            if (isEmpty("txtUsername")) {
                parent.document.body.rows = "55,*";
                showAlertMessage("errMsg", "Please enter UserName.", 1);
                obj("txtUsername").focus();
                return;
            }
            else {
                var f = self.frames["frCheckUser"];
                if (f) {
                    f.location.href = "CheckUserName.ashx?listType=" + obj("listType").value + "&user=" + obj("txtUsername").value;
                }
            }
        }
        function On_SelectSuper() {
            if (obj("exSuper").checked) {
                obj("listCurrency").disabled = true;
                obj("listSite").disabled = true;
                obj("listSuper").disabled = false;
            }
            else if (obj("inSuper").checked) {
                obj("listCurrency").disabled = false;
                obj("listSite").disabled = false;
                obj("listSuper").disabled = true;
            }
        }
        function On_KeyPress(e) {

            if (!e) e = window.event;
            var code = (e.keyCode) ? e.keyCode : e.which;
            if (code == 13) {
                CheckUserName();
                return false;
            }
            return true;
        }
    </script>
</head>
<body style="margin: 0px;">
    <iframe name="frCheckUser" frameborder="1" class="hidden" id="frCheckUser" height="0px"
        width="500px"></iframe>
    <form id="fSelectType" method="post" action="SelectType_New.aspx" runat="server">
    <input type="hidden" name="custRoleId" id="custRoleId" value="<%=Session["CustRoleId"]%>" />
    <input type="hidden" name="custId" id="custId" value="<%=Session["CustId"]%>" />
    <asp:Literal ID="SelectTypeContentHtml" runat="server"></asp:Literal>
    </form>
</body>
</html>