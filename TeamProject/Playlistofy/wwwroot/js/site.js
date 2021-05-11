// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }
    $("#searchQuery").autocomplete({
        source: function (request, response) {
            var srchType = document.getElementById("searchType").value;
            $.ajax({
                url: '/Home/AutoComplete/',
                data: { prefix: extractLast(request.term), searchType: srchType },
                type: "POST",
                success: function (data) {
                    response($.ui.autocomplete.filter($.map(data, function (item) {
                        return item;
                    }), extractLast(request.term)))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (e, i) {
            $("#itemId").val(i.item.Id);
            var terms = split(this.value);
            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(i.item.value);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(", ");
            return false;
        },
        minLength: 2
    });
});

$(function () {
    function split(val) {
        return val.split(/,\s*/);
    }
    function extractLast(term) {
        return split(term).pop();
    }
    $("#searchTerm").autocomplete({
        source: function (request, response) {
            var srchType = document.getElementById("searchType").value;
            $.ajax({
                url: '/Playlists/AutoComplete/',
                data: { prefix: extractLast(request.term), searchType: srchType },
                type: "POST",
                success: function (data) {
                    response($.ui.autocomplete.filter($.map(data, function (item) {
                        return item;
                    }), extractLast(request.term)))
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (e, i) {
            $("#itemId").val(i.item.Id);
            var terms = split(this.value);
            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(i.item.value);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(", ");
            return false;
        },
        minLength: 2
    });
});