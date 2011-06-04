var storeController = $.sammy("#main", function () {

    this.get("#/store", function (context) {
        rest.loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Store.html", {
                count: response.Results.length,
                genres: response.Results
            });
        });
    });

    this.get("#/store/genre/:genre", function (context) {
        var genre = context.params["genre"];
        rest.loadData("/indexes/dynamic/album?query=Genre.Name:" + genre, function (response) {
            var viewData = {
                genre: genre,
                albums: response.Results
            };
            context.partial("/Content/Views/Albums.html", viewData);
        });
    });

    this.get("#/store/details/:id", function (context) {
        rest.loadData("/indexes/dynamic/album?query=Id:" + context.params["id"], function (response) {
            context.partial("/Content/Views/AlbumDetails.html", response.Results[0]);
        });
    });

});