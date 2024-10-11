//use in Creditterm_Group_Feed_Form.htm
function OpenPopupTransfer(value, custName, RewardMaxTransId, RewardAmount, RewardCurrency, TotalBalReward) {
   
    var url = 'CrossTransfer/Index?fromCustName=' + custName + '&fromAmount=' + value + '&isOpenInPopup=' + '1'  + '&RewardMaxTransId=' + RewardMaxTransId + '&RewardAmount=' + RewardAmount + '&RewardCurrency=' + RewardCurrency + '&TotalBalReward=' + TotalBalReward ;

    parent.OpenPopup(url, "Transfer", 750, 570, true, true, window, 0, function (){refreshPage(); removeGuideline();},removeGuideline );
}

function removeGuideline (){
    $($(window.top.document).find("#content").get(0).contentDocument).find("div[ugpopup]").remove();
}
var cacheSortSate={
    columnIndex:1,
    typeColumn: "string",
    defaultSort: "asc"
}
function sortColumn(columnIndex, typeColumn, defaultSort=undefined) {
    const table = document.getElementById("creditTermTable");
    const headers = table.rows[0].cells;
    const canSortHeadersIndex = Array.from(headers).reduce((acc, cell, index) => {
        if (cell.classList.contains("sorter")) {
            acc.push(index);
        }
        return acc;
    }, []);

    const headerClicked = headers[columnIndex];
    const isAsc = headerClicked.classList.contains("asc");
    let order = defaultSort || isAsc ? "desc" : "asc";
    if(defaultSort) {
        order = defaultSort;
    }
    for (const element of canSortHeadersIndex) {
        headers[element].classList.remove("asc", "desc");
    }
    headerClicked.classList.add(order);

    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.rows);
    const sortFunc = getSortFunc(columnIndex, typeColumn, order);

    rows.sort(sortFunc);
    cacheSortSate={columnIndex:columnIndex,typeColumn:typeColumn,defaultSort:order};
    rows.forEach((row, index) => {
        row.cells[0].textContent = index + 1;
        tbody.appendChild(row);
    });
}
function refreshPage(){
    let param = '';
    param += '&mode=9';
    param += '&type=' + 4; //type > 3 for wait service delay time update
    try { parent.jQuery.BlockUI(); } catch (e) { }
    $.ajax(
        {
            type: "POST",
            url: "../Report/GetData.ashx",
            data: param,
            success: GetJSONResult
        }
    );
}

function GetJSONResult(resContent) {
    let reportInfo = $.evalJSON(resContent);
    let sData = reportInfo.CreditTerm;
    $("#sContent").html(sData);
    sortColumn(cacheSortSate.columnIndex,cacheSortSate.typeColumn,cacheSortSate.defaultSort);
    try {
        setTimeout(function () { parent.jQuery.unblockUI(); }, 2000);
    } catch (e) { }
    
}
function getSortFunc(columnIndex, typeColumn, order) {
    if (typeColumn === "string") {
        return (rowA, rowB) => {
            const cellA = rowA.cells[columnIndex].getAttribute('value').toLowerCase();
            const cellB = rowB.cells[columnIndex].getAttribute('value').toLowerCase();
            if (order === "asc") {
                return cellA.localeCompare(cellB);
            } else {
                return cellB.localeCompare(cellA);
            }
        };
    } else if (typeColumn === "number") {
        return (rowA, rowB) => {
            const cellA = Number(rowA.cells[columnIndex].getAttribute('value').toLowerCase().replace(/,/g, ""));
            const cellB = Number(rowB.cells[columnIndex].getAttribute('value').toLowerCase().replace(/,/g, ""));
            if (order === "asc") {
                return cellA - cellB;
            } else {
                return cellB - cellA;
            }
        };
    }
}
