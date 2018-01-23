
var msg = new Message();
var liveindicator = 2;

function CheckDecNumber(e) {

	var code = "";
	if (e.which)
		code = e.which;
	else if (e.keyCode)
		code = e.keyCode;

	
	if (code == 37 || code == 39 || code == 36 || code == 35 || code == 118) //e or E or "-"
		return true;
	if ((code < 48 || code > 57) && code != 8 && code != 13 && code != 0 && code != 46 && code != 9)
		return false;
	return true;
}

function on_delete() {
	$('#ErrorMessage').html('');
	$('#trMsg2').hide();
	if ($('#username').val() == "") {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "Please enter username", 1);        
		$('#username').focus();
		return false;
	}
	else if ($('#userpwd').val() == "") {
		$('#trMsg2').show();        
		showAlertMessage("ErrorMessage", "Please enter password", 1);
		$('#userpwd').focus();
		return false;
	}
	else if ($('#men').val() == "") {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must enter English message", 1);
		$('#men').focus();
		return false;
	}
	else if ($('#men').val().indexOf('@@') == -1) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in  English message for replacing ticket number", 1);
		$('#men').focus();
		return false;
	}
	else if ($('#mch').val() == "") {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must enter Chinese message", 1);
		$('#mch').focus();
		return false;
	}
	else if ($('#mch').val().indexOf('@@') == -1) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Chinese message for replacing ticket number", 1);
		$('#mch').focus();
		return false;
	}
	else if (($('#mcs').val() != "") && ($('#mcs').val().indexOf('@@') == -1)) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Simple Chinese message for replacing ticket number", 1);
		$('#mcs').focus();
		return false;
	}
	else if (($('#mth').val() != "") && ($('#mth').val().indexOf('@@') == -1)) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Thai message for replacing ticket number", 1);
		$('#mcs').focus();
		$('#mth').focus();
		return false;
	}
	else if (($('#mit').val() != "") && ($('#mit').val().indexOf('@@') == -1)) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Italian message for replacing ticket number", 1);
		$('#mit').focus();
		return false;
	}
	else if (($('#mjp').val() != "") && ($('#mjp').val().indexOf('@@') == -1)) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Japanese message for replacing ticket number", 1);
		$('#mjp').focus();
		return false;
	}
	else if (($('#mes').val() != "") && ($('#mes').val().indexOf('@@') == -1)) {
		$('#trMsg2').show();
		showAlertMessage("ErrorMessage", "You must include @@ in Spanish message for replacing ticket number", 1);
		$('#mes').focus();
		return false;
	}

	//parent.currentExecFrame.test = "anhhuy";
	try { parent.BlockUI(); } catch (e) { }
	parent.currentExecFrame.msg.Content_en = $('#men').val();
	parent.currentExecFrame.msg.Content_ch = $('#mch').val();
	parent.currentExecFrame.msg.Content_cs = $('#mcs').val();
	parent.currentExecFrame.msg.Content_it = $('#mit').val();
	parent.currentExecFrame.msg.Content_jp = $('#mjp').val();
	parent.currentExecFrame.msg.Content_th = $('#mth').val();
	parent.currentExecFrame.msg.Content_es = $('#mes').val();
	parent.currentExecFrame.msg.UserName = $('#username').val();
	parent.currentExecFrame.msg.UserId = $('#accId').val();
	parent.currentExecFrame.msg.userPwd = $('#userpwd').val();
	afterUpdate(false, 0);
	return true;
}

function ddl_change(type) {
	var ddl = 'ddl';
	ddl = ddl + type;

	var checkbox = 'chk';
	checkbox = checkbox + type;
	
	if ($('#'.concat(ddl)).length==0) return;
	var DelOption = $('#'.concat(ddl)).val();

	var chkAlls = document.getElementsByName(checkbox);
	if (!chkAlls) return;
	switch (DelOption) {
		case '0': //Not Delete
			for (i = 0; i < chkAlls.length; i++) {
				chkAlls[i].disabled = true;
				chkAlls[i].checked = false;
			}
			break;
		case '1': //Delete All

			for (i = 0; i < chkAlls.length; i++) {
				var id = chkAlls[i].id;
				var IsDelete = id.split('-')[2];
				if (IsDelete == '0') {
					chkAlls[i].checked = false;
					chkAlls[i].disabled = true;
				}
				else {
					chkAlls[i].checked = true;
					chkAlls[i].disabled = true;
				}
			}
			break;
		case '2': //Option
			for (i = 0; i < chkAlls.length; i++) {

				var id = chkAlls[i].id;
				var IsDelete = id.split('-')[2];
				if (IsDelete == '0') {
					chkAlls[i].disabled = true;
					chkAlls[i].checked = false;
				}
				else {
					chkAlls[i].disabled = false;
					chkAlls[i].checked = false;
				}
			}
			break;

	}
}

function on_deposit() {
	if (!obj('ddlDeposit')) return;
	var DelOption = obj('ddlDeposit').value;

	DelOption = '1';
	obj('ddlDeposit').value = '1';

	var chkAlls = document.getElementsByName('chkDeposit');
	if (!chkAlls) return;
	switch (DelOption) {
		case '1': //CheckAll
			for (i = 0; i < chkAlls.length; i++) {
				var id = chkAlls[i].id;
				var IsDelete = id.split('-')[2];
				if (IsDelete == '0') {
					chkAlls[i].checked = false;
					chkAlls[i].disabled = true;
				}
				else {
					chkAlls[i].checked = true;
					chkAlls[i].disabled = true;
				}
			}
			break;

	}
}

var timeoutID = 0;
function hideLanguage(obj) {
	if (String(obj.innerHTML).indexOf("-") != -1) {
		//current opened
		$("#otherLanguage_title").hide();
		$("#otherLanguage_content").hide();
		$("#otherLanguage_title2").hide();
		$("#otherLanguage_content2").hide();
		obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [+]";
	}
	else {
		//current closed
		$("#otherLanguage_title").show();
		$("#otherLanguage_content").show();
		$("#otherLanguage_title2").show();
		$("#otherLanguage_content2").show();
		obj.innerHTML = "<span style=\"color: Black;\">Other Language</span> [-]";
	}

	window.clearTimeout(timeoutID);
	timeoutID = setTimeout('gotoBottom()', 1);
}

function gotoBottom() {
	window.location = '#men';
}

function on_submit() {

	$('#ErrMsg').html('');
	$('#dvContent').html('');
	$('#btnDeleteBets').hide();

	//--------------

	if ($('#matchid').val() == '') {
		showAlertMessage("ErrMsg", "Please enter a valid Match Id (number)!", 1);
		$('#matchid').focus();
		return false;
	}
	if ($('#frefno').val() == '') {
		showAlertMessage("ErrMsg", "Please enter a valid From Ref Number!", 1);
		$('#frefno').focus();
		return false;
	}
	if ($('#trefno').val() == '') {
		showAlertMessage("ErrMsg", "Please enter a valid To Ref Number!", 1);
		$('#trefno').focus();
		return false;
	}

	

	var dead = [];
	$('input[name^="chkBettypenone-live"]:checked').each(function (i) {
		dead[i] = $(this).val().split('^')[0];
	});

	var live = [];
	$('input[name^="chkBettypelive"]:checked').each(function (i) {
		live[i] = $(this).val().split('^')[0];
	});


	if (dead.length == 0 && live.length == 0) {
		showAlertMessage("ErrMsg", "Please select a bet type!", 1);
		return false;
	}

	var data = {
		"matchId": $('#matchid').val(),
		"fromRefNo": $('#frefno').val(),
		"toRefNo": $('#trefno').val(),
		"live": live.join('^'),
		"dead": dead.join('^'),
		"userName": $('#userName').val()
	};

	try{parent.BlockUI();}catch(e){}
	
	specialdelete.renderSpecialDeleteGrid(data, true);

	

	return true;
}

function initDataGrid(rs) {
	//Render DataGrid
	
	$("<table id='tblSpecialDelete'></table>").appendTo($("#dvContent"));
	$('#tblSpecialDelete').datagrid({

		width: 950,
		height: 480,
		nowrap: false,
		// data: rs, 
		url: '',
		pagination: false,
		collapsible: true,
		singleSelect: true,
		idFied: 'Type',
		columns: [[
							{ field: 'Order', title: 'NO', width: 30, align: 'center' },
							{ field: 'Account', title: 'ACCOUNT', width: 100, align: 'center' },
							{ field: 'Refno', title: 'REFNO', width: 100, align: 'center' },
							{ field: 'Bettype', title: 'BETTYPE', width: 120, align: 'center' },
							{ field: 'Betteam', title: 'BET', width: 30, align: 'center' },
							{ field: 'Odds', title: 'ODDS', width: 60, align: 'center', formatter: function (value, row, index) {
								return '<span class="b"><font color="blue" style="font-size:7pt">' + row.Odds + '</font></span> <span class="b" style="font-size:5pt">' + row.OddType + '&nbsp;</span>';
							}
							},
							{ field: 'Amount', title: 'AMOUNT', width: 70, align: 'center', formatter: function (value, row, index) {
								return '<span class="b"><font color="red">' + row.Amount + '</font></span>';
							}
							},
							{ field: 'Currency', title: 'CURRENCY', width: 80, align: 'center' },
							{ field: 'TransTime', title: 'TRANS.TIME', width: 130, align: 'center', formatter: function (value, row, index) {
								return convertUnitToLocalDatetime(row.TransTime, 'm/d/Y h:i A');
							}
							},                            
							{ field: 'Creator', title: 'CREATOR', align: 'center', width: 80 },
							{ field: 'Status', title: 'STATUS', align: 'center', width: 60 },
							{ field: 'Type', title: 'OPTION', align: 'center', width: 80, formatter: function (value, row, index) {
								return (row.Type == 'Normal') 
								? ""
								: '<input class="hand" type="checkbox" disabled="disabled" id="' + row.Refno + '" value="' + row.Refno + '" name="chk' + row.Type + '"/>';
							}  
							}
						]],

		groupField: 'Type',
		view: groupview,
		groupFormatter: function (value, rows) {
			if (value == "Grand Total") {
				return "";
			}
			else {
				return "<font style='color:#000;'>" + value + ' - ' + (rows.length) + ' Item(s)</font>';
			}
		}
	});

	$('#tblSpecialDelete').datagrid('loadData', rs);
	$(".datagrid-view2 .datagrid-group").css("width", "950px").css("border-right", "1px dotted #CCCCCC"); //Fix losing bottom border of grouped rows
	$('.datagrid-group, .datagrid-group table tr td').css("background-color", "#B5C4D0").css('color', '#FFFFFF'); //Add color to Grand Total row
	$(".datagrid-body .datagrid-group table").css("float", "left");
	$(".datagrid-view2 .datagrid-body .datagrid-group").each(function () {
		var optselect = $('<select name="optdelete" style="float:right; padding:2px; margin-top:3px; margin-right:5px; width:75px"></select>');
		optselect.append($('<option value="0">Not Delete</option>'));
		optselect.append($('<option value="1">Delete All</option>'));
		optselect.append($('<option value="2">Optional</option>'));
		optselect.change(function () {
			//alert($(this).val());
			var val = $(this).val();
			var chklist = $(this).parent().next().find('input[type="checkbox"]');
			switch (val) {
				case '0': //uncheck
					chklist.attr("checked", "").attr('disabled', true);
					break;
				case '1': //check all
					chklist.attr("checked", "checked").attr('disabled', true);
					break;
				case '2': //option
					chklist.attr("checked", "").attr('disabled', false);
					break;
			}
		});

		optselect.appendTo($(this));
		var chknormallist = optselect.parent().next().find('input[type="checkbox"]');
		if (chknormallist.length == 0) {
			optselect.remove();
		}
		else {
			var chklistdeposit = optselect.parent().next().find('input[name="chkDeposit"]');
			if (chklistdeposit.length > 0) {
				optselect.attr('id', 'ddlDeposit');
				chklistdeposit.attr("checked", "checked").attr('disabled', true);
				optselect.find("option[value='1']").attr("selected", "selected");
			}

			var chklistchkSpecial = optselect.parent().next().find('input[name="chkSpecial"]');
			if (chklistchkSpecial.length > 0) {
				optselect.attr('id', 'ddlSpecial');
			}
			var chklistchk12EU = optselect.parent().next().find('input[name="chk12EU"]');
			if (chklistchk12EU.length > 0) {
				optselect.attr('id', 'ddl12EU');
			}

			var chklistchkMHC = optselect.parent().next().find('input[name="chkMHC"]');
			if (chklistchkMHC.length > 0) {
				optselect.attr('id', 'ddlMHC');
			}
		}
	});

	if (rs.rows.length > 0) {
		$('#btnDeleteBets').show();
	}
	else {
		$('#btnDeleteBets').hide();
	}
}


function CheckAllBettype(obj) {
	var IsCheck = $('#chkAllBettype').is(':checked');
	$('input[name^=chkBettype]').attr('checked', IsCheck);

	
}

function on_changeBetype(obj) {
	var cntAll = $('input[name^=chkBettype]').length;
	var cntElementChecked = $("input[name^=chkBettype]:checked").length;
	$('#chkAllBettype').attr('checked', cntAll == cntElementChecked);
}

function ShowPopupDeleteBets() {
	var val = [];
	$('.content input[type="checkbox"]:checked').each(function (i) {
		val[i] = $(this).val();
	});
	//alert(val);

	var title = 'Delete bets';
	var url = 'SpecialDelete/NumberGame/FormDeleteBet.aspx';
	var top = 170;
	var left = 150;
	var width = 980;
	var height = 370;
	showPopup(title, url, left, top, width, height, false, 3000, function () { CallAfterDeleted(); })
}

function CallAfterDeleted() {
	$('#ErrMsg').html('');
	//for Deposit  
	try { parent.BlockUI(); } catch (e) { } 
	var DelOption = 0;
	var transids = [];
	if ($('#ddlDeposit').length > 0) {
		DelOption = $('#ddlDeposit').val();
		if (DelOption == 2) {
			$('input[name="chkDeposit"]:checked').each(function (i) {
				transids[i] = $(this).val();
			});
		}
	}

	// for BetDaq
	var DelOption_BD = 0;
	var transids_BD = [];
	if ($('#ddlBetDaq').length > 0) {
		DelOption_BD = $('#ddlBetDaq').val();
		if (DelOption_BD == 2) {
			$('input[name="chkBetDaq"]:checked').each(function (i) {
				transids_BD[i] = $(this).val();
			});
		}
	}

	// for 12EU
	var DelOption_12EU = 0;
	var transids_12EU = [];
	if ($('#ddl12EU').length > 0) {
		DelOption_12EU = $('#ddl12EU').val();
		$('input[name="chk12EU"]:checked').each(function (i) {
			transids_12EU[i] = $(this).val();
		});
	}

	// for MHC 
	var DelOption_MHC = 0;
	var transids_MHC = [];
	if ($('#ddlMHC').length > 0) {
		DelOption_MHC = $('#ddlMHC').val();
		if (DelOption_MHC == 2) {
			$('input[name="chkMHC"]:checked').each(function (i) {
				transids_MHC[i] = $(this).val();
			});
		}
	}

	var dead = [];
	$('input[name^="chkBettypenone-live"]:checked').each(function (i) {
		dead[i] = $(this).val().split('^')[0];
	});

	var live = [];
	$('input[name^="chkBettypelive"]:checked').each(function (i) {
		live[i] = $(this).val().split('^')[0];
	});

	var data = {
		"matchId": $('#matchid').val(),
		"fromRefNo": $('#frefno').val(),
		"toRefNo": $('#trefno').val(),
		"live": live.join('^'),
		"dead": dead.join('^'),
		"userName": $('#userName').val()
	};
		
	var rsmsg = JSON.stringify(msg);
	var data =
	{
		'matchId': $('#matchid').val(),
		'frefno': $('#frefno').val(),
		'trefno': $('#trefno').val(),
		'live': live.join('^'),
		'dead': dead.join('^'),
		'DelOption': DelOption,
		'transids': transids.join(','),        
		'DelOption_BD': DelOption_BD,
		'transids_BD': transids_BD.join(','),
		'DelOption_12EU': DelOption_12EU,
		'transids_12EU': transids_12EU.join(','),
		'DelOption_MHC': DelOption_MHC,
		'transids_MHC': transids_MHC.join(','),
		'message': msg
	}

	$.ajax({
		type: "POST",
		url: "../Services/SpecialDeleteService.svc/DeleteSpecialTransactionNumberGame",
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		data: JSON.stringify(data),
		success: function (rs) {
			$('#ErrMsg').html(rs.ErrorMessage);
			window.scrollTo(0, 0);
			if (rs.ErrorCode == 0) {
				$('div.bg_table').html('');
			}
			parent.jQuery.unblockUI();
		}
	});
}



function Message() {
	this.Content_en = '';
	this.Content_ch = '';
	this.Content_cs = '';
	this.Content_it = '';
	this.Content_jp = '';
	this.Content_th = '';
	this.Content_es = '';
	this.UserId = 0;
	this.UserName = '';
	this.userPwd = '';
}
