var rest = (function () {
    var ravenUrl = "http://localhost:8080/databases/musicstore";

    this.loadData = function (path, callback) {
        $.ajax({
            url: ravenUrl + path,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: function (data) {
                if (data.Results) {
                    for (var i = 0; i < data.Results.length; ++i) {
                        data.Results[i].Id = data.Results[i].Id || data.Results[i]["@metadata"]["@id"];
                    }
                }
                callback(data);
            }
        });
    }

    return {
        loadData: loadData
    };
})();