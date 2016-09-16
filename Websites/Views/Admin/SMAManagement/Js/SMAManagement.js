(function ($) {
	"use strict";

	var errorElement = $("#err"),
		currencyElement = $("#ddlCurrency"),
		headlineTypeElement = $("#ddlHeadlineType"),
		excludeTestAccountsElement = $("#chkExcludeTestAccounts"),
		usernameElement = $("#txtUsername"),
		groupHeadlineElement = $("#txtGroupHeadline"),
		ownerElement = $("#txtOwner"),
		positionElement = $("#ddlPosition"),
		exportExcelElement = $("#btnExportExcel"),
		smaMangementGridElement = $("#SMAMangementGrid"),
		tblContentHeaderElement = $("#tblContentHeader"),
		tblContentElement = $("#tblContent");

	var colNoWidth = 30,
		colGroupHeadlineWidth = 150,
		colHeadlineTypeWidth = 150,
		colOwnerWidth = 250,
		colUsernameWidth = 120,
		colStatusWidth = 120,
		colSiteWidth = 100,
		colCurrencyWidth = 60,
		colPositionWidth = 80;

	var pageIndex = 1,
		pageSize = 100,
		totalGroups = 0,
		currentSMAGroupHeadlines = [];

	var sortType = "",
		sortClass = "",
		sortCol = "";

	var gridOptions = {};

	function assignNoInfo() {
		showAlertMessage("err", "No information is available.", 1);
		exportExcelElement.addClass("hidden");
		smaMangementGridElement.addClass("hidden");
	}

	function assignSites() {
		var site;
		var multiselectData = [];

		var totalSites = jsonSites ? jsonSites.length : 0;

		for (var i = 0; i < totalSites; i++) {
			site = jsonSites[i];

			multiselectData.push({
				Id: "selectSite" + site.Value,
				Value: site.Value,
				Text: site.Text,
				Checked: true,
				Enable: true,
				CssClass: "",
				Items: []
			});
		}

		$("#ddlSites").multiSelect({
			minWidth: 110,
			maxHeight: 450,
			shadow: true,
			showHeader: true,
			isCheckAll: true,
			showFooter: false,
			checkAllText: "All",
			listData: multiselectData,
			onCheck: function () {
				if ($("#ddlSites").multiSelect("getCheckedItems").length === 0) {
					showAlertMessage("err", "Please choose Site!", 1);
				}
				else {
					errorElement.empty();
				}
			},
			noItemText: "No site found."
		});
	}

	function assignStatus() {
		var status;
		var multiselectData = [];

		for (var i = 0, totalStatus = jsonStatuses.length; i < totalStatus; i++) {
			status = jsonStatuses[i];

			multiselectData.push({
				Id: "ddlStatus" + status.Value,
				Value: status.Value,
				Text: status.Text,
				Checked: true,
				Enable: true,
				CssClass: "",
				Items: []
			});
		}

		$("#ddlStatus").multiSelect({
			minWidth: 100,
			maxHeight: 450,
			shadow: true,
			showHeader: true,
			isCheckAll: true,
			showFooter: false,
			checkAllText: "All",
			listData: multiselectData,
			onCheck: function () {
				if ($("#ddlStatus").multiSelect("getCheckedItems").length === 0) {
					showAlertMessage("err", "Please choose Status!", 1);
				}
				else {
					errorElement.empty();
				}
			},
			noItemText: "No status found."
		});
	}

	function initGridOptions() {
		var frozenWidth = (colNoWidth + colGroupHeadlineWidth + colHeadlineTypeWidth + colOwnerWidth + colUsernameWidth) + (5 * 4);

		return {
			tblContentId: "tblContent",
			manualCopyHeader: true,
			isShowPager: true,
			fullWidth: true,
			fullHeight: true,
			gridHeight: 110,
			freezeWidth: frozenWidth,
			numberFreezeCol: 5,
			pagerOpts: {
				callback: pageSelectCallback,
				start_per_page: 100,
				step_per_page: 100,
				max_items_per_page: 1000,
				items_per_page: 100,
				current_page: 0
			}
		};
	}

	function getSelectedSites() {
		if ($("#ddlSitesHeaderCheckBox").attr("checked")) {
			return "";
		}

		return $("#ddlSites").multiSelect("getCheckedItems").join(",");
	}

	function getParams() {
		var username = usernameElement.val().toLowerCase();
		if (username === "all") {
			username = "";
		}

		var groupHeadline = groupHeadlineElement.val().toLowerCase();
		if (groupHeadline === "all") {
			groupHeadline = "";
		}

		var owner = ownerElement.val().toLowerCase();
		if (owner === "all") {
			owner = "";
		}

		return {
			siteId: getSelectedSites(),
			currency: currencyElement.val(),
			isOpen: $("#ddlStatusisOpen").prop("checked"),
			isSusupended: $("#ddlStatusSuspended").prop("checked"),
			isClosed: $("#ddlStatusisClosed").prop("checked"),
			isDisabled: $("#ddlStatusisDisabled").prop("checked"),
			roleId: positionElement.val(),
			username: username,
			groupHeadline: groupHeadline,
			headlineType: headlineTypeElement.val(),
			owner: owner,
			excludeTestAccounts: excludeTestAccountsElement.prop("checked"),
			pageIndex: pageIndex,
			pageSize: pageSize
		};
	}

	function resetSorting() {
		$(".sorter").removeClass("asc desc");
		sortType = "";
	}

	function addSortingIconAfterRender(obj) {
		resetSorting();

		if (sortClass === "asc") {
			$(obj).addClass("asc");
		}
		else {
			$(obj).addClass("desc");
		}
	}

	function getSMAGroupHeadlines() {
		if (pageIndex === 1) {
			if ($("#ddlSites").multiSelect("getCheckedItems").length === 0) {
				showAlertMessage("err", "Please choose Site!", 1);
				return false;
			}

			if ($("#ddlStatus").multiSelect("getCheckedItems").length === 0) {
				showAlertMessage("err", "Please choose Status!", 1);
				return false;
			}

			errorElement.empty();
		}

		var formData = getParams();
		resetSorting();

		$.ajax({
			type: "POST",
			url: window.rootUrl + "SMAManagement/GetSMAGroupHeadlines",
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify(formData),
			dataType: "json",
			beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
			success: function (rs) {
				if (!rs || !rs.SMAGroupHeadlines || rs.SMAGroupHeadlines.length === 0) {
					assignNoInfo();
					return;
				}

				currentSMAGroupHeadlines = rs.SMAGroupHeadlines;
				totalGroups = rs.TotalGroups;

				renderSMAGroupHeadlines(rs.SMAGroupHeadlines);
				initPager(rs.TotalGroups, pageSize, pageIndex - 1);

				exportExcelElement.removeClass("hidden");
			},
			error: function () {
				assignNoInfo();
			},
			complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
		});
	}

	function renderSMAGroupHeadlines(groupHeadlines) {
		var arr = [];
		var arrFrozen = [];
		var membersNotAssignedToGroup = [];

		var index = (pageSize * (pageIndex - 1) + 1);
		var notAssignedGroupIndex = index;

		var group;
		for (var i = 0, max = groupHeadlines.length; i < max; i++) {
			group = groupHeadlines[i];

			if (group.GroupId === 0) {
				membersNotAssignedToGroup.push(group);
			}
			else {
				arr.push(renderScrollRows(group));
				arrFrozen.push(renderFrozenRows(group, index));
			}

			index++;
		}

		var memberNotAssignedToGroupHtml = renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
		var frozenMemberNotAssignedToGroupHtml = renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, notAssignedGroupIndex);

		assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
	}

	function renderFrozenRows(smaGroup, groupIndex) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.SMAEntities.length : 0;

		var row, css, rowsSpan, rowHtml;

		for (var i = 0; i < totalItems; i++) {
			row = smaGroup.SMAEntities[i];

			rowsSpan = "";
			css = getCssFormatForEachRow(row.IsClosed, row.IsSuspend, row.IsEnable);

			if (i === 0) {
				rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colNoWidth + 'px; height: 20px">' + groupIndex + '</div>\
                                </td>\
                                <td rowspan="' + totalItems + '" class="group">\
                                    <div class="c " style="width: ' + colGroupHeadlineWidth + 'px; height: 20px" des="Group Headline: ' + smaGroup.GroupName + '">' + smaGroup.GroupName + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">' + renderHeadlineTypeRow(smaGroup.GroupId, row.HeadlineType) + '</td>\
								<td rowspan="' + totalItems + '">' + renderOwnerRow(smaGroup.GroupId, row.Owner) + '</td>';
			}

			rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

			html.push(rowHtml);
		}

		return html.join("");
	}

	function renderScrollRows(smaGroup) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.SMAEntities.length : 0;

		var row, css, rowsSpan, rowHtml;

		for (var i = 0; i < totalItems; i++) {
			row = smaGroup.SMAEntities[i];

			rowsSpan = "";
			css = getCssFormatForEachRow(row.IsClosed, row.IsSuspend, row.IsEnable);

			if (i === 0) {
				var height = totalItems * 17;
				rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colNoWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
                                <td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colGroupHeadlineWidth + 'px; height: ' + height + 'px">' + smaGroup.GroupName + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colHeadlineTypeWidth + 'px; height: ' + height + 'px">' + "NORMAL" + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colOwnerWidth + 'px; height: ' + height + 'px">' + "XXX" + '</div>\
                                </td>';
			}

			var rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px">' + row.Username + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colStatusWidth + 'px; height: 20px; line-height: 20px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colSiteWidth + 'px; height: 20px; line-height: 20px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colCurrencyWidth + 'px; height: 20px; line-height: 20px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colPositionWidth + 'px; height: 20px; line-height: 20px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

			html.push(rowHtml);
		}

		return html.join("");
	}

	function renderFrozenForMemberNotAssignedToGroup(smaGroup, groupIndex) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.length : 0;

		if (totalItems > 0) {
			var i = 0;
			$.each(smaGroup, function () {
				if (this.SMAEntities != null && this.SMAEntities.length > 0) {
					var row = this.SMAEntities[0];

					var rowsSpan = '';
					var css = getCssFormatForEachRow(row.IsClosed, row.IsSuspend, row.IsEnable);

					if (i === 0) {
						rowsSpan = '<td class="group" rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colGroupHeadlineWidth + 'px; height: 20px" des="Not assigned to group">Not assigned to group</div>\
                                </td>\
								<td rowspan="' + totalItems + '">&nbsp;</td>';
					}

					var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + colNoWidth + 'px; height: 20px; line-height: 20px">' + groupIndex + '</div>\
                                </td>' + rowsSpan + '\
								<td>' + renderOwnerRow(row.CustId, row.Owner) + '</td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

					html.push(rowHtml);
					groupIndex++;
					i++;
				}
			});
		}

		return html.join("");
	}

	function renderScrollRowsForMemberNotAssignedToGroup(smaGroup) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.length : 0;

		if (totalItems > 0) {
			var i = 0;
			$.each(smaGroup, function () {
				if (this.SMAEntities != null && this.SMAEntities.length > 0) {
					var row = this.SMAEntities[0];

					var rowsSpan = "";
					var css = getCssFormatForEachRow(row.IsClosed, row.IsSuspend, row.IsEnable);

					if (i === 0) {
						rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colGroupHeadlineWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colHeadlineTypeWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + colOwnerWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>';
					}

					var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + colNoWidth + 'px; height: 20px; line-height: 20px">&nbsp;</div>\
                                </td>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colUsernameWidth + 'px; height: 20px; line-height: 20px">&nbsp;</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colStatusWidth + 'px; height: 20px; line-height: 20px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colSiteWidth + 'px; height: 20px; line-height: 20px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colCurrencyWidth + 'px; height: 20px; line-height: 20px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + colPositionWidth + 'px; height: 20px; line-height: 20px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

					html.push(rowHtml);
					i++;
				}
			});
		}

		return html.join("");
	}

	function renderHeadlineTypeRow(headlineId, headlineType) {
		if (!hostPermission) {
			return '<div class="c " style="width:' + colHeadlineTypeWidth + 'px;height:20px">' + headlineType + '</div>';
		}

		return '<div class="c " style="width:' + colHeadlineTypeWidth + 'px;height:20px">\
					<span id="spanHeadlineType' + headlineId + '">' + headlineType + '</span>' +
					renderHeadlineTypeSelect(headlineId, headlineType) +
					'<div style="float:right">\
						<img style="cursor:pointer" src="../App_Themes/Images/icn_edit.png" title="Edit" alt="Edit" class="edit-headlinetype" data-headlineid="' + headlineId + '">\
						<div style="float:right" class="hidden">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/save1.jpg" title="Save" alt="Save" class="save-headlinetype" data-headlineid="' + headlineId + '">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/undo.png" title="Cancel" alt="Cancel" class="cancel-headlinetype" data-headlineid="' + headlineId + '">\
						</div>\
					</div>\
				</div>';
	}

	function renderHeadlineTypeSelect(headlineId, headlineType) {
		var html = ['<select class="hidden" style="width:100px" id="select' + headlineId + '">'];
		var type;

		for (var i = 1, max = jsonHeadlineTypes.length; i < max; i++) {
			type = jsonHeadlineTypes[i];

			html.push('<option value="');
			html.push(type.Value);
			html.push('"');

			if (headlineType === type.Text) {
				html.push(' selected="selected"');
			}

			html.push('>');
			html.push(type.Text);
			html.push('</option>');
		}

		html.push('</select>');
		return html.join("");
	}

	function bindHeadlineTypeRowEvents() {
		$(".edit-headlinetype")
			.unbind("click")
			.bind("click", function () {
				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");

				$("#spanHeadlineType" + headlineId).addClass("hidden");
				$this.addClass("hidden");

				$("#select" + headlineId)
					.removeClass("hidden");
				$this.next().removeClass("hidden");
			});

		$(".save-headlinetype")
			.unbind("click")
			.bind("click", function () {
				errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var $span = $("#spanHeadlineType" + headlineId);
				var $select = $("#select" + headlineId);

				var oldHeadlineType = $span.html();
				var newHeadlineType = $select.val();

				if (oldHeadlineType === newHeadlineType) {
					var parent = $this.parent();

					$span.removeClass("hidden");
					parent.prev().removeClass("hidden");

					$select.addClass("hidden");
					parent.addClass("hidden");
				}
				else {
					updateHeadlineType(headlineId, newHeadlineType);
				}
			});

		$(".cancel-headlinetype")
			.unbind("click")
			.bind("click", function () {
				errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var parent = $this.parent();

				var $span = $("#spanHeadlineType" + headlineId).removeClass("hidden");
				parent.prev().removeClass("hidden");

				$("#select" + headlineId)
					.addClass("hidden")
					.val($span.html());
				parent.addClass("hidden");
			});
	}

	function renderOwnerRow(headlineId, owner) {
		if (!hostPermission) {
			return '<div class="c " style="width:' + colOwnerWidth + 'px;height:20px">' + owner + '</div>';
		}

		return '<div class="c " style="width:' + colOwnerWidth + 'px;height:20px">\
					<span id="spanOwner' + headlineId + '">' + owner + '</span>\
					<input type="text" class="hidden" style="width:200px" value="' + owner + '" id="input' + headlineId + '"/>\
					<div style="float:right">\
						<img style="cursor:pointer" src="../App_Themes/Images/icn_edit.png" title="Edit" alt="Edit" class="edit-owner" data-headlineid="' + headlineId + '">\
						<div style="float:right" class="hidden">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/save1.jpg" title="Save" alt="Save" class="save-owner" data-headlineid="' + headlineId + '">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/undo.png" title="Cancel" alt="Cancel" class="cancel-owner" data-headlineid="' + headlineId + '">\
						</div>\
					</div>\
				</div>';
	}

	function bindOwnerRowEvents() {
		$(".edit-owner")
			.unbind("click")
			.bind("click", function () {
				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");

				$("#spanOwner" + headlineId).addClass("hidden");
				$this.addClass("hidden");

				$("#input" + headlineId)
					.removeClass("hidden")
					.focus()
					.select();
				$this.next().removeClass("hidden");
			});

		$(".save-owner")
			.unbind("click")
			.bind("click", function () {
				errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var $span = $("#spanOwner" + headlineId);
				var $input = $("#input" + headlineId);

				var oldOwner = $.trim($span.html());
				var newOwner = $.trim($input.val());

				if (oldOwner === newOwner) {
					var parent = $this.parent();

					$span.removeClass("hidden");
					parent.prev().removeClass("hidden");

					$input.addClass("hidden").val(oldOwner);
					parent.addClass("hidden");
				}
				else {
					updateOwner(headlineId, newOwner);
				}
			});

		$(".cancel-owner")
			.unbind("click")
			.bind("click", function () {
				errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var parent = $this.parent();

				var $span = $("#spanOwner" + headlineId).removeClass("hidden");
				parent.prev().removeClass("hidden");

				$("#input" + headlineId)
					.addClass("hidden")
					.val($span.html());
				parent.addClass("hidden");
			});
	}

	function getCssFormatForEachRow(isClosed, isSuspended, isEnable) {
		// The top priority will be Disabled - Closed - Suspended - Open.
		var css = 'open';

		if (!isEnable) {
			css = 'disabled';
		}
		else if (isClosed) {
			css = 'closed';
		}
		else if (isSuspended) {
			css = 'suspended';
		}

		return css;
	}

	function sorting(obj, sortCol) {
		if (sortClass === "" || sortClass === "desc") {
			sortClass = "asc";
			sortType = "";
		}
		else {
			sortClass = "desc";
			sortType = "-";
		}

		sortCol = sortCol;
		var sortedSMAGroupHeadlines = [];

		if (sortCol === "GroupName") {
			var allMembersNotAssignedToGroup = jlinq.from(currentSMAGroupHeadlines).equals('GroupId', 0).select();
			var allMembersHasAssigned = jlinq.from(currentSMAGroupHeadlines)
				.greater("GroupId", 0)
				.sort(sortType + "GroupName")
				.select();

			if (allMembersNotAssignedToGroup && allMembersNotAssignedToGroup.length > 0) {
				sortedSMAGroupHeadlines = sortedSMAGroupHeadlines.concat(allMembersNotAssignedToGroup);
			}

			if (allMembersHasAssigned && allMembersHasAssigned.length > 0) {
				sortedSMAGroupHeadlines = sortedSMAGroupHeadlines.concat(allMembersHasAssigned);
			}
		}
		else if (sortCol === "Username") {
			var newGroup = [];
			var memberNotAssignedToGroup = [];

			$.each(currentSMAGroupHeadlines, function () {
				if (this.GroupId === 0 && this.SMAEntities && this.SMAEntities.length > 0) {
					memberNotAssignedToGroup.push(this.SMAEntities[0]);
				}
				else {
					var smaList = jlinq.from(this.SMAEntities).sort(sortType + "Username").select();
					newGroup.push({
						GroupId: this.GroupId,
						GroupName: this.GroupName,
						Owner: this.Owner,
						SMAEntities: smaList
					});
				}
			});

			if (memberNotAssignedToGroup && memberNotAssignedToGroup.length > 0) {
				var sortingMembers = jlinq.from(memberNotAssignedToGroup).sort(sortType + "Username").select();
				$.each(sortingMembers, function () {
					sortedSMAGroupHeadlines = sortedSMAGroupHeadlines.concat({
						GroupId: this.GroupId,
						GroupName: this.GroupName,
						Owner: this.Owner,
						SMAEntities: [this]
					});
				});
			}

			if (newGroup && newGroup.length > 0) {
				sortedSMAGroupHeadlines = sortedSMAGroupHeadlines.concat(newGroup);
			}
		}
		else if (sortCol === "Owner") {
			sortByOwner(obj, sortCol);
			return;
		}

		currentSMAGroupHeadlines = sortedSMAGroupHeadlines;

		renderSMAGroupHeadlines(sortedSMAGroupHeadlines);
		addSortingIconAfterRender(obj);
	}

	function sortByOwner(obj) {
		var sortedOwners = jlinq.from(currentSMAGroupHeadlines)
			.sort(sortType + "Owner")
			.select();

		currentSMAGroupHeadlines = sortedOwners;

		var arr = [];
		var arrFrozen = [];
		var index = (pageSize * (pageIndex - 1) + 1);

		var group;
		for (var i = 0, max = sortedOwners.length; i < max; i++) {
			group = sortedOwners[i];

			arr.push(renderScrollRows(group));
			arrFrozen.push(renderFrozenRows(group, index++));
		}

		assignGridContent("", "", arrFrozen, arr);
		addSortingIconAfterRender(obj);
	}

	function replaceSpecialCharactersOnIE9(text) {
		var expr = new RegExp(">[ \t\r\n\v\f]*<", "g");
		var html = text.replace(expr, "><");

		return html;
	}

	function showDescription(obj, content) {
		obj.tipsy({ fade: false, gravity: "sw", html: false, title: function () { return content; }, opacity: 1 });
	}

	function assignGridContent(memberNotAssgnedToGroupFrozenHtml, memberNotAssgnedToGroupScrollHtml, arrFrozen, arr) {
		var htmlScrollRows = arr.join("");
		var htmlFrozenlRows = arrFrozen.join("");

		if ($.browser.msie && parseInt($.browser.version) == 9) {
			memberNotAssgnedToGroupFrozenHtml = replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupFrozenHtml);
			memberNotAssgnedToGroupScrollHtml = replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupScrollHtml);
			htmlScrollRows = replaceSpecialCharactersOnIE9(htmlScrollRows);
			htmlFrozenlRows = replaceSpecialCharactersOnIE9(htmlFrozenlRows);
		}

		tblContentHeaderElement.find("tbody").empty().append(memberNotAssgnedToGroupFrozenHtml).append(htmlFrozenlRows);
		tblContentElement.find("tbody").empty().append(memberNotAssgnedToGroupScrollHtml).append(htmlScrollRows);

		smaMangementGridElement.removeClass("hidden").gridScroller(gridOptions);

		// Tooltip.
		$(".tipsy").remove();
		tblContentHeaderElement.find("tbody td.group").each(function () {
			var $this = $(this);

			var des = $this.find("div").attr("des");
			$this.tipsy({ fade: false, gravity: "sw", html: false, title: function () { return des; }, opacity: 1 });

			$this.hover(function (e) {
				if (parseInt($("div.tipsy-sw").css("top")) < 90) {
					$("div.tipsy-sw").css({ top: 100 });
				}
			});
		});

		if (hostPermission) {
			bindHeadlineTypeRowEvents();
			bindOwnerRowEvents();
		}
	}

	function initPager(total_items, items_per_page, current_page) {
		var pagerOpts = gridOptions.pagerOpts;
		pagerOpts.items_per_page = items_per_page;
		pagerOpts.current_page = current_page;

		$("#pagination").pagination(total_items, pagerOpts);
	}

	function pageSelectCallback(page_index, _items_per_page, jq) {
		var max_page = Math.ceil(totalGroups / _items_per_page);
		page_index = page_index < max_page ? page_index : max_page - 1;

		pageIndex = page_index + 1;
		pageSize = _items_per_page;
		getSMAGroupHeadlines();

		return false;
	}

	function updateHeadlineType(headlineId, headlineType) {
		var url = "SMAManagement/UpdateHeadlineType";
		var postParams = { headlineId: headlineId, headlineType: headlineType };

		postUpdate(url, postParams);
	}

	function updateOwner(headlineId, owner) {
		var url = "SMAManagement/UpdateOwner";
		var postParams = { headlineId: headlineId, ownerName: owner };

		postUpdate(url, postParams);
	}

	function postUpdate(url, postParams) {
		$.ajax({
			type: "POST",
			url: window.rootUrl + url,
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify(postParams),
			dataType: "json",
			beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
			success: function (rs) {
				if (rs) {
					if (rs.ErrorCode === 0) {
						showAlertMessage("err", "Update Successfully!", 0);

						setTimeout(function () {
							errorElement.empty();

							pageIndex = 1;
							getSMAGroupHeadlines();
						}, 1000 * 3);
					}
					else {
						showAlertMessage("err", rs.ErrorMessage, 1);
					}
				}
			},
			error: function () {
				showAlertMessage("err", "Internal Server Error.", 1);
			},
			complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
		});
	}

	function initialize() {
		assignSites();
		assignStatus();

		gridOptions = initGridOptions();
		smaMangementGridElement.initGrid(gridOptions);

		$("#btnSubmit").click(function () {
			pageIndex = 1;
			getSMAGroupHeadlines();
		});

		$("#divSortingByGroupHeadline").click(function () {
			sorting(this, "GroupName");
		});

		$("#divSortingByOwner").click(function () {
			sorting(this, "Owner");
		});

		$("#divSortingByUsername").click(function () {
			sorting(this, "Username");
		});

		exportExcelElement.click(function () {
			var formData = getParams();
			var url = linkExportExcel + "?" + $.param(formData);

			$("#frameExportExcel").attr("src", url);
		});
	}

	initialize();
})(jQuery);
