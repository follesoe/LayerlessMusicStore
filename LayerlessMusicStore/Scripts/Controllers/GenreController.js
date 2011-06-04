var genreController = $.sammy("#main", function () {

    this.get("#/admin/genre", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Admin/Genre/List.html", { genres: response.Results });
        });
    });

    this.get("#/admin/genre/add", function (context) {
        context.partial("/Content/Views/Admin/Genre/Add.html");
    });

    this.get("#/admin/genre/edit/:id", function (context) {
        loadData("/indexes/dynamic/genre?query=Id:" + context.params["id"], function (response) {
            context.partial("/Content/Views/Admin/Genre/Edit.html", response.Results[0]);
        });
    });

    this.post("#/admin/genre/save", function (context) {
        var genre = this.params;
        $.post("/App/Save/genre/", JSON.stringify(genre), function () {
            context.redirect("#/admin/genre");
        });
    });

    this.post("#/admin/genre/delete/:id", function (context) {
        var id = context.params["id"];
        console.log("delete " + id);
    });

    var ravenUrl = "http://localhost:8080";

    function loadData(path, callback) {
        $.ajax({
            url: ravenUrl + path,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: callback
        });
    }

});