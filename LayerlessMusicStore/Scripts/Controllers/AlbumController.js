var albumController = $.sammy("#main", function () {

    this.get("#/admin/album", function (context) {
        rest.loadData("/indexes/dynamic/album", function (response) {
            context.partial("/Content/Views/Admin/Album/List.html", { albums: response.Results });
        });
    });

    this.get("#/admin/album/add", function (context) {
        rest.loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Admin/Album/Add.html", { genres: response.Results });
        });
    });

    this.get("#/admin/album/edit/:id", function (context) {
        rest.loadData("/indexes/dynamic/genre", function (genreResponse) {
            rest.loadData("/indexes/dynamic/album?query=Id:" + context.params["id"], function (albumResponse) {
                var album = albumResponse.Results[0];
                album.genres = genreResponse.Results;
                context.partial("/Content/Views/Admin/Album/Edit.html", album);
            });
        });
    });

    this.post("#/admin/album/save", function (context) {
        var album = this.params;
        album.Genre.Name = $(context.target).find("select :selected").text();
        album.AlbumArtUrl = album.AlbumArtUrl || "/Content/Images/placeholder.gif";
        $.post("/App/Save/album/", JSON.stringify(album), function () {
            context.redirect("#/admin/album");
        });
    });

    this.get("#/admin/album/delete/:id", function (context) {
        rest.loadData("/indexes/dynamic/album?query=Id:" + context.params["id"], function (response) {
            context.partial("/Content/Views/Admin/Album/Delete.html", response.Results[0]);
        });
    });

    this.post("#/admin/album/delete/:id", function (context) {
        $.post("/App/Delete/" + context.params["id"], function () {
            context.redirect("#/admin/album");
        });
    });

});