function OpenFormAdd(parentId, parentRoleId, siteName) {
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
        f.location.href = "../AddNewMember/AddNewMemberPage?agentId=" + parentId + "&username=" + obj("txtUsername").value + "&siteName=" + siteName;
    }
    else if (roleId == '3') {
        parent.document.body.rows = tfHeigth + ",*";
        //f.location.href = "v2/AddNewAgent.aspx?masterId=" + parentId + "&username=" + obj("txtUsername").value; ;
        f.location.href = "../AddNewAgent/AddNewAgentPage?masterId=" + parentId + "&username=" + obj("txtUsername").value + "&siteName=" + siteName;
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
                var custId_siteName = obj("listSuper").options[obj("listSuper").selectedIndex].value;
                custId = custId_siteName.split('_')[0];
                siteNameFromSuperList = custId_siteName.split('_')[1];
                url += "superId=" + custId + "&username=" + uname + "&siteName=" + siteNameFromSuperList;
                f.location.href = url;
                return;
            }
            else if (obj("inSuper").checked) {
                uname = obj("listCurrency").options[obj("listCurrency").selectedIndex].text;
                custId = obj("listCurrency").options[obj("listCurrency").selectedIndex].value;
                var site = obj("listSite").value;
                url += "superId=" + custId + "&username=" + uname + "&siteName=" + site;
                f.location.href = url;
                return;
            }
        }
        url += "&set=1";
        f.location.href = url;
    }
}
function OpenInputParent() {
    try{
        document.getElementById("errMsg").innerHTML = "";
    }catch (e){}
    var selectType = obj("listType").options[obj("listType").selectedIndex].value;
    var f = parent.frames["mainCust"];
    if (selectType == "agent") {
        parent.document.body.rows = "30,*";
        f.location.href = "Empty.html";
        document.getElementById('fSelectType').submit();
    }
    else if (selectType == "master") {
        parent.document.body.rows = "130,*";
        f.location.href = "Empty.html";
        document.getElementById('fSelectType').submit();
    }
    else if (selectType == "super" || selectType == "member") {
        document.getElementById('uid-upline').style.display = 'none';
        parent.document.body.rows = "30,*";
        //f.location.href="AddNewSuper.aspx";
        //f.location.href = "V2/AddNewSuper.aspx"; //<-- Khoa.Tran add new v2
        //f.location.href = "../AddNewSuper/AddNewSuperPage";// <-- Thai.Tran remove this one to add Search_Copy function
        f.location.href = "../AddNewSuper/SearchCopyAddNewSuperPage?level=" + selectType; // <-- Thai.Tran add this one
    }
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

var timeout = setTimeout(function () {
    clearTimeout(timeout);
    var selectType = obj("listType").options[obj("listType").selectedIndex].value;
    if (selectType == "member") {
        OpenInputParent();
        document.getElementById('uid-upline').style.display = 'none';
    }
}, 1000);