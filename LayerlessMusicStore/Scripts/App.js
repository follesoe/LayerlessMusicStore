var app = $.sammy("#main", function() {
    this.use(Sammy.Template, "html");

    this.get("#/", function(context) {
        loadData("/indexes/dynamic/genre", function(response) {
            context.partial("/Content/Views/Main.html", { genres: response.Results });
        });
    });

    this.get("#/store/:genre", function(context) {

    });

    this.get("#/admin", function(context) {
        context.partial("/Content/Views/Admin/Main.html");
    });

    this.get("#/admin/genre", function(context) {
        loadData("/indexes/dynamic/genre", function(response) {
            context.partial("/Content/Views/Admin/Genre/List.html", { genres: response.Results });
        });
    });

    this.get("#/admin/genre/add", function(context) {
        context.partial("/Content/Views/Admin/Genre/Add.html");
    });

    this.post("#/admin/genre/add", function(context) {
        var data = $(context.target).serializeObject();
        $.post("/App/Save/genre/", JSON.stringify(data), function() {
            context.redirect("#/admin/genre");
        });
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