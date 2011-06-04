var app = $.sammy("#main", function () {
    this.use(Sammy.Mustache, "html");
    this.use(Sammy.NestedParams);

    this.get("#/", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Main.html", { genres: response.Results });
        });
    });

    this.get("#/store", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Store.html", {
                count: response.Results.length,
                genres: response.Results
            });
        });
    });

    this.get("#/store/genre/:genre", function (context) {
        var genre = context.params["genre"];
        loadData("/indexes/dynamic/album?query=Genre.Name:" + genre, function (response) {
            var viewData = {
                genre: genre,
                albums: response.Results
            };
            context.partial("/Content/Views/Albums.html", viewData);
        });
    });

    this.get("#/store/details/:id", function (context) {
        loadData("/indexes/dynamic/album?query=Id:" + context.params["id"], function (response) {
            context.partial("/Content/Views/AlbumDetails.html", response.Results[0]);
        });
    });

    this.get("#/admin", function (context) {
        context.partial("/Content/Views/Admin/Main.html");
    });

    this.get("#/admin/album", function (context) {
        loadData("/indexes/dynamic/album", function (response) {
            context.partial("/Content/Views/Admin/Album/List.html", { albums: response.Results });
        });
    });

    this.get("#/admin/album/add", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
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

    var ravenUrl = "http://localhost:8080";

    function loadData(path, callback) {
        $.ajax({
            url: ravenUrl + path,
            dataType: "jsonp",
            jsonp: "jsonp",
            success: callback
        });
    }

    function imageOrDefault() {
        return function (text, render) {
            var rendered = render(text);
            if (rendered) return rendered;
            return "/Content/Images/placeholder.gif";
        }
    }
});

$(function () {
    app.run("#/");
});