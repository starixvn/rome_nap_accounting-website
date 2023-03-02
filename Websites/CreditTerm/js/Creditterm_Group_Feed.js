const mockValue ='333946626.2'
const mockVCustName = 'SuperTest1'
function OpenPopupTransfer(permission,value=mockValue, custName=mockVCustName) {
    if (!permission) return;
    var url = 'CrossTransfer/Index?fromCustName=' + custName + '&fromAmount=' + value;

    parent.OpenPopup(url, "Transfer", 770, 440, true, true, window, 2000, null, null);
}
