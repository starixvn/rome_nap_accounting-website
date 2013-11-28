var Sphinx = Sphinx || {};
// creates or use a namespace and fill it with the specified properties
Sphinx.PlaceHolder = function (message) {
    var element = document.getElementById('console2');
    element.innerHTML = element.innerHTML + message + '<br />';
};
        