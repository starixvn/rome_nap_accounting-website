$(document).ready(function () {
	smaManagement.init();
});

var smaManagement = {
	errorElement: $("#err"),
	currencyElement: $("#ddlCurrency"),
	headlineTypeElement: $("#ddlHeadlineType"),
	usernameElement: $("#txtUsername"),
	groupHeadlineElement: $("#txtGroupHeadline"),
	ownerElement: $("#txtOwner"),
	positionElement: $("#ddlPosition"),
	exportExcelElement: $("#btnExportExcel"),
	smaMangementGridElement: $("#SMAMangementGrid"),
	tblContentHeaderElement: $("#tblContentHeader"),
	tblContentElement: $("#tblContent"),

	colNoWidth: 30,
	colGroupHeadlineWidth: 150,
	colHeadlineTypeWidth: 150,
	colOwnerWidth: 150,
	colUsernameWidth: 120,
	colStatusWidth: 120,
	colSiteWidth: 100,
	colCurrencyWidth: 60,
	colPositionWidth: 80,

	pageIndex: 1,
	pageSize: 100,
	totalItems: 0,
	currentSMAList: [],

	sortType: '',
	sortClass: '',
	sortCol: '',

	gridOptions: {},

	init: function () {
		smaManagement.assignSites();
		smaManagement.assignStatus();

		smaManagement.gridOptions = smaManagement.initGridOptions();
		smaManagement.smaMangementGridElement.initGrid(smaManagement.gridOptions);

		$('#btnSubmit').click(function () {
			smaManagement.pageIndex = 1;
			smaManagement.getSMAListByGroupHeadline();
		});

		$('#divSortingByGroupHeadline').click(function () {
			smaManagement.sorting(this, 'GroupName');
		});

		$('#divSortingByOwner').click(function () {
			smaManagement.sorting(this, 'Owner');
		});

		$('#divSortingByUsername').click(function () {
			smaManagement.sorting(this, 'Username');
		});

		smaManagement.exportExcelElement.click(function () {
			smaManagement.exportExcel();
		});
	},
	assignSites: function () {
		var totalSites = jsonSites ? jsonSites.length : 0;

		if (totalSites > 0) {
			var detail;
			var tempData = [];

			for (var i = 0; i < totalSites; i++) {
				detail = jsonSites[i];

				tempData.push({
					Id: 'selectSite' + detail.Value,
					Value: detail.Value,
					Text: detail.Text,
					Checked: true,
					Enable: true,
					CssClass: '',
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
				checkAllText: 'All',
				listData: tempData,
				onCheck: function () {
					if ($('#ddlSites').multiSelect('getCheckedItems').length <= 0) {
						showAlertMessage("err", "Please choose Site!", 1);
					}
					else {
						smaManagement.errorElement.empty();
					}
				},
				noItemText: 'No site found.'
			});
		}
		else {
			$("#ddlSites").multiSelect({
				minWidth: 110,
				maxHeight: 450,
				shadow: true,
				showHeader: true,
				isCheckAll: true,
				showFooter: false,
				listData: [],
				noItemText: 'No site found.'
			});
		}
	},
	assignStatus: function () {
		var detail;
		var tempData = [];

		for (var i = 0, totalStatus = jsonStatuses.length; i < totalStatus; i++) {
			detail = jsonStatuses[i];

			tempData.push({
				Id: 'ddlStatus' + detail.Value,
				Value: detail.Value,
				Text: detail.Text,
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
			checkAllText: 'All',
			listData: tempData,
			onCheck: function () {
				if ($('#ddlStatus').multiSelect('getCheckedItems').length <= 0) {
					showAlertMessage("err", "Please choose Status!", 1);
				}
				else {
					smaManagement.errorElement.empty();
				}
			},
			noItemText: 'No status found.'
		});
	},
	initGridOptions: function () {
		var frozenWidth = ((smaManagement.colNoWidth + smaManagement.colGroupHeadlineWidth + smaManagement.colHeadlineTypeWidth + smaManagement.colOwnerWidth + smaManagement.colUsernameWidth) + (5 * 4));

		return {
			tblContentId: "tblContent",
			manualCopyHeader: true,
			isShowPager: true,
			fullWidth: true,
			fullHeight: true,
			gridHeight: 110,
			freezeWidth: frozenWidth,
			numberFreezeCol: 3,
			pagerOpts: {
				callback: smaManagement.pageSelectCallback,
				start_per_page: 100,
				step_per_page: 100,
				max_items_per_page: 1000,
				items_per_page: 100,
				current_page: 0
			}
		};
	},
	getParams: function () {
		var username = smaManagement.usernameElement.val().toLowerCase();
		if (username === 'all') {
			username = '';
		}

		var groupHeadline = smaManagement.groupHeadlineElement.val().toLowerCase();
		if (groupHeadline === 'all') {
			groupHeadline = '';
		}

		var owner = smaManagement.ownerElement.val().toLowerCase();
		if (owner === 'all') {
			owner = '';
		}

		return {
			siteId: smaManagement.getSelectedSites(),
			currency: smaManagement.currencyElement.val(),
			isOpen: $('#ddlStatusisOpen').prop('checked'),
			isSusupended: $('#ddlStatusisSusupended').prop('checked'),
			isClosed: $('#ddlStatusisClosed').prop('checked'),
			isDisabled: $('#ddlStatusisDisabled').prop('checked'),
			roleId: smaManagement.positionElement.val(),
			username: username,
			groupHeadline: groupHeadline,
			headlineType: smaManagement.headlineTypeElement.val(),
			owner: owner,
			pageIndex: smaManagement.pageIndex,
			pageSize: smaManagement.pageSize
		};
	},
	getSMAListByGroupHeadline: function () {
		if (smaManagement.pageIndex === 1) {
			if ($('#ddlSites').multiSelect('getCheckedItems').length === 0) {
				showAlertMessage("err", "Please choose Site!", 1);
				return false;
			}

			if ($('#ddlStatus').multiSelect('getCheckedItems').length === 0) {
				showAlertMessage("err", "Please choose Status!", 1);
				return false;
			}

			smaManagement.errorElement.empty();
		}

		var formData = smaManagement.getParams();
		smaManagement.resetSorting();

		$.ajax({
			type: "POST",
			url: window.rootUrl + "SMAManagement/GetSMAGroupHeadlines",
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify(formData),
			dataType: "json",
			beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
			success: function (rs) {
				if (!rs || !rs.SMAGroupsInfo || rs.SMAGroupsInfo.length === 0) {
					smaManagement.assignNoInfo();
					return;
				}

				smaManagement.currentSMAList = rs.SMAGroupsInfo;
				smaManagement.totalItems = rs.TotalGroups;
				var membersNotAssignedToGroup = [];

				var arr = [];
				var arrFrozen = [];
				var memberNotAssignedToGroupHtml = '';
				var frozenMemberNotAssignedToGroupHtml = '';
				var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);
				var notAssignedGroupIndex = index;
				var totalSMAInCurrentPage = rs.SMAGroupsInfo.length;
				for (var i = 0; i < totalSMAInCurrentPage; i++) {
					var groupInfo = rs.SMAGroupsInfo[i];

					if (groupInfo.GroupId === 0) {
						membersNotAssignedToGroup.push(groupInfo);
					}
					else {
						arr.push(smaManagement.renderScrollRows(groupInfo));
						arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index));
					}

					index++
				}

				memberNotAssignedToGroupHtml = smaManagement.renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
				frozenMemberNotAssignedToGroupHtml = smaManagement.renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, (notAssignedGroupIndex));

				smaManagement.assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
				smaManagement.initPager(smaManagement.totalItems, smaManagement.pageSize, smaManagement.pageIndex - 1);

				smaManagement.exportExcelElement.removeClass('hidden');
			},
			error: function () {
				smaManagement.assignNoInfo();
			},
			complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { }; }
		});
	},
	updateHeadlineType: function (headlineId, headlineType) {
		$.ajax({
			type: "POST",
			url: window.rootUrl + "SMAManagement/UpdateHeadlineType",
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify({ headlineId: headlineId, headlineType: headlineType }),
			dataType: "json",
			beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
			success: function (rs) {
				if (rs) {
					if (rs.ErrorCode === 0) {
						showAlertMessage("err", "Update Successfully!", 0);
						setTimeout(function () {
							smaManagement.errorElement.empty();

							smaManagement.pageIndex = 1;
							smaManagement.getSMAListByGroupHeadline();
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
	},
	updateOwner: function (headlineId, owner) {
		$.ajax({
			type: "POST",
			url: window.rootUrl + "SMAManagement/UpdateOwner",
			contentType: "application/json; charset=utf-8",
			data: JSON.stringify({ headlineId: headlineId, ownerName: owner }),
			dataType: "json",
			beforeSend: function () { try { parent.BlockUI(); } catch (e) { } },
			success: function (rs) {
				if (rs) {
					if (rs.ErrorCode === 0) {
						showAlertMessage("err", "Update Successfully!", 0);
						setTimeout(function () {
							smaManagement.errorElement.empty();

							smaManagement.pageIndex = 1;
							smaManagement.getSMAListByGroupHeadline();
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
	},
	initPager: function (total_items, items_per_page, current_page) {
		var pagerOpts = smaManagement.gridOptions.pagerOpts;
		pagerOpts.items_per_page = items_per_page;
		pagerOpts.current_page = current_page;

		$("#pagination").pagination(total_items, pagerOpts);
	},
	pageSelectCallback: function (page_index, _items_per_page, jq) {
		var max_page = Math.ceil(smaManagement.totalItems / _items_per_page);
		page_index = page_index < max_page ? page_index : max_page - 1;

		smaManagement.pageIndex = page_index + 1;
		smaManagement.pageSize = _items_per_page;
		smaManagement.getSMAListByGroupHeadline();

		return false;
	},
	getSelectedSites: function () {
		if ($('#ddlSitesHeaderCheckBox').attr('checked')) {
			return '';
		}

		return $('#ddlSites').multiSelect('getCheckedItems').join(',');
	},
	assignNoInfo: function () {
		showAlertMessage("err", "No information is available.", 1);
		smaManagement.exportExcelElement.addClass('hidden');
		smaManagement.smaMangementGridElement.addClass('hidden');
	},
	renderFrozenRows: function (smaGroup, groupIndex) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.SMAList.length : 0;

		var row, css, rowsSpan, rowHtml;

		for (var i = 0; i < totalItems; i++) {
			row = smaGroup.SMAList[i];

			rowsSpan = '';
			css = smaManagement.getCssFormatForEachRow(row.Status);

			if (i === 0) {
				rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 20px">' + groupIndex + '</div>\
                                </td>\
                                <td rowspan="' + totalItems + '" class="group">\
                                    <div class="c " style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 20px" des="Group Headline: ' + smaGroup.GroupName + '">' + smaGroup.GroupName + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">' + smaManagement.renderHeadlineTypeRow(smaGroup.GroupId, row.HeadlineType) + '</td>\
								<td rowspan="' + totalItems + '">' + smaManagement.renderOwnerRow(smaGroup.GroupId, row.Owner) + '</td>';
			}

			rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 20px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

			html.push(rowHtml);
		}

		return html.join('');
	},
	renderScrollRows: function (smaGroup) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.SMAList.length : 0;

		var row, css, rowsSpan, rowHtml;

		for (var i = 0; i < totalItems; i++) {
			row = smaGroup.SMAList[i];

			rowsSpan = '';
			css = smaManagement.getCssFormatForEachRow(row.Status);

			if (i === 0) {
				var height = totalItems * 17;
				rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
                                <td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: ' + height + 'px">' + smaGroup.GroupName + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colHeadlineTypeWidth + 'px; height: ' + height + 'px">' + "NORMAL" + '</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colOwnerWidth + 'px; height: ' + height + 'px">' + "XXX" + '</div>\
                                </td>';
			}

			var rowHtml = '<tr>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 20px">' + row.Username + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colStatusWidth + 'px; height: 20px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colSiteWidth + 'px; height: 20px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colCurrencyWidth + 'px; height: 20px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colPositionWidth + 'px; height: 20px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

			html.push(rowHtml);
		}

		return html.join('');
	},
	renderFrozenForMemberNotAssignedToGroup: function (smaGroup, groupIndex) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.length : 0;

		if (totalItems > 0) {
			var i = 0;
			$.each(smaGroup, function () {
				if (this.SMAList != null && this.SMAList.length > 0) {
					var row = this.SMAList[0];

					var rowsSpan = '';
					var css = smaManagement.getCssFormatForEachRow(row.Status);

					if (i === 0) {
						rowsSpan = '<td class="group" rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 20px" des="Not assigned to group">Not assigned to group</div>\
                                </td>\
								<td rowspan="' + totalItems + '">&nbsp;</td>';
					}

					var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 20px">' + groupIndex + '</div>\
                                </td>' + rowsSpan + '\
								<td>' + smaManagement.renderOwnerRow(row.CustId, row.Owner) + '</td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 20px">' + row.Username + '</div>\
                                </td>\
                            </tr>';

					html.push(rowHtml);
					groupIndex++;
					i++;
				}
			});
		}

		return html.join('');
	},
	renderScrollRowsForMemberNotAssignedToGroup: function (smaGroup) {
		var html = [];
		var totalItems = smaGroup ? smaGroup.length : 0;

		if (totalItems > 0) {
			var i = 0;
			$.each(smaGroup, function () {
				if (this.SMAList != null && this.SMAList.length > 0) {
					var row = this.SMAList[0];

					var rowsSpan = '';
					var css = smaManagement.getCssFormatForEachRow(row.Status);

					if (i === 0) {
						rowsSpan = '<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colGroupHeadlineWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colHeadlineTypeWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
								<td rowspan="' + totalItems + '">\
                                    <div class="c" style="width: ' + smaManagement.colOwnerWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>';
					}

					var rowHtml = '<tr>\
                                <td>\
                                    <div class="c" style="width: ' + smaManagement.colNoWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>' + rowsSpan + '\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colUsernameWidth + 'px; height: 20px">&nbsp;</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colStatusWidth + 'px; height: 20px">' + row.Status + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colSiteWidth + 'px; height: 20px">' + row.Site + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colCurrencyWidth + 'px; height: 20px">' + row.Currency + '</div>\
                                </td>\
                                <td class="' + css + '">\
                                    <div class="c" style="width: ' + smaManagement.colPositionWidth + 'px; height: 20px">' + row.Position + '</div>\
                                </td>\
                            </tr>';

					html.push(rowHtml);
					i++;
				}
			});
		}

		return html.join('');
	},
	getCssFormatForEachRow: function (status) {
		// The top priority will be Disabled - Closed - Suspended - Open.
		var css = 'open';
		status = status.toLowerCase();

		if (status.indexOf('disabled') != -1) {
			css = 'disabled';
		}
		else if (status.indexOf('closed') != -1) {
			css = 'closed';
		}
		else if (status.indexOf('suspended') != -1) {
			css = 'suspended';
		}

		return css;
	},
	sorting: function (obj, sortCol) {
		if (smaManagement.sortClass === '' || smaManagement.sortClass === 'desc') {
			smaManagement.sortClass = 'asc';
			smaManagement.sortType = '';
		}
		else {
			smaManagement.sortClass = 'desc';
			smaManagement.sortType = '-';
		}

		smaManagement.sortCol = sortCol;

		var smaListSorted = [];
		if (sortCol === 'GroupName') {
			var allMembersNotAssignedToGroup = jlinq.from(smaManagement.currentSMAList).equals('GroupId', 0).select();
			var allMembersHasAssigned = jlinq.from(smaManagement.currentSMAList)
				.greater('GroupId', 0)
				.sort(smaManagement.sortType + "GroupName")
				.select();

			if (allMembersNotAssignedToGroup && allMembersNotAssignedToGroup.length > 0) {
				smaListSorted = smaListSorted.concat(allMembersNotAssignedToGroup);
			}

			if (allMembersHasAssigned && allMembersHasAssigned.length > 0) {
				smaListSorted = smaListSorted.concat(allMembersHasAssigned);
			}
		}
		else if (sortCol === 'Username') {
			var newGroup = [];
			var memberNotAssignedToGroup = [];

			$.each(smaManagement.currentSMAList, function () {
				if (this.GroupId == 0 && this.SMAList && this.SMAList.length > 0) {
					memberNotAssignedToGroup.push(this.SMAList[0]);
				}
				else {
					var smaList = jlinq.from(this.SMAList).sort(smaManagement.sortType + 'Username').select();
					newGroup.push({
						GroupId: this.GroupId,
						GroupName: this.GroupName,
						SMAList: smaList
					});
				}
			});

			if (memberNotAssignedToGroup && memberNotAssignedToGroup.length > 0) {
				var sortingMembers = jlinq.from(memberNotAssignedToGroup).sort(smaManagement.sortType + 'Username').select();
				$.each(sortingMembers, function () {
					smaListSorted = smaListSorted.concat({
						GroupId: 0,
						GroupName: 'Not assigned to group',
						SMAList: [this]
					});
				});
			}

			if (newGroup && newGroup.length > 0) {
				smaListSorted = smaListSorted.concat(newGroup);
			}
		}
		else if (sortCol === 'Owner') {
			smaManagement.sortByOwner(obj, sortCol);
			return;
		}

		smaManagement.currentSMAList = smaListSorted;
		var membersNotAssignedToGroup = [];
		var arr = [];
		var arrFrozen = [];
		var memberNotAssignedToGroupHtml = '';
		var frozenMemberNotAssignedToGroupHtml = '';
		var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);
		var notAssignedGroupIndex = index;
		var totalSMAInCurrentPage = smaListSorted.length;
		for (var i = 0; i < totalSMAInCurrentPage; i++) {
			var groupInfo = smaListSorted[i];
			if (groupInfo.GroupId == 0) {
				membersNotAssignedToGroup.push(groupInfo);
			}
			else {
				arr.push(smaManagement.renderScrollRows(groupInfo));
				arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index));
			}
			index++;
		}

		memberNotAssignedToGroupHtml = smaManagement.renderScrollRowsForMemberNotAssignedToGroup(membersNotAssignedToGroup);
		frozenMemberNotAssignedToGroupHtml = smaManagement.renderFrozenForMemberNotAssignedToGroup(membersNotAssignedToGroup, (notAssignedGroupIndex));

		smaManagement.assignGridContent(frozenMemberNotAssignedToGroupHtml, memberNotAssignedToGroupHtml, arrFrozen, arr);
		smaManagement.addSortingIconAfterRender(obj);
	},
	sortByOwner: function (obj) {
		var sortedOwners = jlinq.from(smaManagement.currentSMAList)
			.sort(smaManagement.sortType + "Owner")
			.select();

		var totalItems = sortedOwners ? sortedOwners.length : 0;
		if (totalItems === 0) {
			return;
		}

		smaManagement.currentSMAList = sortedOwners;

		var arr = [];
		var arrFrozen = [];
		var index = (smaManagement.pageSize * (smaManagement.pageIndex - 1) + 1);

		var groupInfo;

		for (var i = 0; i < totalItems; i++) {
			groupInfo = sortedOwners[i];

			arr.push(smaManagement.renderScrollRows(groupInfo));
			arrFrozen.push(smaManagement.renderFrozenRows(groupInfo, index++));
		}

		smaManagement.assignGridContent('', '', arrFrozen, arr);
		smaManagement.addSortingIconAfterRender(obj);
	},
	addSortingIconAfterRender: function (obj) {
		smaManagement.resetSorting();

		if (smaManagement.sortClass === 'asc') {
			$(obj).addClass("asc");
		}
		else {
			$(obj).addClass("desc");
		}
	},
	exportExcel: function () {
		var formData = smaManagement.getParams();
		var url = linkExportExcel + '?' + $.param(formData);

		$('#frameExportExcel').attr('src', url);
	},
	resetSorting: function () {
		$('.sorter').removeClass('asc desc');
		smaManagement.sortType = '';
	},
	replaceSpecialCharactersOnIE9: function (text) {
		var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
		var html = text.replace(expr, '><');

		return html;
	},
	showDescription: function (obj, content) {
		obj.tipsy({ fade: false, gravity: "sw", html: false, title: function () { return content; }, opacity: 1 });
	},
	assignGridContent: function (memberNotAssgnedToGroupFrozenHtml, memberNotAssgnedToGroupScrollHtml, arrFrozen, arr) {
		var htmlScrollRows = arr.join('');
		var htmlFrozenlRows = arrFrozen.join('');
		if ($.browser.msie && parseInt($.browser.version) == 9) {
			memberNotAssgnedToGroupFrozenHtml = smaManagement.replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupFrozenHtml);
			memberNotAssgnedToGroupScrollHtml = smaManagement.replaceSpecialCharactersOnIE9(memberNotAssgnedToGroupScrollHtml);
			htmlScrollRows = smaManagement.replaceSpecialCharactersOnIE9(htmlScrollRows);
			htmlFrozenlRows = smaManagement.replaceSpecialCharactersOnIE9(htmlFrozenlRows);
		}

		smaManagement.tblContentHeaderElement.find('tbody').empty().append(memberNotAssgnedToGroupFrozenHtml).append(htmlFrozenlRows);
		smaManagement.tblContentElement.find('tbody').empty().append(memberNotAssgnedToGroupScrollHtml).append(htmlScrollRows);

		smaManagement.smaMangementGridElement.removeClass("hidden").gridScroller(smaManagement.gridOptions);

		// Tooltip.
		$(".tipsy").remove();
		smaManagement.tblContentHeaderElement.find("tbody td.group").each(function () {
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
			smaManagement.bindHeadlineTypeRowEvents();
			smaManagement.bindOwnerRowEvents();
		}
	},
	renderHeadlineTypeRow: function (headlineId, headlineType) {
		if (!hostPermission) {
			return '<div class="c " style="width:' + smaManagement.colHeadlineTypeWidth + 'px;height:20px">' + headlineType + '</div>';
		}

		return '<div class="c " style="width:' + smaManagement.colHeadlineTypeWidth + 'px;height:20px">\
					<span id="spanHeadlineType' + headlineId + '">' + headlineType + '</span>' +
					smaManagement.renderHeadlineTypeSelect(headlineId, headlineType) +
					'<div style="float:right">\
						<img style="cursor:pointer" src="../App_Themes/Images/icn_edit.png" title="Edit" alt="Edit" class="edit-headlinetype" data-headlineid="' + headlineId + '">\
						<div style="float:right" class="hidden">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/save1.jpg" title="Save" alt="Save" class="save-headlinetype" data-headlineid="' + headlineId + '">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/undo.png" title="Cancel" alt="Cancel" class="cancel-headlinetype" data-headlineid="' + headlineId + '">\
						</div>\
					</div>\
				</div>';
	},
	renderHeadlineTypeSelect: function (headlineId, headlineType) {
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
	},
	bindHeadlineTypeRowEvents: function () {
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
				smaManagement.errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var $select = $("#select" + headlineId);
				
				smaManagement.updateHeadlineType(headlineId, $select.val());
			});

		$(".cancel-headlinetype")
			.unbind("click")
			.bind("click", function () {
				smaManagement.errorElement.empty();

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
	},
	renderOwnerRow: function (headlineId, owner) {
		if (!hostPermission) {
			return '<div class="c " style="width:' + smaManagement.colOwnerWidth + 'px;height:20px">' + owner + '</div>';
		}

		return '<div class="c " style="width:' + smaManagement.colOwnerWidth + 'px;height:20px">\
					<span id="spanOwner' + headlineId + '">' + owner + '</span>\
					<input type="text" class="hidden" style="width:100px" value="' + owner + '" id="input' + headlineId + '"/>\
					<div style="float:right">\
						<img style="cursor:pointer" src="../App_Themes/Images/icn_edit.png" title="Edit" alt="Edit" class="edit-owner" data-headlineid="' + headlineId + '">\
						<div style="float:right" class="hidden">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/save1.jpg" title="Save" alt="Save" class="save-owner" data-headlineid="' + headlineId + '">\
							<img border="0" style="cursor:pointer" src="../App_Themes/Images/undo.png" title="Cancel" alt="Cancel" class="cancel-owner" data-headlineid="' + headlineId + '">\
						</div>\
					</div>\
				</div>';
	},
	bindOwnerRowEvents: function () {
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
				smaManagement.errorElement.empty();

				var $this = $(this);
				var headlineId = $this.attr("data-headlineid");
				var $input = $("#input" + headlineId);

				smaManagement.updateOwner(headlineId, $.trim($input.val()));
			});

		$(".cancel-owner")
			.unbind("click")
			.bind("click", function () {
				smaManagement.errorElement.empty();

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
};