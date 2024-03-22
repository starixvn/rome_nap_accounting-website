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

    var custName = obj("custName").value;
    var uplineUsername = obj("txtUsername").value;
    custName = (custName == undefined || custName == '') ? obj("txtUsername").value : custName;
    var f = parent.frames["mainCust"];

    if (roleId == '2') {
        parent.document.body.rows = tfHeigth + ",*";
        //f.location.href="AddNewMember.aspx?agentId="+parentId + "&username="+obj("txtUsername").value;
        //Khoa.tran add new member (Original code from IBCAgent)
        //f.location.href = "v2/AddNewMember.aspx?agentId=" + parentId + "&username=" + obj("txtUsername").value;
        f.location.href = "../Customer/CreateCustomer?uplineId=" + parentId + "&uplineRoleId=" + roleId + "&uplineUsername=" + uplineUsername + "&siteName=" + siteName;
    }
    else if (roleId == '3') {
        parent.document.body.rows = tfHeigth + ",*";
        //f.location.href = "v2/AddNewAgent.aspx?masterId=" + parentId + "&username=" + obj("txtUsername").value; ;
        f.location.href = "../Customer/CreateCustomer?uplineId=" + parentId + "&uplineRoleId=" + roleId + "&uplineUsername=" + uplineUsername + "&theme=nap";
    }
    else if (roleId == '4') {
        parent.document.body.rows = tfHeigth + ",*";
        //var url="AddNewMaster.aspx?";
        //var url = "V2/AddNewMaster.aspx?";
        var url = "../Customer/CreateCustomer?";
        var uname = "";
        var custId = "";
        if (!parentRoleId) {
            url += "uplineId=" + parentId + "&uplineRoleId=" + roleId + "&theme=nap" + "&uplineUsername=" + uplineUsername;
        }
        else {
            if (obj("exSuper").checked) {
                uname = obj("listSuper").options[obj("listSuper").selectedIndex].text;
                var custId_siteName = obj("listSuper").options[obj("listSuper").selectedIndex].value;
                custId = custId_siteName.split('_')[0];
                siteNameFromSuperList = custId_siteName.split('_')[1];
                url += "uplineId=" + custId + "&uplineRoleId=" + roleId + "&uplineUsername=" + uname + "&theme=nap";
                f.location.href = url;
                return;
            }
            else if (obj("inSuper").checked) {
                uname = obj("listCurrency").options[obj("listCurrency").selectedIndex].text;
                custId = obj("listCurrency").options[obj("listCurrency").selectedIndex].value;
                var listSite = obj("listSite");
                const selectedIndex = listSite.selectedIndex;
                const selectedItem = listSite.options[selectedIndex];
                const site = selectedItem.value;
                const siteLabel = (selectedItem).closest("optgroup").label;
                url += "uplineId=" + custId + "&uplineRoleId=" + roleId + "&uplineUsername=" + uname + "&site=" + site + "&siteLabel=" + siteLabel + "&theme=nap";
                f.location.href = url;
                return;
            }
        }
        //url += "&set=1";
        f.location.href = url;
    }
}
function OpenInputParent() {
    try {
        document.getElementById("errMsg").innerHTML = "";
    } catch (e) { }
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
        f.location.href = "../Customer/NewCustomer?level=" + selectType;
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