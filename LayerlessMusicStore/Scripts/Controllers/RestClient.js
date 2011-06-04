var rest = (function () {
    var ravenUrl = "http://localhost:8080/databases/musicstore";

    this.loadData = function (path, callback) {
        $.ajax({
            url: ravenUrl + path,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: callback
        });
    }

    return {
        loadData: loadData
    };
})();