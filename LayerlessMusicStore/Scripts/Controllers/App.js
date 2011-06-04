var app = $.sammy("#main", function () {
    this.use(Sammy.Mustache, "html");
    this.use(Sammy.NestedParams);

    this.get("#/", function (context) {
        rest.loadData("/indexes/dynamic/genre", function (genreResponse) {
            rest.loadData("/indexes/Raven/DocumentsByEntityName?query=Tag:album&start=0&pageSize=5", function (albumResponse) {
                context.partial("/Content/Views/Main.html", {
                    genres: genreResponse.Results,
                    albums: albumResponse.Results
                });
            });
        });
    });

    this.get("#/admin", function (context) {
        context.partial("/Content/Views/Admin/Main.html");
    });
});

$(function () {
    app.run("#/");
});