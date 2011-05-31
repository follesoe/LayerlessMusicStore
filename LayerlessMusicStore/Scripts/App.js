﻿var app = $.sammy("#main", function () {
    this.use(Sammy.Mustache, "html");
    this.use(Sammy.NestedParams);

    this.get("#/", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Main.html", { genres: response.Results });
        });
    });

    this.get("#/store/genre/:genre", function (context) {
        var genre = context.params["genre"];
        loadData("/indexes/dynamic/album?query=Genre.Name:" + genre, function (response) {
            var viewData = {
                genre: genre,
                albums: response.Results,
                getUrl: function () {
                    return function (text, render) {
                        var rendered = render(text);
                        if (rendered) return rendered;
                        return "/Content/Images/placeholder.gif";
                    }
                }
            };
            context.partial("/Content/Views/Albums.html", viewData);
        });
    });

    this.get("#/admin", function (context) {
        context.partial("/Content/Views/Admin/Main.html");
    });

    this.get("#/admin/genre", function (context) {
        loadData("/indexes/dynamic/genre", function (response) {
            context.partial("/Content/Views/Admin/Genre/List.html", { genres: response.Results });
        });
    });

    this.get("#/admin/genre/add", function (context) {
        context.partial("/Content/Views/Admin/Genre/Add.html");
    });

    this.get("#/admin/genre/edit/:id", function (context) {
        var id = context.params["id"];
        loadData("/indexes/dynamic/genre?query=Id:" + id, function (response) {
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
});