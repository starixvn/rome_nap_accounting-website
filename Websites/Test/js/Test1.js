var Sphinx = Sphinx || {};
// creates or use a namespace and fill it with the specified properties


Sphinx.Resource = {
    Id: "en-US"
};

Sphinx.Language = new function () {
    var english = function () {
        var resource = Sphinx.Resource;
        resource.Id = "en-UA";
        return resource;
    };

    this.getLanguage = function() {
        return english();
    };
};

Sphinx.Login = {
    log: function (message) {
        var element = document.getElementById('console2');
        element.innerHTML = element.innerHTML + message + '<br />';

        var resource = Sphinx.Language.getLanguage();
        console.log(resource);
    }
};

        