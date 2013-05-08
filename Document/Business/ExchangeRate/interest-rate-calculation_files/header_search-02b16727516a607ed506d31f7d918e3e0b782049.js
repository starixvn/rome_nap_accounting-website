/* Transformed JS from: /jslib/header_search.js */
if (document.getElementById('query')) {
    document.getElementById('query').onfocus = function() {
        if (this.value == "" || this.value == this.getAttribute('default')) {
            this.value = "";
        }
        this.style.color = "#000000";
    };

    document.getElementById('query').onblur = function() {
        if (this.value == "" || this.value == this.getAttribute('default')) {
            this.value = this.getAttribute('default');
        }
        this.style.color = "#cbcbcb";
    };
}

function searchRedirect() {
    var search_url = document.location.protocol + "//" + document.location.hostname + "/search?q=";
    var query = document.getElementById('query').value;

    if (query != document.getElementById('query').getAttribute('default')) {
        window.location = search_url + query;
    }
}

function checkSubmit(e) {
    if(e && e.keyCode == 13) {
        searchRedirect();
    }
}
