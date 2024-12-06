// JScript File
var selectedGroupId = '0';
var $confirmUpdate = $("#confirmUpdate");

function reSelectGroupId(check)
{
    var groupId = document.getElementById("cbogroup").value;
    var frm = document.getElementById('frmterm');
    frm.src = "TermController.aspx?groupid=" + groupId + "&check=" + check;
    selectedGroupId = groupId;
}

function setvalue(check)
{
    var f = frames["frmterm"] ;        
    var grade = obj('cbograde');
    if(f.groupid == 0)
    {
        /*obj('txtPreAmt').disabled = true;
        obj('txtInsAmt').disabled = true;
        obj('s_currency').disabled = true;        
        obj('day').disabled = true;        
        obj('remark').disabled = true;        
        obj('cbograde').disabled = true; */       
    } 
    else
    {
        /*obj('txtPreAmt').disabled = false;
        obj('txtInsAmt').disabled = false;
        obj('s_currency').disabled = false;        
        obj('day').disabled = false;        
        obj('remark').disabled = false;        
        obj('cbograde').disabled = false;*/        
        
        if(check == '0') // if group head line is checked then just load checker
        {
            for(i = 0;i <grade.length; i++)
            {
                if(grade.options[i].value == f.creditgrade)
                {
                    grade.options[i].selected = true;
                    break;
                }
            }
            obj('txtPreAmt').value = f.pre;
            obj('txtInsAmt').value = f.installamt;
            obj('s_currency').innerHTML = f.curr;
            obj('remark').value = f.remark;
            
            var day = obj('day');
            for(i = 0; i<day.length; i++)
            {
                if(day.options[i].value == f.installdate)
                {
                    day.options[i].selected = true;
                    break;
                }    
            }
        }
        
        var CheckerList = obj('ddlCheckerList');               
        for(i = 0; i<CheckerList.length; i++)
        {
            if(CheckerList.options[i].value == f.checkerid)
            {
                CheckerList.options[i].selected = true;
            }
        }       
        
        if(!obj('groupheadline').checked )//uncheck 
        {
            obj('ddlCheckerList').options[0].selected = true;
        }
    }
}

function ConfirmRemoveHeadLine() {
    var isHeadLine = document.getElementById('groupheadline').checked;
    if (isHeadLine)
    { 
        return true;
    }
    else {
        return confirm('The group has only 1 member (is Headline). Are you sure you want to remove this group?');
    }
}

function SetChecker(elemt, check) {
    //elemt.checked=!elemt.checked;

    var isHeadLine = elemt.checked;
    //alert(isHeadLine);
    reSelectGroupId(check);
    
    var CheckerList = obj('ddlCheckerList');               
    var f = frames["frmterm"] ;        
    
    //var id = (checkerid) ? checkerid : f.checkerid;
    //alert('checkerid1 = ' + checkerid + '\ncheckerid = ' + f.checkerid);
    
    var id = f.checkerid;

    CheckerList.disabled = !isHeadLine;  //!(elemt.checked);

    if (!isHeadLine)//uncheck 
    {
        obj('ddlCheckerList').options[0].selected = true;
    }
    else//check must be restore status of checker for group
    {   
        for(i = 0; i<CheckerList.length; i++)
        {
            if(CheckerList.options[i].value == id)
            {
                CheckerList.options[i].selected = true;
            }
        }
    }
    
}

function SubmitUpdate() {
    var username = $("#txtcustomer").val();
    var isRemoveMapping = !$('#groupheadline').is(":checked") && $("#cbogroup option:selected").text() === username;
    if (selectedGroupId === '0' || isRemoveMapping) {
        var message = "Are you sure you want to save changes for " + username + "?";
        GetListWhiteLabel(username, function (result) {          
            if (result != "") {
                message += "<br><br><b>Note:</b> Remove/Update group headline will affect White Label settings: <span style=\"word-wrap: break-word; color:r ed\">" + result + "</span>.<br>";
                message += "By your action, headline earning of White Label setting will be updated accordingly.If you need to revise the rate, please help update it in the [Manage White Label] function.";
            }
            __confirm("Please Confirm", message, 425, 'auto', null,
                function () {
                    __close(false, null, null, null);
                    $("#btnCreditViewSubmit").click();
                },
                function () {
                    __close(false, null, null, null);
                },
                null, false);
        });      

    } else {
        OpenDialogConfirm($("#txtcustomer").val(), selectedGroupId);
    }
}

function GetListWhiteLabel(usernames, callback) { 
    $.ajax({
        type: "GET",
        url: window.rootUrl + "WhiteLabel/GetListWhiteLabelByUsernames?usernames=" + usernames,
        contentType: "text/plain; charset=utf-8",
        success: function (result) {
            callback(result);
        },
        error: function () {
        }
    });
}

function OpenDialogConfirm(usernames, groupId) {
    try { parent.jQuery.BlockUI(); } catch (e) { }
    $.ajax({
        type: "GET",
        url: window.rootUrl + "HeadlineExternalGroupMapping/GetExternalGroupHeadlineMappingSuper?usernames=" + usernames + "&groupId=" + groupId,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var headlineName = $("#cbogroup option:selected").text();
            var supername = $("input[name='txtcustomer']")[0].value;
            if ($("#groupheadline").is(":checked")) {
                headlineName = supername;
            }

            if (result.Supers !== null && result.Supers.length > 0) {
                $confirmUpdate.removeClass("hidden").dialog({
                    title: "Confirm Auto Adding Super To External Group",
                    resizable: true,
                    autoOpen: false,
                    modal: true,
                    width: '950px',
                    height: 'auto'
                }).dialog("open");
                $("#headlineName").html("Headline " + headlineName);
                RenderMapping(result.Supers);
                
            } else {
                $("#btnCreditViewSubmit").click();
            }
        },
        error: function () {
        },
        complete: function () {
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RenderMapping(data) {
    var bodyRows = [];
    for (var i = 0; i < data.length; i++) {
        var row = { Columns: [] };
        row.Columns.push({ Name: 'Super', Value: data[i].Username });
        row.Columns.push({ Name: 'Site', Value: data[i].Site });
        row.Columns.push({ Name: 'Currency', Value: data[i].Currency });
        row.Columns.push({ Name: 'NormalGroup', Value: data[i].NormalGroup });
        row.Columns.push({ Name: 'ExtraPTGroup', Value: data[i].ExtraPTGroup });
        row.Columns.push({ Name: 'PrimaryExtraPTGroup', Value: data[i].PrimaryExtraPTGroup });
        bodyRows.push(row);
    }

    var gridOptions =
    {
        columns: [
            { HeaderText: "Super", Width: 100, Name: "Super", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Site", Width: 100, Name: "Site", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Currency", Width: 100, Name: "Currency", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "NormalGroup", Width: 150, Name: "NormalGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "ExtraPTGroup", Width: 250, Name: "ExtraPTGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "PrimaryExtraPTGroup", Width: 150, Name: "PrimaryExtraPTGroup", HeaderAlign: "center", CellAlign: "left" }
        ],
        bodyRows: bodyRows,
        gridExpandHeight: 200,
        showDistinctRow: false
    };

    $('#supermapping').fanexGrid(gridOptions);
}

$(document).ready(function () {
    if ($('#ckbIsOpenTab').is(":checked")) {
        var newWindow = window.open(window.rootUrl + "HeadlineExternalGroupMapping/Index");
        if (newWindow) newWindow.focus();
    }

    $("#confirmUpdate").dialog({
        title: "Confirm Auto Adding Super To External Group",
        resizable: true,
        autoOpen: false,
        modal: true,
        width: '800px',
        height: 'auto',
        buttons: [
            {
                text: "Submit",
                click: function () {
                    $(this).dialog("close");
                    $("#btnCreditViewSubmit").click();
                }
            }
        ]
    });
});