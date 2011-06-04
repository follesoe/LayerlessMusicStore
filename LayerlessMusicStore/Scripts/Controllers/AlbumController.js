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

    this.post("#/admin/album/save", function (context) {
        var album = this.params;
        album.Genre.Name = $(context.target).find("select :selected").text();
        album.AlbumArtUrl = album.AlbumArtUrl || "/Content/Images/placeholder.gif";
        $.post("/App/Save/album/", JSON.stringify(album), function () {
            context.redirect("#/admin/album");
        });
    });

});