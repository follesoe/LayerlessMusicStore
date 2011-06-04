var app = $.sammy("#main", function () {
    this.use(Sammy.Mustache, "html");
    this.use(Sammy.NestedParams);

    this.get("#/", function (context) {
        rest.loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Main.html", { genres: response.Results });
        });
    });

    this.get("#/admin", function (context) {
        context.partial("/Content/Views/Admin/Main.html");
    });
});

$(function () {
    app.run("#/");
});