const mockValue = '333946626.2'
const mockVCustName = 'SuperTest1'

//use in Creditterm_Group_Feed_Form.htm
function OpenPopupTransfer(permission, value = mockValue, custName = mockVCustName) {
    if (!permission) return;
    var url = 'CrossTransfer/Index?fromCustName=' + custName + '&fromAmount=' + value;

    parent.OpenPopup(url, "Transfer", 770, 440, true, true, window, 1500, null, null);
}


function sortColumn(columnIndex, typeColumn) {
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
    const order = isAsc ? "desc" : "asc";

    for (const element of canSortHeadersIndex) {
        headers[element].classList.remove("asc", "desc");
    }
    headerClicked.classList.add(order);

    const tbody = table.tBodies[0];
    const rows = Array.from(tbody.rows);
    const sortFunc = getSortFunc(columnIndex, typeColumn, order);

    rows.sort(sortFunc);
    rows.forEach((row, index) => {
        row.cells[0].textContent = index + 1;
        tbody.appendChild(row);
    });
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
